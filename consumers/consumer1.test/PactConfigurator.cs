﻿using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace IranianMusic.Instruments.Consumer1.Tests
{
    public class PactConfigurator:IDisposable
    {
        public int MockServerPort => 6666;
        public string MockBaseUri=> $"http://localhost:{MockServerPort}";
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }
        
        public PactConfigurator()
        {
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
        }

        private static bool _disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                PactBuilder.Build();
            }

            _disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
