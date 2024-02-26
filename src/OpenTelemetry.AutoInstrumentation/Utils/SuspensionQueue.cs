using System.Collections.Generic;
using System.Threading;

namespace WhaTap.Trace.Utils
{
    public class SuspensionQueue<T>
    {
        public void Enqueue(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);
                Monitor.Pulse(_lock);
            }
        }

        public T Dequeue()
        {
            lock (_lock)
            {
                while (_queue.Count == 0)
                {
                    Monitor.Wait(_lock);
                }

                return _queue.Dequeue();
            }
        }

        private readonly object _lock = new object();
        private readonly Queue<T> _queue = new Queue<T>();
    }
}
