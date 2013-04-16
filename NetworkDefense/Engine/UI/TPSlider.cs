using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;

namespace Engine.UI
{
    public class TPSlider : TPUIElement
    {

       private float minValue = 0;
        private float maxValue = 0;
        private float range;
        public TPSprite m_VolumeBar = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art\Menu\volumeBar"));
        public TPSprite m_VolumeCursor = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art\Menu\cursorSlider"));
        public float currentPos;

        public TPSlider()
        {
        }

        protected override void Load()
        {
            base.Load();
            currentPos = this.m_VolumeCursor.Position.X;
            minValue += this.m_VolumeBar.Position.X + 20;
            maxValue += this.m_VolumeBar.Position.X + 530;
            range = maxValue - minValue;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void MoveDown()
        {
            if ((currentPos - 20) > minValue)
            {
                this.m_VolumeCursor.Position.X -= 20;
                currentPos -= 20;
            }
        }


        public void MoveUp()
        {
            if ((currentPos + 20) < maxValue)
            {
                this.m_VolumeCursor.Position.X += 20;
                currentPos += 20;
            }
        }

        public void Select()
        {
            //m_MenuItemSet[m_Index].Select();
        }

        public float GetCurValue()
        {
            float retVal = (maxValue - minValue) / range;
            return retVal;
        }
        /// <summary>
        /// Manual override. 
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            m_VolumeBar.Draw(batch);
            m_VolumeCursor.Draw(batch);
        }

        private static void DoNothingCallback() { }

    }
}
