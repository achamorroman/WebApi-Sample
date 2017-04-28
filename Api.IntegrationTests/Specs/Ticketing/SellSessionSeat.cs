using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.BindingModels;
using Api.IntegrationTests.Infrastructure;
using Api.IntegrationTests.Infrastructure.CollectionFixtures;
using Aplication.Commands;
using Aplication.Queries.ViewModels;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Xunit;

namespace Api.IntegrationTests.Specs.Ticketing
{
    [Collection(Collections.Database)]
    public class SellSessionSeat
    {
        private readonly DatabaseFixture _fixture;

        private readonly int _cinemaId;
        private readonly int _publishedSessionId;
        private readonly int _unpublishedSessionId;

        public SellSessionSeat(DatabaseFixture fixture)
        {
            _fixture = fixture;

            _cinemaId = _fixture.SeedData.Cinema.Id;
            _publishedSessionId = _fixture.SeedData.Sessions.First().Id;
            _unpublishedSessionId = _fixture.SeedData.Sessions.Last().Id;
        }

        [Fact]
        public async Task SellSessionSeat_For_Published_Session_And_Not_Sold_Seat_Should_Return_TicketId()
        {
            var endpoint = $"api/cinemas/{_cinemaId}/ticketing/sessions/{_publishedSessionId}/seat/1/1";
            var response = await _fixture.Server.CreateRequest(endpoint)
                .WithIdentity(Identities.User)
                .PostAsync();

            await response.IsSuccessStatusCodeOrTrow();

            var value = await response.Content.ReadAsAsync<SellSessionSeatResponse>();

            value.Should().NotBeNull();
            value.TicketId.Should().NotBeEmpty();
        }
    }
}