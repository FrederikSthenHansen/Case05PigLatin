using System;

namespace PigLatinTextParser
{
    class Program
    {
        //private const string myPlaceHolder = "This is a 'placeholder text', to make my piglatin parser. /r/n It acts as a placeholder for a fully parsed text of some sort /r /n This specific placeholder i 3 lines long";
        static void Main(string[] args)
        {
            TextFileHandler myTextFileHandler = new TextFileHandler();
            myTextFileHandler.ProcessInputFiles();
            Console.WriteLine();
            Console.WriteLine("Parsing of all input files complete: Press any key to close the App");
            Console.ReadKey();
        }
    }
}
 