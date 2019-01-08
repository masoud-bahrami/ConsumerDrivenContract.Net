using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;

namespace IranianMusic.Instruments.Provider.Tests
{
    public class ProviderApiTests : IDisposable
    {
        private string ProviderUri { get; }
        private string PactServiceUri { get; }
        private IWebHost WebHost { get; }
        private ITestOutputHelper OutputHelper { get; }

        public ProviderApiTests(ITestOutputHelper output)
        {
            OutputHelper = output;
            ProviderUri = "http://localhost:5000";
            PactServiceUri = "http://localhost:9001";

            WebHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                .UseUrls(PactServiceUri)
                .UseStartup<TestStartup>()
                .Build();

            WebHost.Start();
        }

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

        [Fact]
        public void EnsureProviderApiHonoursPactWithConsumer2()
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
                .ServiceProvider("Provider", ProviderUri )
                .HonoursPactWith("Consumer2")
                .PactUri(@"..\..\..\..\..\pacts\consumer2-provider.json")
                .Verify();
        }
        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                WebHost.StopAsync().GetAwaiter().GetResult();
                WebHost.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}