using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Sprites
{
    public class DemoTileSprite : IsometricSprite
    {
        Texture2D frameTexture;
        public DemoTileSprite leader { set; get; }
        public bool visited { set; get; }

        public DemoTileSprite(Vector2 pos, DemoPlayerSprite sprite, Point mapPos)
            : base(pos, "tilefull", sprite, mapPos)
        {
            frameTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/tile");
            Position = pos;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.Draw(m_Texture, worldPosition + (playerSprite.Position - playerSprite.roomPosition), new Color(255, 255, 255, 100));
            batch.Draw(frameTexture, worldPosition + (playerSprite.Position - playerSprite.roomPosition), Color.White);
        }
    }
}
