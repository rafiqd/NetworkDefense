/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Game.Sprites;

namespace Game.Area
{
    /// <summary>
    /// AreaInfo describes the basic characteristics of an area such as the size, where doors lead, etc.
    /// </summary>
    public struct AreaInfo
    {
        public int areaX, areaY, startX, startY;
        public char[][] tileTypeArray;
        public TileSprite[][] tileSearchArray;
        public List<string> targets;
        public string areaName;
    }

    /// <summary>
    /// FileReader reads and parses a text file for use by the AreaBuilder
    /// </summary>
    public static class FileReader
    {
        /// <summary>
        /// Get the area info from the text file
        /// </summary>
        /// <param name="mapName">The name of the text file to load</param>
        /// <returns>The area information from the text file</returns>
        public static AreaInfo GetAreaInfo(string mapName)
        {
            AreaInfo areaInfo = new AreaInfo();
            areaInfo.targets = new List<string>();
            using (StreamReader reader = new StreamReader("Content/areas/" + mapName + ".txt"))
            {
                // Read the first line to determine the number of lines and columns tha need to be read.
                string line = reader.ReadLine();
                int locationOfDelimiter = line.IndexOf(',');
                areaInfo.areaY = int.Parse(line.Substring(0, locationOfDelimiter));
                areaInfo.areaX = int.Parse(line.Substring(locationOfDelimiter + 1, line.Length - locationOfDelimiter - 1));

                // Instanciate the tile array according to the size specified in the text file and read in each character.
                areaInfo.tileTypeArray = new char[areaInfo.areaY][];

                for(int r = 0; r < areaInfo.areaY; r++)
                {
                    line = reader.ReadLine();
                    areaInfo.tileTypeArray[r] = new char[areaInfo.areaX];
                    for(int c = 0; c < areaInfo.areaX; c++){
                        areaInfo.tileTypeArray[r][c] = line[c];
                    }
                }

                // Read in the door targets.
                while (!reader.EndOfStream)
                {
                    areaInfo.targets.Add(reader.ReadLine());
                }

                // Return the parsed data.
                return areaInfo;
            }
        }
    }
}
