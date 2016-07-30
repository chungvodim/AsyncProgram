using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDataStructures.ConcurrentDictionary
{
    public class ConcurrentDictionaryLazyCsvRepository
    {
        private readonly string directory;
        private ConcurrentDictionary<string, Lazy<List<string[]>>> csvFiles;
        public ConcurrentDictionaryLazyCsvRepository(string directory)
        {
            this.directory = directory;
            csvFiles = new ConcurrentDictionary<string, Lazy<List<string[]>>>();
        }
        public IEnumerable<string> Files
        {
            get { return new DirectoryInfo(directory).GetFiles().Select(fi => fi.FullName); }
        }

        public IEnumerable<T> Map<T>(string dataFile, Func<string[], T> map)
        {
            var csvFile = new Lazy<List<string[]>>(() => LoadData(dataFile).ToList());
            // GetOrAdd = TryGetValue + TryAdd
            csvFile = csvFiles.GetOrAdd(dataFile, csvFile);
            return csvFile.Value.Skip(1).Select(map);
        }
        //public IEnumerable<T> Map<T>(string dataFile, Func<string[], T> map)
        //{
        //    List<string[]> csvFile;
        //    if (!csvFiles.TryGetValue(dataFile, out csvFile))
        //    {
        //        csvFile = LoadData(dataFile).ToList();
        //        // TryAdd method, which will return false if the add fails due to the key already being present
        //        csvFiles.TryAdd(dataFile, csvFile);
        //    }
        //    return csvFile.Skip(1).Select(map);
        //}

        //public IEnumerable<T> Map1<T>(string dataFile, Func<string[], T> map)
        //{
        //    // GetOrAdd = TryGetValue + TryAdd
        //    List<string[]> csvFile = csvFiles.GetOrAdd(dataFile, df => LoadData(df).ToList());
        //    return csvFile.Skip(1).Select(map);
        //}

        private IEnumerable<string[]> LoadData(string filename)
        {
            using (var reader = new StreamReader(Path.Combine(directory, filename)))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine().Split(',');
                }
            }
        }
    }
}
