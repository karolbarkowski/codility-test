using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodilityTest
{
    internal class FileData
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
    }

    class Program
    {
        private static void Main()
        {
            var input = File.ReadAllText("input.txt");

            var inputFilesUnsorted = ParseInput(input);

            //part 1- group by location
            var groupedByLocation = inputFilesUnsorted.OrderBy(f => f.Date).GroupBy(f => f.Location);

            //part 2 - rename files in groups using ordinal numbers
            var filesRenamed = new List<FileData>();

            foreach (var location in groupedByLocation)
            {
                var locationFiles = location.ToList();
                for (var i = 0; i < locationFiles.Count; i++)
                {
                    var counter = BuildCounterString(location.Count().ToString().Length, i);

                    filesRenamed.Add(new FileData()
                    {
                        Name = $"{counter}{Path.GetExtension(locationFiles[i].Name)}",
                        Date = locationFiles[i].Date,
                        Location = locationFiles[i].Location,
                        Guid = locationFiles[i].Guid
                    });
                }
            }

            //part 3 - iterate over input array, match files with renamed versions and build result file
            StringBuilder result = new StringBuilder();
            foreach (var fileData in inputFilesUnsorted)
            {
                var renamedFile = filesRenamed.First(f => f.Guid == fileData.Guid);

                result
                    .Append(renamedFile.Location)
                    .Append(renamedFile.Name)
                    .Append(Environment.NewLine);
            }

            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static StringBuilder BuildCounterString(int counterLength, int i)
        {
            var counter = new StringBuilder();
            counter.Append(i + 1);
            while (counter.Length < counterLength)
            {
                counter.Insert(0, "0");
            }

            return counter;
        }

        private static List<FileData> ParseInput(string input)
        {
            var lines = input.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var inputFilesUnsorted = new List<FileData>();
            foreach (var line in lines)
            {
                var lineElements = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                inputFilesUnsorted.Add(new FileData()
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = lineElements[0].Trim(),
                    Location = lineElements[1].Trim(),
                    Date = DateTime.Parse(lineElements[2])
                });
            }

            return inputFilesUnsorted;
        }
    }
}
