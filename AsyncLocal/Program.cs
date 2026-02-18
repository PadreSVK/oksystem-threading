// AsyncLocal flows across await boundaries
AsyncLocal<string> _requestId = new AsyncLocal<string>();

async Task ProcessRequestAsync(string requestId)
{
    // Set the AsyncLocal value for this async context
    _requestId.Value = requestId;
    
    Log("Request started");

    // Simulate async work - thread may change but AsyncLocal flows
    await Task.Delay(Random.Shared.Next(50, 150));
    
    Log("After first delay");

    // Spawn a child task - AsyncLocal value is inherited!
    await Task.Run(async () =>
    {
        Log("Inside Task.Run (child task)");
        await Task.Delay(50);
        Log("Child task completing");
    });

    // Do more async work
    await ProcessDataAsync();

    Log("Request completed");
}

async Task ProcessDataAsync()
{
    // This method can access the AsyncLocal value without passing it as a parameter!
    Log("Processing data");
    await Task.Delay(50);
    Log("Data processed");
}

void Log(string message)
{
    // AsyncLocal value is automatically available here
    var threadId = Environment.CurrentManagedThreadId;
    Console.WriteLine($"[{_requestId.Value}] Thread {threadId}: {message}");
}

Console.WriteLine("Starting concurrent request processing...\n");

// Simulate multiple concurrent requests
var tasks = new[]
{
    ProcessRequestAsync("REQ-001"),
    ProcessRequestAsync("REQ-002"),
    ProcessRequestAsync("REQ-003")
};

await Task.WhenAll(tasks);

Console.WriteLine("\nAll requests completed!");
Console.ReadLine();