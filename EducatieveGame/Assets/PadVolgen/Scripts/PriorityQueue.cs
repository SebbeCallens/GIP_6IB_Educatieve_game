using System;
using System.Collections.Generic;

//hulpklasse A Star
public class PriorityQueue<T>
{
    private List<Tuple<T, float>> elements = new List<Tuple<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(new Tuple<T, float>(item, priority));
        elements.Sort((x, y) => x.Item2.CompareTo(y.Item2));
    }

    public T Dequeue()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        T item = elements[0].Item1;
        elements.RemoveAt(0);
        return item;
    }
}
