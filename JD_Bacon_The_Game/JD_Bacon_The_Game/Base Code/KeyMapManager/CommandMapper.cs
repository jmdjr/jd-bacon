using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace JD_Bacon_The_Game
{
    /// <summary>
    /// Maps keyboard and controller keys to unique names to allow customizing 
    /// the controls at runtime.
    /// </summary>
    public static class CommandMapper
    {
        /// <summary>
        /// Error codes for why mapping functions failed.
        /// </summary>
        [Flags]
        public enum MappingError : uint
        {
            NO_ERROR = 0,
            DUPLICATES_NOT_ALLOWED = 1 << 1,
            NAME_ALREADY_EXISTS = 1 << 2,
            KEY_ALREADY_MAPPED_ON_CONTROLLER = 1 << 3,
            KEY_ALREADY_MAPPED_ON_KEYBOARD = 1 << 4,
            MAPPING_NOT_FOUND = 1 << 5,
        }


        public enum MappingDevice : uint
        {
            CONTROLLER,
            KEYBOARD,
        }

        /// <summary>
        /// Maps the keys on a keyboard to strings
        /// </summary>
        private static Dictionary<string, Keys[]> _keyboardMap = new Dictionary<string, Keys[]>();

        /// <summary>
        /// Maps a specific button configuration to a string (buttons is a flag so it can have multiple buttons simultaneously
        /// </summary>
        private static Dictionary<string, Buttons> _controllerMap = new Dictionary<string, Buttons>();

        /// <summary>
        /// If true, allows more than one name to have the same key or button configuration.
        /// set to false by default.
        /// </summary>
        public static bool AllowDuplicateCommands { get; set; }

        /// <summary>
        /// If true, identical names will be allowed to exist between the keys and controllers.<para/>
        /// Example uses: <para/>
        /// set to true if you want to allow the game to switch between keyboard and controllers.<para/>
        /// set to false if you want to keep all names unique.
        /// </summary>
        public static bool AllowBridgedCommands { get; set; }

        /// <summary>
        /// Error Value used to identify the details as to why a specific mapping function failed to execute properly.
        /// </summary>
        public static MappingError LastError { get; set; }

        /// <summary>
        /// Adds a new Key-set, Command-Name pair to the keyboard mapping.
        /// </summary>
        /// <param name="commandName">Command to be assigned to the key set</param>
        /// <param name="keys">Keyboard keys to map to this Command</param>
        /// <returns>True if the mapping was added successfully</returns>
        public static bool AddMapping(string commandName, Keys[] keys)
        {
            LastError = MappingError.NO_ERROR;

            if (_keyboardMap.Keys.Contains(commandName))
            {
                LastError |= MappingError.NAME_ALREADY_EXISTS;
            }

            if (!AllowBridgedCommands && _controllerMap.Keys.Contains(commandName))
            {
                LastError |= MappingError.KEY_ALREADY_MAPPED_ON_CONTROLLER;
            }

            if (!AllowDuplicateCommands && _keyboardMap.Values.Contains(keys))
            {
                LastError |= MappingError.DUPLICATES_NOT_ALLOWED;
            }

            if (LastError != MappingError.NO_ERROR)
            {
                return false;
            }

            _keyboardMap.Add(commandName, keys);
            return true;
        }

        /// <summary>
        /// Adds a new button, Name pair to the controller mapping.
        /// </summary>
        /// <param name="commandName">Name to be assigned to the button combination</param>
        /// <param name="buttons">controller button combination to map to this name</param>
        /// <returns>True if the mapping was added successfully</returns>
        public static bool AddMapping(string commandName, Buttons buttons)
        {
            LastError = MappingError.NO_ERROR;

            if (_controllerMap.Keys.Contains(commandName))
            {
                LastError |= MappingError.NAME_ALREADY_EXISTS;
            }

            if (!AllowBridgedCommands && _keyboardMap.Keys.Contains(commandName))
            {
                LastError |= MappingError.KEY_ALREADY_MAPPED_ON_KEYBOARD;
            }

            if (!AllowDuplicateCommands && _controllerMap.Values.Contains(buttons))
            {
                LastError |= MappingError.DUPLICATES_NOT_ALLOWED;
            }

            if (LastError != MappingError.NO_ERROR)
            {
                return false;
            }

            _controllerMap.Add(commandName, buttons);

            return true;
        }

        public static bool UpdateMapping(string commandName, Keys[] keys)
        {
            LastError = MappingError.NO_ERROR;

            if (!_keyboardMap.Keys.Contains(commandName))
            {
                LastError |= MappingError.MAPPING_NOT_FOUND;
            }

            if (LastError != MappingError.NO_ERROR)
            {
                return false;
            }

            _keyboardMap[commandName] = keys;

            return true;
        }
        public static bool UpdateMapping(string commandName, Buttons buttons)
        {
            LastError = MappingError.NO_ERROR;

            if (!_controllerMap.Keys.Contains(commandName))
            {
                LastError |= MappingError.MAPPING_NOT_FOUND;
            }

            if (LastError != MappingError.NO_ERROR)
            {
                return false;
            }

            _controllerMap[commandName] = buttons;

            return true;
        }

        public static bool DeleteMapping(string commandName, MappingDevice device)
        {
            LastError = MappingError.NO_ERROR;

            if ((device == MappingDevice.KEYBOARD && !_keyboardMap.Keys.Contains(commandName)) ||
                (device == MappingDevice.CONTROLLER && _controllerMap.Keys.Contains(commandName)))
            {
                LastError |= MappingError.MAPPING_NOT_FOUND;
            }

            if (LastError != MappingError.NO_ERROR)
            {
                return false;
            }

            switch (device)
            {
                case MappingDevice.KEYBOARD:
                    _keyboardMap.Remove(commandName);
                    break;

                case MappingDevice.CONTROLLER:
                    _controllerMap.Remove(commandName);
                    break;
            }

            return true;
        }

        /// <summary>
        /// Checks the keyboardState passed in and verifies if the command name has been inputted by the user.
        /// </summary>
        /// <param name="commandName">command name to check</param>
        /// <param name="keyStates">the keyboardState identifying what keys are pressed.</param>
        /// <returns>true if the command is being activated by the KeboardState</returns>
        public static bool CheckCommand(string commandName, KeyboardState keyStates)
        {
            LastError = MappingError.NO_ERROR;

            if (!_keyboardMap.Keys.Contains(commandName))
            {
                LastError |= MappingError.MAPPING_NOT_FOUND;
            }

            if (LastError != MappingError.NO_ERROR)
            {
                return false;
            }

            // returns true only if all keys associated with this command are down.
            return _keyboardMap[commandName].All(keys => keyStates.IsKeyDown(keys));
        }

        /// <summary>
        /// Checks the Gamepad passed in and verifies if the command name has been inputted by the user.
        /// </summary>
        /// <param name="commandName">command name to check</param>
        /// <param name="buttonStates">the button combination identifying what buttons are pressed</param>
        /// <returns>true if the command is being activated by the GamePadButtons</returns>
        public static bool CheckCommand(string commandName, GamePadButtons buttonStates)
        {
            LastError = MappingError.NO_ERROR;

            if (!_controllerMap.Keys.Contains(commandName))
            {
                LastError |= MappingError.MAPPING_NOT_FOUND;
            }

            if (LastError != MappingError.NO_ERROR)
            {
                return false;
            }

            // this should return true if the buttons set in the controller map are the same as those in the buttonStates.
            return buttonStates.Equals(new GamePadButtons(_controllerMap[commandName]));
        }
    }
}
