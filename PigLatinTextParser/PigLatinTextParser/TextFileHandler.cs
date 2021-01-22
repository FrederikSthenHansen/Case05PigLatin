using System;
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
using AODL.Document.TextDocuments;
using AODL.Document.Content;
using System.Xml.Linq;
using System.Linq;
using System.Text;

namespace PigLatinTextParser
{
    class TextFileHandler
    {
       private string _outputPath= new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName+ @"\OutputText\";
        
        TextParser myParser = new TextParser();

        #region General Properties
        private string[] RawTextArray = new string[] { "" };
        private string[] TreatedTextArray = new string[] { "" };
        private string TreatedText;
        private string _myFileType = "";
        private string _fileName = "";
        private bool _fileIsValid = true;
        #endregion

        #region PDF layout properties
        PdfDocumentBuilder builder = new PdfDocumentBuilder();

        #endregion

        #region ODT specific properties
        //TextDocument doc = new TextDocument();
        int loops = 0;
        #endregion

        public void ProcessInputFiles()
        {

            //Oceans of ".Parents" to reverse out of /bin/debug, where it defaults to for some reason. 
            string myPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName+@"\InputText";
            Console.WriteLine("processing all files in: " + myPath);
            string[] files = Directory.GetFiles(myPath);
            int filenr = 0;
            foreach (string file in files)
            {
                filenr++;
                //WritePigLatinFile(file, filenr, files.Length);
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
            TreatedTextArray = TreatedText.Split(". ");

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

        //TODO: improve this method to make its output more readable for the the method acting after it.
        private string[] formatOddFileLayout( string inputText)
        {
            
           // string lineChange = ". /r/n ";
            //inputText = inputText.Replace(". ", lineChange);

            //split text on every "."
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


       
        private async Task<string[]> readODT(string path)
        {
            string[] ret= new string[] {""};
            
            var sb = new StringBuilder();


            using (var doc = new TextDocument())
            {
               
                try
                {
                    doc.Load(path);

                    //Main content
                    var mainPart = doc.Content.Cast<IContent>();
                    var mainText = String.Join("\r\n", mainPart.Select(x => x.Node.InnerText));

                    //Append both text variables

                    sb.Append(mainText);
                    //}
                    string fullText = sb.ToString();
                    ret = formatOddFileLayout(fullText);
                }
                catch (System.IO.IOException)
                {
                    loops++;
                    Console.WriteLine(_fileName +" is waiting in que for the "+loops+" time...");
                    //wait 2 sec and try again
                    System.Threading.Thread.Sleep(4000);
                    /*try { */ret = Task.Run(()=> readODT(path)).Result;/* }*/
                    //catch (System.IO.IOException) { }
                }
                Console.WriteLine(_fileName+" has been read and the reader will now Dispose of itself");
                doc.Dispose();
            }
                //replace with odt text readout
              
            return ret;
        }

        

        private string[] readTXT(string path)
        {
     
            string[] ret = new string[] { "" };
            string[] lines = File.ReadAllLines(path);
            ret = lines;
            return ret;
        }

        
        private async Task<string[]> readFile(string path)
        {
            

            string[] ret = new string[] { "" };


            //Console.WriteLine("File is called "+_fileName+ ", and has the extension: " + _myFileType);
            Console.WriteLine("Now atttempting to read "+_fileName);
            //reset _fileisvalid
            _fileIsValid = true;

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
            else if (_myFileType == ".odt")
            {
                ret = await readODT(path);
            }
            else
            {
                Console.WriteLine("input file format "+_myFileType+" is not supported. Please only put valid text files in the input folder");
                Console.WriteLine("Currenly supported file formats are: .txt, .pdf, .odt and .docx ");
                _fileIsValid = false;
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

        public async Task<bool> WritePigLatinFile(string filePath, string output) 
        {
            //Console.WriteLine("Current outputpat is: " + _outputPath);
            _outputPath = output;
            string fullPath = filePath;
            Console.WriteLine();
            _myFileType = Path.GetExtension(filePath);
            _fileName = Path.GetFileName(filePath);
            Console.WriteLine("Currently processing: "+_fileName);
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
                    writePDFOutput();
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
    