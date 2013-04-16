using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Sprites
{
    /// <summary>
    /// Ammo bar class, providing the sprite of ammobar for birdhunt game
    /// </summary>
    class Ammobar : TPSprite
    {
        /// <summary>
        /// class variable to keep track of the ammo quantity
        /// </summary>
        private static int ammo;

        /// <summary>
        /// static variable used to keep track of the ammo quantity
        /// </summary>
        public static int Ammo
        {
            get { return ammo; }
            set { ammo = value; }
        }

        /// <summary>
        /// texture for a bullet
        /// </summary>
        Texture2D bullet;

        /// <summary>
        /// Peter
        /// constructor for Ammobar class, initialze the ammo bar and load the bullet sprites
        /// </summary>
        public Ammobar()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/ammobar"))
        {
            ammo = 30;
            bullet = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/bullet");
        }

        /// <summary>
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        /// <summary>
        /// Peter
        /// Draw function.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            for (int i = 0; i < ammo; i++)
            {
                batch.Draw(bullet, new Vector2(Position.X + (i * 25), Position.Y + 5), null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
            }
        }

        /// <summary>
        /// Peter
        /// calculate the ammo when the user fires a bullet in-game
        /// </summary>
        public static void ShotFired()
        {
            if (ammo != 0)
            {
                ammo -= 1;
            }
        }

        /// <summary>
        /// Cyril
        /// reload the ammo to 30
        /// </summary>
        public static void Reload()
        {
            if (ammo < 30)
            {
                ammo = 30;
            }
        }
    }
}
