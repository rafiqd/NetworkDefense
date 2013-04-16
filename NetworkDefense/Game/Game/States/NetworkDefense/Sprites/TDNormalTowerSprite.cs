using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Game.States.TDsrc.Environment;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.Sprites
{
    /// <summary>
    /// Sprite for the towers
    /// </summary>
    class TDNormalTowerSprite : TPSprite
    {
        /// <summary>
        /// Texture used by towers
        /// </summary>
        public static Texture2D NormalTowerTexture;

        /// <summary>
        /// static initalization, loads the texture used by towers
        /// </summary>
        static TDNormalTowerSprite()
        {
            NormalTowerTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/Tower");
        }

        /// <summary>
        /// Constructor for the tower sprite
        /// </summary>
        /// <param name="texture">custom texture for a tower, leave empty if you want the default</param>
        public TDNormalTowerSprite(Texture2D texture = null)
            : base(NormalTowerTexture)
        {
            if (texture == null)
                m_Texture = NormalTowerTexture;
            else
                m_Texture = texture;
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Game.anim1.Update(gameTime.ElapsedGameTime.Ticks * 5);
        }

        /// <summary>
        /// draws the sprite
        /// </summary>
        /// <param name="batch">current spritebatch</param>
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Game.anim1.GetTexture(), Position ,Color.White);
        }
    }
}
