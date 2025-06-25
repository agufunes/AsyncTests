public class AsyncTest2
{
	public async Task DownloadDataAsync()
    {
        Console.WriteLine("Download started...");
        await Task.Delay(3000);
        Console.WriteLine("Download completed.");
    }
    public static async Task Main(string[] args)
    {
        AsyncTest2 program = new();
        await program.DownloadDataAsync();
        Console.WriteLine("Main method completed.");
    }
}

/// <summary>
/// Represents a class that contains asynchronous methods for downloading data.
/// </summary>
public class AsyncTest3
{
    /// <summary>
    /// Asynchronously simulates downloading data with a delay of 3 seconds.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DownloadDataAsync()
    {
        Console.WriteLine("Download started...");
        await Task.Delay(3000);
        Console.WriteLine("Download completed.");
    }

    /// <summary>
    /// Asynchronously simulates downloading data with a delay of 2 seconds.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DownloadDataAsync2()
    {
        Console.WriteLine("Download 2 started...");
        await Task.Delay(2000);
        Console.WriteLine("Download 2 completed.");
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        AsyncTest3 program = new AsyncTest3();
        Task task1 = program.DownloadDataAsync();
        Task task2 = program.DownloadDataAsync2();
        await Task.WhenAll(task1, task2);
        Console.WriteLine("All downloads completed.");
    }
}


public class AsyncTest4
{
	
    public static async Task DownloadDataAsync()
    {
        try
        {
            Console.WriteLine("Download started...");
            await Task.Run(() => throw new InvalidOperationException("Simulated download error."));
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public static async Task DownloadDataAsync2()
    {
        Console.WriteLine("Download 2 started...");
        await Task.Delay(2000);
        Console.WriteLine("Download 2 completed.");
    }

    public static async Task Main(string[] args)
    {
        AsyncTest4 program = new();
        Task task1 = DownloadDataAsync();
        Task task2 = DownloadDataAsync2();
        await Task.WhenAll(task1, task2);
        Console.WriteLine("All downloads completed.");
    }
}
