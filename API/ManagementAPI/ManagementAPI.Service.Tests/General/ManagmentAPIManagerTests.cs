﻿namespace ManagementAPI.Service.Tests.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using Database;
    using Database.Models;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using GolfClub;
    using GolfClubMembership;
    using ManagementAPI.GolfClub;
    using ManagementAPI.GolfClub.DomainEvents;
    using ManagementAPI.GolfClubMembership;
    using ManagementAPI.Player;
    using Manager;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using Player;
    using Services;
    using Services.ExternalServices.DataTransferObjects;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Shared.General;
    using Shouldly;
    using Xunit;

    public class ManagmentAPIManagerTests
    {
        public ManagmentAPIManagerTests()
        {
            Logger.Initialise(NullLogger.Instance);      
        }

        #region Methods



        [Fact]
        public async Task ManagementAPIManager_GetClubList_ListOfClubsReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
                                 {
                                     EmailAddress = GolfClubTestData.EmailAddress,
                                     Name = GolfClubTestData.Name,
                                     AddressLine1 = GolfClubTestData.AddressLine1,
                                     Town = GolfClubTestData.Town,
                                     Region = GolfClubTestData.Region,
                                     TelephoneNumber = GolfClubTestData.TelephoneNumber,
                                     PostalCode = GolfClubTestData.PostalCode,
                                     AddressLine2 = GolfClubTestData.AddressLine2,
                                     GolfClubId = GolfClubTestData.AggregateId,
                                     WebSite = GolfClubTestData.Website
                                 });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            List<GetGolfClubResponse> result = await manager.GetGolfClubList(CancellationToken.None);

            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_GetClubList_NoClubsFound_ListOfClubsReturnedIsEmpty()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            List<GetGolfClubResponse> result = await manager.GetGolfClubList(CancellationToken.None);

            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubMembersList_ClubHasNoMembers_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate);

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () => { await manager.GetGolfClubMembersList(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubMembersList_ClubNotCreated_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate);

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate);

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () => { await manager.GetGolfClubMembersList(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubMembersList_MembersListReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregateWithMultipleMembershipRequested);

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            List<GetGolfClubMembershipDetailsResponse> result = await manager.GetGolfClubMembersList(GolfClubTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ManagementAPIManager_GetPlayersClubMemberships_MembershipListReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.PlayerClubMembership.Add(new PlayerClubMembership
                                             {
                                                 AcceptedDateTime = GolfClubMembershipTestData.AcceptedDateAndTime,
                                                 PlayerId = GolfClubMembershipTestData.PlayerId,
                                                 MembershipNumber = GolfClubMembershipTestData.MembershipNumber,
                                                 GolfClubId = GolfClubMembershipTestData.AggregateId,
                                                 MembershipId = GolfClubMembershipTestData.MembershipId,
                                                 RejectionReason = null,
                                                 Status = GolfClubMembershipTestData.AcceptedStatus,
                                                 RejectedDateTime = null,
                                                 GolfClubName = GolfClubMembershipTestData.GolfClubName
                                             });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            List<ClubMembershipResponse> membershipList = await manager.GetPlayersClubMemberships(PlayerTestData.AggregateId, CancellationToken.None);

            membershipList.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ManagementAPIManager_GetPlayersClubMemberships_NotMemberOfAnyClubs_MembershipListReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () => { await manager.GetPlayersClubMemberships(PlayerTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_DuplicateRecord_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
                                 {
                                     EmailAddress = GolfClubTestData.EmailAddress,
                                     Name = GolfClubTestData.Name,
                                     AddressLine1 = GolfClubTestData.AddressLine1,
                                     Town = GolfClubTestData.Town,
                                     Region = GolfClubTestData.Region,
                                     TelephoneNumber = GolfClubTestData.TelephoneNumber,
                                     PostalCode = GolfClubTestData.PostalCode,
                                     AddressLine2 = GolfClubTestData.AddressLine2,
                                     GolfClubId = GolfClubTestData.AggregateId,
                                     WebSite = GolfClubTestData.Website
                                 });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubCreatedEvent domainEvent = GolfClubTestData.GetGolfClubCreatedEvent();

            Should.NotThrow(async () => { await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () => { await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertClubInformationToReadModel_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubCreatedEvent domainEvent = GolfClubTestData.GetGolfClubCreatedEvent();

            await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.GolfClub.Count().ShouldBe(1);
        }
        
        [Fact]
        public void ManagmentAPIManager_GetGolfClub_GolfClubNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate());

            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.ThrowAsync<NotFoundException>(async () => { await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public void ManagmentAPIManager_GetGolfClub_GolfClubNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.ThrowAsync<NotFoundException>(async () => { await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagmentAPIManager_GetGolfClub_GolfClubReturned()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());

            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetGolfClubResponse result = await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(GolfClubTestData.Name);
            result.AddressLine1.ShouldBe(GolfClubTestData.AddressLine1);
            result.AddressLine2.ShouldBe(GolfClubTestData.AddressLine2);
            result.Town.ShouldBe(GolfClubTestData.Town);
            result.Region.ShouldBe(GolfClubTestData.Region);
            result.PostalCode.ShouldBe(GolfClubTestData.PostalCode);
            result.TelephoneNumber.ShouldBe(GolfClubTestData.TelephoneNumber);
            result.Website.ShouldBe(GolfClubTestData.Website);
            result.EmailAddress.ShouldBe(GolfClubTestData.EmailAddress);
        }

        [Fact]
        public void ManagmentAPIManager_GetGolfClub_InvalidGolfClubId_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.ThrowAsync<ArgumentNullException>(async () => { await manager.GetGolfClub(Guid.Empty, CancellationToken.None); });
        }

        [Fact]
        public void ManagmentAPIManager_RegisterClubAdministrator_ClubAdministratorRegistered()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None)).ReturnsAsync(new RegisterUserResponse
                                                                                                                             {
                                                                                                                                 UserId = Guid.NewGuid()
                                                                                                                             });
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            RegisterClubAdministratorRequest request = GolfClubTestData.RegisterClubAdministratorRequest;

            Should.NotThrow(async () => { await manager.RegisterClubAdministrator(request, CancellationToken.None); });
        }

        [Fact]
        public void ManagmentAPIManager_RegisterClubAdministrator_ErrorCreatingUser_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None)).ThrowsAsync(new InvalidOperationException("Error"));
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            RegisterClubAdministratorRequest request = GolfClubTestData.RegisterClubAdministratorRequest;

            Should.Throw<Exception>(async () => { await manager.RegisterClubAdministrator(request, CancellationToken.None); });
        }

        private ManagementAPIReadModel GetContext(String databaseName)
        {
            DbContextOptionsBuilder<ManagementAPIReadModel> builder = new DbContextOptionsBuilder<ManagementAPIReadModel>()
                                                                      .UseInMemoryDatabase(databaseName)
                                                                      .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            ManagementAPIReadModel context = new ManagementAPIReadModel(builder.Options);

            return context;
        }

        [Fact]
        public async Task ManagmentAPIManager_GetPlayer_PlayerReturned()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetPlayerDetailsResponse result = await manager.GetPlayerDetails(PlayerTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.EmailAddress.ShouldBe(PlayerTestData.EmailAddress);
            result.DateOfBirth.ShouldBe(PlayerTestData.DateOfBirth);
            result.ExactHandicap.ShouldBe(PlayerTestData.ExactHandicap);
            result.FirstName.ShouldBe(PlayerTestData.FirstName);
            result.FullName.ShouldBe(PlayerTestData.FullName);
            result.Gender.ShouldBe(PlayerTestData.Gender);
            result.HandicapCategory.ShouldBe(PlayerTestData.HandicapCategory);
            result.HasBeenRegistered.ShouldBeTrue();
            result.MiddleName.ShouldBe(PlayerTestData.MiddleName);
            result.LastName.ShouldBe(PlayerTestData.LastName);
            result.PlayingHandicap.ShouldBe(PlayerTestData.PlayingHandicap);
        }

        [Fact]
        public async Task ManagmentAPIManager_GetPlayer_PlayerNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () =>
                         {
                             await manager.GetPlayerDetails(PlayerTestData.AggregateId, CancellationToken.None);
                         });
        }

        [Fact]
        public async Task ManagementAPIManager_GetMeasuredCourseList_ListOfCoursesReturned()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetGolfClubAggregateWithMeasuredCourse);
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetMeasuredCourseListResponse measuredCourseList = await manager.GetMeasuredCourseList(GolfClubTestData.AggregateId, CancellationToken.None);

            measuredCourseList.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            measuredCourseList.MeasuredCourses.Count.ShouldBe(1);
            
            MeasuredCourseListResponse measuredCourse = measuredCourseList.MeasuredCourses.FirstOrDefault();
            measuredCourse.ShouldNotBeNull();

            MeasuredCourseDataTransferObject measuredCourseToCompare = GolfClubTestData.GetMeasuredCourseToAdd();

            measuredCourse.MeasuredCourseId.ShouldBe(measuredCourseToCompare.MeasuredCourseId);
            measuredCourse.Name.ShouldBe(measuredCourseToCompare.Name);
            measuredCourse.StandardScratchScore.ShouldBe(measuredCourseToCompare.StandardScratchScore);
            measuredCourse.TeeColour.ShouldBe(measuredCourseToCompare.TeeColour);
        }

        [Fact]
        public void ManagementAPIManager_GetMeasuredCourseList_GolfClubNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate);
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () => { await manager.GetMeasuredCourseList(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertUserRecordToReadModel_GolfClubAdministrator_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.GetUserById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetClubAdministratorUserResponse);
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubAdministratorSecurityUserCreatedEvent domainEvent = GolfClubTestData.GetGolfClubAdministratorSecurityUserCreatedEvent();

            await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Users.Count().ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_InsertUserRecordToReadModel_GolfClubAdministrator_DuplicateRecord_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.Users.Add(new User
                                 {
                                     GolfClubId = GolfClubTestData.AggregateId,
                                     UserId = GolfClubTestData.GolfClubAdministratorSecurityUserId,
                                     FamilyName = GolfClubTestData.RegisterClubAdministratorRequest.FamilyName,
                                     MiddleName = GolfClubTestData.RegisterClubAdministratorRequest.MiddleName,
                                     GivenName = GolfClubTestData.RegisterClubAdministratorRequest.GivenName,
                                     UserName = GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                                     PhoneNumber = GolfClubTestData.RegisterClubAdministratorRequest.TelephoneNumber,
                                     Email = GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                                     UserType = "Club Administrator"
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.GetUserById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetClubAdministratorUserResponse);
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubAdministratorSecurityUserCreatedEvent domainEvent = GolfClubTestData.GetGolfClubAdministratorSecurityUserCreatedEvent();
            
            Should.NotThrow(async () => { await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void ManagementAPIManager_InsertUserRecordToReadModel_GolfClubAdministrator_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubAdministratorSecurityUserCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () => { await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertUserRecordToReadModel_MatchSecretary_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.GetUserById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetMatchSecretaryUserResponse);
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            MatchSecretarySecurityUserCreatedEvent domainEvent = GolfClubTestData.GetMatchSecretarySecurityUserCreatedEvent();

            await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Users.Count().ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_InsertUserRecordToReadModel_MatchSecretary_DuplicateRecord_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.Users.Add(new User
            {
                GolfClubId = GolfClubTestData.AggregateId,
                UserId = GolfClubTestData.MatchSecretarySecurityUserId,
                FamilyName = GolfClubTestData.CreateMatchSecretaryRequest.FamilyName,
                MiddleName = GolfClubTestData.CreateMatchSecretaryRequest.MiddleName,
                GivenName = GolfClubTestData.CreateMatchSecretaryRequest.GivenName,
                UserName = GolfClubTestData.CreateMatchSecretaryRequest.EmailAddress,
                PhoneNumber = GolfClubTestData.CreateMatchSecretaryRequest.TelephoneNumber,
                Email = GolfClubTestData.CreateMatchSecretaryRequest.EmailAddress,
                UserType = "Match Secretary"
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.GetUserById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetClubAdministratorUserResponse);
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            MatchSecretarySecurityUserCreatedEvent domainEvent = GolfClubTestData.GetMatchSecretarySecurityUserCreatedEvent();

            Should.NotThrow(async () => { await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void ManagementAPIManager_InsertUserRecordToReadModel_MatchSecretary_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            MatchSecretarySecurityUserCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () => { await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None); });
        }

        #endregion
    }
}