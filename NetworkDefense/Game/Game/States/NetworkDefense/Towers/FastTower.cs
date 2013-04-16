using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.States.TDsrc.Stats;
using Game.States.TDsrc.Management;
using Game.Sprites;
using Game.States.TDsrc.Character;

namespace Game.States.TDsrc.Towers
{
    /// <summary>
    /// Represents a FastTower : inherits from Tower
    /// </summary>
    class FastTower : Tower
    {
        /// <summary>
        /// Damage the tower does
        /// </summary>
        public static int Damage = 3;

        /// <summary>
        /// Speed of the projectile
        /// </summary>
        public static double Speed = 3;

        /// <summary>
        /// Intervals between the shots fired
        /// </summary>
        public static TimeSpan Rate = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Range of the tower in pixels
        /// </summary>
        public static int Range = 250;

        /// <summary>
        /// Is this class of tower unlocked
        /// </summary>
        public static bool Unlocked;

        /// <summary>
        /// Cost to build a tower
        /// </summary>
        public static int Cost;

        /// <summary>
        /// Cost to unlock the tower class
        /// </summary>
        public static int CostToUnlock;

        /// <summary>
        /// static initalization
        /// </summary>
        static FastTower()
        {
            Unlocked = false;
            Cost = 10;
            CostToUnlock = 10;
        }

        /// <summary>
        /// Unlocks this tower class
        /// </summary>
        /// <returns>true if it unlocked, false if it didn't</returns>
        public static bool UnlockTowerClass()
        {
            if ((TDPlayerStats.Money - CostToUnlock) >= 0)
            {
                Unlocked = true;
                TDPlayerStats.Money -= CostToUnlock;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="centerX">x position it's built at</param>
        /// <param name="centerY">y position it's built at</param>
        /// <param name="color">colour of the tower</param>
        public FastTower( int centerX, int centerY, string color) :
            base(Damage, Speed, Rate, Range, centerX, centerY, Cost, color)
        {
            Tower.towerList.Add(this);
            towerSprite = new TDNormalTowerSprite();
            TowerDefenseManager.TDLayers[2].AddEntity(towerSprite);
            towerSprite.Position.Y = centerY - (towerSprite.Width) /2; // times 5 to convert from tiles to pixels
            towerSprite.Position.X = centerX - (towerSprite.Width) / 2;
        }

        /// <summary>
        /// Finds the targts for this tower to shoot
        /// </summary>
        /// <returns>List of enemies to shoot</returns>
        public override List<Enemy> findTarget()
        {
            List<Enemy> target = new List<Enemy>();
            double shortestdist = double.MaxValue;
            for (int i = 0; i < Enemy.enemyList.Count; ++i)
            {
                double dist = isWithinRange(Enemy.enemyList[i]);
                if (dist != -1 && dist < shortestdist)
                {
                    if (target.Count == 0)
                        target.Add(Enemy.enemyList[i]);
                    else
                        target[0] = Enemy.enemyList[i];
                    shortestdist = dist;
                }
            }
            return target;
        }
    }
}
