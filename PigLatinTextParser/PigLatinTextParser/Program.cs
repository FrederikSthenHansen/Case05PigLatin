using System;

namespace PigLatinTextParser
{
    class Program
    {
        private const string myPlaceHolder = "This is a 'placeholder text', to make my piglatin parser. /r/n It acts as a placeholder for a fully parsed text of some sort /r /n This specific placeholder i 3 lines long";
        static void Main(string[] args)
        {
            TextParser myParser = new TextParser();

            string myOutput= myParser.MakePigLatinWord("'only'");

            string[] arrey = myParser.BreakUpText(myPlaceHolder);

            foreach (string s in arrey)
            { Console.WriteLine(s); }
            dsd
            Console.WriteLine(myOutput);
            
            //myParser.makePigLatin(myPlaceHolder);
            
        }
    }
}
