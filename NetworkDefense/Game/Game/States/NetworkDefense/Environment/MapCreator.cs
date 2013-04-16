using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.States.TDsrc.Environment
{
    /// <summary>
    /// Used to build and load maps
    /// </summary>
    internal class MapCreator
    {
        /// <summary>
        /// Reference to a map that you want to edit
        /// </summary>
        private Map map;

        /// <summary>
        /// MapCreator ctor, edits the map that is passed in
        /// </summary>
        /// <param name="map">Map that you want to edit</param>
        public MapCreator(Map map)
        {
            this.map = map;
        }

        /// <summary>
        /// Updates the map creator, used for creating maps.
        /// </summary>
        /// <param name="xpos">mouse x position </param>
        /// <param name="ypos">mouse y position </param>
        public void update(int xpos, int ypos)
        {
            bool botleft = true;
            bool botright = true;
            bool topleft = true;
            bool topright = true;


            // convert mouse xy pos into tile coords
            xpos = (int) Math.Floor(xpos/(double) map.size);
            ypos = (int) Math.Floor(ypos/(double) map.size);



            // creates an "+" shape to convert grass to path
            for (int i = 0; i <= 7; ++i)
            {

                // if's are to make sure you don't get out of bounds on the array
                if (xpos >= 0 && xpos < map.map[0].Length)
                {

                    if (ypos + i < map.map.Length && ypos + i >= 0)
                    {
                        if (i == 0)
                            map.map[ypos + i][xpos] = 2;
                        else if (map.map[ypos + i][xpos] != 2)
                            map.map[ypos + i][xpos] = 1; // bot
                    }
                    else
                    {
                        botleft = false;
                        botright = false;
                    }

                    if (ypos - i >= 0 && ypos - i < map.map.Length)
                    {
                        if (i == 0)
                            map.map[ypos - i][xpos] = 2;
                        else if (map.map[ypos - i][xpos] != 2)
                            map.map[ypos - i][xpos] = 1; // top
                    }
                    else
                    {
                        topleft = false;
                        topright = false;
                    }
                }

                if (ypos >= 0 && ypos < map.map.Length)
                {
                    if (xpos + i < map.map[0].Length && xpos + i >= 0)
                    {
                        if (i == 0)
                            map.map[ypos][xpos + i] = 2;
                        else if (map.map[ypos][xpos + i] != 2)
                            map.map[ypos][xpos + i] = 1; // right
                    }
                    else
                    {
                        topright = false;
                        botright = false;
                    }

                    if (xpos - i >= 0 && xpos - i < map.map[0].Length)
                    {
                        if (i == 0)
                            map.map[ypos][xpos - i] = 2;
                        else if (map.map[ypos][xpos - i] != 2)
                            map.map[ypos][xpos - i] = 1; // left
                    }
                    else
                    {
                        topleft = false;
                        botleft = false;
                    }

                    if (topleft && map.map[ypos - i][xpos - i] != 2)
                        map.map[ypos - i][xpos - i] = 1;


                    if (topright && map.map[ypos - i][xpos + i] != 2)
                        map.map[ypos - i][xpos + i] = 1;


                    if (botleft && map.map[ypos + i][xpos - i] != 2)
                        map.map[ypos + i][xpos - i] = 1;


                    if (botright && map.map[ypos + i][xpos + i] != 2)
                        map.map[ypos + i][xpos + i] = 1;

                }

            }

        }

        /// <summary>
        /// Saves a map to a file -- Currently hard coded for my dir, change if the dir if you want to use this
        /// </summary>
        public void save()
        {
            int i, j;
            
            StreamWriter sw = new StreamWriter("NewCurrentMapData.dat");
            for (i = 0; i < map.map.Length; ++i)
            {
                for (j = 0; j < map.map[i].Length; ++j)
                {
                    sw.Write(map.map[i][j] + " ");
                }
                sw.WriteLine();
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// Loads previously saved map
        /// Saved map must be the same area or larger than the current screen size, if it is larger parts will be cut out
        /// this shouldn't be an issue.
        /// also hard coded for my dir change it if you want to use on your machine.
        /// </summary>
        public void load(string path)
        {
            StreamReader sw = new StreamReader(path);
            int i, j;
            string[] line;

            for (i = 0; i < map.map.Length; ++i)
            {
                line = sw.ReadLine().Split(' ');

                for (j = 0; j < map.map[i].Length; ++j)
                {
                    map.map[i][j] = int.Parse(line[j]);
                }
            }
        }
    }
}