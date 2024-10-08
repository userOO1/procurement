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
        IParserSettings parserSettings;

        HtmlLoader loader;

        

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

        public IParserSettings Settings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        

        #endregion

        
        public Parse(IParser<T> parser)
        {
            this.parser = parser;
        }

        public Parse(IParser<T> parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }

        

        public async Task<List<T>> Worker()
        {
            List<T> lines = new List<T>();
            for (int i = parserSettings.StartPoint; i <= parserSettings.EndPoint; i++)
            {
                

                var source = await loader.GetSourceByPageId(i);
                var domParser = new HtmlParser();

                var document = await domParser.ParseDocumentAsync(source);

                var result = parser.Parse(document);
                //Task.Run(() => Print(this, result));
                //Program.Print(result);
                //Program.Print(this, result);
                lines.Add(result);
            }
            return lines;
        }

        
        

    }
}
