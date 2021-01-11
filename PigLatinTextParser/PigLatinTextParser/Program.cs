using System;

namespace PigLatinTextParser
{
    class Program
    {
        //private const string myPlaceHolder = "This is a 'placeholder text', to make my piglatin parser. /r/n It acts as a placeholder for a fully parsed text of some sort /r /n This specific placeholder i 3 lines long";
        static void Main(string[] args)
        {
            TextParser myParser = new TextParser();
            TextFileHandler myTextFileHandler = new TextFileHandler();

            const string _inputTextPathTxt = @"C:\Users\SA02- Frederik\Desktop\InputText.txt";
            const string _inputTextPathPDF = @"C:\Users\SA02- Frederik\Desktop\Case5.pdf";

            #region language logic test
            const string quote = "\"";
            string myOutput= myParser.MakePigLatinWord(System.Environment.NewLine+$"{quote}TestOutSideFile!{quote}"+System.Environment.NewLine);
            Console.WriteLine($"Testing language logic outside file: Parsing {quote}TestOutSideFile!{quote} --> {myOutput}");
            Console.WriteLine();
            #endregion

            myTextFileHandler.WritePigLatinFile(_inputTextPathTxt);
            // myTextFileHandler.WritePigLatinFile(_inputTextPathProject);


            Console.WriteLine("Parsing complete: Press any key to close the App");
            Console.ReadKey();
            
            //myParser.makePigLatin(myPlaceHolder);
            
        }
    }
}
