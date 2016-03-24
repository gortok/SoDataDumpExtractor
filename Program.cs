using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCompress.Archive;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace XmlParser
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
                try
                {
                    foreach (var file in Directory.GetFiles(args[0], "*.7z"))
                    {
                        
                        try
                        {
                            var archive = ArchiveFactory.Open(File.OpenRead(file));
                            foreach (var entry in archive.Entries)
                            {

                                var directoryToWriteTo = args[0];
                                entry.WriteToDirectory(Path.Combine(directoryToWriteTo, file.Replace(".7z", "")),
                                    ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                                successfulExtracts += String.Format("{0}: {1}\n", file, entry.Key);
                                
                            }
                        }
                        catch (System.InvalidOperationException ex)
                        {

                            failedFileReads += String.Format("{0}\n", file);
                            failedFileReadCount++;
                        }
                        catch (System.IndexOutOfRangeException ex)
                        {
                            failedIndexes += String.Format("{0}\n", file);
                            failedIndexCount++;

                        }
                        successfulExtractFileCount++;
                    }
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
                Console.WriteLine("Results:\n");
                Console.WriteLine("Successful: {0}, Failed To Read: {1}, Failed to Extract (IndexOutOfRangeException): {2}\n", failedFileReadCount, failedIndexCount, successfulExtractFileCount);
                Console.WriteLine("Files Successfully extracted: {0}", successfulExtracts);
                Console.WriteLine("Failures:");
                Console.WriteLine(failedFileReads);
                Console.WriteLine("----------");
                Console.WriteLine("Index Out of bounds\n");
                Console.WriteLine(failedIndexes);
                
            }
        }
    }
}
