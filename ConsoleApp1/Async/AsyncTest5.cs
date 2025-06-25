public class AsyncTest5
{
    public async Task ProcessDataChunkAsync(int chunkNumber)
    {
        Console.WriteLine($"Processing chunk {chunkNumber}...");
        await Task.Delay(1000*chunkNumber); // Simulate processing time
        Console.WriteLine($"Completed processing of chunk {chunkNumber}.");
    }

    public async Task ProcessLargeDatasetAsync(int numberOfChunks)
    {
        var tasks = new List<Task>();

        // Start processing each chunk concurrently
        for (int i = 1; i <= numberOfChunks; i++)
        {
            tasks.Add(ProcessDataChunkAsync(i));
        }

        // Wait for all tasks to complete
        await Task.WhenAll(tasks);

        Console.WriteLine("All data chunks processed.");
    }

    public static async Task Main(string[] args)
    {
        AsyncTest5 program = new AsyncTest5();
        await program.ProcessLargeDatasetAsync(5); // Process 5 chunks
    }
}
