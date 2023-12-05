using System.Collections.Generic;
using UnityEngine;


//hulpklasse A Star
public class PriorityQueue<T>
{
    private List<T> elements = new List<T>();
    private Dictionary<T, float> priorities = new Dictionary<T, float>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(item);
        priorities[item] = priority;
        elements.Sort((a, b) => priorities[a].CompareTo(priorities[b]));
    }

    public T Dequeue()
    {
        if (Count == 0)
        {
            Debug.LogError("PriorityQueue is empty");
            return default(T);
        }

        T item = elements[0];
        elements.RemoveAt(0);
        priorities.Remove(item);
        return item;
    }
}
