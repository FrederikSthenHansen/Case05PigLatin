using System;

namespace PigLatinTextParser
{
    class Program
    {
        private const string myPlaceHolder = "This is a placeholder text, to make my piglatin parser. /r/n it acts a a placeholder for a fully parsed text of some sort /r /n This specific placeholder i 3 lines long";
        static void Main(string[] args)
        {
            TextParser myParser = new TextParser();

            string myOutput= myParser.MakePigLatinWord("strong");

            string[] arrey = myParser.BreakUpText(myPlaceHolder);

            foreach (string s in arrey)
            { Console.WriteLine(s); }

            Console.WriteLine(myOutput);
            
            //myParser.makePigLatin(myPlaceHolder);
            
        }
    }
}
