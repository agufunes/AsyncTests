// using System;
// using System.Collections.Generic;  // Needed for List<T>
// class CopilotTestAnswer
// {
//     static void Main(string[] args)
//     {
//         List<string> tasks = [];
//         bool exit = false;

//         while (!exit)
//         {
//             Console.WriteLine("1. View Tasks");
//             Console.WriteLine("2. Add Task");
//             Console.WriteLine("3. Mark Task Complete");
//             Console.WriteLine("4. Exit");
//             Console.Write("Enter your choice: ");


//             if (int.TryParse(Console.ReadLine(), out int choice))
//             {
//                 switch (choice)
//                 {
//                     case 1:
//                         // Display tasks
//                         if (tasks.Count == 0)
//                         {
//                             Console.WriteLine("No tasks available.");
//                         }
//                         else
//                         {
//                             for (int i = 0; i < tasks.Count; i++)
//                             {
//                                 Console.WriteLine($"{i + 1}. {tasks[i]}");
//                             }
//                         }
//                         break;

//                     case 2:
//                         Console.Write("Enter the task: ");
//                         string task = Console.ReadLine();

//                         // Ensure task is not empty
//                         if (!string.IsNullOrWhiteSpace(task))
//                         {
//                             tasks.Add(task);
//                             Console.WriteLine("Task added.");
//                         }
//                         else
//                         {
//                             Console.WriteLine("Task cannot be empty.");
//                         }
//                         break;

//                     case 3:
//                         Console.Write("Enter the task number to mark complete: ");
//                         int taskNumber;
//                         if (int.TryParse(Console.ReadLine(), out taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
//                         {
//                             // Check if the task is already marked complete
//                             if (!tasks[taskNumber - 1].Contains("[Complete]"))
//                             {
//                                 tasks[taskNumber - 1] += " [Complete]";
//                                 Console.WriteLine("Task marked as complete.");
//                             }
//                             else
//                             {
//                                 Console.WriteLine("This task is already marked complete.");
//                             }
//                         }
//                         else
//                         {
//                             Console.WriteLine("Invalid task number.");
//                         }
//                         break;

//                     case 4:
//                         exit = true;
//                         break;

//                     default:
//                         Console.WriteLine("Invalid option, please try again.");
//                         break;
//                 }
//             }
//             else
//             {
//                 Console.WriteLine("Please enter a valid number.");
//             }
//         }
//     }
// }