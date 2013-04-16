using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.Objects
{
    public abstract class TPVisibleEntity : TPEntity
    {
        /// <summary>
        /// Direct memeber access issues with public properties. Is now an exposed field. 
        /// </summary>
        public Color RenderColor;
        public virtual void Draw(SpriteBatch batch) { }


    }
}
