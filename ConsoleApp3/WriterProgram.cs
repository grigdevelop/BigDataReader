using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class WriterProgram
    {
        public const string FPath = @"C:\BigData\data.txt";
        public const string DIRECTORY = @"C:\BigData\";

        string[] words = new[] { "tralala", "hohoho", "Vardan", "Ruben", "Armen", "kokordilos", "mknik", "whyyoudothis", "neveragain", "nevermind" };

        Random rd = new Random();

        public static void Run()
        {
            Console.WriteLine("Starting creating file...");
            new WriterProgram().Start();
            Console.WriteLine("File successfully created.");
        }

        public void Start()
        {
            File.Delete(FPath);
            List<Task> tasks = new List<Task>();
            for(int i = 0; i < 55; i++)
            {
                var streamNumber = i;
                tasks.Add(new Task(() => CreateFile(streamNumber)));
            }
            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());
            CombineMultipleFilesIntoSingleFile(DIRECTORY, "s*", FPath);
        }

        private static void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string inputFileNamePattern, string outputFilePath)
        {
            string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath, inputFileNamePattern);
            Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);
            using (var outputStream = File.Create(outputFilePath))
            {
                foreach (var inputFilePath in inputFilePaths)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        // Buffer size can be passed as the second argument.
                        inputStream.CopyTo(outputStream);
                    }
                    //Console.WriteLine("The file {0} has been processed.", inputFilePath);
                    File.Delete(inputFilePath);
                }
            }
        }

        void CreateFile(int streamNumber)
        {
            StreamAccess(stream =>
            {
                for (int i = 0; i < 200000; i++)
                {
                    string line = string.Format("{0} | {1}", RandomNumber(), RandomWords());
                    stream.WriteLine(line);
                }

            }, streamNumber);
        }

        void StreamAccess(Action<StreamWriter> swLoop, int streamNumber)
        {
            using (StreamWriter sw = new StreamWriter(DIRECTORY + "stream_" + streamNumber + ".txt"))
            {
                swLoop(sw);
            }
        }

        string RandomWords()
        {
            int count = rd.Next(0, 9);
            return string.Join(" ", words.OrderBy(w => rd.Next()));
        }

        int RandomNumber()
        {
            return rd.Next();
        }
    }
}
