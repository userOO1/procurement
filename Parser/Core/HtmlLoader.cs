using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Parser.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            var retryPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
    .AddRetry(new HttpRetryStrategyOptions
    {
        BackoffType = DelayBackoffType.Exponential,
        MaxRetryAttempts = 3
    })
    .Build();

            var socketHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15)
            };

#pragma warning disable EXTEXP0001 // Тип предназначен только для оценки и может быть изменен или удален в будущих обновлениях. Чтобы продолжить, скройте эту диагностику.
            var resilienceHandler = new ResilienceHandler(retryPipeline)
            {
                InnerHandler = socketHandler,
            };
#pragma warning restore EXTEXP0001 // Тип предназначен только для оценки и может быть изменен или удален в будущих обновлениях. Чтобы продолжить, скройте эту диагностику.


            client = new HttpClient(resilienceHandler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
            //url = $"{settings.BaseUrl}/{settings.Prefix}/";
            url = settings.BaseUrl.Replace("pageNumber=1", "pageNumber={CurrentId}");
        }

        public async Task<string> GetSourceByPageId(int id)
        {
            var currentUrl = url.Replace("{CurrentId}", id.ToString());

            try
            {
                var response = await client.GetAsync(currentUrl);

                string source = null;
                Console.WriteLine(response.StatusCode);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    while (response.StatusCode != HttpStatusCode.OK)
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "xyCustomUserAgent/5.0");
                        response = await client.GetAsync(currentUrl);
                        Console.WriteLine(currentUrl);


                    }
                    Console.WriteLine(client);
                    source = await response.Content.ReadAsStringAsync();

                }
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    source = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(response.);
                }

                return source;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine($"Детали: {ex.StackTrace}");
                return ex.ToString();
            }
            
        }
    }
}
