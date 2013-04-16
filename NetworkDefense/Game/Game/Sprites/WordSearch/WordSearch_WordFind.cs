using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game.Sprites
{
    /// <summary>
    /// Class to hold methods for checking the positions of words
    /// </summary>
    class WordSearch_WordFind
    {
        /// <summary>
        /// variables to store the start and end points of the characters from the word search
        /// </summary>
        Point startPoint,
              endPoint;

        /// <summary>
        /// if the word has been found already, then dont check again
        /// </summary>
        bool alreadyFound = false;

        /// <summary>
        /// the word that corresponds to the start and end positions passed in
        /// </summary>
        string wordToBeFound = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">start point of the character Pos</param>
        /// <param name="end">end point of the character Pos</param>
        public WordSearch_WordFind(Point start, Point end, string word) 
        {
            wordToBeFound = word;

            startPoint = new Point((start.X * 30) + 260, (start.Y * 30) + 25);
            endPoint = new Point((end.X * 30) + 260, (end.Y * 30) + 25);
        }


        public string getWord() 
        {
            return wordToBeFound;
        }

        /// <summary>
        /// Checks to see if the position of the word to find positions is the same as the mouse clicks positions
        /// </summary>
        /// <param name="p1">point of the first click</param>
        /// <param name="p2">point of the second click</param>
        /// <returns></returns>
        public bool checkWord(Point p1, Point p2)
        {

            //moves any point that was clicked within the character sprite to the top left corner of that sprite image
            int p1X = ((int)((p1.X - 260) / 30)) * 30 + 260;
            int p1Y = ((int)((p1.Y - 25)/30))*30 + 25;

            //moves any point that was clicked within the character sprite to the top left corner of that sprite image
            int p2X = ((int)((p2.X - 260) / 30)) * 30 + 260;
            int p2Y = ((int)((p2.Y - 25)/30))*30 + 25;

            Point newP1 = new Point(p1X, p1Y);
            Point newP2 = new Point(p2X, p2Y);


            if (alreadyFound)
            {
                return false;
            }
            else if ((newP1 == startPoint && newP2 == endPoint) || (newP1 == endPoint && newP2 == startPoint))
            {
                alreadyFound = true;
                return true;
            }

            return false;
        }
    }
}
