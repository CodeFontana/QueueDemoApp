using System.Diagnostics;

////////////////////////////////////////////////////////
// Using normal Queue, in order to represent
// varying priorities, you would need to employ
// multiple queues.

CancellationTokenSource cts = new();

Queue<string> normalQueue = new();
Queue<string> goldQueue = new();
Queue<string> platinumQueue = new();

normalQueue.Enqueue("Lacey");
platinumQueue.Enqueue("Horsie");
goldQueue.Enqueue("Stacy");
goldQueue.Enqueue("Kasie");
platinumQueue.Enqueue("Heysie");
normalQueue.Enqueue("Tracy");
platinumQueue.Enqueue("Chantasey");

while(cts.IsCancellationRequested == false)
{
    await Task.Delay(1000);

	if (platinumQueue.Count > 0)
	{
		string platinumItem = platinumQueue.Dequeue();
        Console.WriteLine($"Platinum: {platinumItem}");
        continue;
    }

    if (goldQueue.Count > 0)
    {
        string goldItem = goldQueue.Dequeue();
        Console.WriteLine($"Gold: {goldItem}");
        continue;
    }

    if (normalQueue.Count > 0)
    {
        string normalItem = normalQueue.Dequeue();
        Console.WriteLine($"Normal: {normalItem}");
        continue;
    }

    cts.Cancel();
}

////////////////////////////////////////////////////////

// Priority queue doesn't guarantee order within
// items of the same priority. Likely a design
// decision in favor of performance.
PriorityQueue<string, int> queue = new();

queue.Enqueue("Lacey", 3);
queue.Enqueue("Horsie", 1);
queue.Enqueue("Stacy", 2);
queue.Enqueue("Kasie", 2);
queue.Enqueue("Heysie", 1);
queue.Enqueue("Tracy", 3);
queue.Enqueue("Chantasey", 1);

cts = new();
Console.WriteLine();

while (cts.IsCancellationRequested == false)
{
    await Task.Delay(1000);

    if (queue.Count > 0)
    {
        string item = queue.Dequeue();
        Console.WriteLine($"Dequeue: {item}");
        continue;
    }

    cts.Cancel();
}

////////////////////////////////////////////////////////

// By using a timestamp from the Stopwatch, we are
// now able to guarantee order, within items of the
// same priority.
PriorityQueue<string, (Status, long)> priorityQueue = new(StatusComparer.Instance);

priorityQueue.Enqueue("Lacey", (Status.Normal, Stopwatch.GetTimestamp()));
priorityQueue.Enqueue("Horsie", (Status.Platinum, Stopwatch.GetTimestamp()));
priorityQueue.Enqueue("Stacy", (Status.Gold, Stopwatch.GetTimestamp()));
priorityQueue.Enqueue("Kasie", (Status.Gold, Stopwatch.GetTimestamp()));
priorityQueue.Enqueue("Heysie", (Status.Platinum, Stopwatch.GetTimestamp()));
priorityQueue.Enqueue("Tracy", (Status.Normal, Stopwatch.GetTimestamp()));
priorityQueue.Enqueue("Chantasey", (Status.Platinum, Stopwatch.GetTimestamp()));

cts = new();
Console.WriteLine();

while (cts.IsCancellationRequested == false)
{
    await Task.Delay(1000);

    if (priorityQueue.Count > 0)
    {
        string item = priorityQueue.Dequeue();
        Console.WriteLine($"Dequeue: {item}");
        continue;
    }

    cts.Cancel();
}

enum Status
{
    Normal,
    Gold,
    Platinum
}

class StatusComparer : IComparer<(Status, long)>
{
    public static StatusComparer Instance { get; } = new();

    private StatusComparer() { }

    public int Compare((Status, long) x, (Status, long) y)
    {
        if (x.Item1 == y.Item1)
        {
            return x.CompareTo(y);
        }

        return y.CompareTo(x);
    }
}
