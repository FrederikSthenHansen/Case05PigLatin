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

            const string _inputTextPathProject = @"C:\Users\SA02- Frederik\Documents\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\InputText\";
            const string _inputTextPathDesktopDoc = @"C:\Users\SA02- Frederik\Desktop\CSharpNote.doc";


            const string quote = "\"";
            string myOutput= myParser.MakePigLatinWord(System.Environment.NewLine+$"{quote}TestOutSideFile!{quote}"+System.Environment.NewLine);
            Console.WriteLine(myOutput);
           
            myTextFileHandler.WritePigLatinFile(_inputTextPathProject);
           
            Console.WriteLine(myOutput);

            Console.ReadKey();
            
            //myParser.makePigLatin(myPlaceHolder);
            
        }
    }
}
