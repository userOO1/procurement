using System.Text;
using AngleSharp.Browser;
using Parser.Core;
using Parser.Core.ss;

class Program

{
    public static void Print(object arg1, string[] args)
    {

        Console.OutputEncoding = Encoding.UTF8;
        foreach (string s in args) Console.WriteLine(s);


    }
    public static async Task Main(string[] args)
    {
        ParserWorker<string[]> parser;
        int page = 1;

        parser = new ParserWorker<string[]>(
                            new HabraParser()
                        );
        
        
        

        
        

        parser.Settings = new HabraSettings(1, 5);
        await parser.Worker();



        // Если нужно остановить парсер
        // parser.Abort(); // Раскомментируйте при необходимости
    }
}
