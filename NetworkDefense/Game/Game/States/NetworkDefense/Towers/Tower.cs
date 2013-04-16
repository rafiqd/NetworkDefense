using System;
using System.Collections.Generic;
using Game.States.TDsrc.Character;
using Microsoft.Xna.Framework;
using Game.States.TDsrc.Projectiles;
using Engine.Objects;
using Game.States.TDsrc.Stats;


namespace Game.States.TDsrc.Towers
{
    /// <summary>
    /// Parent class for all the towers, if you want to make a new tower class
    /// it must inherit from this.
    /// </summary>
    abstract class Tower
    {
        /// <summary>
        /// List of all the towers
        /// </summary>
        public static List<Tower> towerList;

        /// <summary>
        /// Static initalization block
        /// </summary>
        static Tower()
        {
            towerList = new List<Tower>();
        }

        /// <summary>
        /// Resets the towers lock
        /// </summary>
        public static void ResetTowerLocks()
        {
            FastTower.Unlocked = false;
            NormalTower.Unlocked = false;
            StrongTower.Unlocked = false;
            SprayTower.Unlocked = false;
        }


        /// <summary>
        /// current targets
        /// </summary>
        protected List<Enemy> currentList;

        /// <summary>
        /// X position of the center point of the tower on the grid
        /// </summary>
        public int xposCenter;

        /// <summary>
        /// X position of the center point of the tower on the grid
        /// </summary>
        public int yposCenter;

        /// <summary>
        /// the amount of grid spaces taken up in horizontal direction
        /// </summary>
        public int width;

        /// <summary>
        /// How much damage a tower attack does
        /// </summary>
        public double damage;

        /// <summary>
        /// The speed of the projectile the tower shoots
        /// </summary>
        public double speed;

        /// <summary>
        /// The rate the tower shoots at
        /// </summary>
        public TimeSpan fireRate;

        /// <summary>
        /// The range the tower can fire
        /// </summary>
        public double range;

        /// <summary>
        /// colour of the tower
        /// </summary>
        public string Color;

        /// <summary>
        /// time of the last tower shot
        /// </summary>
        private TimeSpan previousFireTime;

        /// <summary>
        /// sprite that represents the tower
        /// </summary>
        public TPSprite towerSprite;

        /// <summary>
        /// parent constructor to initalize variables
        /// </summary>
        /// <param name="damage">damage of the tower</param>
        /// <param name="speed">speed of the tower's projectile</param>
        /// <param name="firerate">rate of fire of the tower</param>
        /// <param name="range">tower's range</param>
        /// <param name="centerX">x position to build</param>
        /// <param name="centerY">y position to build</param>
        /// <param name="cost">cost to build</param>
        /// <param name="color">colour of the tower</param>
        public Tower(int damage, double speed, TimeSpan firerate, double range,
                     int centerX, int centerY, int cost, string color)
        {
            Color = color;
            this.damage = damage;
            this.speed = speed;
            this.fireRate = firerate;
            this.range = range;
            xposCenter = centerX;
            yposCenter = centerY;
            previousFireTime = TimeSpan.Zero;
            TDPlayerStats.Money -= cost;
            currentList = new List<Enemy>();
        }

        /// <summary>
        /// Updates the tower, all it currently does is shoot, if you need to
        /// update anything call it from here.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void update(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - previousFireTime) > fireRate)
            {
                previousFireTime = gameTime.TotalGameTime;
                shootTargets();
            }
        }

        /// <summary>
        /// updates all the towers
        /// </summary>
        /// <param name="gametime"></param>
        public static void Update(GameTime gametime)
        {
            for (int i = 0; i < towerList.Count; ++i)
                towerList[i].update(gametime);
        }

        /// <summary>
        /// Main tower logic should go here
        /// 
        /// Finds a list of enemy targets, this will be different for every tower type
        /// some towers may only be able to shoot 1 target at a time while others might do AOE
        /// and hit multiple targets at once
        /// 
        /// Note this is where the logic should go on finding targets(s), use the withinRange 
        /// then do what ever you want to determine if it's a valid target, can be multiple targets
        /// etc, what ever you want.
        /// </summary>
        /// <returns>A list of targets</returns>
        public abstract List<Enemy> findTarget();

        /// <summary>
        /// Shoots a projectile at an enemy
        /// </summary>
        public void shootTargets()
        {
            if (currentList.Count == 0)
            {
                currentList = findTarget();
            }
            else if (isWithinRange(currentList[0]) == -1 || currentList[0].dead)
            {
                currentList = findTarget();
            }

            //List<Enemy> enemyList = findTarget();
            for (int i = 0; i < currentList.Count; ++i)
            {
                Projectile.projectileList.Add(new BasicBolt(damage, speed, currentList[i], xposCenter, yposCenter, Color));
            }
        }

        /// <summary>
        /// Checks if the target is within range of the tower.
        /// </summary>
        /// <returns>If an enemy is within range it returns the distance
        /// other wise it returns -1</returns>
        public double isWithinRange(Enemy enemy)
        {
            double xdist = xposCenter - (enemy.xpos * 5);
            double ydist = yposCenter - (enemy.ypos * 5);
            double distance = Math.Sqrt(Math.Pow(xdist, 2) + Math.Pow(ydist, 2));
            if (distance <= range)
                return distance;
            return -1;
        }
    }
}
