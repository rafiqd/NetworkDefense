using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.States.TDsrc.Character;
using Game.States.TDsrc.Management;
using Game.Sprites;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.States.TDsrc.Projectiles
{
    /// <summary>
    /// BasicBolt which inheirts from Projectile represents a simple
    /// bolt that travels in a line from the tower to the target, use this if you don't
    /// need anything fancy for your bolt.
    /// </summary>
    class BasicBolt : Projectile
    {
        /// <summary>
        /// Width of the texture
        /// </summary>
        static int TextureWidth = 30;

        /// <summary>
        /// center pixel x position
        /// </summary>
        private int centerX;

        /// <summary>
        /// center pixel y position
        /// </summary>
        private int centerY;

        /// <summary>
        /// number of projectiles made in total
        /// </summary>
        static int count = 0;

        /// <summary>
        /// unique id of the projectile
        /// </summary>
        public int id;

        /// <summary>
        /// tells if the projectile is alive or dead
        /// </summary>
        public bool alive;

        /// <summary>
        /// BasicBolt Constructor
        /// Sets the damage, speed, target, and current x/y positions, and colour
        /// </summary>
        /// <param name="damage">Damage the projectile does</param>
        /// <param name="speed">Speed the projectile travels at</param>
        /// <param name="enemy">Target of the projectile</param>
        /// <param name="xpos">Starting X position in pixels</param>
        /// <param name="ypos">starting Y position in pixels</param>
        /// <param name="color">Colour of the projectile</param>
        public BasicBolt(double damage, double speed, Enemy enemy, int xpos, int ypos, string color) 
            :base(damage, speed, enemy, color)
        {
            centerX = xpos;
            centerY = ypos;
            projectileSprite = new TDProjectileSprite();
            TowerDefenseManager.TDLayers[2].AddEntity(projectileSprite);
            projectileSprite.Position.Y = centerY ;
            projectileSprite.Position.X = centerX ;
            id = count++;
            alive = true;

        }

        /// <summary>
        /// Updates the projectile
        /// </summary>
        /// <param name="gametime">current game time</param>
        protected override void update(Microsoft.Xna.Framework.GameTime gametime)
        {
            if(alive)
                moveToTarget();
        }

        /// <summary>
        /// Moves the projectile 1 pixel up/down and left/right till it's at equal x / y positions with the target
        /// </summary>
        protected override void moveToTarget()
        {
            double distX = projectileSprite.Position.X - ((target.xpos * 5)+3);
            double distY = projectileSprite.Position.Y - ((target.ypos * 5)+3);
            double targetX = target.xpos * 5;
            double targetY = target.ypos * 5;

            if (distX < 5 && distX > -5)
                distX = 0;

            if (distY < 5 && distY > -5)
                distY = 0;

            if (distX > 0)
                projectileSprite.Position.X -= (float) speed;
            else if (distX < 0)
                projectileSprite.Position.X += (float) speed;

            if (distY > 0)
                projectileSprite.Position.Y -= (float) speed;
            else if (distY < 0)
                projectileSprite.Position.Y += (float) speed;


            if (distX == 0 && distY == 0)
                triggerDeath();

            if (projectileSprite.Position.X == targetX && projectileSprite.Position.Y == targetY)
            {
                triggerDeath();
            }

        }

        /// <summary>
        /// Happens when the projectile dies, all logic involved with a projectile hitting
        /// the target should be done here.
        /// </summary>
        public void triggerDeath()
        {
            alive = false;
            projectileList.Remove(this);
            projectileSprite.Kill();
            TowerDefenseManager.TDLayers[2].RemoveEntity(projectileSprite);
            target.health -= TDDamageManager.ModifiedDamage(this);
            
        }

    }
}
