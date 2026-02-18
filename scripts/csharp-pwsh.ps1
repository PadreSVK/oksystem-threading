$code = @"
using System;
using System.Linq;

public class Runner
{
    public static void Run()
    {
        var persons = Enumerable.Range(10, 100)
            .Select(i =>
            {
                Console.WriteLine($"Creating Person {i}");
                return new Person
                {
                    Name = $"Person {i}",
                    Age = i,
                    SomeBool = i % 7 == 0
                };
            })
            .Where(p => p.SomeBool);

        foreach (var person in persons)
        {
            Console.WriteLine($"{person.Name} is an adult: {person.IsAdult}");
        }
    }
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool SomeBool { get; set; }
    public bool IsAdult => Age >= 18;
}
"@

# Compile the C# code
Add-Type -TypeDefinition $code -Language CSharp

# Execute the C# entry point
[Runner]::Run()
