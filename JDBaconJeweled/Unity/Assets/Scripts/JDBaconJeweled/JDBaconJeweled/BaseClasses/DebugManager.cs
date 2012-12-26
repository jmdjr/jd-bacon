// --------------------------------------------------------------------------------
// Copyright (C)2010 BitFlip Games
// Written by Guy Somberg guy@bitflipgames.com
//
// I, the copyright holder of this work, hereby release it into the public domain.
// This applies worldwide.  In case this is not legally possible, I grant any
// entity the right to use this work for any purpose, without any conditions,
// unless such conditions are required by law.
// --------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

public class DebugManager: MonoBehaviour
{
    private DebugConsole DebugConsole = null;

    public bool ConsoleEnabled
    {
        get { return (DebugConsole != null) ? DebugConsole.DisplayConsole : false; }
    }

    void Awake()
    {
        DebugConsole = new DebugConsole();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (DebugConsole.DisplayConsole)
            {
                DebugConsole.DisplayConsole = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            DebugConsole.DisplayConsole = true;
        }
    }

    void OnGUI()
    {
        DebugConsole.OnGUI();
    }
}
public class DebugConsole
{
    private string ConsoleText = "";
    private bool displayConsole = false;
    public bool DisplayConsole
    {
        get { return displayConsole; }
        set
        {
            displayConsole = value;
            if (!DisplayConsole)
            {
                ConsoleText = "";
                PreviousCommandIndex = -1;
            }
        }
    }

    private List<string> PreviousCommands = new List<string>();
    private int PreviousCommandIndex = -1;

    private string AutoCompleteBase = "";
    private List<string> AutoCompleteOptions = new List<string>();
    private int AutoCompleteOptionsIndex = -1;

    private ConsoleCommand GetCommand(string CommandText)
    {
        foreach (ConsoleCommand Command in ConsoleCommands.Instance.Commands)
        {
            if (Command.CommandText.Equals(CommandText, StringComparison.CurrentCultureIgnoreCase))
            {
                return Command;
            }
        }
        return null;
    }

    private void ExecuteCommand(string CommandText)
    {
        CommandText = CommandText.Trim();
        PreviousCommands.Add(CommandText);

        string[] SplitCommandText = CommandText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        ConsoleCommand Command = GetCommand(SplitCommandText[0]);
        if (Command != null)
        {
            Command.Callback(SplitCommandText);
        }
    }

    private void AutoComplete()
    {
        string AutoCompleteText = AutoCompleteBase.Trim().ToLower();

        if (AutoCompleteOptionsIndex < 0)
        {
            AutoCompleteOptions.Clear();
            foreach (ConsoleCommand Command in ConsoleCommands.Instance.Commands)
            {
                if (Command.CommandText.ToLower().StartsWith(AutoCompleteText))
                {
                    AutoCompleteOptions.Add(Command.CommandText);
                }
            }
            AutoCompleteOptions.Sort();

            if (AutoCompleteOptions.Count > 0)
            {
                AutoCompleteOptionsIndex = 0;
                PreviousCommandIndex = -1;
            }
        }
        else
        {
            if (AutoCompleteOptions.Count > 0)
            {
                AutoCompleteOptionsIndex = (AutoCompleteOptionsIndex + 1) % AutoCompleteOptions.Count;
            }
            else
            {
                AutoCompleteOptionsIndex = -1;
            }
        }

        if (AutoCompleteOptionsIndex >= 0)
        {
            ConsoleText = AutoCompleteOptions[AutoCompleteOptionsIndex];
        }
    }

    private void ClearAutoComplete()
    {
        AutoCompleteBase = "";
        AutoCompleteOptions.Clear();
        AutoCompleteOptionsIndex = -1;
    }
    public void OnGUI()
    {
        if (DisplayConsole)
        {
            string BaseText = ConsoleText;
            if (PreviousCommandIndex >= 0)
            {
                BaseText = PreviousCommands[PreviousCommandIndex];
            }

            Event CurrentEvent = Event.current;
            if ((CurrentEvent.isKey) &&
                (!CurrentEvent.control) &&
                (!CurrentEvent.shift) &&
                (!CurrentEvent.alt))
            {
                bool isKeyDown = (CurrentEvent.type == EventType.KeyDown);
                if (isKeyDown)
                {
                    if (CurrentEvent.keyCode == KeyCode.Return || CurrentEvent.keyCode == KeyCode.KeypadEnter)
                    {
                        ExecuteCommand(BaseText);
                        DisplayConsole = false;
                        return;
                    }

                    if (CurrentEvent.keyCode == KeyCode.UpArrow)
                    {
                        if (PreviousCommandIndex <= -1)
                        {
                            PreviousCommandIndex = PreviousCommands.Count - 1;
                            ClearAutoComplete();
                        }
                        else if (PreviousCommandIndex > 0)
                        {
                            PreviousCommandIndex--;
                            ClearAutoComplete();
                        }
                        return;
                    }

                    if (CurrentEvent.keyCode == KeyCode.DownArrow)
                    {
                        if (PreviousCommandIndex == PreviousCommands.Count - 1)
                        {
                            PreviousCommandIndex = -1;
                            ClearAutoComplete();
                        }
                        else if (PreviousCommandIndex >= 0)
                        {
                            PreviousCommandIndex++;
                            ClearAutoComplete();
                        }
                        return;
                    }

                    if (CurrentEvent.keyCode == KeyCode.Tab)
                    {
                        if (AutoCompleteBase.Length == 0)
                        {
                            AutoCompleteBase = BaseText;
                        }
                        AutoComplete();
                        return;
                    }
                }
            }

            GUI.SetNextControlName("ConsoleTextBox");
            Rect TextFieldRect = new Rect(0.0f, (float)Screen.height - 20.0f, (float)Screen.width, 20.0f);

            string CommandText = GUI.TextField(TextFieldRect, BaseText);
            if (PreviousCommandIndex == -1)
            {
                ConsoleText = CommandText;
            }
            if (CommandText != BaseText)
            {
                ConsoleText = CommandText;
                PreviousCommandIndex = -1;
                ClearAutoComplete();
            }
            GUI.FocusControl("ConsoleTextBox");
        }
    }
}

