namespace ChatBot;

public class ResponseService
{
    public void RespondToUser(string input, ConsoleUI consoleUI)
    {
        if (input.Contains("how are you"))
        {
            consoleUI.TypeText("I'm just code, but I'm running perfectly and ready to help you!");
        }
        else if (input.Contains("purpose"))
        {
            consoleUI.TypeText("My purpose is to help you stay safe online by teaching cybersecurity awareness.");
        }
        else if (input.Contains("what can i ask"))
        {
            consoleUI.TypeText("You can ask me about passwords, phishing, safe browsing, and suspicious links.");
        }
        else if (input.Contains("password"))
        {
            consoleUI.TypeText("Use strong passwords with a mix of uppercase, lowercase, numbers, and symbols.");
            consoleUI.TypeText("Avoid using personal info like your name or birthdate.");
            consoleUI.TypeText("Tip: Use a password manager to store your passwords securely.");
        }
        else if (input.Contains("phishing") || input.Contains("email"))
        {
            consoleUI.TypeText("Phishing is when scammers trick you into giving personal information.");
            consoleUI.TypeText("Never click suspicious email links or download unknown attachments.");
            consoleUI.TypeText("Always verify the sender’s email address.");
        }
        else if (input.Contains("safe browsing") || input.Contains("browser"))
        {
            consoleUI.TypeText("Always check for HTTPS in the website URL.");
            consoleUI.TypeText("Avoid entering personal data on untrusted websites.");
            consoleUI.TypeText("Keep your browser updated.");
        }
        else if (input.Contains("link") || input.Contains("url"))
        {
            consoleUI.TypeText("Be careful with links, especially shortened ones.");
            consoleUI.TypeText("Hover over links to see the real URL before clicking.");
        }
        else
        {
            consoleUI.TypeText("I didn’t quite understand that. Could you rephrase?");
        }
    }
}