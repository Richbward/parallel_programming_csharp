using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp12
{
    class Program
    {
        static void Main(string[] args)
        {


            PizzaTask().GetAwaiter().GetResult();
        }


        static void Compare_ParallelFor_Performance(int n)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (var x = 0; x < n; x++)
            {
                RunJob(x);
            }

            stopWatch.Stop();

            Console.WriteLine("Classic for loop execution time: " + stopWatch.Elapsed.TotalSeconds);

            Console.WriteLine("---------------------------------\n\n");

            stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.For(0, n, x =>
            {
                RunJob(x);
            });
            stopWatch.Stop();

            Console.WriteLine("Parallel.For execution time: " + stopWatch.Elapsed.TotalSeconds);

            Console.ReadKey();
        }

        static void Compare_ParallelForEach_Performance()
        {
            List<string> fruits = new List<string>();
            fruits.Add("Apple");
            fruits.Add("Banana");
            fruits.Add("Bilberry");
            fruits.Add("Blackberry");
            fruits.Add("Blackcurrant");
            fruits.Add("Blueberry");
            fruits.Add("Cherry");
            fruits.Add("Coconut");
            fruits.Add("Cranberry");
            fruits.Add("Date");
            fruits.Add("Durian");
            fruits.Add("Fig");
            fruits.Add("Grape");
            fruits.Add("Guava");
            fruits.Add("Jack-fruit");
            fruits.Add("Kiwi fruit");
            fruits.Add("Lemon");
            fruits.Add("Lime");
            fruits.Add("Lychee");
            fruits.Add("Mango");
            fruits.Add("Melon");
            fruits.Add("Olive");
            fruits.Add("Orange");
            fruits.Add("Papaya");
            fruits.Add("Plum");
            fruits.Add("Pineapple");
            fruits.Add("Pomegranate");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach (string fruit in fruits)
            {
                PrintFruit(fruit);
            }

            stopWatch.Stop();

            Console.WriteLine("Foreach loop execution time: " + stopWatch.Elapsed.TotalSeconds);

            Console.WriteLine("---------------------------------\n\n");

            stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.ForEach(fruits, fruit =>
            {
                PrintFruit(fruit);
            });

            stopWatch.Stop();

            Console.WriteLine("Parallel.ForEach execution time: " + stopWatch.Elapsed.TotalSeconds);

            Console.ReadKey();
        }

        static void RunJob(int x)
        {
            Console.WriteLine("Executing task {0} ", x);
            Thread.Sleep(500); // Simulate processing time
        }

        static void PrintFruit(string fruit)
        {
            Console.WriteLine("Fruit Name: {0}, Thread Id: {1}", fruit, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(500); // Simulate processing time
        }

        static void Compare_Task_Performance()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            RunTaskMethod(1);
            RunTaskMethod(2);
            RunTaskMethod(3);

            stopWatch.Stop();

            Console.WriteLine("Execution time: " + stopWatch.Elapsed.TotalSeconds);

            System.Console.WriteLine("Execute without 'Task'.");

            System.Console.WriteLine("\n\n\n");

            stopWatch = new Stopwatch();
            stopWatch.Start();

            // Define and run the task.

            Task task1 = Task.Run(() => RunTaskMethod(1));
            Task task2 = Task.Run(() => RunTaskMethod(2));
            Task task3 = Task.Run(() => RunTaskMethod(3));

            // If not using await, then use WaitAll
            // Task.WaitAll blocks the current thread until everything has completed.
            Task.WaitAll(task1, task2, task3);

            stopWatch.Stop();

            Console.WriteLine("Execution time: " + stopWatch.Elapsed.TotalSeconds);
            // Execution time here is not accurate because stopWatch was executed
            // BEFORE the these tasks completed its job.

            System.Console.WriteLine("Execute with 'Task.WaitAll (without async await)'.");
        }

        static void RunTaskMethod(int n)
        {
            System.Console.WriteLine("Task {0} started.", n);
            Thread.Sleep(1000);// Simulate task processing time.

            Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("Task {0} completed.", n);
            Console.ResetColor();
        }

        static async Task Compare_Task_Async_Await()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Define and run the task.
            Task task1 = Task.Run(() => RunTaskMethod(1));
            Task task2 = Task.Run(() => RunTaskMethod(2));
            Task task3 = Task.Run(() => RunTaskMethod(3));

            // await task1;
            // await task2;
            // await task3;

            // Use await with WhenAll. Why?
            // Task.WhenAll returns a task which represents 
            // the action of waiting until everything has completed.
            // That means that from an async method, you can use:
            await Task.WhenAll(task1, task2, task3);
            // which means your method will continue when 
            // everything's completed, but you won't tie up a thread 
            // to just hang around until that time.

            stopWatch.Stop();

            Console.WriteLine("Execution time: " + stopWatch.Elapsed.TotalSeconds);
            System.Console.WriteLine("Execute with 'Task and Async Await'.");

        }

        static void ContinueWith_Method1a()
        {
            System.Console.WriteLine("Continue With Method 1a started.");
            Thread.Sleep(1000);// Simulate task processing time.

            System.Console.WriteLine("Continue With Method 1a completed.");
        }

        static void ContinueWith_Method1b()
        {
            System.Console.WriteLine("Continue With Method 1b started.");
            Thread.Sleep(1000);// Simulate task processing time.

            System.Console.WriteLine("Continue With Method 1b completed.");
        }

        static void ContinueWith_Method2a()
        {
            System.Console.WriteLine("Continue With Method 2a started.");
            Thread.Sleep(1000);// Simulate task processing time.

            System.Console.WriteLine("Continue With Method 2a completed.");
        }

        static void ContinueWith_Method2b()
        {
            System.Console.WriteLine("Continue With Method 2b started.");
            Thread.Sleep(1000);// Simulate task processing time.

            System.Console.WriteLine("Continue With Method 2b completed.");
        }

        static void Task_ContinueWith()
        {
            ContinueWith_Method1a();
            ContinueWith_Method1b();
            ContinueWith_Method2a();
            ContinueWith_Method2b();

            // Call Task.Run and invoke Method1.
            // ... Then call Method2.
            //     Finally wait for Method2 to finish for terminating the program.
            Task.Run(() => ContinueWith_Method1a())
            .ContinueWith(task => ContinueWith_Method1b())
            .Wait();

            Task.Run(() => ContinueWith_Method2a())
                        .ContinueWith(task => ContinueWith_Method2b())
                        .Wait();


            System.Console.WriteLine("Task with ContinueWith done.");
        }


        static async Task PizzaTask()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int totalPizza = 10;
            Console.WriteLine($"Started preparing {totalPizza} pizza");

            // don't use Parallel.For with async delegates. This method doesn't 
            // understand them, and you end up with async void methods that 
            // cannot be awaited, and their exceptions cannot be handled.
            //for (var x = 1; x <= totalPizza; x++)
            //var tasks = Task.Run(() => Parallel.For(1, totalPizza, i =>
            //  MakePizza(i)
            //  ));

            // Parallelism applied here - Start
            var tasks = Enumerable.Range(1, totalPizza).Select(i => MakePizza(i));

            // 1. ConfigureAwait(false) improve performance by freeing up resource 
            // staying in the background.
            // 2. Use ConfigureAwait(false) for every await wherever possible to prevent deadlock.
            //await Task.WhenAll(tasks); // 20 min

            await Task.WhenAll(tasks).ConfigureAwait(false); // 20min
                                                             // WhenAll vs Multiple await
                                                             // Use WhenAll because it propagates all errors at once. With the multiple awaits, 
                                                             // you lose errors if one of the earlier awaits throws.

            // WhenAll vs WaitAll
            // WhenAll: non-blocking - doens't block current thread.
            // WaitAll: blocking - block current thread.

            // Parallelism applied here - End

            stopwatch.Stop();

            Console.WriteLine($"Finished preparing {totalPizza} pizza");
            Console.WriteLine("Elapsed time: " + stopwatch.Elapsed.TotalSeconds);

        }

        // Task as return type to apply Parallelism
        // async await is to apply Concurrency. 
        // Non-Blocking. Can prepare another pizza while waiting for BakePizza to finish.
        static async Task MakePizza(int n)
        {
            // PizzaTask has both synchronous (PreparingPizza) 
            // and asynchronous (BakingPizza)  operation.

            // The composition of an asynchronous operation followed by 
            // synchronous work is an asynchronous operation.
            // Stated another way, if any portion of an operation is 
            // asynchronous, the entire operation is asynchronous.

            // Another example is making toast. 
            // Making the toast is the composition of an asynchronous 
            // operation (toasting the bread), and 
            // synchronous operations (adding the butter and the jam).
            // Resource - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/

            // Need to finish preparing pizza first, then only can bake
            // While baking pizza, cook can start to prepare pizza.

            // synchronous. 
            // Blocking. Can't Prepare another pizza until BakePizza completed.
            // PreparePizza(n);
            // BakePizza(n);

            // ContinueWith is not recommend. 
            // Use ConfigureAwait(false) and await instead.

            // await here ensure PreparePizza is completed first before start BakePizza.



            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    if (n == 4)
                    {

                        cancellationTokenSource.Cancel();
                    }

                    //await PreparePizza(n).ConfigureAwait(false);
                    await PreparePizza(n, cancellationTokenSource.Token).ConfigureAwait(false);  // no performance difference with ConfigureAwait(false)

                    await BakePizza(n, cancellationTokenSource.Token).ConfigureAwait(false);
                    //await BakePizza(n).ConfigureAwait(false);
                    //await BakePizza(n); // no performance difference with ConfigureAwait(false)

                }
                catch (TaskCanceledException ex)
                {
                    // ex.Message
                    Console.WriteLine($"Order for pizza {n} was cancelled by customer. ");
                }
            }
        }

        // async await is to apply Concurrency.
        static async Task PreparePizza(int n, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            Console.WriteLine("Start preparing pizza " + n);
            await Task.Delay(5000); // asynchronous 
            //Thread.Sleep(5000); // synchronous. 65 minutes (and without parallelism) 
            Console.WriteLine("Finished preparing pizza " + n);
        }

        // async await is to apply Concurrency.
        static async Task BakePizza(int n, CancellationToken cancellationToken)
        {

            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            Console.WriteLine("Start baking pizza " + n);
            await Task.Delay(15000); // asynchronous 
            //Thread.Sleep(15000); // synchronous 
            Console.WriteLine("Finished baking pizza " + n);
        }
    }
}
