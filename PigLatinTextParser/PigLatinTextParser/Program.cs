using System;

namespace PigLatinTextParser
{
    class Program
    {
        private string myPlaceHolder = "This is a placeholder text to make my piglatin parser. /r/n it acts a a placeholder for a fully parsed text of some sort /r /n This specific placeholder i 3 lines long";
        static void Main(string[] args)
        {
            TextParser myParser = new TextParser();
            myParser.makePigLatin(myPlaceHolder);
            Console.WriteLine("Hello World!");
        }
    }
}
