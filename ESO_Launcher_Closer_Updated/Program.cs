//============================================================================
// Name        : ESO Launcher Closer Updated
// Author(s)   : Draco9990 and rewritten by Tygrtraxx
// Version     : 2
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
    *		The text that will be output by the console later on
    * @var color
    *		The color that will be used for the text
    *=============================================================================================*/
    private static void custom_console_text(string text_input, string color)
    {
        //Make sure the color is in lowercase to prevent bugs
        color = color.ToLower();

        //Convert the color input to a ConsoleColor enum value
        ConsoleColor consoleColor = ConsoleColor.White; // Default color
        switch (color)
        {
            case "red":
                consoleColor = ConsoleColor.Red;
                break;
            case "green":
                consoleColor = ConsoleColor.Green;
                break;
            case "blue":
                consoleColor = ConsoleColor.Blue;
                break;
            case "yellow":
                consoleColor = ConsoleColor.Yellow;
                break;
            case "cyan":
                consoleColor = ConsoleColor.Cyan;
                break;
            case "magenta":
                consoleColor = ConsoleColor.Magenta;
                break;
            case "white":
                consoleColor = ConsoleColor.White;
                break;
            default:
                Console.WriteLine("Invalid color input. Using default color (white).");
                break;
        }

        //Set the console color and write a colored message and then reset the color
        Console.ForegroundColor = consoleColor;
        Console.WriteLine("\n\n" + text_input + "\n");
        Console.ResetColor();
    }

    /*=============================================================================================*
    * Function: modify_registry(string action)
    *
    * Description: This function will set a registry key so the program automatcially runs on startup
    * @var action
    *		The action that will happen next based on the user's input (Create or Delete reg key)
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
                        custom_console_text("Registry key has been added successfully.", "green");
                    }

                    else
                    {
                        //Tell the user the registry key already exists
                        custom_console_text("The registry key already exists.", "cyan");
                    }
                }

                else
                {
                    //Tell the user something went wrong
                    custom_console_text("Error: Unable to add the registry key. (Invalid path ??)", "red");
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
                        custom_console_text("Registry key has been deleted successfully.", "green");
                    }

                    else
                    {
                        //Tell the user the registry key doesn't exist
                        custom_console_text("The registry key doesn't exist.", "yellow");
                    }
                }

                else
                {
                    //Tell the user something went wrong
                    custom_console_text("Error: Unable to add the registry key. (Invalid path ??)", "red");
                }

                break;

            default:
                //Tell the user something went wrong
                custom_console_text("Error: Unknown action (Somehow you bypassed create or delete registry key 'actions'??)", "red");
                break;
        }
    }

    /*=============================================================================================*
    * Function: console_loading_animation()
    *
    * Description: This function will give the illusion of loading to the user to show its working
    *@var total_looped_times
    *		The amount of times we want to loop the whole animation for loading, to be shown to the user
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
                            Console.WriteLine("\nStarting the Program: ");
                            console_load_animation(5);
                            Console.WriteLine("\n\nProgram is now running.");

                            //While the program is active, we will find all of the launcher processes and end them
                            while (true)
                            {
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
                    custom_console_text("Error: Invalid input. Please enter a number between 1 and 4.", "red");
                }
            }
            else
            {
                //Tell the user that there's an error
                custom_console_text("Invalid input. Please enter a valid number.", "red");
            }
        } while (user_choice < 1 || user_choice > 4);

        //Exit the program after telling the user (No abrupt exit that confuses people)
        Console.WriteLine("\nExiting the program: ");
        console_load_animation(4);
        System.Threading.Thread.Sleep(1000);
    }
}