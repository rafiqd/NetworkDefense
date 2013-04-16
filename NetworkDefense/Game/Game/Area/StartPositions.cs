/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game.Area
{
    /// <summary>
    /// StartPositions is a data class that defines certain magic numbers that represent important world positions like where in each
    /// area a player will start if the are in that area on load.  This class is a singleton.
    /// </summary>
    class StartPositions
    {
        /// <summary>
        /// startPoints define the place the player will start depending on what room they are in.
        /// </summary>
        private static Dictionary<string, Point> startPoints = new Dictionary<string,Point>();

        /// <summary>
        /// backdropPoints define the offset for drawing the backdrops.
        /// </summary>
        private static Dictionary<string, Vector2> backdropPoints = new Dictionary<string,Vector2>();

        /// <summary>
        /// The static instance of this singleton class.
        /// </summary>
        private static StartPositions instance;
        
        /// <summary>
        /// Private constructor sets up the points on the first call of this class.
        /// </summary>
        private StartPositions()
        {
            startPoints.Add("lab", new Point(5, 8));
            startPoints.Add("lecturehall", new Point(11, 8));
            startPoints.Add("dormroom", new Point(11, 0));
            startPoints.Add("studyhall", new Point(5, 0));

            backdropPoints.Add("hall", new Vector2(-70, -146));
            backdropPoints.Add("lab", new Vector2(-15, -146));
            backdropPoints.Add("lecturehall", new Vector2(-70, -146));
            backdropPoints.Add("dormroom", new Vector2(314, -146));
            backdropPoints.Add("studyhall", new Vector2(258, -146));
        }

        /// <summary>
        /// Provides access to the singleton instance of the class.  If there is no current instance, instanciates it.
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static StartPositions Get()
        {
            if (instance == null)
            {
                instance = new StartPositions();
            }
            return instance;
        }

        /// <summary>
        /// Gets the start point associated with a certain area
        /// </summary>
        /// <param name="s">The name of the area</param>
        /// <returns>The start point for the specified area</returns>
        public Point GetPoint(string s)
        {
            return startPoints[s];
        }

        /// <summary>
        /// Gets the backdrop position for a certain area
        /// </summary>
        /// <param name="s">The name of the area</param>
        /// <returns>The backdrop position for the specified area</returns>
        public Vector2 GetPosition(string s)
        {
            return backdropPoints[s];
        }
    }
}
