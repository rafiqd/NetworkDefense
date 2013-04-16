using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.States.TDsrc.Stats;
using Game.Sprites;
using Game.States.TDsrc.Management;
using Game.States.TDsrc.Character;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.States.TDsrc.Towers
{

    /// <summary>
    /// Spray Tower class
    /// </summary>
    class SprayTower : Tower
    {
        /// <summary>
        /// Damage the tower does
        /// </summary>
        public static int Damage = 8;

        /// <summary>
        /// Speed the tower's projectile moves at
        /// </summary>
        public static double Speed = 5;

        /// <summary>
        /// Rate of fire
        /// </summary>
        public static TimeSpan Rate = TimeSpan.FromMilliseconds(700);

        /// <summary>
        /// Range of the tower
        /// </summary>
        public static int Range = 150;

        /// <summary>
        /// If the tower class is unlocked
        /// </summary>
        public static bool Unlocked;

        /// <summary>
        /// Cost of the tower to build
        /// </summary>
        public static int Cost;

        /// <summary>
        /// Cost to unlock the tower class
        /// </summary>
        public static int CostToUnlock;

        /// <summary>
        /// Static initalization
        /// </summary>
        static SprayTower()
        {
            Unlocked = false;
            Cost = 40;
            CostToUnlock = 40;
        }

        /// <summary>
        /// Unlocks this class of tower
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
        /// SprayTower constructor
        /// </summary>
        /// <param name="centerX">x position to build at</param>
        /// <param name="centerY">y position to build at</param>
        /// <param name="color">colour of the tower</param>
        public SprayTower( int centerX, int centerY, string color) :
            base(Damage, Speed, Rate, Range, centerX, centerY, Cost, color)
        {
            Tower.towerList.Add(this);
            towerSprite = new TDNormalTowerSprite();
            TowerDefenseManager.TDLayers[2].AddEntity(towerSprite);
            towerSprite.Position.Y = centerY - (towerSprite.Width) /2; // times 5 to convert from tiles to pixels
            towerSprite.Position.X = centerX - (towerSprite.Width) / 2;
        }

        /// <summary>
        /// Finds the targets that the enemy will then shoot
        /// </summary>
        /// <returns>returns a list of targets</returns>
        public override List<Enemy> findTarget()
        {
            List<Enemy> target = new List<Enemy>();
            double shortestdist = double.MaxValue;
            for (int i = 0; i < Enemy.enemyList.Count; ++i)
            {
                double dist = isWithinRange(Enemy.enemyList[i]);
                if (dist != -1 && dist < shortestdist)
                {
                    target.Add(Enemy.enemyList[i]);
                    if (target.Count >= 3)
                        break;
                }
            }
            return target;
        }
    }
}
