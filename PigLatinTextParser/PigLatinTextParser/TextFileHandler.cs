using System;
using System.IO;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;
using Microsoft.Office.Interop.Word;
//using Microsoft.PowerBI.Api.Models;
using Page = UglyToad.PdfPig.Content.Page;

namespace PigLatinTextParser
{
    class TextFileHandler
    { const string _outputPath = @"C:\Users\SA02- Frederik\Documents\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\OutputText\OutputText";
        TextParser myParser = new TextParser();

        #region General Properties
        private string[] RawTextArray = new string[] { "" };
        private string[] TreatedTextArray = new string[] { "" };
        private string TreatedText;
        private string _myFileType = "";
        #endregion

        #region PDF layout properties
        PdfDocument pigLatinPDF;
        PdfDocumentBuilder builder = new PdfDocumentBuilder();

        #endregion

        private void writePDFOutput()
        {
            

            PdfPageBuilder page = builder.AddPage(PageSize.A4);

            // Fonts must be registered with the document builder prior to use to prevent duplication.
            PdfDocumentBuilder.AddedFont font = builder.AddStandard14Font(Standard14Font.Helvetica);
            
            //setting starting poin to write from.
            PdfPoint point = new PdfPoint(25, 700);
            //page.AddText(TreatedText, 12, point, font); 
            TreatedText = TreatedText.Replace(":", ".");
            TreatedTextArray = TreatedText.Split(".");

            page.AddText($"This is a Placeholder! {TreatedTextArray[0]}  This is a placeholder.", 12, point, font);
            point.MoveY(14);
            //for (int line = 0; line < TreatedTextArray.Length; line++)
            //{
            //    try { page.AddText(TreatedTextArray[line], 12, point, font); }
            //    catch (InvalidOperationException)
            //    {

            //    }
            //    point.MoveY(14);
            //}



            byte[] documentBytes = builder.Build();

            File.WriteAllBytes(_outputPath+_myFileType, documentBytes);
        }

        private string[] readPDF(string path)
        {
            
            using (PdfDocument document = PdfDocument.Open(path))
            {
                string[] ret = new string[document.NumberOfPages];
                for (int page=1; page<document.NumberOfPages;page++)
                {
                    #region Reading text for Linguistic logic purposes
                    Page myPage = document.GetPage(page);
                    string pageText = myPage.Text;
                    pageText= pageText.TrimStart();
                    pageText = pageText.TrimEnd();
                    
                    //layout edits
                    ret = pageText.Split(". ");
                    for (int line = 0; line < ret.Length; line++)
                    {
                        ret[line]=ret[line]+".";
                    }
                    #endregion

                    #region Reading Letters for layout purposes
                    //pigLatinPDF = new PdfDocument(document.NumberOfPages);
                   //pigLatinPDF.GetPage(page) readPDFLayout(myPage, document.NumberOfPages);
                    #endregion
                }
                return ret;
            }
            
        }

         private async System.Threading.Tasks.Task readPDFLayout(Page input, int pageCount)
        {
            foreach(Letter letter in input.Letters)
            {
              //  await { }
            }
        }

        private string[] readTXT(string path)
        {
     
            string[] ret = new string[] { "" };
            string[] lines = File.ReadAllLines(path);
            ret = lines;
            return ret;
        }

        //Needs Refactoring to accept docx format and other formats
        public string[] ReadFiles(string path)
        {
            string[] ret = new string[] { "" };
            
           _myFileType = Path.GetExtension(path);
            Console.WriteLine("File has the extension: " + _myFileType);

            if (_myFileType==".txt") 
            {
                ret = readTXT(path);
            }
            else if (_myFileType==".pdf") 
            {
                ret = readPDF(path);
            }
            else if (_myFileType == ".docx")
            {
                //Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                //Console.WriteLine(app.Path);
                //Document doc = app.Documents.Open(path);

                //ret = doc.Content.Text.Split(".");
                //for (int line = 0; line < ret.Length; line++)
                //{
                //    ret[line] = ret[line] + ".";
                //}
            }
            return ret;
        }

        public void WritePigLatinFile(string filePath) 
        {
            RawTextArray = ReadFiles(filePath);
            //int linetracker=0;
            //First we take in the string array (bunch of text lines) from readfiles
            Console.WriteLine();
            Console.WriteLine("Printing the text as read raw from the input file:");
            foreach (string line in RawTextArray)
            {
                Console.WriteLine(line);
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
            //the finished text is printed to console and written to outputFolder:
            Console.WriteLine();
            Console.WriteLine("Now Printing the parsed text:");
            Console.WriteLine(TreatedText);
            Console.WriteLine();


            //Placeholdercode to allow PDF to be converted to txt
            _myFileType = ".txt";
            ///end placeholder

            //Todo: make PDF writing functionality
            if (_myFileType == ".pdf")
            {
                Console.WriteLine("Printing output to PDF file");
                writePDFOutput();
            }
            if (_myFileType == ".txt")
            {
                Console.WriteLine("Printing output to TXT file");
                 File.WriteAllText(_outputPath+_myFileType, TreatedText);
            }
            //clean up
            TreatedText = "";
            TreatedTextArray = new string[] { "" }; 
        }
    }
}

//foreach (string file in files)
//{public string[] MyLines = System.IO.File.ReadAllLines(file); } 
    


            



     
      

       
    

