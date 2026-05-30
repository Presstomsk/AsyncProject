using System;
using System.Threading;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {        
        Console.WriteLine($"Background: {Thread.CurrentThread.IsBackground}, Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        GetTaskAsync().GetAwaiter().GetResult();
        Console.WriteLine($"Background: {Thread.CurrentThread.IsBackground}, Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");
    }

    public static async Task GetTaskAsync()
    {
        Console.WriteLine($"Background: {Thread.CurrentThread.IsBackground}, Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(5000);
        Console.WriteLine($"Background: {Thread.CurrentThread.IsBackground}, Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine("HelloWorld !");
    }
    
    public static void Nai()
    {
        
    }
}