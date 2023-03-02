using System;
UserController uc = new UserController();

Console.WriteLine("Welcome to ChronoKeep\n");
Console.Write("Name: ");
string name = Console.ReadLine();
Console.Write("Email: ");
string email = Console.ReadLine();
Console.Write("password: ");
string password = Console.ReadLine();

Console.WriteLine("Creating User in DB...");

uc.createUser(new User(name, email, password));