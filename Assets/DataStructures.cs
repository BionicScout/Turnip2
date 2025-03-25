using System.Collections.Generic;
using UnityEngine;


class PriorityQueue<T> {

    List<T> values = new List<T>();
    List<float> heap = new List<float>();

    static int Parent(int i) {
        return (i - 1) / 2;
    }

    static int LeftChild(int i) {
        return ((2 * i) + 1);
    }

    static int RightChild(int i) {
        return ((2 * i) + 2);
    }

    void ShiftUp(int i) {
        while(i > 0 && heap[Parent(i)] < heap[i]) {

            // Swap parent and current node
            Swap(Parent(i), i);

            // Update i to parent of i
            i = Parent(i);
        }
    }

    void ShiftDown(int i) {
        int maxIndex = i;

        // Left Child
        int l = LeftChild(i);

        if(l < heap.Count && heap[l] > heap[maxIndex]) {
            maxIndex = l;
        }

        // Right Child
        int r = RightChild(i);

        if(r < heap.Count && heap[r] > heap[maxIndex]) {
            maxIndex = r;
        }

        // If i not same as maxIndex
        if(i != maxIndex) {
            Swap(i, maxIndex);
            ShiftDown(maxIndex);
        }
    }

    // Function to insert a new element in the Binary Heap
    public void Enqueue(T value, float p) {
        values.Add(value);
        heap.Add(p);

        ShiftUp(heap.Count-1);
    }

    // Function to extract the element with maximum priority
    public T Dequeue() {
        Debug.Assert(heap.Count > 0, "DataStructures.PriorityQueue[Dequeue]: Queue is emepty");

        float result = heap[0];
        T val = values[0];

        // Replace the value at the root with the last leaf
        Swap(0, heap.Count - 1);

        values.RemoveAt(values.Count - 1);
        heap.RemoveAt(heap.Count - 1);

        // Shift down the replaced element to maintain the heap property
        ShiftDown(0);
        return val;
    }

    // Function to change the priority of an element
    public void ChangePriority(int i, float p) {
        float oldp = heap[i];
        heap[i] = p;

        if(p > oldp) {
            ShiftUp(i);
        }
        else {
            ShiftDown(i);
        }
    }

    public void ChangePriority(T val, float p) {
        int index = values.FindIndex(v => v.Equals(val)); 
        Debug.Assert(index != -1, "DataStructures.PriorityQueue[ChangePriority]: value does not exist");
        ChangePriority(index, p);
    }

    // Function to remove the element located at given index
    public void Remove(T val) {
        int index = values.FindIndex(v => v.Equals(val));
        Debug.Assert(index != -1, "DataStructures.PriorityQueue[Remove]: value does not exist");
        RemoveAt(index);
    }

    public void RemoveAt(int i) {
        heap[i] = heap[0] + 1;

        // Shift the node to the root of the heap
        ShiftUp(i);

        // Extract the node
        Dequeue();
    }

    void Swap(int i, int j) {
        float temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;

        T temp2 = values[i];
        values[i] = values[j];
        values[j] = temp2;
    }

    public int Count() {
        return heap.Count;
    }

    public override string ToString() {
        string str = "Count: " + Count() + "\n";
        for(int i = 0; i < heap.Count; i++) {
            float p = heap[i];
            T val = values[i];
            str += "(" + val + ", " + p + ")\n"; 
        }
        return str;
    }
}

class MinPriorityQueue<T> {

    List<T> values = new List<T>();
    List<float> heap = new List<float>();

    static int Parent(int i) {
        return (i - 1) / 2;
    }

    static int LeftChild(int i) {
        return ((2 * i) + 1);
    }

    static int RightChild(int i) {
        return ((2 * i) + 2);
    }

    void ShiftUp(int i) {
        while(i > 0 && heap[Parent(i)] > heap[i]) {

            // Swap parent and current node
            Swap(Parent(i), i);

            // Update i to parent of i
            i = Parent(i);
        }
    }

    void ShiftDown(int i) {
        int minIndex = i;

        // Left Child
        int l = LeftChild(i);

        if(l < heap.Count && heap[l] < heap[minIndex]) {
            minIndex = l;
        }

        // Right Child
        int r = RightChild(i);

        if(r < heap.Count && heap[r] < heap[minIndex]) {
            minIndex = r;
        }

        // If i not same as maxIndex
        if(i != minIndex) {
            Swap(i, minIndex);
            ShiftDown(minIndex);
        }
    }

    // Function to insert a new element in the Binary Heap
    public void Enqueue(T value, float p) {
        values.Add(value);
        heap.Add(p);

        ShiftUp(heap.Count - 1);
    }

    // Function to extract the element with maximum priority
    public T Dequeue() {
        Debug.Assert(heap.Count > 0, "DataStructures.MinPriorityQueue[Dequeue]: Queue is emepty");

        float result = heap[0];
        T val = values[0];

        // Replace the value at the root with the last leaf
        Swap(0, heap.Count - 1);

        values.RemoveAt(values.Count - 1);
        heap.RemoveAt(heap.Count - 1);

        // Shift down the replaced element to maintain the heap property
        ShiftDown(0);
        return val;
    }

    // Function to change the priority of an element
    public void ChangePriority(int i, float p) {
        float oldp = heap[i];
        heap[i] = p;

        if(p < oldp) {
            ShiftUp(i);
        }
        else {
            ShiftDown(i);
        }
    }

    public void ChangePriority(T val, float p) {
        int index = values.FindIndex(v => v.Equals(val));
        Debug.Assert(index != -1, "DataStructures.MinPriorityQueue[ChangePriority]: value does not exist");
        ChangePriority(index, p);
    }

    // Function to remove the element located at given index
    public void Remove(T val) {
        int index = values.FindIndex(v => v.Equals(val));
        Debug.Assert(index != -1, "DataStructures.MinPriorityQueue[Remove]: value does not exist");
        RemoveAt(index);
    }

    public void RemoveAt(int i) {
        heap[i] = heap[0] + 1;

        // Shift the node to the root of the heap
        ShiftUp(i);

        // Extract the node
        Dequeue();
    }

    void Swap(int i, int j) {
        float temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;

        T temp2 = values[i];
        values[i] = values[j];
        values[j] = temp2;
    }

    public int Count() {
        return heap.Count;
    }

    public override string ToString() {
        string str = "Count: " + Count() + "\n";
        for(int i = 0; i < heap.Count; i++) {
            float p = heap[i];
            T val = values[i];
            str += "(" + val + ", " + p + ")\n";
        }
        return str;
    }
}
