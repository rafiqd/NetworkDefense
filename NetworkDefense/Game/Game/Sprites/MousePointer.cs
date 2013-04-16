/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Game.Area;
using Microsoft.Xna.Framework.Input;
using Engine.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Sprites
{
    /// <summary>
    /// MousePointer is the main game mouse pointer used to activate objects or move around.  This may not be implemented.
    /// </summary>
    class MousePointer : TPSprite
    {
        /// <summary>
        /// The player sprite
        /// </summary>
        private PlayerSprite playerSprite;

        /// <summary>
        /// Area info for the current area
        /// </summary>
        private AreaInfo areaInfo;

        /// <summary>
        /// The position of the mouse pointer in the viewport
        /// </summary>
        Vector2 viewportPosition;

        /// <summary>
        /// The texture used for drawing
        /// </summary>
        Texture2D drawText;

        /// <summary>
        /// A color array used for pixel perfect collision detection
        /// </summary>
        Color[] colors;

        /// <summary>
        /// The tile that we are pointing at
        /// </summary>
        Point tileToDraw = new Point(-1, -1);

        /// <summary>
        /// The tile we are moving to
        /// </summary>
        Point targetTile = new Point(-1, -1);

        /// <summary>
        /// Whether or not we are pointing at a tile
        /// </summary>
        bool tileDetected = false;

        /// <summary>
        /// The previous mouse state
        /// </summary>
        MouseState prevMouseState;

        /// <summary>
        /// The name of the current area
        /// </summary>
        string areaName;

        /// <summary>
        /// Constructor for MousePointer
        /// </summary>
        /// <param name="sprite">The player sprite</param>
        /// <param name="info">The info for the current area</param>
        /// <param name="area">The name of the current area</param>
        public MousePointer(PlayerSprite sprite, AreaInfo info, string area)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/point"))
        {
            areaName = area;
            viewportPosition = Vector2.Zero;
            viewportPosition.X = Mouse.GetState().X;
            viewportPosition.Y = Mouse.GetState().Y;
            areaInfo = info;
            playerSprite = sprite;
            drawText = TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/pointer");
            prevMouseState = Mouse.GetState();

            colors = new Color[Width * Height];
            GetTexture().GetData(colors);
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Position = viewportPosition - (playerSprite.Position - playerSprite.roomPosition);
            viewportPosition.X = Mouse.GetState().X;
            viewportPosition.Y = Mouse.GetState().Y;

            /*for (int x = 0; x < areaInfo.areaX && !tileDetected; x++)
            {
                for (int y = 0; y < areaInfo.areaY && !tileDetected; y++)
                {
                    if (areaInfo.tileSearchArray[x][y] != null)
                    {
                        Rectangle rect = new Rectangle((int)areaInfo.tileSearchArray[x][y].Position.X, (int)areaInfo.tileSearchArray[x][y].Position.Y, areaInfo.tileSearchArray[x][y].GetTexture().Width, areaInfo.tileSearchArray[x][y].GetTexture().Height);
                        if (TPCollider.PerPixelTest(this, this.colors, areaInfo.tileSearchArray[x][y], areaInfo.tileSearchArray[x][y].colors))
                        {
                            tileToDraw.X = x;
                            tileToDraw.Y = y;
                            tileDetected = true;
                        }
                    }
                }
            }*/

            if (tileDetected)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    ((PlayerSprite)TPEngine.Get().SpriteDictionary[areaName + "PlayerSprite"]).SetTargetTile(areaInfo.tileSearchArray[tileToDraw.X][tileToDraw.Y]);
                }
            }

            base.Update(gameTime);

            prevMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (tileDetected)
            {
                areaInfo.tileSearchArray[tileToDraw.X][tileToDraw.Y].Draw(batch);
            }
            tileDetected = false;
            batch.Draw(drawText, viewportPosition, Color.White);
        }
    }
}
