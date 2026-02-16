var balance = 1000;
object lockA = new(), lockB = new();

// creates background thread
var t1 = new Thread(() => 
{
    lock (lockA)
    {
        Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Acquired lockA");
        Thread.Sleep(100); // ← Forces thread 2 to grab lockB first
        lock (lockB)
        {
            balance += 100;
        }
    }
});

var t2 = new Thread(() =>
{
    lock (lockB)
    {
        Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Acquired lockB");
        Thread.Sleep(100); // ← Forces thread 1 to be stuck on lockB
        lock (lockA) // ← DEADLOCK: t1 holds lockA, waits for lockB
        {
            balance -= 100;
        }
    }
});

t1.Start(); t2.Start();
t1.Join(TimeSpan.FromSeconds(5)); // ← Will timeout — deadlock!
t2.Join(TimeSpan.FromSeconds(5));
Console.WriteLine(t1.IsAlive ? "DEADLOCK detected!" : $"Balance: {balance}"); 
// explain why is console/process "stucked"