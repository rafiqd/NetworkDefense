using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.States.TDsrc.Character;
using Game.States.TDsrc.Management;
using Game.Sprites;
using Game.States.TDsrc.Stats;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.States.TDsrc.Towers
{
    /// <summary>
    /// Normal Tower Class
    /// </summary>
    class NormalTower : Tower
    {
        /// <summary>
        /// Damage the tower does
        /// </summary>
        public static int Damage = 15;

        /// <summary>
        /// Speed of the tower's projectile
        /// </summary>
        public static double Speed = 5;

        /// <summary>
        /// Rate of fire
        /// </summary>
        public static TimeSpan Rate = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Range of the tower
        /// </summary>
        public static int Range = 120;

        /// <summary>
        /// if the tower class is unlocked
        /// </summary>
        public static bool Unlocked;

        /// <summary>
        /// cost to build a tower
        /// </summary>
        public static int Cost;

        /// <summary>
        /// cost to unlock this tower class
        /// </summary>
        public static int CostToUnlock;

        /// <summary>
        /// static initalization block
        /// </summary>
        static NormalTower()
        {
            Unlocked = false;
            Cost = 10;
            CostToUnlock = 0;
        }

        /// <summary>
        /// unlocks this class of tower
        /// </summary>
        /// <returns></returns>
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
        /// Normal Tower constructor
        /// </summary>
        /// <param name="centerX">x position</param>
        /// <param name="centerY">y position</param>
        /// <param name="color">color</param>
        public NormalTower( int centerX, int centerY, string color) :
            base(Damage, Speed, Rate, Range, centerX, centerY, Cost, color)
        {
            Tower.towerList.Add(this);
            towerSprite = new TDNormalTowerSprite();
            TowerDefenseManager.TDLayers[2].AddEntity(towerSprite);
            towerSprite.Position.Y = centerY - (towerSprite.Width) /2; // times 5 to convert from tiles to pixels
            towerSprite.Position.X = centerX - (towerSprite.Width) / 2;
        }

        /// <summary>
        /// Finds the targets for the tower to shoot
        /// </summary>
        /// <returns>list of targets for the tower to shoot</returns>
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
