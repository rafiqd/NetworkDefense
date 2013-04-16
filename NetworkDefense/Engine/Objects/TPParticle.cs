using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.Objects
{
    public class TPParticle : TPVisibleEntity
    {
        private Texture2D m_Texture;

        protected int m_FrameSize; 
        protected int m_Frame;
        protected int m_Timer;
        protected int m_LifeTimer;
        protected int m_LifeTimerMax;
        protected bool m_DieOnAnimEnd = false;
        public int FrameRate { get; set; }

        public TPParticle(Texture2D tex,int lifeTime = 3000,int frameRate = 16,bool dieOnAnim = false)
            : base()
        {
            m_Texture = tex;
            Width = m_Texture.Width;
            Height = m_Texture.Height;
            m_FrameSize = Height;
            FrameRate = frameRate;
            m_DieOnAnimEnd = dieOnAnim;
            m_LifeTimer = m_LifeTimerMax = lifeTime;
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (Alive)
            {
                m_Timer -= gameTime.ElapsedGameTime.Milliseconds;
                if (m_Timer <= 0)
                {
                    m_Timer = 1000 / FrameRate;
                    m_Frame++;
                    if (((m_Frame * m_FrameSize) + m_FrameSize) >= Width)
                    {
                        m_Frame = 0;
                        if (m_DieOnAnimEnd)
                        {
                            this.Kill();
                        }
                    }
                }
                m_LifeTimer -= gameTime.ElapsedGameTime.Milliseconds;
                if (m_LifeTimer <= 0)
                {
                    this.Kill();
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            if (Alive)
            {
                batch.Draw(m_Texture, new Microsoft.Xna.Framework.Rectangle((int)Position.X, (int)Position.Y, (int)(m_FrameSize * Scale.X), (int)(m_FrameSize * Scale.Y)), new Microsoft.Xna.Framework.Rectangle((m_Frame * m_FrameSize), 0, m_FrameSize, m_FrameSize), Color.White);
            }
        }

        
    }
}
