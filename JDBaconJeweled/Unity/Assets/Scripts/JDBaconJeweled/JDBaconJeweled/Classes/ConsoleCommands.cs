using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

public class ConsoleCommands
{
    private ConsoleCommands()
    {
        Commands = new List<ConsoleCommand>() {
            new ConsoleCommand("Help", "This displays the help text for all events.", HelpCommand),
        };
    }

    private static ConsoleCommands instance;
    public static ConsoleCommands Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ConsoleCommands();
            }

            return instance;
        }
    }

    public List<ConsoleCommand> Commands;

    public void HelpCommand(string[] Params)
    {
        StringBuilder builder = new StringBuilder();
        string formatString = "{0,-20} {1}\n";
        foreach (ConsoleCommand command in Commands)
        {
            builder = builder.AppendFormat(formatString, command.CommandText, command.ShortHelpText);
        }

        Debug.Log(builder.ToString());
    }

    public void AddCommand(ConsoleCommand command)
    {
        if (command != null)
        {
            this.Commands.Add(command);
        }
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
    }

    public string CommandText;
    public string ShortHelpText;
    public string FullHelpText;
    public Command Callback;
}
