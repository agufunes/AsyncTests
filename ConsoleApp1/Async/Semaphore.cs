using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.Async
{
    public class SemaphoreExample
    {
        // Create a semaphore with initial count of 1 (only 1 thread can access the resource)
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        // Simulate a shared resource (like a file, database connection, etc.)
        private static string sharedResource = "Initial Value";

        public static async Task RunExample()
        {
            Console.WriteLine("=== Semaphore Example: Only 1 thread can access the resource at a time ===\n");

            // Create multiple tasks that will try to access the shared resource
            Task[] tasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                int taskId = i + 1;
                tasks[i] = AccessSharedResourceAsync(taskId);
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            Console.WriteLine("\n=== All tasks completed ===");
        }

        private static async Task AccessSharedResourceAsync(int taskId)
        {
            Console.WriteLine($"Task {taskId}: Waiting to access the shared resource...");

            // Wait to enter the semaphore (acquire permission)
            await semaphore.WaitAsync();

            try
            {
                Console.WriteLine($"Task {taskId}: ✓ Acquired access to the shared resource!");

                // Simulate some work with the shared resource
                Console.WriteLine($"Task {taskId}: Reading current value: '{sharedResource}'");

                // Simulate processing time
                await Task.Delay(2000);

                // Modify the shared resource
                sharedResource = $"Modified by Task {taskId} at {DateTime.Now:HH:mm:ss}";
                Console.WriteLine($"Task {taskId}: Updated resource to: '{sharedResource}'");

                Console.WriteLine($"Task {taskId}: ✓ Finished working with the shared resource");
            }
            finally
            {
                // Always release the semaphore, even if an exception occurs
                semaphore.Release();
                Console.WriteLine($"Task {taskId}: Released access to the shared resource\n");
            }
        }

        // Alternative example showing synchronous version
        public static void RunSynchronousExample()
        {
            Console.WriteLine("=== Synchronous Semaphore Example ===\n");

            // Create multiple threads
            Thread[] threads = new Thread[3];
            for (int i = 0; i < 3; i++)
            {
                int threadId = i + 1;
                threads[i] = new Thread(() => AccessSharedResourceSync(threadId));
                threads[i].Start();
            }

            // Wait for all threads to complete
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("\n=== All threads completed ===");
        }

        private static void AccessSharedResourceSync(int threadId)
        {
            Console.WriteLine($"Thread {threadId}: Waiting to access the shared resource...");

            // Wait to enter the semaphore (acquire permission)
            semaphore.Wait();

            try
            {
                Console.WriteLine($"Thread {threadId}: ✓ Acquired access to the shared resource!");

                // Simulate some work with the shared resource
                Console.WriteLine($"Thread {threadId}: Reading current value: '{sharedResource}'");

                // Simulate processing time
                Thread.Sleep(1500);

                // Modify the shared resource
                sharedResource = $"Modified by Thread {threadId} at {DateTime.Now:HH:mm:ss}";
                Console.WriteLine($"Thread {threadId}: Updated resource to: '{sharedResource}'");

                Console.WriteLine($"Thread {threadId}: ✓ Finished working with the shared resource");
            }
            finally
            {
                // Always release the semaphore
                semaphore.Release();
                Console.WriteLine($"Thread {threadId}: Released access to the shared resource\n");
            }
        }

        // Rate Limiting Examples

        // Example 1: Limit concurrent API calls (e.g., max 3 concurrent requests)
        private static readonly SemaphoreSlim apiConcurrencyLimiter = new SemaphoreSlim(3, 3);

        public static async Task RunApiRateLimitingExample()
        {
            Console.WriteLine("=== API Rate Limiting Example: Max 3 concurrent requests ===\n");

            // Simulate 10 API calls, but only 3 can run concurrently
            Task[] apiCalls = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                int requestId = i + 1;
                apiCalls[i] = MakeApiCallAsync(requestId);
            }

            await Task.WhenAll(apiCalls);
            Console.WriteLine("\n=== All API calls completed ===");
        }

        private static async Task MakeApiCallAsync(int requestId)
        {
            Console.WriteLine($"Request {requestId}: Waiting for API slot...");

            await apiConcurrencyLimiter.WaitAsync();

            try
            {
                Console.WriteLine($"Request {requestId}: ✓ Making API call (slot acquired)");

                // Simulate API call duration (1-3 seconds)
                var delay = new Random().Next(1000, 3000);
                await Task.Delay(delay);

                Console.WriteLine($"Request {requestId}: ✓ API call completed in {delay}ms");
            }
            finally
            {
                apiConcurrencyLimiter.Release();
                Console.WriteLine($"Request {requestId}: Released API slot");
            }
        }

        // Example 2: Time-based rate limiting (e.g., max 5 requests per 10 seconds)
        private static readonly SemaphoreSlim timeBucketLimiter = new SemaphoreSlim(5, 5);

        public static async Task RunTimeBucketRateLimitingExample()
        {
            Console.WriteLine("=== Time Bucket Rate Limiting: Max 5 requests per 10 seconds ===\n");

            // Start the bucket refill task
            var refillTask = RefillBucketPeriodically();

            // Simulate 15 requests over time
            Task[] requests = new Task[15];
            for (int i = 0; i < 15; i++)
            {
                int requestId = i + 1;
                requests[i] = MakeTimeLimitedRequestAsync(requestId);

                // Space out the requests a bit
                await Task.Delay(500);
            }

            await Task.WhenAll(requests);
            Console.WriteLine("\n=== All time-limited requests completed ===");
        }

        private static async Task MakeTimeLimitedRequestAsync(int requestId)
        {
            Console.WriteLine($"Time Request {requestId}: Checking rate limit...");

            bool acquired = await timeBucketLimiter.WaitAsync(TimeSpan.FromSeconds(1));

            if (!acquired)
            {
                Console.WriteLine($"Time Request {requestId}: ❌ Rate limited! Try again later.");
                return;
            }

            try
            {
                Console.WriteLine($"Time Request {requestId}: ✓ Processing request");
                await Task.Delay(200); // Simulate quick processing
                Console.WriteLine($"Time Request {requestId}: ✓ Request completed");
            }
            catch
            {
                // If we got the semaphore but failed, we need to release it
                timeBucketLimiter.Release();
                throw;
            }
            // Note: We don't release here - the bucket refill task manages the semaphore
        }

        private static async Task RefillBucketPeriodically()
        {
            while (true)
            {
                await Task.Delay(5000); // Wait 10 seconds

                // Refill the bucket to maximum capacity (5 requests)5
                int currentCount = timeBucketLimiter.CurrentCount;
                int toRelease = Math.Max(0, 5 - currentCount);

                if (toRelease > 0)
                {
                    timeBucketLimiter.Release(toRelease);
                    Console.WriteLine($"🔄 Bucket refilled: Added {toRelease} permits (Total available: {timeBucketLimiter.CurrentCount})");
                }
            }
        }

        // Example 3: Website scraping rate limiter


        public static async Task RunWebScrapingRateLimitingExample()
        {
            Console.WriteLine("=== Web Scraping Rate Limiting: 1 request every 2 seconds ===\n");

            string[] urls = {
                "https://example.com/page1",
                "https://example.com/page2",
                "https://example.com/page3",
                "https://example.com/page4",
                "https://example.com/page5"
            };

            foreach (string url in urls)
            {
                await ScrapeWebsiteAsync(url);
                // Enforce minimum delay between requests
                await Task.Delay(2000);
            }

            Console.WriteLine("\n=== Web scraping completed ===");
        }

        private static async Task ScrapeWebsiteAsync(string url)
        {
            await scrapingLimiter.WaitAsync();

            try
            {
                Console.WriteLine($"🕷️  Scraping: {url}");

                // Simulate web scraping work
                await Task.Delay(1000);

                Console.WriteLine($"✓ Scraped: {url} - Data extracted successfully");
            }
            finally
            {
                scrapingLimiter.Release();
            }
        }

        private static readonly SemaphoreSlim scrapingLimiter = new SemaphoreSlim(2, 2); // Allow 2 concurrent scraping tasks7

        // Example 4: Yahoo Finance async web scraping example
        public static async Task RunYahooFinanceAsyncExample()
        {
            List<Task> tasks = new List<Task>();
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            Console.WriteLine("=== Yahoo Finance Async Web Scraping Example ===\n");
            string[] stockSymbols = ["AAPL", "GOOGL", "MSFT", "AMZN", "TSLA"];
            foreach (var symbol in stockSymbols)
            {
                Console.WriteLine($"Fetching historical quotes for {symbol}...");
                tasks.Add(GetYahooHistoryQuotesAsync(symbol, DateTime.Now.AddDays(-1), DateTime.Now));
                Console.WriteLine($"Data for {symbol} fetched successfully.");
                // // Enforce minimum delay between requests
                // await Task.Delay(1000);
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("\n=== Yahoo Finance scraping completed ===");
            stopwatch1.Stop();
            Console.WriteLine($"Total time taken for all requests: {stopwatch1.ElapsedMilliseconds}" + " ms");
        }

        public static async Task<JsonDocument?> GetYahooHistoryQuotesAsync(string ticker, DateTime fromDate, DateTime toDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await scrapingLimiter.WaitAsync();

            try
            {
                // Create an HttpClient instance and set the base address
                var httpClient = new HttpClient
                {
                    // get the base address from configuration or set a default one
                    BaseAddress = new Uri("https://query2.finance.yahoo.com")
                };
                // Add required User-Agent header
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0");
                // Set the request parameters
                var period1 = new DateTimeOffset(fromDate).ToUnixTimeSeconds();
                var period2 = new DateTimeOffset(toDate).ToUnixTimeSeconds();
                var interval = "1d";
                var events = "history";
                // Construct the request URL
                var path = $"/v8/finance/chart/{ticker}?period1={period1}&period2={period2}&interval={interval}&events={events}";
                // Read the response content and parse it as JSON
                return await httpClient.GetFromJsonAsync<JsonDocument>(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data for {ticker}: {ex.Message}");
                return null;
            }
            finally
            {
                await Task.Delay(500);
                scrapingLimiter.Release();
                stopwatch.Stop();
                Console.WriteLine($"Time taken to fetch data for {ticker}: {stopwatch.ElapsedMilliseconds} ms");
            }

        }
    }
}