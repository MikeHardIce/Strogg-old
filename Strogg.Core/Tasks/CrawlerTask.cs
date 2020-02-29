

using System.Collections.Generic;
using System.Linq;
using Huh.Core.Data;
using Huh.Core.Tasks;

namespace Strogg.Core.Tasks
{
    public class CrawlerTask : ITask
    {
        public Queue<string> KeyWord { get; set; } = new Queue<string>();

        public string SearchTerm { get; set; }

        public string Url { get; set; }

        public string OriginUrl { get; set; }
        public long Priority { get; set; } = 0;
        public IList<IData<string>> Data { get; set; }

        public CrawlerTask ()
            => Data = new List<IData<string>>();

        public object Clone()
        {
            var task = new CrawlerTask{
                SearchTerm    = SearchTerm
                , Url           = Url
                , OriginUrl     = OriginUrl
                , Priority      = Priority
            };

            CopyQueue(KeyWord, task.KeyWord);

            Data.Select(m => new SimpleData{
                ContentHint     = m.ContentHint
                , ContentType   = m.ContentType
                , Data  = m.Data
                , Key   = m.Key
            }).ToList().ForEach(m => task.Data.Add(m));

            return task;
        }

        public static void CopyQueue (Queue<string> from, Queue<string> to)
        {
            while(from.Count > 0)
                to.Enqueue(from.Dequeue());
        }
    }
}