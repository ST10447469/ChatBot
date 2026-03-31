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
        // Voice Greeting
        static void PlayGreeting()
        {
            try
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("welcome.wav.wav");
                player.PlaySync();
            }
            catch
            {

            }
        }

        // ASCII Logo
        static void DisplayLogo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
  ==========================================
     CYBERSECURITY AWARENESS BOT 
  ==========================================
        \   ^__^
         \  (oo)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
  Stay Safe. Stay Smart. Stay Secure.
  ==========================================");
            Console.ResetColor();
        }

        // Typing Effect
        static void TypeText(string message)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(20);
            }
            Console.WriteLine();
        }
