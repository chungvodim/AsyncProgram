using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDataStructures
{
    public class CsvRepository
    {
        private readonly string directory;
        private Dictionary<string, List<string[]>> csvFiles;
        public CsvRepository(string directory)
        {
            this.directory = directory;
            //csvFiles = new DirectoryInfo(directory).GetFiles("*.csv").ToDictionary(f => f.Name, f => LoadData(f.FullName).ToList());
            csvFiles = new DirectoryInfo(directory).GetFiles("*.csv").ToDictionary<FileInfo, string,List<string[]>>(f => f.Name, f => null);
        }
        public IEnumerable<string> Files { get { return csvFiles.Keys; } }
        public IEnumerable<T> Map<T>(string dataFile, Func<string[], T> map)
        {
            //return csvFiles[dataFile].Skip(1).Select(map);
            return LazyLoadData(dataFile).Skip(1).Select(map);
        }

        private IEnumerable<string[]> LoadData(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                { yield return reader.ReadLine().Split(','); }
            }
        }

        private IEnumerable<string[]> LazyLoadData(string filename)
        {
            List<string[]> csvFile = null;
            lock (csvFiles) //bottle neck
            {
                csvFile = csvFiles[filename];
            }
            if (csvFile == null)
            {
                // Two threads could be loading the same csv
                csvFile = LoadData(Path.Combine(directory, filename)).ToList();
                lock (csvFiles)
                {
                    csvFiles[filename] = csvFile;
                }
            }
            return csvFile;
        }
    }
}
