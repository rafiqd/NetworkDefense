using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.Objects
{
    public class TPSprite : TPVisibleEntity
    {
        /// <summary>
        /// The texture this sprite uses.
        /// </summary>
        protected Texture2D m_Texture;

        public SpriteEffects Effect { get; set; }
        /// <summary>
        /// The draw color of the sprite. Used to tint the sprite and affect the sprites alpha value.
        /// </summary>
        protected Color drawColor = Color.White;
        public TPSprite(string pathToTex)
            : this(TPEngine.Get().TextureManager.LoadTexture(pathToTex))
        {

        }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tex"></param>
        public TPSprite(Texture2D tex)
        {
            m_Texture = tex;
            Alive = true;
            Width = tex.Width;
            Height = tex.Height;
        }
        /// <summary>
        /// Load function.
        /// </summary>
        protected override void Load()
        {
            base.Load();
            Width = m_Texture.Width;
            Height = m_Texture.Height;
        }

        
        /// <summary>
        /// Draw function.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        { 
            // if ((Visible) && (m_Texture != null))
            if (Alive && m_Texture != null)
            {
                batch.Draw(m_Texture, Position, null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
                //Render children. Interesting side effect is that the way this is implemented, children can have children and so on infinitely. 
            }
        }

        public Texture2D GetTexture()
        {
            return m_Texture;
        }
    }
}
