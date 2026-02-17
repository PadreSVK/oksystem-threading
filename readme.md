```csharp
int counter = 0;

void Increase()
{
    Thread.SpinWait(Random.Shared.Next(100, 5000)); // simulate work
    for (int i = 0; i < 1000000; i++)
    {
        Interlocked.Increment(ref counter);
    }
    Console.WriteLine("The counter is " + counter);
}

Thread t1 = new Thread(Increase);
Thread t2 = new Thread(Increase);
// Start threads (Fire and forget)
t1.Start();
t2.Start(); 


Console.WriteLine("The final counter is " + counter);

Console.ReadLine(); // wait for thread1 and thread2 to finish work

/* Why this (👆) code write:

The final counter is 0
The counter is 1936770
The counter is 2000000

hint: log Thread id (`Thread.CurrentThread.ManagedThreadId`) to `The counter is` log.
do not forget wait for threads completion (`Thread.Join()`)

📝 Fix code to write 
The counter is 1000000
The counter is 1000000
The final counter is 2000000

```
<details>
  <summary>Solutions</summary>

```csharp
ThreadLocal<int> threadCounter = new ThreadLocal<int>(() => 0, trackAllValues: true);

void Increase()
{
    Thread.SpinWait(Random.Shared.Next(100, 5000)); // simulate work
    for (int i = 0; i < 1000000; i++)
    {
         threadCounter.Value++;
    }
    Console.WriteLine("The counter is " + threadCounter.Value);
}

Thread t1 = new Thread(Increase);
Thread t2 = new Thread(Increase);
// Start threads
t1.Start();
t2.Start();

// Wait for threads to finish
t1.Join();
t2.Join();

Console.WriteLine("The final counter is " + threadCounter.Values.Sum());

Console.ReadLine(); // wait for thread1 and thread2 to finish work
```
```csharp
int counter = 0;

void Increase()
{
    // Local variable to track this specific thread's work
    int localCount = 0;

    Thread.SpinWait(Random.Shared.Next(100, 5000)); 

    for (int i = 0; i < 1000000; i++)
    {
        Interlocked.Increment(ref counter);
        localCount++;
    }

    // Log Thread ID and the local work done
    Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] The counter is {localCount}");
}

Thread t1 = new Thread(Increase);
Thread t2 = new Thread(Increase);

t1.Start();
t2.Start();

// Wait for threads to complete before moving forward
t1.Join();
t2.Join();

// Now this will accurately show 2000000
Console.WriteLine("The final counter is " + counter);

Console.ReadLine();
```

</details> 