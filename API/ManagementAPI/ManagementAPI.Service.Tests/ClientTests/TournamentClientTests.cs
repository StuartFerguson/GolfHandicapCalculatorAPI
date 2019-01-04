﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Client;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.ClientTests
{
    public class TournamentClientTests
    {
        [Fact]
        public async Task TournamentClient_CreateTournament_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(TournamentTestData.CreateTournamentResponse))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            var response = await client.CreateTournament(passwordToken, TournamentTestData.CreateTournamentRequest, CancellationToken.None);

            response.ShouldNotBeNull();
            response.TournamentId.ShouldBe(TournamentTestData.AggregateId);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task TournamentClient_CreateTournament_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.CreateTournament(passwordToken, TournamentTestData.CreateTournamentRequest, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task TournamentClient_RecordPlayerScore_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            await client.RecordPlayerScore(passwordToken, TournamentTestData.AggregateId,TournamentTestData.RecordMemberTournamentScoreRequest, CancellationToken.None);            
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task TournamentClient_RecordPlayerScore_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);
            
            var exception = Should.Throw(async () =>
            {
                await client.RecordPlayerScore(passwordToken, TournamentTestData.AggregateId,TournamentTestData.RecordMemberTournamentScoreRequest, CancellationToken.None);            
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task TournamentClient_CancelTournament_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            await client.CancelTournament(passwordToken, TournamentTestData.AggregateId, CancellationToken.None);            
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task TournamentClient_CancelTournament_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);
            
            var exception = Should.Throw(async () =>
            {
                await client.CancelTournament(passwordToken, TournamentTestData.AggregateId, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task TournamentClient_CompleteTournament_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            await client.CompleteTournament(passwordToken, TournamentTestData.AggregateId, CancellationToken.None);            
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task TournamentClient_CompleteTournament_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.CompleteTournament(passwordToken, TournamentTestData.AggregateId, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);           
        }

        [Fact]
        public async Task TournamentClient_ProduceTournamentResult_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            await client.ProduceTournamentResult(passwordToken, TournamentTestData.AggregateId, CancellationToken.None);            
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task TournamentClient_ProduceTournamentResult_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.ProduceTournamentResult(passwordToken, TournamentTestData.AggregateId, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);      
        }
    }
}