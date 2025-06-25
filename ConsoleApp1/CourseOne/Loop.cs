// using System;

// public class Loop
// {

// 	// public static void Main() {
// 	// 	IfElseDoWhile();
// 	// 	IfElseLoop();
// 	// 	SwitchLoop();
// 	// }

// 	private static void IfElseDoWhile(){
// 		int input;
// 		do {
// 			Console.WriteLine("Enter an even number between 1 and 10:");
// 			input = int.Parse(Console.ReadLine());
// 			if (input >= 1 && input <= 10 && input % 2 == 0) {
// 				Console.WriteLine("Valid input: " + input);
// 				break;
// 			} else {
// 				Console.WriteLine("Invalid input. Please try again.");
// 			}
// 		} while (true);
// 	}

// 	private static void IfElseLoop(){
// 		int[] grades = [85, 92, 78, 64, 89];

// 		for (int i = 0; i < grades.Length; i++) {
// 			int grade = grades[i];
// 			string result;
// 			if (grade >= 65) {
// 				result = "Pass";
// 			} else {
// 				result = "Fail";
// 			}
// 			Console.WriteLine($"Score: {grade} Result: {result}");
// 		}
// 	}

// 	private static void SwitchLoop(){
// 		string[] orderStatuses = { "Pending", "Shipped", "Delivered", "Cancelled" };

// 		for (int i = 0; i < orderStatuses.Length; i++) {
// 			string status = orderStatuses[i];
// 			switch (status) {
// 				case "Pending":
// 					Console.WriteLine("Order is pending.");
// 					break;
// 				case "Shipped":
// 					Console.WriteLine("Order has been shipped.");
// 					break;
// 				case "Delivered":
// 					Console.WriteLine("Order has been delivered.");
// 					break;
// 				case "Cancelled":
// 					Console.WriteLine("Order has been cancelled.");
// 					break;
// 				default:
// 					Console.WriteLine("Unknown status.");
// 					break;
// 			}
// 		}
// 	}

// 	private static void SwitchLoop2(){
// 		int[] scores = { 95, 82, 75, 63, 58 };

// 		for (int i = 0; i < scores.Length; i++) {
// 			int score = scores[i];
// 			switch (score) {
// 				case int n when (n >= 90):
// 					Console.WriteLine("Grade A: Excellent!");
// 					break;
// 				case int n when (n >= 80):
// 					Console.WriteLine("Grade B: Good job!");
// 					break;
// 				case int n when (n >= 70):
// 					Console.WriteLine("Grade C: Fair.");
// 					break;
// 				case int n when (n >= 60):
// 					Console.WriteLine("Grade D: Needs improvement.");
// 					break;
// 				default:
// 					Console.WriteLine("Grade F: Fail.");
// 					break;
// 			}
// 		}
// 	}
// }