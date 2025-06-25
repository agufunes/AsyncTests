using ConsoleApp1.Async;

public class AsyncTest6
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Choose an example to run:");
        Console.WriteLine("1. Long Operation Example");
        Console.WriteLine("2. Semaphore Example (Async)");
        Console.WriteLine("3. Semaphore Example (Synchronous)");
        Console.WriteLine("4. API Rate Limiting Example");
        Console.WriteLine("5. Time Bucket Rate Limiting Example");
        Console.WriteLine("6. Web Scraping Rate Limiting Example");
        Console.WriteLine("7. Yahoo finance Asyn web scraping example");
        Console.Write("Enter your choice (1-6): ");

        await SemaphoreExample.RunYahooFinanceAsyncExample();

        // var choice = Console.ReadLine();
        // switch (choice)
        // {
        //     case "2":
        //         // Semaphore async example
        //         await SemaphoreExample.RunExample();
        //         break;
        //     case "3":
        //         // Semaphore synchronous example
        //         SemaphoreExample.RunSynchronousExample();
        //         break;
        //     case "4":
        //         // API rate limiting example
        //         await SemaphoreExample.RunApiRateLimitingExample();
        //         break;
        //     case "5":
        //         // Time bucket rate limiting example
        //         await SemaphoreExample.RunTimeBucketRateLimitingExample();
        //         break;
        //     case "6":
        //         // Web scraping rate limiting example
        //         await SemaphoreExample.RunWebScrapingRateLimitingExample();
        //         break;
        //     case "7":
        //         // Yahoo finance async web scraping example
        //         await SemaphoreExample.RunYahooFinanceAsyncExample();
        //         break;
        //     default:
        //         Console.WriteLine("Invalid choice. Running semaphore example by default...");
        //         await SemaphoreExample.RunExample();
        //         break;
        // }

        Console.WriteLine("Main method completed.");
    }
}

