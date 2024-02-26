using System;
using System.Threading;

// TODO: 고정크기로 만들어서 더 빠르게 동작하도록 수정
public class InterlockedRingBuffer<T>
{
    public InterlockedRingBuffer(int capacity)
    {
        Capacity = capacity;
        tail = 0;
        buffer = new T[Capacity];
    }

    public T Enqueue(T value)
    {
        var index = Interlocked.Increment(ref tail);
        var oldItem = buffer[index % Capacity];
        buffer[index % Capacity] = value;

        if (index > Capacity)
        {
            Interlocked.CompareExchange(ref tail, index % Capacity, index);
        }

        return oldItem;
    }

    public void Iterate(Action<T> action)
    {
        int currentTail = tail + (Capacity - 1);
        currentTail %= Capacity;
        for (int i = 0; i < Capacity; i++)
        {
            action(buffer[currentTail % Capacity]);
            currentTail++;
        }
    }

    public void Iterate(Func<T, bool> action)
    {
        int currentTail = tail + (Capacity - 1);
        currentTail %= Capacity;
        for (int i = 0; i < Capacity; i++)
        {
            if (action(buffer[currentTail % Capacity])) break;
            currentTail++;
        }
    }

    public int Capacity { get; private set; }

    private readonly T[] buffer;
    private int tail;
}
