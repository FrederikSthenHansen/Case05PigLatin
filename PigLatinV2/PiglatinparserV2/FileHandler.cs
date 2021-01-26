using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;
using Page = UglyToad.PdfPig.Content.Page;
using PageSize = UglyToad.PdfPig.Content.PageSize;

namespace PiglatinparserV2
{
    class FileHandler
    {
        #region General Properties

        private string _outputPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + @"\OutputText\";

        TextParser myParser = new TextParser();

        private string[] RawTextArray = new string[] { "" };
        private string[] TreatedTextArray = new string[] { "" };
        private string TreatedText;
        private string _myFileType = "";
        private string _fileName = "";
        private bool _fileIsValid = true;

        private string[] _splitter = { ". " };
        #endregion

        private async Task<string[]> readFile(string path)
        {
            string[] ret = new string[] { "" };


            //Console.WriteLine("File is called "+_fileName+ ", and has the extension: " + _myFileType);
            Console.WriteLine("Now atttempting to read " + _fileName);
            //reset _fileisvalid
            _fileIsValid = true;

            //if (_myFileType == ".txt")
            //{
            //    ret = readDocx(path);
            //}
            if (_myFileType == ".pdf")
            {
                ret = readPDF(path);
            }
            else if (_myFileType == ".docx"|| _myFileType == ".odt"|| _myFileType == ".doc"|| _myFileType == ".txt")
            {
                ret = readDocx(path);
            }
            //else if (_myFileType == ".odt")
            //{
            //    ret = readDocx(path);
            //}
            else
            {
                Console.WriteLine("input file format " + _myFileType + " is not supported. Please only put valid text files in the input folder");
                Console.WriteLine("Currenly supported file formats are: .txt, .pdf, .odt and .docx ");
                _fileIsValid = false;
            }
            return ret;
        }

        private string[] formatOddFileLayout(string inputText)
        {

            // string lineChange = ". /r/n ";
            //inputText = inputText.Replace(". ", lineChange);
            //string[] splitter = { ". "};
            //split text on every "."
            string[] ret = inputText.Split(_splitter,StringSplitOptions.None);

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
                for (int page = 1; page < document.NumberOfPages; page++)
                {
                    #region Reading text for Linguistic logic purposes
                    Page myPage = document.GetPage(page);
                    string pageText = myPage.Text;
                    pageText = pageText.TrimStart();
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
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object pathObject = path;
            //object path2 = @"C:\Users\SA02- Frederik\Documents\Case05PigLatin\testInputFiles\InputText.txt";
            object readOnly = true;
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref pathObject, ref miss, ref readOnly, ref miss, ref miss,
                        ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            string totalText = "";      //the whole document
            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                //Determine the beginning of an entire paragraph and intercept the table name
                //Get the column name
                //......
                totalText += docs.Paragraphs[i + 1].Range.Text.ToString();
            }
            Console.Write(totalText);
            docs.Close();
            word.Quit();
            string[] ret;

            if (_myFileType != ".txt")
                {  ret = formatOddFileLayout(totalText); }
            else { ret = totalText.Split(_splitter, StringSplitOptions.None); }
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

        public async Task<bool> WritePigLatinFile(string filePath/*, string output*/)
        {
            string fullPath = filePath;
            _outputPath = filePath.Replace("InputText","OutputText");
            Console.WriteLine("Current output path is: " + _outputPath);
            
            Console.WriteLine();
            _myFileType = Path.GetExtension(filePath);
            _fileName = Path.GetFileName(filePath);
            Console.WriteLine("Currently processing: " + _fileName);
            //Console.WriteLine("path to intput number: " + counter + "/" + total + " is: "+fullPath);

            RawTextArray = await readFile(filePath);
            //int linetracker=0;
            //First we take in the string array (bunch of text lines) from readfiles
            Console.WriteLine();
            //Console.WriteLine("Printing the text as read raw from the input file:");

            if (_fileIsValid == true)
            {
                Console.WriteLine($"Parsing {_fileName} to PigLatin...");
                foreach (string line in RawTextArray)
                {
                    //Console.WriteLine(line);
                    //Strings are broken down into individual words
                    string[] words = myParser.BreakUpText(line);

                    TreatedTextArray = new string[words.Length];

                    for (int w = 0; w < words.Length; w++)
                    {   //All words in each line are turned to Pig latin.
                        words[w] = myParser.MakePigLatinWord(words[w]);

                        //write to console
                        //Console.WriteLine(words[w]);

                        //each line is reconstruced word by word
                        TreatedTextArray[w] = myParser.RebuildTextLine(words[w]);
                    }

                    // Console.WriteLine("Rebuilding the text of " + _fileName); ;
                    //Then all lines are added back together to reform the text.
                    TreatedText = TreatedText + myParser.RebuildWholeText(TreatedTextArray);


                }
                //the finished text is printed to console and written to outputFolder:
                //Console.WriteLine();
                //Console.WriteLine("Now Printing the parsed text:");
                //Console.WriteLine(TreatedText);
                //Console.WriteLine();

                #region Placeholder code to allow PDF to be converted to txt

                Console.WriteLine("printing " + _fileName + " to .txt file");
                _myFileType = ".txt";



                fullPath = filePath.Replace(@"\InputText\", @"\OutputText\");


                _fileName = _fileName.Replace(".pdf", _myFileType);
                _fileName = _fileName.Replace(".docx", _myFileType);
                _fileName = _fileName.Replace(".odt", _myFileType);
                _fileName = _fileName.Replace(".doc", _myFileType);
                fullPath = fullPath.Replace(".doc", _myFileType);
                fullPath = fullPath.Replace(".pdf", _myFileType);
                fullPath = fullPath.Replace(".docx", _myFileType);
                fullPath = fullPath.Replace(".odt", _myFileType);

                #endregion

                #region Make the file unique to prevent overwriting previous files in folder
                string newFileName = "";
                newFileName = makeFileUnique(fullPath);
                fullPath = fullPath.Replace(_fileName, newFileName);
                #endregion

                if (_myFileType == ".pdf")
                {
                    Console.WriteLine("Printing output to PDF file");
                    //writePDFOutput();
                }
                if (_myFileType == ".txt")
                {
                    Console.WriteLine($"Printing the parsed {_fileName} to TXT file");
                    File.WriteAllText(fullPath, TreatedText);
                }
            }
            //clean up
            TreatedText = "";
            TreatedTextArray = new string[] { "" };
            RawTextArray = new string[] { "" };
            _fileName = "";
            _myFileType = "";


            //return bool to tell caller if the process was succesfull.
            return _fileIsValid;
        }



    }
}
