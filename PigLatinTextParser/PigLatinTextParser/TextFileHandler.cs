using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace PigLatinTextParser
{
    class TextFileHandler
    {   const string outputPath = @"C:\Users\SA02- Frederik\Documents\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\OutputText\OutputText.txt"; 
        TextParser myParser = new TextParser();
        
        private string[] RawTextArray = new string[] { "" };
        private string[] TreatedTextArray= new string[] {""};
        private string TreatedText;

        private string[] readPDF(string path)
        {
            
            using (PdfDocument document = PdfDocument.Open(path))
            {
                string[] ret = new string[document.NumberOfPages];
                for (int page=1; page<document.NumberOfPages;page++)
                {
                    Page myPage= document.GetPage(page);
                    string pageText = myPage.Text;

                    ret[page - 1] = pageText;
                }
                return ret;
            }
            
        }

        private string[] readTXT(string path)
        {
     
            string[] ret = new string[] { "" };
            string[] lines = File.ReadAllLines(path);
            return ret;
        }

        //Needs Refactoring to accept docx format and other formats
        public string[] ReadFiles(string path)
        {
            string[] ret = new string[] { "" };
            
            string myFileType = Path.GetExtension(path);
            Console.WriteLine("File has the extension: " + myFileType);

            if (myFileType==".txt") 
            {
                ret = readTXT(path);
            }
            else if (myFileType==".pdf") 
            {
                ret = readPDF(path);
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
            File.WriteAllText(outputPath, TreatedText);
            //TODO: write finished text to new .TXT file in output folder.
        }
    }
}

//foreach (string file in files)
//{public string[] MyLines = System.IO.File.ReadAllLines(file); } 
    


            



     
      

       
    

