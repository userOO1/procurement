using System.Text;
using AngleSharp.Browser;
using Parser.Core;
using Parser.Core.ss;

class Program

{
    
    public static async Task Main(string[] args)
    {
        Parse<string[]> parser;
        

        parser = new Parse<string[]>(
                            new HabraParser()
                        );
  
        
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
