// using System;
// using System.Collections.Generic;

// class User
// {
//     public string Name { get; set; }
//     public List<string> BorrowedBooks { get; set; } = new List<string>();

//     public User(string name)
//     {
//         Name = name;
//     }
// }

// class Book
// {
//     public string Title { get; set; }
//     public bool IsBorrowed { get; set; }

//     public Book(string title)
//     {
//         Title = title;
//         IsBorrowed = false;
//     }
// }

// class Copilot
// {
//     static void Main()
//     {
//         Book[] books = new Book[5];
//         const int maxBorrowedBooks = 3;
//         Dictionary<string, User> users = new Dictionary<string, User>();

//         while (true)
//         {
//             Console.WriteLine("Enter your name:");
//             string userName = Console.ReadLine().ToLower();

//             if (!users.ContainsKey(userName))
//             {
//                 users[userName] = new User(userName);
//             }

//             User currentUser = users[userName];

//             Console.WriteLine("Would you like to add, remove, search, borrow, return, or exit? (add/remove/search/borrow/return/exit)");
//             string action = Console.ReadLine().ToLower();

//             if (action == "add")
//             {
//                 bool isFull = true;
//                 for (int i = 0; i < books.Length; i++)
//                 {
//                     if (books[i] == null)
//                     {
//                         isFull = false;
//                         break;
//                     }
//                 }

//                 if (isFull)
//                 {
//                     Console.WriteLine("The library is full. No more books can be added.");
//                 }
//                 else
//                 {
//                     Console.WriteLine("Enter the title of the book to add:");
//                     string newBookTitle = Console.ReadLine().ToLower();

//                     if (string.IsNullOrWhiteSpace(newBookTitle))
//                     {
//                         Console.WriteLine("Invalid input. Book title cannot be empty.");
//                     }
//                     else
//                     {
//                         for (int i = 0; i < books.Length; i++)
//                         {
//                             if (books[i] == null)
//                             {
//                                 books[i] = new Book(newBookTitle);
//                                 break;
//                             }
//                         }
//                     }
//                 }
//             }
//             else if (action == "remove")
//             {
//                 bool isEmpty = true;
//                 for (int i = 0; i < books.Length; i++)
//                 {
//                     if (books[i] != null)
//                     {
//                         isEmpty = false;
//                         break;
//                     }
//                 }

//                 if (isEmpty)
//                 {
//                     Console.WriteLine("The library is empty. No books to remove.");
//                 }
//                 else
//                 {
//                     Console.WriteLine("Enter the title of the book to remove:");
//                     string removeBookTitle = Console.ReadLine().ToLower();

//                     if (string.IsNullOrWhiteSpace(removeBookTitle))
//                     {
//                         Console.WriteLine("Invalid input. Book title cannot be empty.");
//                     }
//                     else
//                     {
//                         bool found = false;
//                         for (int i = 0; i < books.Length; i++)
//                         {
//                             if (books[i] != null && books[i].Title == removeBookTitle)
//                             {
//                                 books[i] = null;
//                                 found = true;
//                                 break;
//                             }
//                         }

//                         if (!found)
//                         {
//                             Console.WriteLine("Book not found.");
//                         }
//                     }
//                 }
//             }
//             else if (action == "search")
//             {
//                 Console.WriteLine("Enter the title of the book to search for:");
//                 string searchBookTitle = Console.ReadLine().ToLower();

//                 bool found = false;
//                 for (int i = 0; i < books.Length; i++)
//                 {
//                     if (books[i] != null && books[i].Title == searchBookTitle)
//                     {
//                         Console.WriteLine("Book is available in the library.");
//                         found = true;
//                         break;
//                     }
//                 }

//                 if (!found)
//                 {
//                     Console.WriteLine("Book not found in the library.");
//                 }
//             }
//             else if (action == "borrow")
//             {
//                 if (currentUser.BorrowedBooks.Count >= maxBorrowedBooks)
//                 {
//                     Console.WriteLine("You have reached the limit of borrowed books.");
//                 }
//                 else
//                 {
//                     Console.WriteLine("Enter the title of the book to borrow:");
//                     string borrowBookTitle = Console.ReadLine().ToLower();

//                     bool found = false;
//                     for (int i = 0; i < books.Length; i++)
//                     {
//                         if (books[i] != null && books[i].Title == borrowBookTitle)
//                         {
//                             if (!books[i].IsBorrowed)
//                             {
//                                 books[i].IsBorrowed = true;
//                                 currentUser.BorrowedBooks.Add(borrowBookTitle);
//                                 Console.WriteLine("Book borrowed successfully.");
//                                 found = true;
//                                 break;
//                             }
//                             else
//                             {
//                                 Console.WriteLine("Book is already borrowed.");
//                                 found = true;
//                                 break;
//                             }
//                         }
//                     }

//                     if (!found)
//                     {
//                         Console.WriteLine("Book not found.");
//                     }
//                 }
//             }
//             else if (action == "return")
//             {
//                 Console.WriteLine("Enter the title of the book to return:");
//                 string returnBookTitle = Console.ReadLine().ToLower();

//                 bool found = false;
//                 for (int i = 0; i < books.Length; i++)
//                 {
//                     if (books[i] != null && books[i].Title == returnBookTitle && books[i].IsBorrowed)
//                     {
//                         books[i].IsBorrowed = false;
//                         currentUser.BorrowedBooks.Remove(returnBookTitle);
//                         Console.WriteLine("Book returned successfully.");
//                         found = true;
//                         break;
//                     }
//                 }

//                 if (!found)
//                 {
//                     Console.WriteLine("Book not found or not borrowed.");
//                 }
//             }
//             else if (action == "exit")
//             {
//                 break;
//             }
//             else
//             {
//                 Console.WriteLine("Invalid action. Please type 'add', 'remove', 'search', 'borrow', 'return', or 'exit'.");
//             }

//             // Display the list of books
//             Console.WriteLine("Available books:");
//             for (int i = 0; i < books.Length; i++)
//             {
//                 if (books[i] != null)
//                 {
//                     Console.WriteLine($"{books[i].Title} {(books[i].IsBorrowed ? "(borrowed)" : "")}");
//                 }
//             }

//             // Display the list of borrowed books for the current user
//             Console.WriteLine($"Books borrowed by {currentUser.Name}:");
//             foreach (var book in currentUser.BorrowedBooks)
//             {
//                 Console.WriteLine(book);
//             }
//         }
//     }
// }