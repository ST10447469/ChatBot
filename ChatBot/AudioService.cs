namespace ChatBot;

using System.Media;

public class AudioService
{
    public void PlayGreeting()
    {
        try
        {
            SoundPlayer player = new SoundPlayer("welcome.wav");
            player.PlaySync();
        }
        catch
        {
            Console.WriteLine("Audio greeting could not be played.");
        }
    }
}