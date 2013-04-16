using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game.States.TDsrc.Environment
{
    /// <summary>
    /// Class represents the map that the game uses 
    /// </summary>
    public class Map
    {
        /// <summary>
        /// A static reference to the currently used map
        /// </summary>
        public static Map currentMap;

        /// <summary>
        /// The 2D array that represents the map
        /// </summary>
        public int[][] map;

        /// <summary>
        /// Amount of pixels one tile in the  map translates to
        /// </summary>
        public int size = 5;

        /// <summary>
        /// Creates a new Map
        /// </summary>
        /// <param name="ixlen">number of tiles wide</param>
        /// <param name="iylen">number of tiles high</param>
        public Map(int ixlen, int iylen)
        {
            int xlen = ixlen / size;
            int ylen = iylen / size;
            map = new int[ylen][];
            for (int i = 0; i < ylen; ++i)
            {
                map[i] = new int[xlen];
                for (int j = 0; j < xlen; ++j)
                    map[i][j] = 0;
            }
            currentMap = this;
        }
    }
}
