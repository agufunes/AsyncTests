
using System.Threading.Tasks;


/// <summary>
/// Represents a test class for demonstrating asynchronous operations.
/// </summary>
public class AsyncTest(){
    public static async Task Main() {
        Console.WriteLine("Client make an order...");
    	Task<string> taskMeal = GetMeal();
        Console.WriteLine("Client is waiting for the order...");
        string meal = await taskMeal;
        Console.WriteLine($"Client received the order: {meal}");
    }

    /// <summary>
    /// Gets the meal asynchronously.
    /// </summary>
    private static async Task<string> GetMeal(){
        Console.WriteLine("Starting cooking...");
        await Task.Delay(2000);
        Console.WriteLine("Cooking completed.");
        return "Meal";
    }

}