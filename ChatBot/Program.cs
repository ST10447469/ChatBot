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
        //  Chatbot Responses
        static void RespondToUser(string input)
        {
            // General questions
            if (input.Contains("how are you"))
            {
                TypeText("I'm just code, but I'm running perfectly and ready to help you!");
            }
            else if (input.Contains("purpose"))
            {
                TypeText("My purpose is to help you stay safe online by teaching cybersecurity awareness.");
            }
            else if (input.Contains("what can i ask"))
            {
                TypeText("You can ask me about passwords, phishing, safe browsing, and suspicious links.");
            }

            // Password Safety
            else if (input.Contains("password"))
            {
                TypeText(" Use strong passwords with a mix of uppercase, lowercase, numbers, and symbols.");
                TypeText("Avoid using personal info like your name or birthdate.");
                TypeText("Tip: Use a password manager to store your passwords securely.");
            }

            // Phishing
            else if (input.Contains("phishing") || input.Contains("email"))
            {
                TypeText(" Phishing is when scammers trick you into giving personal information.");
                TypeText("Never click suspicious email links or download unknown attachments.");
                TypeText("Always verify the sender’s email address.");
            }

            // Safe Browsing
            else if (input.Contains("safe browsing") || input.Contains("browser"))
            {
                TypeText(" Always check for HTTPS in the website URL.");
                TypeText("Avoid entering personal data on untrusted websites.");
                TypeText("Keep your browser updated.");
            }

            // Suspicious Links
            else if (input.Contains("link") || input.Contains("url"))
            {
                TypeText(" Be careful with links, especially shortened ones.");
                TypeText("Hover over links to see the real URL before clicking.");
            }

            // Default response
            else
            {
                TypeText(" I didn’t quite understand that. Could you rephrase?");
            }
        }
    }

