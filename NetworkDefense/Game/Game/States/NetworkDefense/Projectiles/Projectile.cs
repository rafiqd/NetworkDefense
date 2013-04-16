using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Game.States.TDsrc.Character;
using Engine.Objects;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.States.TDsrc.Projectiles
{
    /// <summary>
    /// Class represents a projectile, developers should inherit from this class and implement the movetotarget()
    /// method to modify how their projectile should move, eg, straight line, in a curve or anything fancy.
    /// </summary>
    abstract class Projectile
    {
        #region static variables/methods
        /// <summary>
        /// List of projectiles
        /// </summary>
        public static List<Projectile> projectileList;

        /// <summary>
        /// Static ctor that initalizes the projectile list
        /// </summary>
        static Projectile()
        {
            projectileList = new List<Projectile>();
        }

        /// <summary>
        /// Updates all the projectiles, this should be the only method called from the main
        /// game area
        /// </summary>
        public static void UpdateProjectiles(GameTime gametime)
        {
            for (int i = 0; i < projectileList.Count; ++i)
                projectileList[i].update(gametime);
        }

        #endregion

        /// <summary>
        /// Damage that the projectile does
        /// </summary>
        public double damage;

        /// <summary>
        /// Speed at which the projectile travels
        /// </summary>
        public double speed;

        /// <summary>
        /// Target that the projectile is aiming at
        /// </summary>
        public Enemy target;

        /// <summary>
        /// Colour of the projectile, this is set by the tower
        /// </summary>
        public string Color;

        /// <summary>
        /// sprite that represents the projectile
        /// </summary>
        public TPSprite projectileSprite;

        /// <summary>
        /// Projectile ctor, sets the damage, speed, target, and colour
        /// </summary>
        /// <param name="damage">Damage projectile will do</param>
        /// <param name="speed">Speed it travels</param>
        /// <param name="enemy">Target it's aiming at</param>
        /// <param name="color">colour of the projectile</param>
        public Projectile(double damage, double speed, Enemy enemy, string color)
        {
            this.damage = damage;
            this.speed = speed;
            target = enemy;
            Color = color;
        }

        /// <summary>
        /// This one should call moveToTarget, and any other updating methods required
        /// </summary>
        /// <param name="gametime">current game time</param>
        abstract protected void update(GameTime gametime);

        /// <summary>
        /// Logic for the movment of the projectile should go here
        /// </summary>
        abstract protected void moveToTarget();
    }
}
