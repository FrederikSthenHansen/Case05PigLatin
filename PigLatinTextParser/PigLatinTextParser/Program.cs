using System;
using System.IO;

namespace PigLatinTextParser
{
    class Program
    {
        private const string myPlaceHolder = "This is a 'placeholder text', to make my piglatin parser. /r/n It acts as a placeholder for a fully parsed text of some sort /r /n This specific placeholder i 3 lines long";
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            //try
            //{
            //    //Set the current directory.
            //    Directory.SetCurrentDirectory(dir);
            //}
            //catch (DirectoryNotFoundException e)
            //{
            //    Console.WriteLine("The specified directory does not exist. {0}", e);
            //}



            TextParser myParser = new TextParser();
            TextFileHandler myTextFileHandler = new TextFileHandler();

            const string _inputTextPathProject = @"C:\Users\SA02 - Frederik\Documents\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\InputText\Case5.pdf";
            const string _inputTextPathDesktop = @"C:\Users\SA02- Frederik\Desktop\Case5.pdf";


            const string quote = "\"";
            string myOutput= myParser.MakePigLatinWord(System.Environment.NewLine+$"{quote}TestOutSideFile!{quote}"+System.Environment.NewLine);
            Console.WriteLine(myOutput);
           
            myTextFileHandler.WritePigLatinFile(_inputTextPathDesktop);
           
            Console.WriteLine(myOutput);

            Console.ReadKey();
            
            //myParser.makePigLatin(myPlaceHolder);
            
        }
    }
}
