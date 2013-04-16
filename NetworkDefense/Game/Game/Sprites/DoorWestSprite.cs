/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Engine.StateManagement;
using System.IO;
using System.Drawing.Imaging;

namespace Game.Sprites
{
    /// <summary>
    /// DoorWestSprite is a door that faces south and provides access to other areas
    /// </summary>
    class DoorSouthSprite : UsableIsometricSprite
    {
        /// <summary>
        /// Textures for the door animation
        /// </summary>
        static Texture2D[] textures;

        /// <summary>
        /// Determines if the textures for the class have been loaded or not.
        /// </summary>
        static bool isLoaded = false;

        /// <summary>
        /// The name of the area that the door leads to
        /// </summary>
        string targetArea = null;

        /// <summary>
        /// A TPString to display the name of the area the door leads to to the player.
        /// </summary>
        TPString targetString;

        /// <summary>
        /// The current frame of the animation
        /// </summary>
        int frame = 0;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        int frameWidth = 196;

        /// <summary>
        /// The number of the last frame
        /// </summary>
        int maxFrame = 37;

        Texture2D label;

        /// <summary>
        /// Constructor for DoorSouthSprite
        /// </summary>
        /// <param name="worldPos">The position of the door in the world</param>
        /// <param name="sprite">The player sprite</param>
        /// <param name="mapPos">The position of the door in the map</param>
        /// <param name="target">The name of the area that the door leads to</param>
        /// <param name="areaName">The name of the area that the door exists in</param>
        public DoorSouthSprite(Vector2 worldPos, PlayerSprite sprite, Microsoft.Xna.Framework.Point mapPos, string target, string areaName)
            : base(new Vector2(worldPos.X + 140, worldPos.Y - 172), "doorwestopen", sprite, mapPos, true, areaName)
        {
            //If the door animation has not been loaded, load it
            if (!isLoaded)
            {
                Bitmap bitmap = (Bitmap)Bitmap.FromFile("door_s.png");
                textures = new Texture2D[maxFrame];
                ImageMetaData meta = ImageDecoder.GetMetaData("door_s.png");
                maxFrame = meta.frames;
                frameWidth = meta.width;
                
                for (int i = 0; i < maxFrame; i++)
                {
                    Bitmap bmp = new Bitmap(frameWidth, meta.height);
                    Graphics g = Graphics.FromImage(bmp);

                    g.DrawImage(bitmap, 0, 0, new RectangleF(1 + (i * frameWidth), 0, frameWidth, meta.height), GraphicsUnit.Pixel);

                    MemoryStream str = new MemoryStream();
                    bmp.Save(str, ImageFormat.Png);
                    textures[i] = Texture2D.FromStream(TPGame.graphics.GraphicsDevice, str);
                }
                isLoaded = true;
            }
            targetArea = target;

            if (target != "back")
            {
                label = TPEngine.Get().TextureManager.LoadTexture(@"art/labels/text_" + target);
            }

            // Special refers to a UsableIsometricSprite that is accessed by standing on it rather than next to it.
            special = true;
            if (target == "back")
            {
                targetString = new TPString("To Hall");
            }
            else
            {
                targetString = new TPString("To " + target);
            }
            targetString.RenderColor = Microsoft.Xna.Framework.Color.GreenYellow;
        }

        /// <summary>
        /// Updates the door according to the game time.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            targetString.Position = worldPosition + (playerSprite.Position - playerSprite.roomPosition);

            // Determine if the player is standing at the door, and if so, allow access.
            if (((facesLeft && playerSprite.currentPosition.Y == mapPosition.Y + 1 && playerSprite.currentPosition.X == mapPosition.X)
                    || (!facesLeft && playerSprite.currentPosition.Y + 1 == mapPosition.Y && playerSprite.currentPosition.X == mapPosition.X))
                    && playerSprite.motionState == PlayerMotionState.Still)
            {
                drawColor = new Microsoft.Xna.Framework.Color(0, 255, 0, 255);
                isAvailable = true;
                playerSprite.ActiveObject = this;
            }
            else
            {
                drawColor = Microsoft.Xna.Framework.Color.White;
                isAvailable = false;
            }
            
            // If access is available, open the door, otherwise close it.
            if (isAvailable && frame < maxFrame - 1)
            {
                frame++;
            }
            else if (frame > 0)
            {
                frame--;
            }
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.Draw(textures[frame], worldPosition + (playerSprite.Position - playerSprite.roomPosition), Microsoft.Xna.Framework.Color.White);
            if (label != null)
            {
                batch.Draw(label, worldPosition + (playerSprite.Position - playerSprite.roomPosition), Microsoft.Xna.Framework.Color.White);
            }
            base.Draw(batch);
        }

        /// <summary>
        /// Get the state of the area that the door points to
        /// </summary>
        /// <returns>A new state for the target area or null if the door returns to the previous area</returns>
        public override TPState GetNewState()
        {
            if (targetArea == "back")
            {
                return null;
            }
            return new AreaState(targetArea);
        }
    }
}
