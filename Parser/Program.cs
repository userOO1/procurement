using AngleSharp.Browser;
using Parser.Core;
using Parser.Core.ss;

class Program

{
    
    public static async Task Main(string[] args)
    {
        ParserWorker<string[]> parser;
        int page = 1;

        parser = new ParserWorker<string[]>(
                            new HabraParser()
                        );
        parser.OnCompleted += Parser_OnCompleted;
        parser.OnNewData += Parser_OnNewData;
        
        

        void Parser_OnCompleted(object obj)
        {
            Console.WriteLine("All works done!");
        }
        void Parser_OnNewData(object arg1, string[] arg2)
        {
            Console.WriteLine($"page:{page}");
            foreach (string s in arg2) Console.WriteLine(s);
            
            page += 1;
        }

        parser.Settings = new HabraSettings(1, 5);
        await parser.Start();
        


        // Если нужно остановить парсер
        // parser.Abort(); // Раскомментируйте при необходимости
    }
}
