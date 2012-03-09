using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsContentLoading
{
    public static class Extensions
    {
        public static char ToChar(this Keys key)
        {
            string characterToString = " ";
            switch (key)
            {
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    characterToString = Enum.GetName(typeof(Keys), key);
                    break;
            }

            return characterToString[0];
        }
    }
}
