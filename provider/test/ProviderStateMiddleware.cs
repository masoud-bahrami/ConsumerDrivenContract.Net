using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Newtonsoft.Json;

namespace IranianMusic.Instruments.Provider.Tests
{
    public class ProviderStateMiddleware
    {
        private const string ConsumerName = "MusicDastgah.Client";
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "There is no Instrument",
                    RemoveAllData
                }, {
                    "Instrument is not Formal",
                    RemoveAllData
                },
                {
                    "Instrument is Formal",
                    AddData
                },{
                    "Get All Instruments",
                    AddData
                }
            };
        }

        private void RemoveAllData()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\pacts");
            var deletePath = Path.Combine(path, "apiResult.txt");

            if (File.Exists(deletePath))
            {
                File.Delete(deletePath);
            }
        }

        private void AddData()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\pacts");
            var writePath = Path.Combine(path, "apiResult.txt");

            if (!File.Exists(writePath))
            {
                File.Create(writePath);
            }
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(String.Empty);
            }
            else
            {
                await _next(context);
            }
        }

        private void HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (string.Equals(context.Request.Method, HttpMethod.Post.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                context.Request.Body != null)
            {
                string jsonRequestBody;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = reader.ReadToEnd();
                }

                var providerState =
                JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (providerState != null 
                    && !string.IsNullOrEmpty(providerState.State) 
                    && providerState.Consumer == ConsumerName)
                {
                    _providerStates[providerState.State].Invoke();
                }
            }
        }
    }
}