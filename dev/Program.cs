using System;

UserController uc = new UserController();
string email = "";
string password = "";


Console.Clear();
Console.WriteLine("Welcome to ChronoKeep!\n");
Console.WriteLine("Enter your email & password at the prompt to log in.\nIf you're a new user, enter 'new' in the email prompt...\n");

Console.Write("\temail: ");
email = Console.ReadLine();

if(email.Equals("new")) {
    Console.Clear();
    Console.WriteLine("Welcome to ChronoKeep!  Enter your information below to create an account...");
    Console.Write("\tname (optional): ");
    string name = Console.ReadLine();
    Console.Write("\temail: ");
    email = Console.ReadLine();
    Console.Write("\tpassword: ");
    password = Console.ReadLine();

    //TODO: validation tests on input...

    uc.createUser(new User(name, email, password));
} else {
    Console.Write("\tpassword: ");
    password = Console.ReadLine();

    uc.queryUser(email, password);
}

Console.Clear();
Console.WriteLine($"You've been authenticated {uc.getUser.name}\n");

bool correctInput = true;

do {
    correctInput = true;

    Console.WriteLine("Please type the corresponding number of what you'd like to do:");
    Console.WriteLine("\t1) Open Projects");
    Console.WriteLine("\t2) Settings\n");

    Console.Write("Selection: ");
    string menuChoice = Console.ReadLine();

    switch(menuChoice) {
        case "1":
            break;
        case "2":
            break;
        default:
            printError($"Invalid Selection: '{menuChoice}'... Type the number next to the menu option.");
            correctInput = false;
            break;
    }
} while(!correctInput);

void printError(string errorText) {
    int width = 30;
    if(errorText.Length > 79) width = 60;
    Console.WriteLine();
    Console.Write("+");
    for(int i = 0; i < width; i++) Console.Write("-");
    Console.Write("+");
    Console.Write("\n|");
    for(int i = 0; i < width / 2 - 3; i++) Console.Write(" ");
    Console.Write("*ERROR*");
    for(int i = 0; i < width / 2 - 4; i++) Console.Write(" ");
    Console.Write("|\n|");

    for(int i = 0; i < width; i++) Console.Write(" ");
    Console.WriteLine("|");
    Console.Write("| ");
    
    int remainingSpaces = width;
    string[] errorTextParts = errorText.Split(" ");
    for(int wordIndex = 0; wordIndex < errorTextParts.Length; wordIndex++) {
        if(errorTextParts[wordIndex].Length < remainingSpaces) {
            Console.Write(errorTextParts[wordIndex] + " ");
            remainingSpaces -= errorTextParts[wordIndex].Length + 1;
        } else {
            for(int i = 0; i < remainingSpaces - 1; i++) Console.Write(" ");
            Console.Write("|\n| ");
            remainingSpaces = width;
            wordIndex--;
        }
    }
    for(int i = 0; i < remainingSpaces - 1; i++) Console.Write(" ");
    Console.Write("|\n|");
    for(int i = 0; i < width; i++) Console.Write(" ");
    Console.Write("|\n+");
    for(int i = 0; i < width; i++) Console.Write("-");
    Console.WriteLine("+");

    Console.WriteLine("\n");
}