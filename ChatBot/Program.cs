namespace ChatBot;

namespace ChatBot;

using System;
using System.Media;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        //Play welcome sound
        PlayGreeting();

        // Display ASCII Art
        DisplayLogo();

        // Welcome user
        Console.ForegroundColor = ConsoleColor.Green;
        TypeText("Hello! Welcome to the Cybersecurity Awareness Bot ");
        Console.ResetColor();

        Console.Write("\nPlease enter your name: ");
        string userName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userName))
        {
            userName = "User";
        }

        TypeText($"\nNice to meet you, {userName}! Ask me anything about staying safe online.");
        TypeText("Type 'exit' anytime to quit.\n");

        // Chat loop
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nYou: ");
            Console.ResetColor();

            string input = Console.ReadLine()?.ToLower();

            // Input validation
            if (string.IsNullOrWhiteSpace(input))
            {
                TypeText(" Please enter something.");
                continue;
            }

            if (input == "exit")
            {
                TypeText("Goodbye! Stay safe online");
                break;
            }

            RespondToUser(input);
        }
