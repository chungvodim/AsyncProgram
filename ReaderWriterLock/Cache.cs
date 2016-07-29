using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{
    public class Cache
    {
        private readonly List<NewsItem> items = new List<NewsItem>();
        ReaderWriterLockSlim guard = new ReaderWriterLockSlim();
        public IEnumerable<NewsItem> GetNews(string tag)
        {
            guard.EnterReadLock();
            try
            {
                return items.Where(ni => ni.Tags.Equals(tag)).ToList();
            }
            finally
            {
                guard.ExitReadLock();
            }
        }
        public void AddNewsItem(NewsItem item)
        {
            guard.EnterWriteLock();
            try
            {
                items.Add(item);
            }
            finally
            {
                guard.ExitWriteLock();
            }
        }
    }

}
