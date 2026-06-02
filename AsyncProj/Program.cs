using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        #region asynchronous programming        

        var progress = new Progress<double>();
        progress.ProgressChanged += (sender, args) =>
        {
            Console.WriteLine($"Progress: {args}");
        };
        await ProgressAsync(progress);
        await WhenAllAsync();
        await WhenAnyAsync();
        await ProcessTasksAsync();

        #endregion

        #region asynchronous streams

        await foreach (int value in GetValuesAsync())
        {
            Console.WriteLine(value);
        }

        using (var cts = new CancellationTokenSource(500))
        {
            CancellationToken token = cts.Token;

            try
            {
            await foreach (int result in GetValuesAsync().WithCancellation(token))
            {
                Console.WriteLine(result);
            }
            }
            catch (TaskCanceledException)
            {
                //
            }
        }

        #endregion
        
    }
        
    // Передача информации о ходе выполнения операции
    public static async Task ProgressAsync(IProgress<double> progress)
    {        
        bool done = false;
        double percentComplete = 0;

        while (!done)
        {
            percentComplete += 5;
            progress?.Report(percentComplete);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            if (percentComplete == 100)
            {
                done = true;
            }
        }
    }

    //Ожидание завершения группы задач
    public static async Task WhenAllAsync()
    {
        Task<int> task1 = Task.FromResult(3);
        Task<int> task2 = Task.FromResult(5);
        Task<int> task3 = Task.FromResult(7);

        int[] results = await Task.WhenAll(task1, task2, task3);

        Console.WriteLine($"{results[0]},{results[1]},{results[2]}");
    }

    //Ожидание завершения любой задачи
    public static async Task WhenAnyAsync()
    {
        Task<int> task1 = Task.FromResult(3);
        Task<int> task2 = Task.FromResult(5);
        Task<int> task3 = Task.FromResult(7);

        Task<int> completedTask = await Task.WhenAny(task1, task2, task3);    

        Console.WriteLine($"{await completedTask}");
    }

    static async Task<int> DelayAndReturnAsync(int value)
    {
        await Task.Delay(TimeSpan.FromSeconds(value));

        return value;
    }

    //Обработка задач при завершении
    static async Task ProcessTasksAsync()
    {
        Task<int> task1 = DelayAndReturnAsync(2);
        Task<int> task2 = DelayAndReturnAsync(3);
        Task<int> task3 = DelayAndReturnAsync(1);

        Task<int>[] tasks = [task1, task2, task3];

        Task[] processingTasks = [.. tasks.Select(async t =>
        {
            var result = await t;
            Trace.WriteLine(result);
        })];

        await Task.WhenAll(processingTasks);
    }

    static async IAsyncEnumerable<int> GetValuesAsync([EnumeratorCancellation] CancellationToken token = default)
    {
        for (int i=0; i !=10; ++i)
        {           
            await Task.Delay(i*1000, token);
            yield return i;
        }      
    }    
}