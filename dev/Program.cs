using System;
using System.Text.RegularExpressions;

UserController uc = new UserController();
string email = "";
string password = "";
bool correctInput = true;

Console.Clear();
Console.WriteLine("Welcome to ChronoKeep!\n");
Console.WriteLine("Enter your email & password at the prompt to log in.\nIf you're a new user, enter 'new' in the email prompt...\n");

do {
    Console.Write("\temail: ");
    email = Console.ReadLine() ?? "";
    if(email.ToLower().Equals("new")) break;
    Regex emailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
    correctInput = emailRegex.IsMatch(email);
    if(!correctInput) printError("Email was invalid!");
} while(!correctInput);

if(email.ToLower().Equals("new")) {
    Console.Clear();
    Console.WriteLine("Welcome to ChronoKeep!  Enter your information below to create an account...");
    string name;
    do {
        Console.Write("\tname (optional): ");
        name = Console.ReadLine() ?? "";
        correctInput = (name.Length < 50 && name.Length > -1 && name != null);
        if(!correctInput) printError("Name must be between 0 and 50 characters...");
    } while(!correctInput);

    do {
        Console.Write("\temail: ");
        email = Console.ReadLine() ?? "";
        Regex emailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        correctInput = emailRegex.IsMatch(email);
        if(!correctInput) printError("Email was invalid!");
    } while(!correctInput);
    
    do {
        Console.Write("\tpassword: ");
        password = Console.ReadLine() ?? "";
        correctInput = (password.Length > 7 && password != "password");
        if(!correctInput) printError("Password must be >7 characters & not be 'password'...");
    } while(!correctInput);

    uc.createUser(new User(name, email, password));
} else {
    int passwordAttemps = 3;
    do {
        if(passwordAttemps == 0) {
            Console.Clear();
            Console.WriteLine("Too many wrong attempts... Exiting...");
            System.Environment.Exit(-1);
        }
        correctInput = true;
        if(email.Equals("")) {
            Console.Write("\temail: ");
            email = Console.ReadLine() ?? "";
        }

        Console.Write("\tpassword: ");
        password = Console.ReadLine() ?? "";

        if(!uc.queryUser(email, password)) {
            correctInput = false;
            passwordAttemps--;
            email = "";
            printError($"Incorrect email or password!  Remaining Attempts: {passwordAttemps}");
        }
    } while(!correctInput);
}

Console.Clear();
Console.WriteLine($"-- You've been authenticated {uc.getUser.name.Trim()} --");

while(true) {
    Console.WriteLine("MAIN MENU\n---------------------------------------");
    Console.WriteLine("\nPlease type the corresponding number of what you'd like to do:");
    Console.WriteLine("\t1) Open Projects");
    Console.WriteLine("\t2) Settings\n");
    Console.WriteLine("\t3) Exit Program\n");

    Console.Write("Selection: ");
    string menuChoice = Console.ReadLine() ?? "";

    if(menuChoice.Equals("1")) { //PROJECTS
        Console.Clear();
        while(true) {
            Console.WriteLine("MAIN MENU -> PROJECTS\n---------------------------------------");
        
            int projectIndex = 0;
            if(uc.getUser.projects.Count > 0)
                foreach(Project p in uc.getUser.projects) {
                    Console.WriteLine($"\t{projectIndex + 1}) {p.title}\t\t{p.getTime()}");
                    projectIndex++;
                }
            Console.WriteLine($"\n\t{projectIndex + 1}) Create New Project");
            Console.WriteLine($"\t{projectIndex + 2}) Back to Main Menu");

            Console.Write("Selection: ");
            menuChoice = Console.ReadLine() ?? "";

            if(int.Parse(menuChoice) - 1 < uc.getUser.projects.Count) { //OPEN PROJECT
            Console.Clear();

                while(true) {
                    Console.WriteLine($"MAIN MENU -> PROJECTS -> {uc.getUser.projects[int.Parse(menuChoice) - 1].title.ToUpper()}");
                    Console.WriteLine(uc.getUser.projects[int.Parse(menuChoice) - 1].description);
                    Console.WriteLine(uc.getUser.projects[int.Parse(menuChoice) - 1].getTime());
                    Console.WriteLine("\n\t1) Start Timer");
                    Console.WriteLine("\n\t2) Back to Projects");
                    Console.Write("Selection: ");
                    menuChoice = Console.ReadLine() ?? "";

                    if(menuChoice.Equals("1")) {
                        //count down from 3
                        for(int i = 3; i > 0; i--) {
                            Console.Clear();
                            Console.WriteLine(Figgle.FiggleFonts.Standard.Render(i.ToString()));
                            System.Threading.Thread.Sleep(1000);
                        }
                        Console.Clear();

                        uint elapsedMillis = 0;
                        System.Threading.Thread listenForInputThread = new System.Threading.Thread(listenForInput);
                        listenForInputThread.Start();
                        uint lastDigit = 10;
                        DateTime startTime = DateTime.Now;
                        while(listenForInputThread.IsAlive) {
                            elapsedMillis = (uint)(DateTime.Now - startTime).TotalMilliseconds;
                            if(lastDigit != (Math.Floor(elapsedMillis / 1000.0) % 10)) { //optimization to not print to screen every 10th of a second.
                                lastDigit = (uint)(Math.Floor(elapsedMillis / 1000.0) % 10);
                                Console.Clear();
                                Console.WriteLine("Press 's' to stop the timer...\n");
                                Console.WriteLine($"Total time spent on {uc.getUser.projects[int.Parse(menuChoice) - 1].title}");
                                Console.WriteLine($"\t{uc.getUser.projects[int.Parse(menuChoice) - 1].getTime(-1, elapsedMillis)}\n");
                                Console.WriteLine("Current Session:");
                                string currentSession = uc.getUser.projects[int.Parse(menuChoice) - 1].getTime(elapsedMillis, 0);
                                Console.WriteLine($"({currentSession})");
                                Console.WriteLine(Figgle.FiggleFonts.Standard.Render(currentSession));
                            }
                            System.Threading.Thread.Sleep(100);
                        }

                        uc.getPC.modifyProject(
                            uc.getUser.projects[int.Parse(menuChoice) - 1].title,
                            uc.getUser.projects[int.Parse(menuChoice) - 1].description,
                            uc.getUser.projects[int.Parse(menuChoice) - 1].millisecondsTotal + elapsedMillis,
                            uc.getUser.Id
                        );
                        uc.getUser.projects[int.Parse(menuChoice) - 1].millisecondsTotal = uc.getUser.projects[int.Parse(menuChoice) - 1].millisecondsTotal + elapsedMillis;

                        Console.Clear();
                        Console.WriteLine("Session Ended!");
                        Console.WriteLine($"Details:\n\tTime spent this session: {uc.getUser.projects[int.Parse(menuChoice) - 1].getTime(elapsedMillis, 0)}");
                        Console.WriteLine($"\tTime spent total: {uc.getUser.projects[int.Parse(menuChoice) - 1].getTime(-1, elapsedMillis)}\n");
                    } else if(menuChoice.Equals("2")) {
                        Console.Clear();
                        break;
                    } else {
                        printError($"{menuChoice} was not a valid input.  Enter the corresponding number next to each option...");
                    }
                }
            } else if(int.Parse(menuChoice) == projectIndex + 1) { //CREATE NEW PROJECT
                Console.Clear();
                while(true) {
                    Console.WriteLine("MAIN MENU -> PROJECTS -> CREATE NEW PROJECT\n---------------------------------------");
                    Console.Write("Project Title: ");
                    string projTitle = Console.ReadLine() ?? "";
                    if(projTitle == "") {
                        printError("Title cannot be blank!");
                        continue;
                    }
                    
                    //check if title is already in use
                    bool isTitleCopied = false;
                    foreach(Project p in uc.getUser.projects) {
                        if(p.title == projTitle) {
                            printError($"{projTitle} is already being used.  Pick another name!");
                            isTitleCopied = true;
                            break;
                        }
                    }
                    if(isTitleCopied) continue;

                    if(projTitle.Length >= 50) {
                        printError("Your title cannot be greater than 50 characters...");
                        continue;
                    }
                    
                    Console.Write("Project Discription (250 char): ");
                    string projDescription = Console.ReadLine() ?? "";
                    if(projDescription.Length >= 250) {
                        printError("Your description cannot be greater than 250 characters...");
                        continue;
                    }

                    Project newProj = new Project(projTitle, projDescription, 0L);
                    uc.getPC.createProject(newProj, uc.getUser.Id);
                    uc.getUser.projects.Add(newProj);

                    Console.WriteLine($"{projTitle} created successfully!");
                    break;
                }
            } else if(int.Parse(menuChoice) == projectIndex + 2) {
                Console.Clear();
                break;
            } else {
                printError($"{menuChoice} wasn't recognized... Enter a corresponding number.");
            }
        }
    } else if(menuChoice.Equals("2")) { //SETTINGS
        Console.Clear();

        while(true) {
            Console.WriteLine("MAIN MENU -> SETTINGS\n---------------------------------------");
            Console.WriteLine("\t1) Edit User Profile\n");
            Console.WriteLine("\t2) Project Settings");
            Console.WriteLine("\n\t3) Back to Main Menu");
            Console.Write("\nSelection: ");
            menuChoice = Console.ReadLine() ?? "";
            if(menuChoice.Equals("1")) { //USER SETTINGS
                Console.Clear();

                while(true) {
                    Console.WriteLine("MAIN MENU -> SETTINGS -> USER SETTINGS\n---------------------------------------");
                    Console.WriteLine("\t1) Edit Name");
                    Console.WriteLine("\t2) Edit Email");
                    Console.WriteLine("\t3) Edit Password\n");
                    Console.WriteLine("\t4) Back to Settings");
                    Console.Write("\nSelection: ");
                    menuChoice = Console.ReadLine() ?? "";
                    
                    if(menuChoice.Equals("1")) { //name
                        Console.WriteLine($"\nCurrent name: {uc.getUser.name}");
                        Console.Write("\tNew Name: ");
                        string newName = Console.ReadLine() ?? "";
                        if (newName.Length < 50 && newName.Length > -1 && newName != null) {
                            uc.modifyUser(newName, uc.getUser.Email, uc.getUser.Password);
                            Console.WriteLine($"\nName successfully changed to {uc.getUser.name}!\n");
                        } else {
                            printError($"{newName} was invalid! Be sure name is between 0 and 50 characters...");
                        }
                    } else if(menuChoice.Equals("2")) { //email
                        Console.WriteLine($"\nCurrent email: {uc.getUser.Email}");
                        Console.Write("\tNew email: ");
                        string newEmail = Console.ReadLine() ?? "";
                        Regex emailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
                        if(emailRegex.IsMatch(newEmail)) {
                            uc.modifyUser(uc.getUser.name, newEmail, uc.getUser.Password);
                            Console.WriteLine($"\nEmail successfully changed to {uc.getUser.Email}!\n");
                        } else {
                            printError($"{newEmail} was invalid!  Email not changed...");
                        }
                    } else if(menuChoice.Equals("3")) { //password
                        Console.Write("\nEnter current password: ");
                        string currPlainTextPassword = Console.ReadLine() ?? "";
                        if(!uc.getUser.Password.Equals(uc.getUser.hashPassword(currPlainTextPassword))) {
                            printError("Password Incorrect!  Password not changed...");
                        } else {
                            Console.Write("\nNew Password: ");
                            string newPlainTextPassword = Console.ReadLine() ?? "";
                            if(newPlainTextPassword.Length > 7 && newPlainTextPassword != "password") {
                                uc.modifyUser(uc.getUser.name, uc.getUser.Email, uc.getUser.hashPassword(newPlainTextPassword));
                                Console.WriteLine("\nPassword successfully changed!");
                            } else {
                                printError("Your password must be at least 8 charactesr and not be 'password'...");
                            }
                        }
                    } else if(menuChoice.Equals("4")){
                        break;
                    } else {
                        Console.Clear();
                        printError($"Did not recognize '{menuChoice}'.  Type a digit corresponding to the action you want to take...");
                    }
                }
            } else if(menuChoice.Equals("2")) { //PROJECT SETTINGS
                Console.Clear();

                while(true) {
                    Console.WriteLine("MAIN MENU -> SETTINGS -> PROJECT SETTINGS\n---------------------------------------");
                    if(uc.getUser.projects.Count == 0) {
                        Console.WriteLine("It doesn't seem you have any projects to modify...\n");
                        break;
                    }

                    int projectIndex = 0;
                    foreach(Project p in uc.getUser.projects) {
                        Console.WriteLine($"\t{projectIndex + 1}) {p.title}\t\t{p.getTime()}");
                        projectIndex++;
                    }
                    Console.WriteLine($"\n\t{projectIndex + 1}) Back to Main Menu");

                    Console.Write("Selection: ");
                    menuChoice = Console.ReadLine() ?? "";

                    if(int.Parse(menuChoice) - 1 < uc.getUser.projects.Count) { //Edit Project
                        int referencedProjectNumber = int.Parse(menuChoice) - 1;
                        Console.Clear();

                        while(true) {
                            Console.WriteLine($"MAIN MENU -> SETTINGS -> PROJECT SETTINGS -> MODIFY {uc.getUser.projects[referencedProjectNumber].title.ToUpper()}\n---------------------------------------");
                            Console.WriteLine("\t1) Change Title");
                            Console.WriteLine("\t2) Change Description");
                            Console.WriteLine("\t3) Change Time");
                            Console.WriteLine("\n\t4)Back to Project Settings");

                            Console.Write("\nSelection: ");
                            menuChoice = Console.ReadLine() ?? "";

                            if(menuChoice.Equals("1")) { //Edit Title
                                Console.Clear();

                                while(true) {
                                    Console.WriteLine($"Current title: {uc.getUser.projects[referencedProjectNumber].title}");
                                    Console.Write("New Title: ");
                                    string newTitle = Console.ReadLine() ?? "";

                                    if(newTitle == "") {
                                    printError("Title cannot be blank!");
                                        continue;
                                    }
                                    
                                    //check if title is already in use
                                    bool isTitleCopied = false;
                                    foreach(Project p in uc.getUser.projects) {
                                        if(p.title == newTitle) {
                                            printError($"{newTitle} is already being used.  Pick another name!");
                                            isTitleCopied = true;
                                            break;
                                        }
                                    }
                                    if(isTitleCopied) continue;

                                    if(newTitle.Length >= 50) {
                                        printError("Your title cannot be greater than 50 characters...");
                                        continue;
                                    }

                                    uc.getPC.modifyProject(uc.getUser.projects[referencedProjectNumber].title, uc.getUser.projects[referencedProjectNumber].description, uc.getUser.projects[int.Parse(menuChoice) - 1].millisecondsTotal, uc.getUser.Id, newTitle);
                                    uc.getUser.projects[referencedProjectNumber].title = newTitle;
                                    break;
                                }
                            } else if(menuChoice.Equals("2")) { //Edit description
                                Console.Clear();

                                while(true) {
                                    Console.WriteLine($"Current description:\n\t{uc.getUser.projects[referencedProjectNumber].description}\n");
                                    Console.Write("New Description:\n\t");
                                    string newDescription = Console.ReadLine() ?? ""; 

                                    if(newDescription.Length >= 250) {
                                        printError("Your description cannot be greater than 250 characters...");
                                        continue;
                                    }

                                    uc.getPC.modifyProject(uc.getUser.projects[referencedProjectNumber].title, newDescription, uc.getUser.projects[referencedProjectNumber].millisecondsTotal, uc.getUser.Id);
                                    uc.getUser.projects[referencedProjectNumber].description = newDescription;
                                    break;
                                }
                            } else if(menuChoice.Equals("3")) { //Edit time
                                Console.Clear();

                                while(true) {
                                    long newMillis = 0;
                                    Console.WriteLine($"Current Time: {uc.getUser.projects[referencedProjectNumber].getTime()}");
                                    Console.WriteLine("*If you would like to skip a field, type 0 and then hit enter. Otherwise, enter a number*");

                                    Console.Write("Years: ");
                                    newMillis += int.Parse(Console.ReadLine() ?? "0") * 1000 * 60 * 60 * 24 * 365;

                                    Console.Write("Days: ");
                                    newMillis += int.Parse(Console.ReadLine() ?? "0") * 1000 * 60 * 60 * 24;

                                    Console.Write("Hours: ");
                                    newMillis += int.Parse(Console.ReadLine() ?? "0") * 1000 * 60 * 60;

                                    Console.Write("Minutes: ");
                                    newMillis += int.Parse(Console.ReadLine() ?? "0") * 1000 * 60;

                                    Console.Write("Seconds: ");
                                    newMillis += int.Parse(Console.ReadLine() ?? "0") * 1000;

                                    uc.getPC.modifyProject(uc.getUser.projects[referencedProjectNumber].title, uc.getUser.projects[referencedProjectNumber].description, newMillis, uc.getUser.Id);
                                    uc.getUser.projects[referencedProjectNumber].millisecondsTotal = newMillis;
                                    break;
                                }
                            } else if(menuChoice.Equals("4")) {
                                break;
                            } else {
                                printError($"Did not recognize {menuChoice}...");
                            }
                        }
                    } else if(menuChoice.Equals((projectIndex + 1).ToString())) {
                        break;
                    } else {
                        printError($"Didn't recognize {menuChoice}...");
                    }
                }
            } else if(menuChoice.Equals("3")) {
                break;
            } else {
                correctInput = false;
                Console.Clear();
                printError($"Did not recognize '{menuChoice}'.  Type a digit corresponding to the action you want to take...");
            }
        }
    } else if(menuChoice.Equals("3")) {
        Console.WriteLine("\nThanks for using ChronoKeep!\n");
        break;
    } else {
        correctInput = false;
        Console.Clear();
        printError($"Did not recognize '{menuChoice}'. Type a digit corresponding to the action you want to take...");
    }
}

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

void listenForInput() {
    while(Console.ReadKey().Key != ConsoleKey.S);
}