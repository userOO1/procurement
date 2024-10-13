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
using Parser.Core.ss;

namespace Parser.Core
{
    static class HtmlLoader
    {
        static readonly HttpClient client;
        static string url;
        
        static  HtmlLoader()
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
        }

        public static async Task<string> GetSourceByPageId(int id)
        {
            string baseUrl = "https://zakupki.gov.ru/epz/order/extendedsearch/results.html?searchString=&morphology=on&search-filter=%D0%94%D0%B0%D1%82%D0%B5+%D0%BE%D0%B1%D0%BD%D0%BE%D0%B2%D0%BB%D0%B5%D0%BD%D0%B8%D1%8F&pageNumber=1&sortDirection=false&recordsPerPage=_10&showLotsInfoHidden=false&savedSearchSettingsIdHidden=&sortBy=UPDATE_DATE&fz44=on&fz223=on&af=on&ca=on&pc=on&pa=on&placingWayList=&selectedLaws=&priceFromGeneral=&priceFromGWS=&priceFromUnitGWS=&priceToGeneral=&priceToGWS=&priceToUnitGWS=&currencyIdGeneral=-1&publishDateFrom=&publishDateTo=&applSubmissionCloseDateFrom=&applSubmissionCloseDateTo=&customerIdOrg=&customerFz94id=&customerTitle=&okpd2Ids=&okpd2IdsCodes=";
            url = baseUrl.Replace("pageNumber=1", "pageNumber={CurrentId}");
            var currentUrl = url.Replace("{CurrentId}", id.ToString());

            try
            {
                var response = await client.GetAsync(currentUrl);
                string source = null;
                Console.WriteLine(response.StatusCode);               

                if (response == null | response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception();                  
                }
                source = await response.Content.ReadAsStringAsync();
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
