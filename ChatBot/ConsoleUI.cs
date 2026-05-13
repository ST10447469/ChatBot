namespace ChatBot;

public class ConsoleUI
{
    public void DisplayLogo()
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

    public void TypeText(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(20);
        }

        Console.WriteLine();
    }
}