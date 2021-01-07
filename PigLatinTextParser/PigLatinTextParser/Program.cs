using System;

namespace PigLatinTextParser
{
    class Program
    {
        private const string myPlaceHolder = "This is a 'placeholder text', to make my piglatin parser. /r/n It acts as a placeholder for a fully parsed text of some sort /r /n This specific placeholder i 3 lines long";
        static void Main(string[] args)
        {
            TextParser myParser = new TextParser();
            TextFileHandler myTextFileHandler = new TextFileHandler();

            string myOutput= myParser.MakePigLatinWord("'Next-to-last.'");
            Console.WriteLine(myOutput);
            myTextFileHandler.ReadFiles();

            foreach (var text in myTextFileHandler.RawText) 
            { 
                string[] words = myParser.BreakUpText(text);
                foreach (string word in words)
                {
                   String newWord= myParser.MakePigLatinWord(word);
                    Console.WriteLine(newWord); 
                }
            }

           
         
            Console.WriteLine(myOutput);
            
            //myParser.makePigLatin(myPlaceHolder);
            
        }
    }
}
