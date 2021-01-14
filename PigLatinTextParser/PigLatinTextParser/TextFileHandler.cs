﻿using System;
using System.IO;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;
using Page = UglyToad.PdfPig.Content.Page;
using PageSize=UglyToad.PdfPig.Content.PageSize;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace PigLatinTextParser
{
    class TextFileHandler
    { const string _outputPath = @"C:\Users\SA02- Frederik\Documents\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\OutputText\";
        TextParser myParser = new TextParser();

        #region General Properties
        private string[] RawTextArray = new string[] { "" };
        private string[] TreatedTextArray = new string[] { "" };
        private string TreatedText;
        private string _myFileType = "";
        private string _fileName = "";
        #endregion

        #region PDF layout properties
        PdfDocumentBuilder builder = new PdfDocumentBuilder();

        #endregion

        public void ProcessInputFiles()
        {

            //Oceans of ".Parents" to reverse out of /bin/debug, where it defaults to for some reason. 
            string myPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName+@"\InputText";
            Console.WriteLine("processing all files in: " + myPath);
            string[] files = Directory.GetFiles(myPath);
            
            foreach (string file in files)
            {
                WritePigLatinFile(file);
                Console.WriteLine("Parsing of file complete: Press any key to continue parsing the next file.");
                Console.ReadKey();
            }

        }

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

        private string[] formatOddFileLayout( string inputText)
        {
            string[] ret = inputText.Split(". ");
            for (int line = 0; line < ret.Length; line++)
            {
                ret[line] = ret[line] + ".";
            }
            return ret;
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
                    ret = formatOddFileLayout(pageText);
                    #endregion

                    #region Reading Letters for layout purposes
                    //pigLatinPDF = new PdfDocument(document.NumberOfPages);
                   //pigLatinPDF.GetPage(page) readPDFLayout(myPage, document.NumberOfPages);
                    #endregion
                }
                
                return ret;
            }
            
        }

        private string[] readDocx(string path)
        {
            string totalText = "";
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
            {
                DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;
                totalText = body.InnerText;   
            }
            string[] ret = formatOddFileLayout(totalText);
            return ret;
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
        private string[] readFile(string path)
        {

            string[] ret = new string[] { "" };
            
           _myFileType = Path.GetExtension(path);
            _fileName = Path.GetFileName(path);
            Console.WriteLine("File is called "+_fileName+ ", and has the extension: " + _myFileType);

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
                ret = readDocx(path);
            }
            else
            {
                Console.WriteLine("input file format "+_myFileType+" cannot be supported. Please only put valid text files in the input folder.");
            }
            return ret;
        }

        private string makeFileUnique(string path)
        {
            string newFileName = "";
            //Check if a file alreaddy exists with this name.
            if (File.Exists(path))
            {//if it does, we need to edit the name of the new output file by adding a timestamp to make it unique.
                newFileName = _fileName.Replace(_myFileType, $"-{DateTime.Now.Ticks}" + _myFileType);
            }
            else
            {
                newFileName = _fileName;
            }
            return newFileName;
        }

        public void WritePigLatinFile(string filePath) 
        {
            string fullPath = filePath;
            Console.WriteLine();
            Console.WriteLine("path to intput is: "+fullPath);
            RawTextArray = readFile(fullPath);
            //int linetracker=0;
            //First we take in the string array (bunch of text lines) from readfiles
            Console.WriteLine();
            //Console.WriteLine("Printing the text as read raw from the input file:");
            foreach (string line in RawTextArray)
            {
                //Console.WriteLine(line);
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
            //Console.WriteLine();
            //Console.WriteLine("Now Printing the parsed text:");
            //Console.WriteLine(TreatedText);
            //Console.WriteLine();

            #region Placeholdercode to allow PDF to be converted to txt

            Console.WriteLine("printing "+_fileName+" to .txt file");
            _myFileType = ".txt";

            #endregion

            fullPath = filePath.Replace(@"\InputText\", @"\OutputText\");

            #region more pdf/docx to txt placeholder code
            _fileName = _fileName.Replace(".pdf", _myFileType);
            _fileName = _fileName.Replace(".docx", _myFileType);
            fullPath = fullPath.Replace(".pdf", _myFileType);
            fullPath = fullPath.Replace(".docx", _myFileType);
            #endregion

            #region Make the file unique to prevent overwriting previous files in folder
            string newFileName ="";
            newFileName = makeFileUnique(fullPath);
            fullPath = fullPath.Replace(_fileName, newFileName);
            #endregion

            if (_myFileType == ".pdf")
            {
                Console.WriteLine("Printing output to PDF file");
                writePDFOutput();
            }
            if (_myFileType == ".txt")
            {
                Console.WriteLine("Printing output to TXT file");
                File.WriteAllText(fullPath, TreatedText);
            }
            //clean up
            TreatedText = "";
            TreatedTextArray = new string[] { "" };
            RawTextArray = new string[] { "" };
            _fileName = "";
            _myFileType = "";
        }


    }
}
    


            



     
      

       
    

