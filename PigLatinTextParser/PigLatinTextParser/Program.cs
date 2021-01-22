using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Runtime.Caching;
using MemoryCache = System.Runtime.Caching.MemoryCache;

namespace PigLatinTextParser
{
    class Program
    {

        private static MemoryCache FilesToProcess = new MemoryCache("myCache", default);
        
        //private const string myPlaceHolder = "This is a 'placeholder text', to make my piglatin parser. /r/n It acts as a placeholder for a fully parsed text of some sort /r /n This specific placeholder i 3 lines long";
        static void Main(string[] args)
        {
            
            //Console.WriteLine("Name of my chache is: " +FilesToProcess.Name);

            TextFileHandler myTextFileHandler = new TextFileHandler();


            //Set path to watch for changes
            var directoryToWatch = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + @"\InputText";

            if (!Directory.Exists(directoryToWatch))
            {
                Console.WriteLine($"ERROR: {directoryToWatch} does not exist");
            }
            else
            {
                Console.WriteLine($"Watching directory {directoryToWatch} for changes");
                Console.WriteLine("You can now drop A COPY of the file you want Pig-latinized into the the watched directory.");
                Console.WriteLine("!BE AWARE THAT ANY FILES PROCESSED WILL BE DELETED FROM THE WATCHED DIRECTORY!.");

                using (var inputFileWatcher = new FileSystemWatcher(directoryToWatch))
                {
                    inputFileWatcher.IncludeSubdirectories = false;
                    inputFileWatcher.InternalBufferSize = 32768; // 32 KB
                    //inputFileWatcher.Filter = "*.*";
                    inputFileWatcher.NotifyFilter = NotifyFilters.FileName;

                    inputFileWatcher.Created += FileCreated;
                   
                  //  inputFileWatcher.Changed += FileChanged;
                    inputFileWatcher.Deleted += FileDeleted;
                    inputFileWatcher.Renamed += FileRenamed;
                    inputFileWatcher.Error += WatcherError;

                    inputFileWatcher.EnableRaisingEvents = true;

                    Console.WriteLine("Press enter to quit.");
                    Console.ReadLine();
                }
            }

            //myTextFileHandler.ProcessInputFiles();
            //Console.WriteLine();
            //Console.WriteLine("Parsing of all input files complete: Press any key to close the App");
            //Console.ReadKey();
        }
        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File created: {e.Name} - type: {e.ChangeType}");
            
            //Add file to a cache, so it can be processed
            AddToCache(e.FullPath);
        }

        private static void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath, fullPath);

            var policy = new CacheItemPolicy
            {
                RemovedCallback = ProcessMyFile,
                SlidingExpiration = TimeSpan.FromSeconds(2),
            };

            FilesToProcess.Add(item, policy);
        }
        private static void ProcessMyFile(CacheEntryRemovedArguments args)
        {
            ProcessMyFileAsync(args);
        }

        private static async Task ProcessMyFileAsync(CacheEntryRemovedArguments args)
        {
            string fileName = new DirectoryInfo(args.CacheItem.Key).Name;
            WriteLine($"* Cache item removed: {fileName} because {args.RemovedReason}");

            
  
            if (args.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var fileProcessor = new TextFileHandler();

                //call the parsing method and get a bool for succes or failure
                bool mybool = await fileProcessor.WritePigLatinFile(args.CacheItem.Key, 1, 1);
                    if (mybool == true)
                    {
                        Console.WriteLine($"Processing of {fileName} completed succesfully! Deleting it from the input folder...");
                        Console.WriteLine("The parsed text File can be found in the 'OutputText' folder in this application.");
                        Console.WriteLine();
                    }
                    else { Console.WriteLine($"Processing of {fileName} Failed, as the input file is invalid! Deleting it from the input folder... "); }
                
                
            }
        
            else
            {
                WriteLine($"WARNING: {args.CacheItem.Key} was removed unexpectedly and may not be processed because {args.RemovedReason}");
                Console.WriteLine("Please remove the file in question from the input folder");
                Console.WriteLine();
            }
            File.Delete(args.CacheItem.Key);
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File deleted: {e.Name} - type: {e.ChangeType}");
            
        }

        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            WriteLine($"* File renamed: {e.OldName} to {e.Name} - type: {e.ChangeType}");
        }

        private static void WatcherError(object sender, ErrorEventArgs e)
        {
            WriteLine($"ERROR: file system watching may no longer be active: {e.GetException()}");
        }
    }
}
 