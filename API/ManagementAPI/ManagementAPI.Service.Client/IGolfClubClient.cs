﻿namespace ManagementAPI.Service.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;

    public interface IGolfClubClient
    {
        #region Methods

        /// <summary>
        /// Adds the measured course to golf club.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMeasuredCourseToGolfClub(String passwordToken,
                                         AddMeasuredCourseToClubRequest request,
                                         CancellationToken cancellationToken);

        /// <summary>
        /// Adds the tournament division.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddTournamentDivision(String passwordToken,
                                   AddTournamentDivisionToGolfClubRequest request,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateGolfClubResponse> CreateGolfClub(String passwordToken,
                                                    CreateGolfClubRequest request,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Creates the match secretary.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CreateMatchSecretary(String passwordToken,
                                  CreateMatchSecretaryRequest request,
                                  CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetGolfClubResponse>> GetGolfClubList(String passwordToken,
                                                        CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club membership list.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetGolfClubMembershipDetailsResponse>> GetGolfClubMembershipList(String passwordToken,
                                                                                   CancellationToken cancellationToken);

        /// <summary>
        /// Gets the measured courses.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetMeasuredCourseListResponse> GetMeasuredCourses(String passwordToken,
                                                               CancellationToken cancellationToken);

        /// <summary>
        /// Gets the single golf club.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetGolfClubResponse> GetSingleGolfClub(String passwordToken,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Registers the golf club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RegisterGolfClubAdministrator(RegisterClubAdministratorRequest request,
                                           CancellationToken cancellationToken);

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RequestClubMembership(String passwordToken,
                                   Guid golfClubId,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club user list.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetGolfClubUserListResponse> GetGolfClubUserList(String passwordToken,
                                                        CancellationToken cancellationToken);

        #endregion
    }
}