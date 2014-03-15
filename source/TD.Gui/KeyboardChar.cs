using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SdlDotNet.Core;
using SdlDotNet.Input;

namespace TD.Gui
{
    public class Keyboard
    {
        public static String GetChar(KeyboardEventArgs args)
        {
            String Entry = "";

            if (args.Key == Key.A)
            {
                Entry = "a";
            }
            else if (args.Key == Key.B)
            {
                Entry = "b";
            }
            else if (args.Key == Key.C)
            {
                Entry = "c";
            }
            else if (args.Key == Key.D)
            {
                Entry = "d";
            }
            else if (args.Key == Key.E)
            {
                Entry = "e";
            }
            else if (args.Key == Key.F)
            {
                Entry = "f";
            }
            else if (args.Key == Key.G)
            {
                Entry = "g";
            }
            else if (args.Key == Key.H)
            {
                Entry = "h";
            }
            else if (args.Key == Key.I)
            {
                Entry = "i";
            }
            else if (args.Key == Key.J)
            {
                Entry = "j";
            }
            else if (args.Key == Key.K)
            {
                Entry = "k";
            }
            else if (args.Key == Key.L)
            {
                Entry = "l";
            }
            else if (args.Key == Key.M)
            {
                Entry = "m";
            }
            else if (args.Key == Key.N)
            {
                Entry = "n";
            }
            else if (args.Key == Key.O)
            {
                Entry = "o";
            }
            else if (args.Key == Key.P)
            {
                Entry = "p";
            }
            else if (args.Key == Key.Q)
            {
                Entry = "q";
            }
            else if (args.Key == Key.R)
            {
                Entry = "r";
            }
            else if (args.Key == Key.S)
            {
                Entry = "s";
            }
            else if (args.Key == Key.T)
            {
                Entry = "t";
            }
            else if (args.Key == Key.U)
            {
                Entry = "u";
            }
            else if (args.Key == Key.V)
            {
                Entry = "v";
            }
            else if (args.Key == Key.W)
            {
                Entry = "w";
            }
            else if (args.Key == Key.X)
            {
                Entry = "x";
            }
            else if (args.Key == Key.Y)
            {
                Entry = "y";
            }
            else if (args.Key == Key.Z)
            {
                Entry = "z";
            }
            else if (args.Key == Key.Space)
            {
                Entry = " ";
            }
            else if (args.Key == Key.Zero)
            {
                Entry = "0";
            }
            else if (args.Key == Key.One)
            {
                Entry = "1";
            }
            else if (args.Key == Key.Two)
            {
                Entry = "2";
            }
            else if (args.Key == Key.Three)
            {
                Entry = "3";
            }
            else if (args.Key == Key.Four)
            {
                Entry = "4";
            }
            else if (args.Key == Key.Five)
            {
                Entry = "5";
            }
            else if (args.Key == Key.Six)
            {
                Entry = "6";
            }
            else if (args.Key == Key.Seven)
            {
                Entry = "7";
            }
            else if (args.Key == Key.Eight)
            {
                Entry = "8";
            }
            else if (args.Key == Key.Nine)
            {
                Entry = "9";
            }
            else if (args.Key == Key.Backspace)
            {
                Entry = "backspace";
            }
			
			if(args.Mod == ModifierKeys.LeftShift || args.Mod == ModifierKeys.RightShift)
			{
				Entry = Entry.ToUpper();
			}

            return Entry;
        }
    }
}
