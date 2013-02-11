using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class DebugCommands
{
    public static DebugCommands Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new DebugCommands();
            }

            return instance;
        }
    }
    private static DebugCommands instance;
    private DebugCommands()
    {
        Commands = new List<ConsoleCommand>() {
            new ConsoleCommand("Help", "This displays the help text for all events.", HelpCommand),
        };
    }


    public List<ConsoleCommand> Commands;

    public void HelpCommand(string[] Params)
    {
        if (Params.Length == 0)
        {
            StringBuilder builder = new StringBuilder();
            string formatString = "{0,-20} {1}\n";
            foreach (ConsoleCommand command in Commands)
            {
                builder = builder.AppendFormat(formatString, command.CommandText, command.ShortHelpText);
            }

            Debug.Log(builder.ToString());
        }
        else if (this.Commands.Any(c => c.CommandText == Params[0]))
        {
            PrintFullHelpText(this.Commands.First(c => c.CommandText == Params[0]));
        }
    }

    public void AddCommand(ConsoleCommand command)
    {
        if (command != null)
        {
            this.Commands.Add(command);
        }
    }

    public static bool IsHelpSwitch(string Param)
    {
        return Param == "/?";
    }

    public static void PrintFullHelpText(ConsoleCommand command)
    {
        Debug.Log(command.FullHelpText + "\n");
    }
}

public class ConsoleCommand
{
    public delegate void Command(string[] Params);
    public ConsoleCommand(string CommandText, string HelpText, Command Callback)
    {
        this.CommandText = CommandText;
        this.ShortHelpText = HelpText;
        this.Callback = Callback;
        this.FullHelpText = CommandText + ": No details...\n";
    }

    public ConsoleCommand(string CommandText, string HelpText, string fullHelpText, Command Callback)
    {
        this.CommandText = CommandText;
        this.ShortHelpText = HelpText;
        this.Callback = Callback;
        this.FullHelpText = CommandText + ": \n" + fullHelpText;
    }

    public string CommandText;
    public string ShortHelpText;
    public string FullHelpText;
    public Command Callback;
}
