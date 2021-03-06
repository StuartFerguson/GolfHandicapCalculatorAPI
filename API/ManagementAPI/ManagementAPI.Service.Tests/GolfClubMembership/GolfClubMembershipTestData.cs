﻿using System;
using ManagementAPI.GolfClub;
using ManagementAPI.GolfClubMembership;

namespace ManagementAPI.Service.Tests.GolfClubMembership
{
    using System.Collections.Generic;
    using DataTransferObjects;
    using DataTransferObjects.Responses;

    public class GolfClubMembershipTestData
    {
        public static Guid AggregateId = Guid.Parse("3EBC89BD-08C0-4032-B4FC-D61FB6F69E73");

        public static String GolfClubName = "Test Club 1";

        public static Guid MembershipId = Guid.Parse("8BEE01CC-369B-4D7B-BA34-CF7B4345D7BB");
        
        public static Guid PlayerId = Guid.Parse("9F14D8A4-D8F7-4E32-9600-C3F038E662F6");
        public static Guid PlayerId2 = Guid.Parse("0B065AF7-7D8D-4E67-8BD2-ECA6F2C87F5E");

        public static String PlayerFullName = "Test Player";
        public static String PlayerFullName2 = "Test Player";
            
        public static String PlayerGender = "M";
        public static String PlayerGender2 = "M";
        
        public static DateTime PlayerDateOfBirth = new DateTime(1980,12,31);
        public static DateTime PlayerDateOfBirth2 = new DateTime(1981,12,13);

        public static DateTime RequestDateAndTime = new DateTime(2019,1,2);

        public static DateTime AcceptedDateAndTime = new DateTime(2019,1,3);

        public static DateTime RejectionDateAndTime = new DateTime(2019,1,4);

        public static String RejectionReason = "A reason";

        public static String MembershipNumber = "000001";
        public static String MembershipNumber2 = "000002";

        public static Int32 AcceptedStatus = (Int32)MembershipStatus.Accepted;
        public static Int32 RejectedStatus = (Int32)MembershipStatus.Rejected;

        public static GolfClubMembershipAggregate GetCreatedGolfClubMembershipAggregate()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipAggregate.Create(GolfClubMembershipTestData.AggregateId);

            return aggregate;
        }

        public static GolfClubMembershipAggregate GetCreatedGolfClubMembershipAggregateWithMembershipRequested()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipAggregate.Create(AggregateId);

            aggregate.RequestMembership(PlayerId, PlayerFullName, PlayerDateOfBirth, PlayerGender, RequestDateAndTime);

            return aggregate;
        }

        public static GolfClubMembershipAggregate GetCreatedGolfClubMembershipAggregateWithMultipleMembershipRequested()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipAggregate.Create(AggregateId);

            aggregate.RequestMembership(PlayerId, PlayerFullName, PlayerDateOfBirth, PlayerGender, RequestDateAndTime);
            aggregate.RequestMembership(PlayerId2, PlayerFullName2, PlayerDateOfBirth2, PlayerGender2, RequestDateAndTime);

            return aggregate;
        }

        public static GolfClubMembershipAggregate GetCreatedGolfClubMembershipAggregateWithNumberOfAcceptedMembershipRequests(Int32 memberCount)
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipAggregate.Create(AggregateId);

            for (Int32 i = 0; i < memberCount; i++)
            {
                aggregate.RequestMembership(Guid.NewGuid(), $"Player {i}", PlayerDateOfBirth, PlayerGender, RequestDateAndTime);
            }

            return aggregate;
        }

        public static List<GetGolfClubMembershipDetailsResponse> GetGolfClubMembershipDetailsResponse()
        {
            return new List<GetGolfClubMembershipDetailsResponse>()
                   {
                       new GetGolfClubMembershipDetailsResponse
                       {
                           GolfClubId = GolfClubMembershipTestData.AggregateId,
                           MembershipNumber = GolfClubMembershipTestData.MembershipNumber,
                           PlayerId = GolfClubMembershipTestData.PlayerId,
                           PlayerGender = GolfClubMembershipTestData.PlayerGender,
                           PlayerDateOfBirth = GolfClubMembershipTestData.PlayerDateOfBirth.ToString("dd/MM/yyyy"),
                           MembershipStatus = MembershipStatus.Accepted,
                           PlayerFullName = GolfClubMembershipTestData.PlayerFullName,
                           Name = GolfClubMembershipTestData.GolfClubName
                       },
                       new GetGolfClubMembershipDetailsResponse
                       {
                           GolfClubId = GolfClubMembershipTestData.AggregateId,
                           MembershipNumber = GolfClubMembershipTestData.MembershipNumber2,
                           PlayerId = GolfClubMembershipTestData.PlayerId2,
                           PlayerGender = GolfClubMembershipTestData.PlayerGender2,
                           PlayerDateOfBirth = GolfClubMembershipTestData.PlayerDateOfBirth2.ToString("dd/MM/yyyy"),
                           MembershipStatus = MembershipStatus.Accepted,
                           PlayerFullName = GolfClubMembershipTestData.PlayerFullName2,
                           Name = GolfClubMembershipTestData.GolfClubName
                       }
                   };
        }
    }
}