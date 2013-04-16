using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Game.Sprites;
using System.IO;
using System.Runtime.InteropServices;


namespace Game.States
{
    /// <summary>
    /// Class to generate the word search puzzle
    /// </summary>
    class WordSearch_WordGenerator
    {

        //Allows to display a message box
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        //-------------------------Globals for Generator-----------------------------------------

        /// <summary>
        /// number of words that can be put into the puzzle
        /// </summary>
        const int NWORDS = 40;
 
        /// <summary>
        /// number of rows in the word search
        /// </summary>
        const int ROW = 14;
 
        /// <summary>
        /// number of columns in the word search
        /// </summary>
        const int COL = 25;  

        /// <summary>
        /// length of the maximum word that can fit diagonally in the word puzzle, which is equal to ROW
        /// </summary>
        const int DIAGONAL = ROW;  

        /// <summary>
        /// list of words from a text file
        /// </summary>
        List<string> words = new List<string>();

        /// <summary>
        /// List of words that need to be found in the puzzle
        /// </summary>
        List<string> wordsToFind = new List<string>();

        /// <summary>
        /// Initialize a random generator
        /// </summary>
        Random generator = new Random();

        /// <summary>
        /// variable to hold the path to wordbank.txt
        /// </summary>
        FileStream file ;

        /// <summary>
        /// flag to see if a word has been used already or not
        /// </summary>
        bool usedWord = false;

        /// <summary>
        /// store the end and start positions of word inserted into the puzzle
        /// </summary>
        WordSearch_WordFind wordFinder;

        /// <summary>
        /// List of the positions of each word inserted into the grid
        /// </summary>
        static List<WordSearch_WordFind> listOfWordPositions = new List<WordSearch_WordFind>();

        /// <summary>
        /// method to reverse a string
        /// </summary>
        /// <param name="s">string to reverse</param>
        /// <returns>reversed string is returned</returns>
        public string stringReverse(string s) 
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);

            return new string(charArray);
        }

        /// <summary>
        /// method to grab and open a file
        /// </summary>
        /// <returns>returns the opened file</returns>
        public FileStream getFile()
        {
            FileStream file = null;

            try
            {
                //Make sure that the file exists.
                file = new FileStream("wordbank.txt", FileMode.Open, FileAccess.Read);
            }
            catch (FileNotFoundException ex)
            {
                //display the error message
                MessageBox(new IntPtr(0), ex.Message, ex.FileName, 0);
            }


            return file;
        }

        /// <summary>
        /// method to initialize a list of strings with words from the file
        /// </summary>
        /// <param name="file">the opened text file containing the words for the wordsearch</param>
        /// <returns>returns the list of words</returns>
        public List<string> initializeWordBank(FileStream file)
        {
            //Read the file
            StreamReader sr = new StreamReader(file);

            //Initialize a List of Strings
            List<string> words = new List<string>();

            while (!sr.EndOfStream)
            {
                //add the words from the file stream to the list of strings
                words.Add(sr.ReadLine());
            }

            return words;
        }
       
       /// <summary>
       /// method to return the list of word positions
       /// </summary>
       /// <returns>list of word positions</returns>
       public List<WordSearch_WordFind> getWordPositionList()
       {
            return listOfWordPositions;
       }


        //--------------------------------------Logic for Generating the Word Search Puzzle --------------------------------------------

        /// <summary>
        /// method to generate the word search puzzle
        /// </summary>
        /// <returns></returns>
        public char[,] generatePuzzle()
        {

            char[,] grid = new char[COL, ROW];
            listOfWordPositions.Clear();

            //Initialize the grid to empty characters
            for (int r = 0; r < COL; r++)
            {
                for (int c = 0; c < ROW; c++)
                {
                    grid[r, c] = ' ';
                }
            }

            //get the word bank file
            file = getFile();

            //populate the list of strings with words
            words = initializeWordBank(file);

            //Grab 6 words
            for (int i = 0; i < NWORDS; i++)
            {
                //used to flag a word to see if it fits or not in the grid
                bool validPosition = false;

                do
                {
                    //variable that holds the index of a random word in the list of strings
                    int index = generator.Next(words.Count);

                    //assign the random word to the string
                    string selWord = words[index];

                    //positions of the words
                    int xSel = generator.Next(COL);
                    int ySel = generator.Next(ROW);

                    //direction of the word(i.e Vertical, Horizontal or Diagonal)
                    int dir = generator.Next(6);

                    /* Try inserting the word into the grid. Depending on the "dir", it will either pick one of the 6:
                     * Vertical, Horizontal or Diagonal
                     * Backwards Vertical, Backwards Horizontal or Backwards Diagonal
                     */

                    if (dir == 0) //Vertical
                    {
                        //insert the word into the grid vertically (if it can fit)
                        validPosition = insertWord_Vertical(index, selWord, wordsToFind, words, xSel, ySel, grid, validPosition);                
                    }
                    else if (dir == 1) //Horizontal
                    {
                        //insert the word into the grid horizontally (if it can fit)
                        validPosition = insertWord_Horizontal(index, selWord, wordsToFind, words, xSel, ySel, grid, validPosition);             
                    }
                    else if (dir == 2) //diagonally
                    {
                        //insert the word into the grid diagonally (if it can fit)
                        validPosition = insertWord_Diagonal(index, selWord, wordsToFind, words, xSel, ySel, grid, validPosition);
                    }
                    else if (dir == 3) // Backwards Vertical
                    {
                        //insert the word into the grid vertically backwards (if it can fit)
                        validPosition = insertWordBack_Vertical(index, selWord, wordsToFind, words, xSel, ySel, grid, validPosition);
                    }
                    else if (dir == 4) // Backwards Horizontal
                    {
                        //insert the word into the grid horizontally backwards (if it can fit)
                        validPosition = insertWordBack_Horizontal(index, selWord, wordsToFind, words, xSel, ySel, grid, validPosition);
                    }
                    else if (dir == 5) // Backwards Diagonal
                    {
                        //insert the word into the grid diagonally backwards (if it can fit)
                        validPosition = insertWordBack_Diagonal(index, selWord, wordsToFind, words, xSel, ySel, grid, validPosition);
                    }

                    //if the word has been used...
                    if (usedWord)
                    {
                        //remove the word
                        words.Remove(selWord);
                        
                        //reset the flag
                        usedWord = false;
                    }

                } while (!validPosition);
            }

            //fill in rest of the empty spaces with random letters
            for (int r = 0; r < COL; r++)
            {
                for (int c = 0; c < ROW; c++)
                {
                    if (grid[r, c] == ' ')
                    {
                        //generate random letters and assign it to the empty spaces
                        grid[r, c] = (char)generator.Next(97, 123);
                        //grid[r, c] = ' ';
                    }
                }
            }

            return grid;
        }

        /// <summary>
        /// returns the list of words to find in the word search
        /// </summary>
        /// <returns>words to find list</returns>
        public List<string> getWordsToFind() 
        {
            return wordsToFind;
        }

        //----------------------------------------End of Logic for Generating the Word Search Puzzle --------------------------------------------

       
        //---------------------------------------------Start of insert_WordVertical--------------------------------------------------------------------------------

        /// <summary>
        /// method to insert a word vertically into the grid
        /// </summary>
        /// <param name="index">index of the word</param>
        /// <param name="selWord">the selected word</param>
        /// <param name="wordsToFind">list of words to find, used to add the sel word into it</param>
        /// <param name="words">list of all the words in the puzzle, used to add the sel word into it</param>
        /// <param name="xSel">X position of the selected word in the grid</param>
        /// <param name="ySel">Y position of the selected word in the grid</param>
        /// <param name="grid">the actual word search grid</param>
        /// <param name="validPosition">boolean used to see if the word can even fit in the puzzle at its location</param>
        /// <returns>return if the insert worked or not</returns>
        public bool insertWord_Vertical(int index, string selWord, List<string> wordsToFind, List<string> words, int xSel, int ySel, char[,] grid, bool validPosition)
        {

            if ((ySel + selWord.Length) < ROW) //Makes sure the word fits
            {
                
                // count of the word index
                int cntWordIndex = 0;

                //See if the word can fit
                for (int indexY = ySel; indexY < (ySel + selWord.Length); indexY++)
                {
                    //checks to see if can place a character in the current cell
                    if (grid[xSel, indexY ] != ' ' && grid[xSel, indexY] != selWord[cntWordIndex])
                    {
                        validPosition = false;
                        break;
                    }
                    else
                    {
                        validPosition = true;
                    }

                    cntWordIndex++;
                }
                //if you can place the character
                if (validPosition)
                {
                    cntWordIndex = 0;

                    //insert the word into the grid.
                    for (int indexY = ySel; indexY < (ySel + selWord.Length); indexY++)
                    {
                        
                        grid[xSel, indexY] = selWord[cntWordIndex];
                        cntWordIndex++;
                        
                    }

                    //store the positions of the word
                    wordFinder = new WordSearch_WordFind(new Point(xSel, ySel), new Point(xSel, (ySel + selWord.Length) - 1), selWord);

                    //add the positions to a list
                    listOfWordPositions.Add(wordFinder);

                    //add selected word to a list of words that need to be found
                    wordsToFind.Add(selWord);
                    usedWord = true;
                }
            }

            return validPosition;
        }

        //---------------------------------------------End of insertWord_Vertical--------------------------------------------------------------------------------


       //---------------------------------------------Start of insertWord_Horizontal--------------------------------------------------------------------------------

       /// <summary>
       /// method to insert a word horizontally into the grid
       /// </summary>
       /// <param name="index">index of the word</param>
       /// <param name="selWord">the selected word</param>
       /// <param name="wordsToFind">list of words to find, used to add the sel word into it</param>
       /// <param name="words">list of all the words in the puzzle, used to add the sel word into it</param>
       /// <param name="xSel">X position of the selected word in the grid</param>
       /// <param name="ySel">Y position of the selected word in the grid</param>
       /// <param name="grid">the actual word search grid</param>
       /// <param name="validPosition">boolean used to see if the word can even fit in the puzzle at its location</param>
       /// <returns>return if the insert worked or not</returns>
       public bool insertWord_Horizontal(int index, string selWord, List<string> wordsToFind, List<string> words, int xSel, int ySel, char[,] grid, bool validPosition)
       {
            if ((xSel + selWord.Length) < COL) //Makes sure the word fits
            {
                // count of the word index
                int cntWordIndex = 0;

                //See if the word can fit
                for (int indexX = xSel; indexX < (xSel + selWord.Length); indexX++)
                {
                    //checks to see if can place a character in the current cell
                    if (grid[indexX, ySel] != ' ' && grid[indexX, ySel] != selWord[cntWordIndex])
                    {
                        validPosition = false;
                        break;
                    }
                    else
                    {
                        validPosition = true;
                    }
                    cntWordIndex++;
                }
                //if you can place the character
                if (validPosition)
                {

                    cntWordIndex = 0;

                    //insert the word into the grid.
                    for (int indexX = xSel; indexX < (xSel + selWord.Length); indexX++)
                    {
                        grid[indexX, ySel] = selWord[cntWordIndex];
                        cntWordIndex++;
                    }

                    //store the positions of the word
                    wordFinder = new WordSearch_WordFind(new Point(xSel, ySel), new Point((xSel + selWord.Length) - 1, ySel), selWord);

                    //add the positions to a list
                    listOfWordPositions.Add(wordFinder);

                    //add selected word to a list of words that need to be found
                    wordsToFind.Add(selWord);
                    usedWord = true;
                }
            }

            return validPosition;
       }

       //---------------------------------------------End of insertWord_Horizontal--------------------------------------------------------------------------------

       //---------------------------------------------Start of insertWord_Diagonal--------------------------------------------------------------------------------
      
        /// <summary>
       /// method to insert a word diagonally into the grid
       /// </summary>
       /// <param name="index">index of the word</param>
       /// <param name="selWord">the selected word</param>
       /// <param name="wordsToFind">list of words to find, used to add the sel word into it</param>
       /// <param name="words">list of all the words in the puzzle, used to add the sel word into it</param>
       /// <param name="xSel">X position of the selected word in the grid</param>
       /// <param name="ySel">Y position of the selected word in the grid</param>
       /// <param name="grid">the actual word search grid</param>
       /// <param name="validPosition">boolean used to see if the word can even fit in the puzzle at its location</param>
       /// <returns>return if the insert worked or not</returns>
       public bool insertWord_Diagonal(int index, string selWord, List<string> wordsToFind, List<string> words, int xSel, int ySel, char[,] grid, bool validPosition)
       {
           if ((ySel + selWord.Length) < DIAGONAL) //Makes sure the word fits
           {

               // count of the word index
               int cntWordIndex = 0;

               //See if the word can fit
               for (int indexY = ySel; indexY < (ySel + selWord.Length); indexY++)
               {
                   //checks to see if can place a character in the current cell
                   if (grid[indexY, indexY] != ' ' && grid[indexY, indexY] != selWord[cntWordIndex])
                   {
                       validPosition = false;
                       break;
                   }
                   else
                   {
                       validPosition = true;
                   }

                   cntWordIndex++;
               }
               //if you can place the character
               if (validPosition)
               {
                   cntWordIndex = 0;

                   //insert the word into the grid.
                   for (int indexY = ySel; indexY < (ySel + selWord.Length); indexY++)
                   {

                       grid[indexY, indexY] = selWord[cntWordIndex];
                       cntWordIndex++;
                   }

                   //store the positions of the word
                   wordFinder = new WordSearch_WordFind(new Point(ySel, ySel), new Point((ySel + selWord.Length) - 1, (ySel + selWord.Length) - 1), selWord);

                   //add the positions to a list
                   listOfWordPositions.Add(wordFinder);

                   //add selected word to a list of words that need to be found
                   wordsToFind.Add(selWord);
                   usedWord = true;
               }
           }
           return validPosition;
       }

       //---------------------------------------------End of insertWord_Diagonal--------------------------------------------------------------------------------

        
       //---------------------------------------------Start of insertWordBack_Vertical--------------------------------------------------------------------------------

       /// <summary>
       /// method to insert a word backword vertically into the grid
       /// </summary>
       /// <param name="index">index of the word</param>
       /// <param name="selWord">the selected word</param>
       /// <param name="wordsToFind">list of words to find, used to add the sel word into it</param>
       /// <param name="words">list of all the words in the puzzle, used to add the sel word into it</param>
       /// <param name="xSel">X position of the selected word in the grid</param>
       /// <param name="ySel">Y position of the selected word in the grid</param>
       /// <param name="grid">the actual word search grid</param>
       /// <param name="validPosition">boolean used to see if the word can even fit in the puzzle at its location</param>
       /// <returns>return if the insert worked or not</returns>
       public bool insertWordBack_Vertical(int index, string selWord, List<string> wordsToFind, List<string> words, int xSel, int ySel, char[,] grid, bool validPosition)
       {

           selWord = stringReverse(selWord);

           if ((ySel + selWord.Length) < ROW) //Makes sure the word fits
           {

               // count of the word index
               int cntWordIndex = 0;

               //See if the word can fit
               for (int indexY = ySel; indexY < (ySel + selWord.Length); indexY++)
               {
                   //checks to see if can place a character in the current cell
                   if (grid[xSel, indexY] != ' ' && grid[xSel, indexY] != selWord[cntWordIndex])
                   {
                       validPosition = false;
                       break;
                   }
                   else
                   {
                       validPosition = true;
                   }

                   cntWordIndex++;
               }
               //if you can place the character
               if (validPosition)
               {
                   cntWordIndex = 0;

                   //insert the word into the grid.
                   for (int indexY = ySel; indexY < (ySel + selWord.Length); indexY++)
                   {

                       grid[xSel, indexY] = selWord[cntWordIndex];
                       cntWordIndex++;
                   }

                   selWord = stringReverse(selWord);

                   //store the positions of the word
                   wordFinder = new WordSearch_WordFind(new Point(xSel, (ySel + selWord.Length) - 1), new Point(xSel, ySel), selWord);

                   //add the positions to a list
                   listOfWordPositions.Add(wordFinder);

                   //add selected word to a list of words that need to be found
                   wordsToFind.Add(selWord);

                   usedWord = true;
               }
           }
           return validPosition;
       }

       //---------------------------------------------End of insertWordBack_Vertical--------------------------------------------------------------------------------


       //---------------------------------------------Start of insertWordBack_Horizontal--------------------------------------------------------------------------------

       /// <summary>
       /// method to insert a word backword horizontally into the grid
       /// </summary>
       /// <param name="index">index of the word</param>
       /// <param name="selWord">the selected word</param>
       /// <param name="wordsToFind">list of words to find, used to add the sel word into it</param>
       /// <param name="words">list of all the words in the puzzle, used to add the sel word into it</param>
       /// <param name="xSel">X position of the selected word in the grid</param>
       /// <param name="ySel">Y position of the selected word in the grid</param>
       /// <param name="grid">the actual word search grid</param>
       /// <param name="validPosition">boolean used to see if the word can even fit in the puzzle at its location</param>
       /// <returns>return if the insert worked or not</returns>
       public bool insertWordBack_Horizontal(int index, string selWord, List<string> wordsToFind, List<string> words, int xSel, int ySel, char[,] grid, bool validPosition)
       {
           selWord = stringReverse(selWord);

           if ((xSel + selWord.Length) < COL) //Makes sure the word fits
           {
               // count of the word index
               int cntWordIndex = 0;

               //See if the word can fit
               for (int indexX = xSel; indexX < (xSel + selWord.Length); indexX++)
               {
                   //checks to see if can place a character in the current cell
                   if (grid[indexX, ySel] != ' ' && grid[indexX, ySel] != selWord[cntWordIndex])
                   {
                       validPosition = false;
                       break;
                   }
                   else
                   {
                       validPosition = true;
                   }
                   cntWordIndex++;
               }
               //if you can place the character
               if (validPosition)
               {

                   cntWordIndex = 0;

                   //insert the word into the grid.
                   for (int indexX = xSel; indexX < (xSel + selWord.Length); indexX++)
                   {
                       grid[indexX, ySel] = selWord[cntWordIndex];
                       cntWordIndex++;
                   }

                   selWord = stringReverse(selWord);

                   //store the positions of the word
                   wordFinder = new WordSearch_WordFind(new Point((xSel + selWord.Length) - 1, ySel), new Point(xSel, ySel), selWord);
                   
                   //add the positions to a list
                   listOfWordPositions.Add(wordFinder);

                   //add selected word to a list of words that need to be found
                   wordsToFind.Add(selWord);

                   usedWord = true;
               }
           }

           return validPosition;
       }

       //---------------------------------------------End of insertWordBack_Horizontal--------------------------------------------------------------------------------
      

       //---------------------------------------------Start of insertWordBack_Diagonal--------------------------------------------------------------------------------


       /// <summary>
       /// method to insert a word backword diagonally into the grid
       /// </summary>
       /// <param name="index">index of the word</param>
       /// <param name="selWord">the selected word</param>
       /// <param name="wordsToFind">list of words to find, used to add the sel word into it</param>
       /// <param name="words">list of all the words in the puzzle, used to add the sel word into it</param>
       /// <param name="xSel">X position of the selected word in the grid</param>
       /// <param name="ySel">Y position of the selected word in the grid</param>
       /// <param name="grid">the actual word search grid</param>
       /// <param name="validPosition">boolean used to see if the word can even fit in the puzzle at its location</param>
       /// <returns>return if the insert worked or not</returns>
       public bool insertWordBack_Diagonal(int index, string selWord, List<string> wordsToFind, List<string> words, int xSel, int ySel, char[,] grid, bool validPosition)
       {
           selWord = stringReverse(selWord);

           if ((ySel + selWord.Length) < DIAGONAL) //Makes sure the word fits
           {

               // count of the word index
               int cntWordIndex = 0;

               //See if the word can fit
               for (int indexY = ySel; indexY < (ySel + selWord.Length); indexY++)
               {
                   //checks to see if can place a character in the current cell
                   if (grid[indexY, indexY] != ' ' && grid[indexY, indexY] != selWord[cntWordIndex])
                   {
                       validPosition = false;
                       break;
                   }
                   else
                   {
                       validPosition = true;
                   }

                   cntWordIndex++;
               }
               //if you can place the character
               if (validPosition)
               {
                   cntWordIndex = 0;

                   //insert the word into the grid.
                   for (int indexY = ySel; indexY < (ySel + selWord.Length); indexY++)
                   {

                       grid[indexY, indexY] = selWord[cntWordIndex];
                       cntWordIndex++;
                   }

                   selWord = stringReverse(selWord);

                   //store the positions of the word
                   wordFinder = new WordSearch_WordFind(new Point((ySel + selWord.Length) - 1, (ySel + selWord.Length) - 1), new Point(ySel, ySel), selWord);

                   //add the positions to a list
                   listOfWordPositions.Add(wordFinder);


                   //add selected word to a list of words that need to be found
                   wordsToFind.Add(selWord);

                   usedWord = true;
               }
           }

           return validPosition;
       }

       //---------------------------------------------End of insertWordBack_Diagonal--------------------------------------------------------------------------------
    }
}
