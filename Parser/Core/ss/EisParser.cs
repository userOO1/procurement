using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;

namespace Parser.Core.ss
{
    class HabraParser : IParser<string[]>
    {
        public string[] Parse(IHtmlDocument document)
        {
            var list = new List<string>();
            var itemsPrice = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("price-block__value"));
            var itemsFz= document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("col-9 p-0 registry-entry__header-top__title text-truncate"));
            var itemsObject = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("registry-entry__body-value"));
            var itemsCustomer = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("registry-entry__body-href"));
            var itemsNumber = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("registry-entry__header-mid__number"));

            for (int i = 0; i < itemsFz.Count();i++)
            {
                list.Add(itemsNumber.ElementAt(i).TextContent.Trim());
                list.Add(itemsObject.ElementAt(i).TextContent.Trim());
                list.Add(itemsPrice.ElementAt(i).TextContent.Trim());
                list.Add(itemsFz.ElementAt(i).TextContent.Trim());
                list.Add(itemsCustomer.ElementAt(i).TextContent.Trim());
                list.Add("-------------------------------------");
            }

            return list.ToArray();
        }
    }
}
