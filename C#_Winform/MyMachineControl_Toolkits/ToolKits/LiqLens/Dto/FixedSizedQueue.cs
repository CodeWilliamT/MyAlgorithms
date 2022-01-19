using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace LensDriverController.Dto
{
    class FixedSizedQueue<T>
    {
        readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        
        public int Size { get; private set; }

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public FixedSizedQueue(int size)
        {
            Size = size;
        }

        public void Enqueue(T obj)
        {
            queue.Enqueue(obj);
            lock (this)
            {
                while (queue.Count > Size)
                {
                    T outObj;
                    queue.TryDequeue(out outObj);
                }
            }
        }

        public T Dequeue()
        {
            T outObj;

            lock (this)
            {
                if (queue.Count > 0)
                {
                    queue.TryDequeue(out outObj);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return outObj;
        }


    }
}
