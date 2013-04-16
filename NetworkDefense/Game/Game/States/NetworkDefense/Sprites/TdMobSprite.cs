using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Game.States.TDsrc.Character;
using Game.States.TDsrc.Environment;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Game.States;


namespace Game.Sprites
{
    /// <summary>
    /// Sprite for the enemy
    /// </summary>
    public class TdMobSprite : TPSprite
    {
        /// <summary>
        /// Green healthbar represents their current life
        /// </summary>
        private Texture2D greenHealth;

        /// <summary>
        /// redhealthbar represents their total life
        /// </summary>
        private Texture2D redHealth;

        /// <summary>
        /// Current health
        /// </summary>
        public int currHealth;

        /// <summary>
        /// Maximum health
        /// </summary>
        public int maxHealth;

        /// <summary>
        /// Consturctor
        /// </summary>
        public TdMobSprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/mob"))
        {   
            m_Texture = TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/mob");
            greenHealth = TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/greenhealth");
            redHealth = TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/redhealth");
        }

        /// <summary>
        /// Loads the resources for the sprite
        /// </summary>
        /// <param name="pos">position vector</param>
        /// <param name="curArea"></param>
        /// <param name="curLayer">layer to draw at</param>
        /// <param name="curPos"></param>
        public void Load(Vector2 pos, int curArea, int curLayer, Point curPos){}

        /// <summary>
        /// Updates this instance of mobsprite
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Game.anim2.Update(gameTime.ElapsedGameTime.Ticks);

        }

        /// <summary>
        /// Draws the mob sprite and the healthbar above the mob
        /// </summary>
        /// <param name="batch">current spritebatch</param>
        public override void Draw(SpriteBatch batch)
        {
            double result = (currHealth/(double)maxHealth)*m_Texture.Width;
            batch.Draw(Game.anim2.GetTexture(),new Rectangle((int)Position.X, (int)Position.Y - 4, m_Texture.Width, m_Texture.Height),Color.White);
            batch.Draw(redHealth,new Rectangle((int)Position.X,(int)Position.Y - 20,m_Texture.Width,3), Color.Red);
            batch.Draw(greenHealth, new Rectangle((int)Position.X, (int)Position.Y - 20, (int)result, 3), Color.Green);
        }
    }
}
