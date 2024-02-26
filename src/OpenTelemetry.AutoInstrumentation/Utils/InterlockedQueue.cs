using System;
using System.Collections.Generic;
using System.Threading;

namespace WhaTap.Trace.Utils
{
    class Node<T>
    {
        public Node(T value)
        {
            Value = value;
        }

        public T Value;
        public Node<T> Previous = null;
    }

    public class InterlockedQueue<T>
    {
        public bool Enqueue(T value)
        {
            var newNode = new Node<T>(value);
            var tail = _tail;

            newNode.Previous = tail;

            var previous = Interlocked.CompareExchange(ref _tail, newNode, tail);

            return previous == newNode.Previous;
        }

        public bool TryEnqueue(T value, int tryCount)
        {
            while (tryCount-- > 0)
            {
                if (Enqueue(value)) return true;
            }
            return false;
        }

        public void EnqueueUntilSuccess(T value)
        {
            while (!Enqueue(value))
            {
                // do nothing
            }
        }

        public List<T> Get()
        {
            var list = new List<T>();
            var stack = new Stack<T>();
            var tail = Interlocked.Exchange(ref _tail, null);
            Node<T> prev = null;

            while (tail != null)
            {
                stack.Push(tail.Value);
                prev = tail.Previous;
                tail.Previous = null;
                tail = prev;
            }

            while (stack.Count > 0)
            {
                list.Add(stack.Pop());
            }

            return list;
        }

        private Node<T> _tail = null;
    }
}

