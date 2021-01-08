using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace PigLatinTextParser
{
    class TextFileHandler
    {   const string outputPath = @"\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\OutputText\OutputText.txt"; 
        TextParser myParser = new TextParser();
        
        private string[] RawTextArray = new string[] { "" };
        private string[] TreatedTextArray= new string[] {""};
        private string TreatedText;

        private string[] readPDF(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine("opening "+ path);
                using (PdfDocument document = PdfDocument.Open(path))
                {
                    string[] ret = new string[document.NumberOfPages];
                    for (int page = 1; page < document.NumberOfPages; page++)
                    {
                        Page myPage = document.GetPage(page);
                        string pageText = myPage.Text;

                        ret[page - 1] = pageText;
                    }
                    return ret;
                }
            }
            Console.WriteLine(path+" does not exist" );
            return new string[] { "" };
        }

        private string[] readTXT(string path)
        {
     
            string[] ret = new string[] { "" };
            string[] lines = File.ReadAllLines(path);
            return ret;
        }


        private void setMyDirectory(string path)
        {
            try
            {
                //Set the current directory.
                Directory.SetCurrentDirectory(path);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("The specified directory does not exist. {0}", e);
            }
        }

        //Needs Refactoring to accept docx format and other formats
        public string[] ReadFiles(string path)
        {

            //setMyDirectory(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()));
            //Console.WriteLine(Directory.GetCurrentDirectory());


            //setMyDirectory(@"C:\Users\SA02 - Frederik\Documents\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\InputText");
            //Console.WriteLine(Directory.GetCurrentDirectory());


            string ProperPath =/* Path.GetFullPath(path);*/ path;
            Console.WriteLine("Path to read file from: "+ ProperPath);

            string[] ret = new string[] { "" };
            //TODO: Be Change method to be able to read from PDF files and other file types.
            //TODO: make this method able to read files format from path, and select the right file reading method.

            //Determine filetype to use correct tools to read it
            string myFileType = Path.GetExtension(path);
            Console.WriteLine("File has the extension: "+ myFileType );

            if (myFileType==".txt" || myFileType==".TXT") 
            {
                ret = readTXT(ProperPath);
            }
            else if (myFileType == ".pdf" || myFileType == ".PDF") 
            {
                ret = readPDF(ProperPath);
            }
            return ret;
        }

        public void WritePigLatinFile(string filePath) 
        {
            RawTextArray = ReadFiles(filePath);

            //First we take in the string array (bunch of text lines) from readfiles
            foreach (string line in RawTextArray)
            {
                //Strings are broken down into individual words
                string[] words = myParser.BreakUpText(line);

                TreatedTextArray = new string[words.Length];

                for (int w=0;w<words.Length; w++ )
                {   //All words in each line are turned to Pig latin.
                    words[w] = myParser.MakePigLatinWord(words[w]);
                    
                    //write to console
                    //Console.WriteLine(words[w]);
                    
                    //each line is reconstruced word by word
                    TreatedTextArray[w] = myParser.RebuildTextLine(words[w]);
                }
                
                //Then all lines are added back together to reform the text.
                TreatedText = TreatedText+ myParser.RebuildWholeText(TreatedTextArray);
                
            }
            //the finished text is printed to console
            Console.WriteLine(TreatedText);
            File.WriteAllText(Path.GetFullPath(outputPath), TreatedText);
            //TODO: write finished text to new .TXT file in output folder.
        }
    }
}

//foreach (string file in files)
//{public string[] MyLines = System.IO.File.ReadAllLines(file); } 
    


            



     
      

       
    

