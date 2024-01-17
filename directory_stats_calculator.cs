using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Project1
{
    /// <summary>
    /// users can recursively calculate the number of files, folders, and file size in parallel or sequential order. 
    /// </summary>
    class Program
    {
        private string dirPath;
        private static int fileCount;
        private static int folderCount;
        private static long byteSize;
        
        /// <summary>
        /// Recursively calculates the fileCount, folderCount and the total space occupied by a directory and its sub
        /// directories that is s a param to the method in a sequential manner.  
        /// </summary>
        /// <param name="dirPath">The absolute path to the directory for which we want to get count and size information.</param>
        private void Sequential(string dirPath)
        {
            try
            {
                var files = Directory.GetFiles(dirPath);
                folderCount += Directory.GetDirectories(dirPath).Length;
                // incrementing the file count by the total files in the current directory. 
                fileCount += files.Length;
                
                foreach (var d in files)
                {
                    var f = new FileInfo(d);
                
                    var fileSize = Convert.ToInt32(f.Length);
                    // Incrementing the byte size by each file
                    byteSize += fileSize;
                }
                
                foreach (var d in Directory.GetDirectories(dirPath))
                {
                    Sequential(d);
                }
            }
            catch
            {
                //do nothing
            }

        }

        /// <summary>
        /// recursively calculates the fileCount, folderCount and the total space occupied by a directory and its sub
        /// directories. creates new threads for each recursive call.
        /// </summary>
        /// <param name="dirPath">The absolute path to the directory for which we want to get count and size
        /// information.</param>
        private void ParallelCount(string dirPath)
        {
            try
            {
                var files = Directory.GetFiles(dirPath);
 
                Interlocked.Add(ref folderCount, Directory.GetDirectories(dirPath).Length);

                Interlocked.Add(ref fileCount, files.Length);
                
                
                Parallel.ForEach(files, d =>
                {
                    var f = new FileInfo(d);

                    var fileSize = Convert.ToInt64(f.Length);
                    Interlocked.Add(ref byteSize, fileSize);
                });


                Parallel.ForEach(Directory.GetDirectories(dirPath), d => { ParallelCount(d); });

            }
            catch
            {
                //do nothing
            }

        }
        /// <summary>
        /// Main method that controls flow of application and allows user to print out the results of the variables. 
        /// </summary>
        /// <param name="args">Command line arguments that state parallel or sequenital and also specify the 
        /// directory path.</param>
        static void Main(string[] args)
        {

            fileCount = 0;
            folderCount = 0;
            byteSize = 0;

            var timer = new Stopwatch();
            var timer1 = new Stopwatch();

            string dirPath = args[1];
            string iterateChoice = args[0];

            Program p1 = new Program();
            
            Console.WriteLine("Directory: '{0}'", dirPath);
            Console.WriteLine("");

            if (iterateChoice == "-s")
            {
                timer.Start();
                p1.Sequential(dirPath);
                timer.Stop();
                Console.WriteLine("Sequential calculated in: {0}m {1}s", timer.Elapsed.Minutes, timer.Elapsed.Seconds);
                Console.WriteLine("{0} folders, {1} files, {2} bytes", folderCount, fileCount, byteSize);

            }
            else if (iterateChoice == "-p")
            {
                timer1.Start();
                p1.ParallelCount(dirPath);
                timer1.Stop();
                Console.WriteLine("Parallel calculated in: {0}m {1}s ", timer1.Elapsed.Minutes, timer1.Elapsed.Seconds);
                Console.WriteLine("{0} folders, {1} files, {2} bytes", folderCount, fileCount, byteSize);
            }
            else if (iterateChoice == "-b")
            {
                timer.Start();
                p1.ParallelCount(dirPath);
                timer.Stop();
                Console.WriteLine("Parallel Calculated in: {0}m {1}s ", timer.Elapsed.Minutes, timer.Elapsed.Seconds);
                Console.WriteLine("{0} folders, {1} files, {2} bytes", folderCount, fileCount, byteSize);

                Console.WriteLine("");
                fileCount = 0;
                folderCount = 0;
                byteSize = 0;
                timer1.Start();
                p1.Sequential(dirPath);
                timer1.Stop();
                Console.WriteLine("Sequential Calculated in: {0}m {1}s", timer1.Elapsed.Minutes, timer1.Elapsed.Seconds);

                Console.WriteLine("{0} folders, {1} files, {2} bytes", folderCount, fileCount, byteSize);
            }
            else
            {
                Console.WriteLine("Invalid iteration choice.");
                Console.WriteLine("You MUST specify one of the parameters: -s, -p, or -b");
                Console.WriteLine("-s:      Run in single threaded mode.");
                Console.WriteLine("-p:      Run in parallel mode.");
                Console.WriteLine("-b:       Run in parallel followed by sequential mode.");
            }
        }

    }
}
