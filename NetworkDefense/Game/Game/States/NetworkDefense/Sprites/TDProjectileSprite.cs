using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;


namespace Game.Sprites
{
    /// <summary>
    /// Projectile sprite
    /// </summary>
    class TDProjectileSprite : TPSprite
    {
        /// <summary>
        /// consturctor
        /// </summary>
        public TDProjectileSprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/bolt"))
        {}
    }
}
