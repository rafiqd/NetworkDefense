using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.States.TDsrc.Stats
{
    /// <summary>
    /// Holds the stats for the player's game
    /// </summary>
    static class TDPlayerStats
    {
        /// <summary>
        /// Players current money
        /// </summary>
        public static int Money;

        /// <summary>
        /// Players current Grade
        /// </summary>
        public static int Grade;

        /// <summary>
        /// Amount of enemies player has killed
        /// </summary>
        public static int KillCount;

        /// <summary>
        /// Current wave of enemies
        /// </summary>
        public static int currentWave;

        /// <summary>
        /// maximum number of waves
        /// </summary>
        public static int maxWaves;

        /// <summary>
        /// Static initalization
        /// </summary>
        static TDPlayerStats()
        {
            ResetStats();
        }

        /// <summary>
        /// resets stats
        /// </summary>
        public static void ResetStats()
        {
            Money = 20;
            Grade = 100;
            KillCount = 0;
            currentWave = 0;
            maxWaves = 8;
        }
    }
}
