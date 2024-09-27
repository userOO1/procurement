using System.Text;
using AngleSharp.Browser;
using Parser.Core;
using Parser.Core.ss;

class Program

{
    
    public static async Task Main(string[] args)
    {
        ParserWorker<string[]> parser;
        

        parser = new ParserWorker<string[]>(
                            new HabraParser()
                        );
  
        parser.Settings = new HabraSettings(1, 5);
        var parse =await parser.Worker();


        for (int page = 0; page < parse.Count; page++)
        {
            foreach(var s in parse[page])
            {
                Console.WriteLine(s);
            }
            Console.WriteLine(page);
        }

    }
}
