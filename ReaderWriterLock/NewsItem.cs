using System.Collections.Generic;

namespace ReaderWriterLock
{
    public class NewsItem
    {
        public NewsItem(string tags)
        {
            this.Tags = tags;
        }
        public string Tags { get; set; }
    }
}