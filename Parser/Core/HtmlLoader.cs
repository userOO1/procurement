using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
            //url = $"{settings.BaseUrl}/{settings.Prefix}/";
            url = settings.BaseUrl.Replace("pageNumber=1", "pageNumber={CurrentId}");
        }

        public async Task<string> GetSourceByPageId(int id)
        {
            var currentUrl = url.Replace("{CurrentId}", id.ToString());
            
            var response = await client.GetAsync(currentUrl);
            string source = null;
            Console.WriteLine(response.StatusCode);
            if (response.StatusCode != HttpStatusCode.OK)
            {   
                while(response.StatusCode != HttpStatusCode.OK)
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
    }
}
