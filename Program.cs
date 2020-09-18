using System;
using System.IO;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace SoDataDumpExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var failedFileReads = "Failed to read files:\n";
                var failedIndexes = "Failed because of IndexOutOfRangeException:\n";
                var failedIndexCount = 0;
                var failedFileReadCount = 0;
                var successfulExtractFileCount = 0;
                var successfulExtracts = "successfully extracted:\n";
                SharpCompress.Common.ExtractionOptions options = new ExtractionOptions
                {
                    Overwrite = true,
                    ExtractFullPath = true
                };
                Console.WriteLine("Start Time: {0}", DateTime.Now);
                try
                {
                    //structure: e:\stackexchange\<sitename>.{meta}.stackexchange.com.7z
                    Parallel.ForEach(Directory.GetFiles(args[0], "*.7z"), (file) =>
                    {
                        {
                            //write back to same directory; 7zip will create a directory matching the filename (minus the .7z)
                            //except for stackoverflow, serverfault, superuser, they're specially named.
                            var writePath = args[0];
                            try
                            {
                                ExtractZippedArchives(writePath, options, file);
                            }
                            catch (System.InvalidOperationException)
                            {
                                failedFileReads += String.Format("{0}\n", file);
                                failedFileReadCount++;
                            }
                            catch (System.IndexOutOfRangeException)
                            {
                                failedIndexes += String.Format("{0}\n", file);
                                failedIndexCount++;

                            }
                            successfulExtractFileCount++;
                        }
                    });
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("User does not have access.\n {0}\n", ex.ToString());
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine(
                        "No file path was typed in. Please invoke this EXE with a directory as the first argument. Viz: c:\\mydirectory\\n: {0}",
                        ex.ToString());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Incorrect file path.\n {0}\n", ex.ToString());
                }
                catch (System.IO.PathTooLongException ex)
                {
                    Console.WriteLine(
                        "Somehow the path you're trying to look for is too long. Consider saving the directory in a shorter name. Sorry. {0} \n",
                        ex.ToString());
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    Console.WriteLine("Couldn't find the path you're looking for. 404. Except those don't exist. Sooooo. {0} \n", ex);
                }
                Console.WriteLine("End Time: {0}", DateTime.Now);
                Console.WriteLine("Results:\n");
                Console.WriteLine("Successful: {0}, Failed To Read: {1}, Failed to Extract (IndexOutOfRangeException): {2}\n", successfulExtractFileCount, failedFileReadCount, failedIndexCount );
                Console.WriteLine("Failures:");
                Console.WriteLine(failedFileReads);
                Console.WriteLine("----------");
                Console.WriteLine("Index Out of bounds:");
                Console.WriteLine(failedIndexes);

            }
        }

        private static void ExtractZippedArchives(string toWritePath, ExtractionOptions options, string file)
        {
            var archive = ArchiveFactory.Open(File.OpenRead(file));
            foreach (var entry in archive.Entries)
            {

                var directoryToWriteTo = toWritePath;
                entry.WriteToDirectory(Path.Combine(directoryToWriteTo, file.Replace(".7z", "")), options);
            }
        }
    }
}
