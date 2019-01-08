using System.Collections.Generic;
using IranianMusic.Instruments.DataAccess;
using IranianMusic.Instruments.Shared;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace IranianMusic.Instruments.Consumer2.Tests
{
    public class Consumer2Tests : IClassFixture<PactConfigurator>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _baseUri;
        public Consumer2Tests(PactConfigurator fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions();
            _baseUri = fixture.MockBaseUri;
        }
        
        [Fact]
        public void ItHandlesInvalidInstrumentName()
        {
            // Arange
            const string instrument = "San";

            //Mocking ProviderService
            //1-Provider State
            //2-Description for Specified Provider State
            //3-Specifing the Request
            //4-The Desired Response for Specified Request 

            _mockProviderService
                .Given("There is no Instrument")
                .UponReceiving("An invalid GET request for Instruments with invalid name")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/api/Instrument/{instrument}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            // Act
            var result = HttpUtility.GetInstrumentDesc(instrument, _baseUri)
                                                    .GetAwaiter()
                                                    .GetResult();

            // Assert
            Assert.Equal(404, (int)result.StatusCode); //Very simple assertion to assert if response message contains expected result.
        }
        
        [Fact]
        public void ItHandlesUnformalInstrumentName()
        {
            // Arange
            const string instrument = "Daf";

            //Mocking ProviderService
            //1-Provider State
            //2-Description for Specified Provider State
            //3-Specifing the Request
            //4-The Desired Response for Specified Request 

            _mockProviderService
                .Given("Instrument is not Formal")
                .UponReceiving("A valid GET request for Instruments with a secoundary instrument name")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/api/Instrument/{instrument}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 400,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new 
                    {
                        message = $"{instrument} is a secoundary Instrument"
                    }                        
                });

            // Act
            var result = HttpUtility.GetInstrumentDesc(instrument, _baseUri)
                .GetAwaiter()
                .GetResult();

            var resultBodyText = result.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            // Assert
            Assert.Contains( $"{instrument} is a secoundary Instrument", resultBodyText); //Very simple assertion to assert if response message contains expected result.
        }
        
        [Fact]
        public void ItHandlesValidAndFormalInstrumentName()
        {
            // Arange
            const string instrument = "Santur";

            var santurDescription = IranianMusicIstrumentsRepository.GetFormalInstrumentDescription(instrument);

            //Mocking ProviderService
            //1-Provider State
            //2-Description for Specified Provider State
            //3-Specifing the Request
            //4-The Desired Response for Specified Request 

            _mockProviderService
                .Given("Instrument is Formal")
                .UponReceiving("A valid GET request for Instruments with successfully response")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/api/Instrument/{instrument}" 
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        result = "Success",
                        data = santurDescription
                    }
                });

            // Act
            var result = HttpUtility.GetInstrumentDesc(instrument, _baseUri)
                .GetAwaiter()
                .GetResult();

            var resultBodyText = result.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            // Assert
            Assert.Contains(santurDescription.Substring(0,30), resultBodyText); //Very simple assertion to assert if response message contains expected result.
        }
    }
}