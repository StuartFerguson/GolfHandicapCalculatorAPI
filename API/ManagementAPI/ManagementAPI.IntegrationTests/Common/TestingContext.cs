﻿namespace ManagementAPI.IntegrationTests.GolfClub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;
    using Service.DataTransferObjects.Responses.v2;
    using CreateGolfClubResponse = Service.DataTransferObjects.Responses.v2.CreateGolfClubResponse;
    using GetGolfClubResponse = Service.DataTransferObjects.Responses.v2.GetGolfClubResponse;
    using GolfClubUserResponse = Service.DataTransferObjects.Responses.v2.GolfClubUserResponse;
    using MeasuredCourseListResponse = Service.DataTransferObjects.Responses.v2.MeasuredCourseListResponse;
    using RegisterPlayerResponse = Service.DataTransferObjects.Responses.v2.RegisterPlayerResponse;
    using ClubMembershipResponse = Service.DataTransferObjects.Responses.v2.ClubMembershipResponse;
    using CreateTournamentResponse = Service.DataTransferObjects.Responses.v2.CreateTournamentResponse;
    using GetMembersHandicapListReportResponse = Service.DataTransferObjects.Responses.v2.GetMembersHandicapListReportResponse;
    using GetNumberOfMembersByAgeCategoryReportResponse = Service.DataTransferObjects.Responses.v2.GetNumberOfMembersByAgeCategoryReportResponse;
    using GetNumberOfMembersByHandicapCategoryReportResponse = Service.DataTransferObjects.Responses.v2.GetNumberOfMembersByHandicapCategoryReportResponse;
    using GetNumberOfMembersByTimePeriodReportResponse = Service.DataTransferObjects.Responses.v2.GetNumberOfMembersByTimePeriodReportResponse;
    using GetNumberOfMembersReportResponse = Service.DataTransferObjects.Responses.v2.GetNumberOfMembersReportResponse;
    using GetPlayerScoresResponse = Service.DataTransferObjects.Responses.v2.GetPlayerScoresResponse;

    public class TestingContext
    {
        public GetNumberOfMembersByHandicapCategoryReportResponse GetNumberOfMembersByHandicapCategoryReportResponse;

        public GetNumberOfMembersByTimePeriodReportResponse GetNumberOfMembersByTimePeriodReportResponse;

        public GetNumberOfMembersByAgeCategoryReportResponse GetNumberOfMembersByAgeCategoryReportResponse;

        public GetMembersHandicapListReportResponse GetMembersHandicapListReportResponse;

        public GetPlayerScoresResponse GetPlayerScoresResponse;

        public TestingContext()
        {
            this.GolfClubAdministratorRequests = new Dictionary<String, RegisterClubAdministratorRequest>();
            this.CreateGolfClubRequests = new Dictionary<String, CreateGolfClubRequest>();
            this.CreateGolfClubResponses = new Dictionary<String, CreateGolfClubResponse>();
            this.RegisterPlayerRequests = new Dictionary<String, RegisterPlayerRequest>();
            this.RegisterPlayerResponses = new Dictionary<String, RegisterPlayerResponse>();
            this.AddMeasuredCourseToClubRequests = new Dictionary<KeyValuePair<String, String>, AddMeasuredCourseToClubRequest>();
            this.CreateTournamentRequests =new Dictionary<Tuple<String, String, String>, CreateTournamentRequest>();
            this.CreateTournamentResponses = new Dictionary<Tuple<String, String, String>, CreateTournamentResponse>();
            this.GetTournamentListResponses = new Dictionary<String, List<TournamentResponse>>();
            this.RecordPlayerTournamentScoreRequests = new Dictionary<Tuple<String, String, String, String>, RecordPlayerTournamentScoreRequest>();
        }

        public List<TournamentResponse> GetTournamentListResponse(String golfClubNumber)
        {
            return this.GetTournamentListResponses[golfClubNumber];
        }

        public CreateTournamentRequest GetCreateTournamentRequest(String golfClubNumber,
                                                                  String measuredCourseName,
                                                                  String tournamentNumber)
        {
            Tuple<String, String, String> key = new Tuple<String, String, String>(golfClubNumber, measuredCourseName, tournamentNumber);

            return this.CreateTournamentRequests[key];
        }

        public CreateTournamentResponse GetCreateTournamentResponse(String golfClubNumber,
                                                                  String measuredCourseName,
                                                                  String tournamentNumber)
        {
            Tuple<String, String, String> key = new Tuple<String, String, String>(golfClubNumber, measuredCourseName, tournamentNumber);

            return this.CreateTournamentResponses[key];
        }

        public RecordPlayerTournamentScoreRequest GetRecordPlayerTournamentScoreRequest(String golfClubNumber,
                                                                                        String measuredCourseName,
                                                                                        String tournamentNumber)
        {
            RecordPlayerTournamentScoreRequest request = this.RecordPlayerTournamentScoreRequests
                .Where(x => x.Key.Item1 == tournamentNumber && x.Key.Item2 == golfClubNumber && x.Key.Item3 == measuredCourseName).Select(x => x.Value).Single();

            return request;
        }

        public String GetRecordPlayerTournamentScoreRequestPlayerNumber(String golfClubNumber,
                                                                                        String measuredCourseName,
                                                                                        String tournamentNumber)
        {
            String playerNumber = this.RecordPlayerTournamentScoreRequests
                              .Where(x => x.Key.Item1 == tournamentNumber && x.Key.Item2 == golfClubNumber && x.Key.Item3 == measuredCourseName).Select(x => x.Key.Item4).Single();

            return playerNumber;
        }

        public Dictionary<Tuple<String, String, String, String>, RecordPlayerTournamentScoreRequest> RecordPlayerTournamentScoreRequests { get; set; }

        public Dictionary<Tuple<String,String, String>, CreateTournamentRequest> CreateTournamentRequests { get; set; }
        public Dictionary<Tuple<String, String, String>, CreateTournamentResponse> CreateTournamentResponses { get; set; }

        public DockerHelper DockerHelper { get; set; }

        public Dictionary<String, RegisterClubAdministratorRequest> GolfClubAdministratorRequests { get; set; }

        public Dictionary<String, CreateGolfClubRequest> CreateGolfClubRequests { get; set; }

        public Dictionary<String, CreateGolfClubResponse> CreateGolfClubResponses { get; set; }

        public Dictionary<String, RegisterPlayerRequest> RegisterPlayerRequests { get; set; }

        public Dictionary<String, RegisterPlayerResponse> RegisterPlayerResponses { get; set; }

        public Dictionary<KeyValuePair<String,String>, AddMeasuredCourseToClubRequest> AddMeasuredCourseToClubRequests { get; set; }
        
        public RegisterClubAdministratorRequest GetRegisterClubAdministratorRequest(String golfClubNumber)
        {
            return this.GolfClubAdministratorRequests[golfClubNumber];
        }

        public CreateGolfClubRequest GetCreateGolfClubRequest(String golfClubNumber)
        {
            return this.CreateGolfClubRequests[golfClubNumber];
        }

        public CreateGolfClubResponse GetCreateGolfClubResponse(String golfClubNumber)
        {
            return this.CreateGolfClubResponses[golfClubNumber];
        }

        public RegisterPlayerRequest GetRegisterPlayerRequest(String playerNumber)
        {
            return this.RegisterPlayerRequests[playerNumber];
        }

        public RegisterPlayerResponse GetRegisterPlayerResponse(String playerNumber)
        {
            return this.RegisterPlayerResponses[playerNumber];
        }

        public AddMeasuredCourseToClubRequest GetAddMeasuredCourseToClubRequest(String golfClubNumber,String measuredCourseNumber)
        {
            return this.AddMeasuredCourseToClubRequests[new KeyValuePair<String, String>(golfClubNumber, measuredCourseNumber)];
        }

        public RegisterClubAdministratorRequest RegisterClubAdministratorRequest { get; set; }

        public String GolfClubAdministratorToken { get; set; }

        public String PlayerToken { get; set; }

        public Guid PlayerId { get; set; }

        public CreateGolfClubRequest CreateGolfClubRequest { get; set; }

        public CreateGolfClubResponse CreateGolfClubResponse { get; set; }

        public GetGolfClubResponse GetGolfClubResponse { get; set; }

        public List<GetGolfClubResponse> GetGolfClubResponseList { get; set; }

        public List<MeasuredCourseListResponse> MeasuredCourseList { get; set; }

        public AddTournamentDivisionToGolfClubRequest AddTournamentDivisionToGolfClubRequest { get; set; }

        public CreateMatchSecretaryRequest CreateMatchSecretaryRequest { get; set; }

        public List<GolfClubMembershipDetailsResponse> GolfClubMembersList { get; set; }

        public List<GolfClubUserResponse> GetGolfClubUserListResponse { get; set; }

        public RegisterPlayerRequest RegisterPlayerRequest { get; set; }

        public RegisterPlayerResponse RegisterPlayerResponse { get; set; }

        public GetNumberOfMembersReportResponse GetNumberOfMembersReportResponse { get; set; }

        public GetPlayerResponse GetPlayerResponse { get; set; }

        public List<ClubMembershipResponse> GetGolfClubMembershipResponseList { get; set; }

        public Dictionary<String, List<TournamentResponse>> GetTournamentListResponses { get; set; }
    }
}