using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace PiglatinparserV2
{
    class Program
    {
        private static MemoryCache FilesToProcess = new MemoryCache("myCache", default);
        static void Main(string[] args)
        {

            //Set path to watch for changes
            var directoryToWatch = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\InputText";



            if (!Directory.Exists(directoryToWatch))
            {
                Console.WriteLine($"ERROR: {directoryToWatch} does not exist");
            }
            else
            {
                Console.WriteLine($"Watching directory {directoryToWatch} for changes");
                Console.WriteLine("You can now drop a COPY of the file you want Pig-latinized into the the watched directory.");
                Console.WriteLine("The App will check for new files to process at 20 second intervals");
                Console.WriteLine("!BE AWARE THAT ANY FILES PROCESSED WILL BE DELETED FROM THE WATCHED DIRECTORY!.");

                using (var inputFileWatcher = new FileSystemWatcher(directoryToWatch))
                {
                    inputFileWatcher.IncludeSubdirectories = false;
                    inputFileWatcher.InternalBufferSize = 32768; // 32 KB
                    //inputFileWatcher.Filter = "*.*";
                    inputFileWatcher.NotifyFilter = NotifyFilters.FileName;

                    inputFileWatcher.Created += FileCreated;

                    inputFileWatcher.Deleted += FileDeleted;

                    inputFileWatcher.Error += WatcherError;

                    inputFileWatcher.EnableRaisingEvents = true;

                    Console.WriteLine("Press enter at any time to quit the App.");
                    Console.ReadLine();
                }


            }
            Console.ReadLine();

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
            /*string outputDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + @"\OutputText\"*/
            ;
            Console.WriteLine($"* Cache item removed: {fileName} because {args.RemovedReason}");



            if (args.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var fileProcessor = new FileHandler();

                //call the parsing method and get a bool for succes or failure
                bool mybool = await fileProcessor.WritePigLatinFile(args.CacheItem.Key/*, outputDirectory*/);
                if (mybool == true)
                {
                    Console.WriteLine($"Processing of {fileName} completed succesfully! Deleting it from the input folder...");
                    Console.WriteLine("The parsed text File can be found in the 'OutputText' folder in this application.");
                    Console.WriteLine();
                }
                else { Console.WriteLine($"Processing of {fileName} Failed, as the input file is invalid!"); }


            }

            else
            {
                Console.WriteLine($"WARNING: {args.CacheItem.Key} was removed unexpectedly and cannot be processed for the following reason: {args.RemovedReason}");
               // Console.WriteLine("Please remove the file in question from the input folder");
                Console.WriteLine();
            }
            Console.WriteLine($"Deleting {fileName} from the input folder...");
            File.Delete(args.CacheItem.Key);
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"* File created: {e.Name} - type: {e.ChangeType}");

            //Add file to a cache, so it can be processed
            AddToCache(e.FullPath);
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"* File deleted: {e.Name} - type: {e.ChangeType}");

        }

        private static void WatcherError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"ERROR: file system watching may no longer be active: {e.GetException()}");
        }

    }
}
