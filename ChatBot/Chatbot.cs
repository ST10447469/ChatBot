namespace ChatBot;

public class Chatbot
{
    private readonly AudioService audioService = new AudioService();
    private readonly ConsoleUI consoleUI = new ConsoleUI();
    private readonly ResponseService responseService = new ResponseService();

    public void Start()
    {
        audioService.PlayGreeting();
        consoleUI.DisplayLogo();

        Console.ForegroundColor = ConsoleColor.Green;
        consoleUI.TypeText("Hello! Welcome to the Cybersecurity Awareness Bot ");
        Console.ResetColor();

        Console.Write("\nPlease enter your name: ");
        string userName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userName))
        {
            userName = "User";
        }

        consoleUI.TypeText($"\nNice to meet you, {userName}! Ask me anything about staying safe online.");
        consoleUI.TypeText("Type 'exit' anytime to quit.\n");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nYou: ");
            Console.ResetColor();

            string input = Console.ReadLine()?.ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                consoleUI.TypeText("Please enter something.");
                continue;
            }

            if (input == "exit")
            {
                consoleUI.TypeText("Goodbye! Stay safe online");
                break;
            }

            responseService.RespondToUser(input, consoleUI);
        }
    }
}
