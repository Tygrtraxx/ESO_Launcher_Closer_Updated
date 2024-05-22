//============================================================================
// Name        : ESO Launcher Closer Updated
// Author(s)   : Draco9990 and rewritten by Tygrtraxx
// Version     : 2.1.0
// Created on  : (2022?)
// Rewritten on: (05/13/2024)
// Last Update :
// Description : Main file for program
//============================================================================

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    /*=============================================================================================*
    * Function: custom_console_text(string text_input, string color)
    *
    * Description: This function will print out text in a specific color, to the user
    * @var text_input
    *	The text that will be output by the console later on
    * @var custom_type
    *   Changes the way the text and colors will output to the user based on the input
    * @var colors_array
    *	An array that holds all of the colors of the input
    * @var console_color
    *	An array that holds all of the enum color values for the text
    *=============================================================================================*/
    private static void custom_console_text(string text_input, string[] colors_array, int custom_type)
    {
        //Make sure the colors are in lowercase to prevent bugs
        for (int i = 0; i < colors_array.Length; i++)
        {
            colors_array[i] = colors_array[i].ToLower();
        }

        //Create an array that will hold all of the color enum values
        ConsoleColor[] console_color = new ConsoleColor[colors_array.Length];

        //Convert the colors input to a ConsoleColor enum value by going through each of them
        for (int i = 0; i < colors_array.Length; i++)
        {
            switch (colors_array[i])
            {
                case "red":
                    console_color[i] = ConsoleColor.Red;
                    break;
                case "green":
                    console_color[i] = ConsoleColor.Green;
                    break;
                case "blue":
                    console_color[i] = ConsoleColor.Blue;
                    break;
                case "yellow":
                    console_color[i] = ConsoleColor.Yellow;
                    break;
                case "cyan":
                    console_color[i] = ConsoleColor.Cyan;
                    break;
                case "magenta":
                    console_color[i] = ConsoleColor.Magenta;
                    break;
                case "white":
                    console_color[i] = ConsoleColor.White;
                    break;
                default:
                    console_color[i] = ConsoleColor.White;
                    Console.WriteLine("Invalid color input. Using default color (white).");
                    break;
            }
        }

        //Displays the custom text and color based on the functionality that was entered
        switch (custom_type)
        {
            //Solid color for the entire text
            case 1:

                //Set the text color and write a single colored message
                Console.ForegroundColor = console_color[0];
                Console.WriteLine("\n\n" + text_input + "\n");

                break;

            //Every other letter is a different color in the text
            case 2:

                //Store each indivdual letter of the text into a character array
                char[] char_array = text_input.ToCharArray();

                //Go through each character in the character array
                for (int c = 0; c < char_array.Length;)
                {
                    //Allow the user to see the color changes
                    Thread.Sleep(100);

                    //Write the character with the first color
                    Console.ForegroundColor = console_color[0];
                    Console.Write(char_array[c]);

                    //Go to the next character in the array
                    c++;

                    //Check and make sure we're not already at the end of the array
                    if (c < char_array.Length)
                    {
                        //Write the next character with the second color
                        Console.ForegroundColor = console_color[1];
                        Console.Write(char_array[c]);

                        //Go to the next character in the array
                        c++;
                    }
                };

                //Allow the user to see the changes
                Thread.Sleep(2000);

                //Start at the beginning of the line to make it look like the colors are changing
                Console.Write("\r");

                //Go through each character in the character array (Opposite colors this time)
                for (int c = 0; c < char_array.Length;)
                {
                    //Allow the user to see the color changes
                    Thread.Sleep(100);

                    //Write the character with the first color
                    Console.ForegroundColor = console_color[1];
                    Console.Write(char_array[c]);

                    //Make sure we move onto the next character in the character_array (Opposite colors this time)
                    c++;

                    //Check and make sure we're not already at the end of the array
                    if (c < char_array.Length)
                    {
                        //Write the next character with the second color
                        Console.ForegroundColor = console_color[0];
                        Console.Write(char_array[c]);

                        //Go to the next character in the array
                        c++;
                    }
                };

                //Start at the beginning of the line to make it look like the colors are changing
                Console.Write("\r");

                break;

            default:

                //Tell the user something went wrong
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Unknown custom font function.");

                break;
        }

        //Reset the color to the default
        Console.ResetColor();
    }

    /*=============================================================================================*
    * Function: modify_registry(string action)
    *
    * Description: This function will set a registry key so the program automatcially runs on startup
    * @var action
    *	The action that will happen next based on the user's input (Create or Delete reg key)
    *=============================================================================================*/
    private static void modify_registry(string action)
    {
        //Get the path to the program file
        string? path = Process.GetCurrentProcess().MainModule?.FileName;

        switch (action)
        {
            //Create a registry key
            case "CreateRegistryKey":

                //Tell the user we are creating the key
                Console.WriteLine("\nCreating a registry key: ");
                console_load_animation(5);

                //Set where the key is going
                RegistryKey crk = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                //Make sure the path is a valid path
                if (path != null)
                {
                    //Check if the key exists (if it's null, it doesn't exist)
                    if (crk.GetValue("ESO Launcher Closer") == null)
                    {
                        //Create a new key based on the path
                        crk.SetValue("ESO Launcher Closer", path);

                        //Tell the user the key has been successfully added
                        custom_console_text("Registry key has been added successfully.", ["green"], 1);
                    }

                    else
                    {
                        //Tell the user the registry key already exists
                        custom_console_text("The registry key already exists.", ["cyan"], 1);
                    }
                }

                else
                {
                    //Tell the user something went wrong
                    custom_console_text("Error: Unable to add the registry key. (Invalid path ??)", ["red"], 1);
                }

                break;

            case "DeleteRegistryKey":

                //Tell the user we are deleting the key
                Console.WriteLine("\nRemoving the registry key.");
                console_load_animation(5);

                //Set where to open the key
                RegistryKey? drk = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                //Make sure the path is a valid path
                if (path != null)
                {

                    //Check if the registry key exists before attempting to delete it
                    if (drk.GetValue("ESO Launcher Closer") != null)
                    {
                        //Delete the key from the registry
                        drk.DeleteValue("ESO Launcher Closer", false);

                        //Tell the user the key has been successfully deleted
                        custom_console_text("Registry key has been deleted successfully.", ["green"], 1);
                    }

                    else
                    {
                        //Tell the user the registry key doesn't exist
                        custom_console_text("The registry key doesn't exist.", ["yellow"], 1);
                    }
                }

                else
                {
                    //Tell the user something went wrong
                    custom_console_text("Error: Unable to add the registry key. (Invalid path ??)", ["red"], 1);
                }

                break;

            default:
                //Tell the user something went wrong
                custom_console_text("Error: Unknown action (Somehow you bypassed create or delete registry key 'actions'??)", ["red"], 1);
                break;
        }
    }

    /*=============================================================================================*
    * Function: console_loading_animation()
    *
    * Description: This function will give the illusion of loading to the user to show its working
    * @var total_looped_times
    *	The amount of times we want to loop the whole animation for loading, to be shown to the user
    *=============================================================================================*/
    private static void console_load_animation(int total_looped_times)
    {
        //The loading characters that will be shown to the user
        var loaderString = new[] { "oooo", "oooO", "ooO0", "oO0o", "O0oo", "0ooo", "oooo" };

        //Used to keep track of the array location while showing the loading animation to the user
        var load = 0;

        //How many times to loop the loading array to give the illusion of "loading" to the user
        int i = 0;

        while (i < total_looped_times)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(loaderString[load++]);

            //If we have reached the end of the loading array (Displayed all the strings at least once to the user), then loop by increasing i
            if (load == loaderString.Length)
            {
                i++;
            }

            //Check if we have reached the end of the loading array and if not, then start from the beginning again
            load = load == loaderString.Length ? 0 : load;

            //Allows us to slow down the shown animation, so the user can see it
            Thread.Sleep(80);
        }
    }

    /*=============================================================================================*
    * Function: "main"
    *
    * Description: This is where all the functions get called and where the main execution happens
    *=============================================================================================*/
    static void Main(string[] args)
    {
        //Stores user's choice as a integer(number) from 1 to 4
        int user_choice;

        //Display the title of the launcher to the user
        Console.Title = "ESO Launcher Closer";

        //Welcome the user to the program and ask them to select an option
        Console.WriteLine("Welcome to the ESO Launcher Closer program.\n");
        Console.WriteLine("Please select one of the options below:\n");
        Console.WriteLine("1: Start the program");
        Console.WriteLine("2: Add a registry key (Starts program on startup)");
        Console.WriteLine("3: Delete the Registry Key (Removes the program on startup)");
        Console.WriteLine("4: Exit\n");

        //Ask the user for input
        do
        {
            Console.WriteLine("Please enter a number between 1 and 4:");
            string? input = Console.ReadLine();

            //Try to parse the input into a number
            if (int.TryParse(input, out user_choice))
            {
                //Check if the input is a number between 1 and 4
                if (user_choice >= 1 && user_choice <= 4)
                {
                    switch (user_choice)
                    {
                        //Start the program for the user
                        case 1:
                            Console.Clear();
                            Console.WriteLine("Starting the Program: ");
                            console_load_animation(5);
                            Console.Clear();

                            //While the program is active, we will find all of the launcher processes and end them
                            while (true)
                            {
                                //Tell the user the program is running
                                custom_console_text("Program is now running.", ["magenta", "blue" ], 2);

                                Process[] games = Process.GetProcessesByName("eso64");
                                if (games.Length > 0)
                                {
                                    Process[] launchers = Process.GetProcessesByName("Bethesda.net_Launcher");
                                    foreach (var launcher in launchers)
                                    {
                                        launcher.Kill();
                                        launcher.Dispose();
                                    }
                                    games[0].WaitForExit();
                                }
                                System.Threading.Thread.Sleep(2000);
                            }
                        //Create the registry key
                        case 2:

                            modify_registry("CreateRegistryKey");

                            break;

                        //Delete registry key
                        case 3:

                            modify_registry("DeleteRegistryKey");

                            break;

                        //Exit the program
                        case 4:
                            break;
                    }
                }
                else
                {
                    //Tell the user there's an error
                    custom_console_text("Error: Invalid input. Please enter a number between 1 and 4.", ["red"], 1);
                }
            }
            else
            {
                //Tell the user that there's an error
                custom_console_text("Invalid input. Please enter a valid number.", ["red"], 1);
            }
        } while (user_choice < 1 || user_choice > 4);

        //Exit the program after telling the user (No abrupt exit that confuses people)
        Console.WriteLine("\nExiting the program: ");
        console_load_animation(4);
        System.Threading.Thread.Sleep(1000);
    }
}