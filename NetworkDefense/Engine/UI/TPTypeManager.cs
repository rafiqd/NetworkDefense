using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Engine.UI
{
    public static class TPTypeManager
    {
        public static char getChar(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'A' : 'a';
            }

            if (currentKeyboardState.IsKeyDown(Keys.B) && previousKeyboardState.IsKeyUp(Keys.B))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'B' : 'b';
            }

            if (currentKeyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'C' : 'c';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D) && previousKeyboardState.IsKeyUp(Keys.D))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'D' : 'd';
            }

            if (currentKeyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'E' : 'e';
            }

            if (currentKeyboardState.IsKeyDown(Keys.F) && previousKeyboardState.IsKeyUp(Keys.F))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'F' : 'f';
            }

            if (currentKeyboardState.IsKeyDown(Keys.G) && previousKeyboardState.IsKeyUp(Keys.G))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'G' : 'g';
            }

            if (currentKeyboardState.IsKeyDown(Keys.H) && previousKeyboardState.IsKeyUp(Keys.H))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'H' : 'h';
            }

            if (currentKeyboardState.IsKeyDown(Keys.I) && previousKeyboardState.IsKeyUp(Keys.I))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'I' : 'i';
            }

            if (currentKeyboardState.IsKeyDown(Keys.J) && previousKeyboardState.IsKeyUp(Keys.J))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'J' : 'j';
            }

            if (currentKeyboardState.IsKeyDown(Keys.K) && previousKeyboardState.IsKeyUp(Keys.K))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'K' : 'k';
            }

            if (currentKeyboardState.IsKeyDown(Keys.L) && previousKeyboardState.IsKeyUp(Keys.L))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'L' : 'l';
            }

            if (currentKeyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'M' : 'm';
            }

            if (currentKeyboardState.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'N' : 'n';
            }

            if (currentKeyboardState.IsKeyDown(Keys.O) && previousKeyboardState.IsKeyUp(Keys.O))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'O' : 'o';
            }

            if (currentKeyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'P' : 'p';
            }

            if (currentKeyboardState.IsKeyDown(Keys.Q) && previousKeyboardState.IsKeyUp(Keys.Q))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'Q' : 'q';
            }

            if (currentKeyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyUp(Keys.R))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'R' : 'r';
            }

            if (currentKeyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'S' : 's';
            }

            if (currentKeyboardState.IsKeyDown(Keys.T) && previousKeyboardState.IsKeyUp(Keys.T))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'T' : 't';
            }

            if (currentKeyboardState.IsKeyDown(Keys.U) && previousKeyboardState.IsKeyUp(Keys.U))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'U' : 'u';
            }

            if (currentKeyboardState.IsKeyDown(Keys.V) && previousKeyboardState.IsKeyUp(Keys.V))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'V' : 'v';
            }

            if (currentKeyboardState.IsKeyDown(Keys.W) && previousKeyboardState.IsKeyUp(Keys.W))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'W' : 'w';
            }

            if (currentKeyboardState.IsKeyDown(Keys.X) && previousKeyboardState.IsKeyUp(Keys.X))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'X' : 'x';
            }

            if (currentKeyboardState.IsKeyDown(Keys.Y) && previousKeyboardState.IsKeyUp(Keys.Y))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'Y' : 'y';
            }

            if (currentKeyboardState.IsKeyDown(Keys.Z) && previousKeyboardState.IsKeyUp(Keys.Z))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? 'Z' : 'z';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D1) && previousKeyboardState.IsKeyUp(Keys.D1))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '!' : '1';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D2) && previousKeyboardState.IsKeyUp(Keys.D2))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '@' : '2';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D3) && previousKeyboardState.IsKeyUp(Keys.D3))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '#' : '3';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D4) && previousKeyboardState.IsKeyUp(Keys.D4))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '$' : '4';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D5) && previousKeyboardState.IsKeyUp(Keys.D5))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '%' : '5';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D6) && previousKeyboardState.IsKeyUp(Keys.D6))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '^' : '6';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D7) && previousKeyboardState.IsKeyUp(Keys.D7))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '&' : '7';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D8) && previousKeyboardState.IsKeyUp(Keys.D8))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '*' : '8';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D9) && previousKeyboardState.IsKeyUp(Keys.D9))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '(' : '9';
            }

            if (currentKeyboardState.IsKeyDown(Keys.D0) && previousKeyboardState.IsKeyUp(Keys.D0))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? ')' : '0';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemTilde) && previousKeyboardState.IsKeyUp(Keys.OemTilde))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '~' : '`';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemMinus) && previousKeyboardState.IsKeyUp(Keys.OemMinus))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '_' : '-';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemPlus) && previousKeyboardState.IsKeyUp(Keys.OemPlus))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '+' : '=';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemOpenBrackets) && previousKeyboardState.IsKeyUp(Keys.OemOpenBrackets))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '{' : '[';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemCloseBrackets) && previousKeyboardState.IsKeyUp(Keys.OemCloseBrackets))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '}' : ']';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemSemicolon) && previousKeyboardState.IsKeyUp(Keys.OemSemicolon))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? ':' : ';';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemQuotes) && previousKeyboardState.IsKeyUp(Keys.OemQuotes))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '"' : '\'';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemPipe) && previousKeyboardState.IsKeyUp(Keys.OemPipe))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '|' : '\\';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemComma) && previousKeyboardState.IsKeyUp(Keys.OemComma))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '<' : ',';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemPeriod) && previousKeyboardState.IsKeyUp(Keys.OemPeriod))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '>' : '.';
            }

            if (currentKeyboardState.IsKeyDown(Keys.OemQuestion) && previousKeyboardState.IsKeyUp(Keys.OemQuestion))
            {
                return (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift)) ? '?' : '/';
            }

            if (currentKeyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                return ' ';
            }

            if (currentKeyboardState.IsKeyDown(Keys.Back) && previousKeyboardState.IsKeyUp(Keys.Back))
            {
                return 'ß';
            }

            return '©';
        }
    }
}
