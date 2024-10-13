using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;

namespace Parser.Core
{
    class Parse<T> where T : class
    {
        IParser<T> parser;                

        #region Properties

        public IParser<T> Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
            }
        }

        

        

        #endregion

        
        public Parse(IParser<T> parser)
        {
            this.parser = parser;
        }
        public async Task<List<T>> Worker()
        {
            List<T> lines = new List<T>();
            for (int i = 1; i <= 5; i++)
            {                

                var source = await HtmlLoader.GetSourceByPageId(i);
                var domParser = new HtmlParser();
                var document = await domParser.ParseDocumentAsync(source);
                var result = parser.Parse(document);
                
                lines.Add(result);
            }
            return lines;
        }

        
        

    }
}
