# ConsumerDrivenContract.Net

In [Microservices](https://martinfowler.com/articles/microservices.html) World or any [Distributed System](https://en.wikipedia.org/wiki/Distributed_computing) we have **APIs** which consumed by many out of control **Consumers**.
Having a good and successful [Integration Tests](https://martinfowler.com/bliki/IntegrationTest.html) set is essential for ensuring that all providers and consumers can understand each other.

## Test Pyramid

Test Pyramid is a metaphor used to categorise all tests based on some specifications.
We have three test level:
* Unit Test
* Integration Test
* End-to-End Tests

***
Unit tests are tests that trying to ensure that all unit peace of software acts as expected. At this level we test software piece in isolation manner without dealing with wiring of software pieces. Unit tests usally replace all dependencies with [Test Doubles](https://martinfowler.com/bliki/TestDouble.html)

![Unit Test](https://martinfowler.com/articles/practical-test-pyramid/unitTest.png)

***
Certainly, any software; deals with other sections, including the database, file, etc. When we write __unite tests__ we ignore this by using __test doubles__ . [Integration Test](https://martinfowler.com/bliki/IntegrationTest.html) tries to make sure that the software pieces(__which tested in isolation using unit test__) work together correctly. 

![Integration Testing](https://martinfowler.com/bliki/images/integrationTesting/sketch.png)

***

End-to-End tests are write against the production version of the software(in production or production-like environments). Actually E2E test, tests the software against its interface.
![E2E tests](https://martinfowler.com/articles/practical-test-pyramid/e2etests.png)

## Contract Tests

One of the most common forms of end-to-end testing is the **API test**.
When we talk about the api test, we really want to test api contracts. The server and its consumers can communicate in two ways. 
* The server can change the contract whenever it wants, and consumers must adapt to it
* The API contract is drived from consumers needs and so server only change contracts if the change does not break any consumer.

Having a large number of consumers with different needs, changing the requirements of a customer may lead to other consumers being broken.


**Consumer Driven Contract Testing** tryies to solve this problem. 

First of all consumer defines its expected contracts, then server will develop an API that meets this contracts and also writes a test for it. This process will also be repeated for other consumers.


At any time when any consumer needs a change, it changes its contracts and push it to server, then server apply its needs but befor accepting the changes, server will run all other **consumer contract tests** again and if any test breaks, server will rejects the chnages.

**Pacto** is a mature tools to write **Consumer Driven Contract Tests**.

Consumers define its contracts as a pact file which is a json file that describes its needs in an Given-When-Then fation.

Pact has a built in Mocking Server:

```
            //Create and Config PactBuilder
            
            PactBuilder = new PactBuilder(new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"..\..\..\..\..\pacts",
                LogDir = @".\pactlogs"
            }).ServiceConsumer("Consumer1")
              .HasPactWith("Provider");
              
            //Using IPactBuilder to Mock a PactMockProvider
            MockProviderService = PactBuilder.MockService(MockServerPort);
```

At minimum we should specify the path to store consumer contract, server name(Provider) and consumer name (Consumer 1).

Then, for each service that the customer needs, a test is written. In  test, we will determine what output we expect when calling the action.

For example we expect when we Get ".../api/Instrument" with query string "name={instrument}" we should receive a successful response with status code 200 and body like this

```
                    {
                        message = "",
                        result = "Success",
                        data = tarDescription
                    }
```

```
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
```

When we run all tests, a JSON file is created.

```
{
  "consumer": {
    "name": "Consumer1"
  },
  "provider": {
    "name": "Provider"
  },
  "interactions": [
    {
      "description": "A valid GET request for Instruments with successfully response",
      "providerState": "Instrument is Formal",
      "request": {
        "method": "get",
        "path": "/api/Instrument",
        "query": "name=Tar"
      },
      "response": {
        "status": 200,
        "headers": {
          "Content-Type": "application/json; charset=utf-8"
        },
        "body": {
          "message": "",
          "result": "Success",
          "data": "Tar (Persian: تار‎; Azerbaijani: tar) is an Iranian long-necked, waistedinstrument, shared by many cultures and countries including Iran, Azerbaijan, Armenia, Georgia,and others near the Caucasus region.[1][2][5] The word tār means string in Persian, and is also related to the names of the guitar, sitar, setar (سه‌تار, \"three strings\") and dutar (دوتار, \"two strings\").It was invented in the 18th century and has since become one of the most important musical instrumentsin Iran and the Caucasus, particularly in Persian classical music, and the favoured instrument for radifs.In 2012, the craftsmanship and performance art of the Azerbaijani tar was added to the UNESCO's List of the Intangible Cultural Heritage of Humanity"
        }
      }
    },
    {
      "description": "An invalid GET request for Instruments with invalid name",
      "providerState": "There is no Instrument",
      "request": {
        "method": "get",
        "path": "/api/Instrument",
        "query": "name=Ta"
      },
      "response": {
        "status": 404,
        "headers": {
          "Content-Type": "application/json; charset=utf-8"
        },
        "body": {
          "message": "Ta is not valid",
          "result": "Failed",
          "data": ""
        }
      }
    },
    {
      "description": "A valid GET request for Instruments with a secoundary instrument name",
      "providerState": "Instrument is not Formal",
      "request": {
        "method": "get",
        "path": "/api/Instrument",
        "query": "name=Oud"
      },
      "response": {
        "status": 400,
        "headers": {
          "Content-Type": "application/json; charset=utf-8"
        },
        "body": {
          "message": "Oud is not a formal instrument",
          "result": "Failed",
          "data": ""
        }
      }
    },
    {
      "description": "A valid GET request for all Instruments with successfully response",
      "providerState": "Get All Instruments",
      "request": {
        "method": "get",
        "path": "/api/Instrument"
      },
      "response": {
        "status": 200,
        "headers": {
          "Content-Type": "application/json; charset=utf-8"
        },
        "body": {
          "message": "",
          "result": "Suucess",
          "data": {
            "Tar": "Tar (Persian: تار‎; Azerbaijani: tar) is an Iranian long-necked, waistedinstrument, shared by many cultures and countries including Iran, Azerbaijan, Armenia, Georgia,and others near the Caucasus region.[1][2][5] The word tār means string in Persian, and is also related to the names of the guitar, sitar, setar (سه‌تار, \"three strings\") and dutar (دوتار, \"two strings\").It was invented in the 18th century and has since become one of the most important musical instrumentsin Iran and the Caucasus, particularly in Persian classical music, and the favoured instrument for radifs.In 2012, the craftsmanship and performance art of the Azerbaijani tar was added to the UNESCO's List of the Intangible Cultural Heritage of Humanity",
            "Setar": "The Setar (Persian: سه‌تار‎, from seh, meaning /\"three/\" and tār, meaning /\"string/\") \r\n                                        is an Iranian musical instrument. It is a member of the lute family, \r\n                                    which is played with the index finger of the right hand. Two and a half centuries ago, \r\n                                    a fourth string was added to the setar. It has 25–27 moveable frets which are usually\r\n                                     made of animal intestines or silk. It originated in Persia before the spread of Islam.",
            "Santur": "The santur was invented and developed in the area of Iran, Kuwait, Syria and Turkey,\r\n                                     and parts of Mesopotamia (modern-day Iraq). \r\n                                     This instrument was traded and traveled to different parts of the middle east \r\n                                     and each country customized and designed their own versions to adapt to their musical \r\n                                     scales and tunings.\r\n                                     The original santur was made with tree bark, stones and stringed with goat intestines.\r\n                                     The Mesopotamian santur is also the father of the harp, the Chinese yangqin, the harpsichord,\r\n                                     the qanun, the cimbalom and the American and European hammered dulcimers.",
            "Ney": "The ney (Persian: نی / نای‎), is an end-blown flute that figures prominently in \r\n                                    Middle Eastern music. In some of these musical traditions, it is the only wind instrument used. \r\n                                    The ney has been played continuously for 4,500–5,000 years, making it one of the oldest musical instruments still in use.\r\n                                    The Persian ney consists of a hollow cylinder with finger-holes. Sometimes a brass, horn, \r\n                                    or plastic mouthpiece is placed at the top to protect the wood from damage,\r\n                                     and to provide a sharper and more durable edge to blow at. The ney consists of a piece of hollow cane or giant reed with\r\n                                     five or six finger holes and one thumb hole. Modern neys may be made instead of metal or plastic tubing. \r\n                                    The pitch of the ney varies depending on the region and the finger arrangement. \r\n                                    A highly skilled ney player, called neyzen, can reach more than three octaves, though it is more common \r\n                                    to have several /\"helper/\" neys to cover different pitch ranges or to facilitate playing \r\n                                    technically difficult passages in other dastgahs or maqams.\r\n                                    \r\n                                    In Romanian, the word nai[1] is also applied to a curved pan flute while an end-blown \r\n                                    flute resembling the Arab ney is referred to as caval.[2]",
            "Kamancheh": "The kamancheh (also kamānche or kamāncha) (Persian: کمانچه‎) is an Iranian bowed \r\n                                        string instrument, used also in Armenian, Azerbaijani, Turkish and Kurdish music and related \r\n                                        to the rebab, the historical ancestor of the kamancheh and also to the bowed Byzantine lyra, \r\n                                        ancestor of the European violin family. The strings are played with a variable-tension bow.\r\n                                         It is widely used in the classical music of Iran, Armenia, Azerbaijan, Uzbekistan, \r\n                                        Turkmenistan and Kurdistan Regions with slight variations in the structure of the instrument.\r\n                                        \r\n                                        In 2017, the art of crafting and playing with Kamantcheh/Kamancha was included into the UNESCO \r\n                                        Intangible Cultural Heritage Lists.",
            "Tonbak": "The tompak (official Persian name) (تنپک, تنبک, دنبک، تمپک), also tombak, donbak,\r\n                                     dombak or zarb (ضَرب or ضرب) in Afghanistan zer baghali (زیر بغلی ), is a goblet drum from Persia (ancient Iran).\r\n                                     It is considered the principal percussion instrument of Persian music. \r\n                                    The tonbak is normally positioned diagonally across the torso while the player uses one or more \r\n                                    fingers and/or the palm(s) of the hand(s) on the drumhead, often (for a ringing timbre) \r\n                                    near the drumhead's edge. Sometimes tonbak players wear metal finger rings for an \r\n                                    extra-percussive /\"click/\" on the drum's shell. Tonbak virtuosi perform solos \r\n                                    lasting ten minutes or more. The tompak had been used to create a goblet drum."
          }
        }
      }
    }
  ],
  "metadata": {
    "pactSpecification": {
      "version": "2.0.0"
    }
  }
}
```

On the provider side, for each consumer, a test is developed that validates the api contract based on the contract file(pact file) produced by the consumer.

```


        [Fact]
        public void EnsureProviderApiHonoursPactWithConsumer1()
        {
            // Arrange
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                                {
                                    new XUnitOutput(OutputHelper)
                                },
                Verbose = false
            };

            //Act / Assert
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier.ProviderState($"{PactServiceUri}/provider-states")
                .ServiceProvider("Provider", ProviderUri)
                .HonoursPactWith("Consumer1")
                .PactUri(@"..\..\..\..\..\pacts\consumer1-provider.json")
                .Verify();
        }
```
