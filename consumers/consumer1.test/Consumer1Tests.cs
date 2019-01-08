using System.Collections.Generic;
using IranianMusic.Instruments.DataAccess;
using IranianMusic.Instruments.Shared;
using Newtonsoft.Json;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace IranianMusic.Instruments.Consumer1.Tests
{
    public class Consumer1Tests : IClassFixture<PactConfigurator>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _baseUri;
        public Consumer1Tests(PactConfigurator pactConfigurator)
        {
            _mockProviderService = pactConfigurator.MockProviderService;
            _mockProviderService.ClearInteractions();
            _baseUri = pactConfigurator.MockBaseUri;
        }
        [Fact]
        public void ItHandlesInvalidInstrumentName()
        {
            // Arange
            const string instrument = "Ta";

            var invalidRequestMessage = $"{instrument} is not valid";

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
                    Path = "/api/Instrument",
                    Query = $"name={instrument}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        message = invalidRequestMessage,
                        result = "Failed",
                        data = ""
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
            Assert.Contains(invalidRequestMessage, resultBodyText); //Very simple assertion to assert if response message contains expected result.
        }
        
        [Fact]
        public void ItHandlesUnformalInstrumentName()
        {
            // Arange
            const string instrument = "Oud";

            var invalidRequestMessage = $"{instrument} is not a formal instrument";

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
                    Path = "/api/Instrument",
                    Query = $"name={instrument}"
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
                        message = invalidRequestMessage,
                        result = "Failed",
                        data = ""
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
            Assert.Contains(invalidRequestMessage, resultBodyText); //Very simple assertion to assert if response message contains expected result.
        }
        
        [Fact]
        public void ItHandlesValidAndFormalInstrumentName()
        {
            // Arange
            const string instrument = "Tar";

            var tarDescription = IranianMusicIstrumentsRepository.GetFormalInstrumentDescription(instrument);

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
                    Path = "/api/Instrument",
                    Query = $"name={instrument}"
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
                        message = "",
                        result = "Success",
                        data = tarDescription
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
            Assert.Contains(tarDescription.Substring(0,30), resultBodyText); //Very simple assertion to assert if response message contains expected result.
        }
        
        [Fact]
        public void ItHandlesGetAllInstruments()
        {
            // Arange
            var instruments = IranianMusicIstrumentsRepository.GetAllFormal();

            //Mocking ProviderService
            //1-Provider State
            //2-Description for Specified Provider State
            //3-Specifing the Request
            //4-The Desired Response for Specified Request 

            _mockProviderService
                .Given("Get All Instruments")
                .UponReceiving("A valid GET request for all Instruments with successfully response")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/api/Instrument"
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
                        message = "",
                        result = "Suucess",
                        data = instruments
                    }
                });

            // Act
            var result = HttpUtility.GetAllInstruments(_baseUri)
                .GetAwaiter()
                .GetResult();

            var resultBodyText = result.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();
            var deserializeObject = JsonConvert.DeserializeObject<ResponseVM>(resultBodyText);
            
            // Assert
            Assert.Equal(instruments.Count, deserializeObject.data.Count); //Very simple assertion to assert if response message contains expected result.
        }
    }
}