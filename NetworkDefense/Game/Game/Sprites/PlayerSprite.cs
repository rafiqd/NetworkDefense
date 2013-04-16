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
using Microsoft.Xna.Framework;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Game.States;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Game.Sprites
{
    /// <summary>
    /// The motion state of the player determines if the player is standing still or if the player is moving in a certain direction.
    /// This helps determine what action to take when the player pushes keys, and what textures to use to draw.
    /// </summary>
    public enum PlayerMotionState
    {
        /// <summary>
        /// Walking south-east
        /// </summary>
        WalkSE,

        /// <summary>
        /// Walking north-east
        /// </summary>
        WalkNE,

        /// <summary>
        /// Walking south-west
        /// </summary>
        WalkSW,

        /// <summary>
        /// Walking north-west
        /// </summary>
        WalkNW,

        /// <summary>
        /// Standing still
        /// </summary>
        Still,
    }

    /// <summary>
    /// The PlayerSprite represents the player on screen in the world.  The human player controls this sprite with the keyboard.
    /// </summary>
    public class PlayerSprite : TPSprite
    {
        /// <summary>
        /// The position that the sprite is currently moving to.
        /// </summary>
        private Vector2 targetPosition;

        /// <summary>
        /// The tile that the sprite is currently moving to.
        /// </summary>
        TileSprite targetTile = null;

        /// <summary>
        /// The position of the player in the room.
        /// </summary>
        public Vector2 roomPosition { set; get; }

        /// <summary>
        /// The vector of motion (the speed and direction) that the player is moving in.
        /// </summary>
        public Vector2 motionVector;

        /// <summary>
        /// The position for the current frame of motion
        /// </summary>
        Vector2 frameVector = Vector2.Zero;
        
        /// <summary>
        /// The current position of the sprite
        /// </summary>
        public Microsoft.Xna.Framework.Point currentPosition;

        /// <summary>
        /// The current and previous keyboard state
        /// </summary>
        private KeyboardState previousKBState, currentKBState;

        /// <summary>
        /// The detailed information about the area that player is currently in
        /// </summary>
        public AreaInfo areaInfo;

        /// <summary>
        /// The current motionstate of the player
        /// </summary>
        public PlayerMotionState motionState { set; get; }

        /// <summary>
        /// The numerical value of the current layer that the sprite is being drawn in
        /// </summary>
        public int currentLayer { set; get; }

        /// <summary>
        /// The layers that exist in the current area
        /// </summary>
        private TPLayer[] layers;

        /// <summary>
        /// Determines if the player needs to change layer immediately
        /// </summary>
        public bool changeLayer { set; get; }

        /// <summary>
        /// Determines if the player needs to change layers at the end of a motion
        /// </summary>
        public bool changeLayerDelay { set; get; }

        /// <summary>
        /// Determines if the player is moving "upwards" (towards the top of the screen.  This helps us determine
        /// what layer the player needs to be in.
        /// </summary>
        private bool changeDirUp = true;

        /// <summary>
        /// The active object that the player can activate if there is one.
        /// </summary>
        public UsableIsometricSprite ActiveObject = null;
        
        /// <summary>
        /// The time of motion, used for determining how far the player has gone.
        /// </summary>
        private double moveTime, lastMoveTime;

        /// <summary>
        /// The maximum amount of time it takes to go from on tile to the next
        /// </summary>
        private double MAX_MOVE_TIME = 200;

        /// <summary>
        /// The texture arrays for the animations when the player moves in each of the 4 directions
        /// </summary>
        static Texture2D[] seTex, swTex, neTex, nwTex;

        /// <summary>
        /// Whether or not the sprite has been previously loaded.  Determines if we need to do any fancy loading stuff in C#.
        /// </summary>
        private static bool isLoaded = false;

        /// <summary>
        /// The number of frames for a walking animation and the speed at which the frames play.
        /// </summary>
        private static int walkframes, walkfps;

        /// <summary>
        /// The current frame of animation.
        /// </summary>
        int currentFrame = 0;

        /// <summary>
        /// The current array of textures for the current motion animation.
        /// </summary>
        Texture2D[] currentTexArray;

        /// <summary>
        /// The animation data loaded from the image.
        /// </summary>
        static ImageMetaData animationData;

        /// <summary>
        /// The PlayerSprite constructor
        /// </summary>
        /// <param name="info">The info for the current area</param>
        /// <param name="lay">the array of layers for the area</param>
        public PlayerSprite(AreaInfo info, TPLayer[] lay)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/demoperson"))
        {
            if (!isLoaded)
            {
                if (AreaState.character.sex == "m")
                {
                    animationData = ImageDecoder.GetMetaData("male_se.png");
                    seTex = LoadTextureArray("male_se.png");
                    neTex = LoadTextureArray("male_ne.png");
                    swTex = LoadTextureArray("male_sw.png");
                    nwTex = LoadTextureArray("male_nw.png");
                }
                else
                {
                    animationData = ImageDecoder.GetMetaData("female_se.png");
                    seTex = LoadTextureArray("female_se.png");
                    neTex = LoadTextureArray("female_ne.png");
                    swTex = LoadTextureArray("female_sw.png");
                    nwTex = LoadTextureArray("female_nw.png");
                }
                isLoaded = true;
            }

            walkfps = animationData.fps;
            walkframes = animationData.frames;

            MAX_MOVE_TIME = walkframes * 250 / walkfps;
            currentTexArray = swTex;
            m_Texture = currentTexArray[0];

            Position = new Vector2(640 - m_Texture.Width / 2, 360 - m_Texture.Height / 2);

            areaInfo = info;
            layers = lay;

            motionState = PlayerMotionState.Still;

            previousKBState = Keyboard.GetState();
        }

        /// <summary>
        /// Loads the player information
        /// </summary>
        /// <param name="pos">The room position of the player</param>
        /// <param name="curLayer">The current layer of the player</param>
        /// <param name="curPos">The current grid position of the player</param>
        public void Load(Vector2 pos, int curLayer, Microsoft.Xna.Framework.Point curPos)
        {
            roomPosition = pos + new Vector2(animationData.center_x - 15, -50);
            currentPosition = curPos;
            currentLayer = curLayer;
            changeLayer = false;
            changeLayerDelay = false;
        }

        /// <summary>
        /// Update the sprite
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            HandleInput(gameTime);

            if (motionState != PlayerMotionState.Still)
            {
                float frameFactor = (float)((gameTime.TotalGameTime.TotalMilliseconds - lastMoveTime) / MAX_MOVE_TIME);
                lastMoveTime = gameTime.TotalGameTime.TotalMilliseconds;
                frameVector.X = motionVector.X * frameFactor;
                frameVector.Y = motionVector.Y * frameFactor;
                roomPosition += frameVector;
                if (gameTime.TotalGameTime.TotalMilliseconds - moveTime >= MAX_MOVE_TIME)
                {
                    motionState = PlayerMotionState.Still;
                    currentFrame = 0;
                    roomPosition = targetPosition;
                    if (changeLayerDelay)
                    {
                        changeLayerDelay = false;
                        changeLayer = true;
                    }
                }
                else
                {
                    currentFrame = ((int)(walkframes * (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds - moveTime)) / MAX_MOVE_TIME * 1000)) / 2000;
                }
                m_Texture = currentTexArray[currentFrame];
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Handle player input
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        private void HandleInput(GameTime gameTime)
        {
            currentKBState = Keyboard.GetState();
            if (currentKBState.IsKeyDown(Keys.Left)
                && motionState == PlayerMotionState.Still && currentPosition.X > 0
                && areaInfo.tileTypeArray[(int)currentPosition.Y][(int)(currentPosition.X - 1)] == 't')
            {
                targetPosition = roomPosition + new Vector2(-92, -35);
                moveTime = gameTime.TotalGameTime.TotalMilliseconds;
                lastMoveTime = moveTime;
                changeLayerDelay = true;
                changeDirUp = true;
                currentPosition.X--;
                motionState = PlayerMotionState.WalkNW;
                motionVector = targetPosition - roomPosition;
                currentTexArray = nwTex;
            }
            else if (currentKBState.IsKeyDown(Keys.Right)
                && motionState == PlayerMotionState.Still && currentPosition.X < areaInfo.areaX - 1
                && areaInfo.tileTypeArray[(int)currentPosition.Y][(int)(currentPosition.X + 1)] == 't')
            {
                targetPosition = roomPosition + new Vector2(92, 35);
                moveTime = gameTime.TotalGameTime.TotalMilliseconds;
                lastMoveTime = moveTime;
                changeLayer = true;
                changeDirUp = false;
                currentPosition.X++;
                motionState = PlayerMotionState.WalkSE;
                motionVector = targetPosition - roomPosition;
                currentTexArray = seTex;
            }
            else if (currentKBState.IsKeyDown(Keys.Down)
                && motionState == PlayerMotionState.Still && currentPosition.Y < areaInfo.areaY - 1
                && areaInfo.tileTypeArray[(int)currentPosition.Y + 1][(int)(currentPosition.X)] == 't')
            {
                targetPosition = roomPosition + new Vector2(-55, 58);
                moveTime = gameTime.TotalGameTime.TotalMilliseconds;
                lastMoveTime = moveTime;
                changeLayer = true;
                changeDirUp = false;
                currentPosition.Y++;
                motionState = PlayerMotionState.WalkSW;
                motionVector = targetPosition - roomPosition;
                currentTexArray = swTex;
            }
            else if (currentKBState.IsKeyDown(Keys.Up)
                && motionState == PlayerMotionState.Still && currentPosition.Y > 0
                && areaInfo.tileTypeArray[(int)currentPosition.Y - 1][(int)(currentPosition.X)] == 't')
            {
                targetPosition = roomPosition + new Vector2(55, -58);
                moveTime = gameTime.TotalGameTime.TotalMilliseconds;
                lastMoveTime = moveTime;
                changeLayer = true;
                changeDirUp = true;
                currentPosition.Y--;
                motionState = PlayerMotionState.WalkNE;
                motionVector = targetPosition - roomPosition;
                currentTexArray = neTex;
            }
            previousKBState = currentKBState;
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(m_Texture, new Vector2(640 - m_Texture.Width / 2, 360  - m_Texture.Height / 2), Microsoft.Xna.Framework.Color.White);
            //base.Draw(batch);
        }

        /// <summary>
        /// Change the current layer of the player
        /// </summary>
        public void ChangeLayer()
        {
            layers[currentLayer].RemoveEntity(this);
            currentLayer = currentPosition.X + currentPosition.Y + 1;
            layers[currentLayer].AddEntityToFront(this);

            changeLayer = false;
        }
        
        /// <summary>
        /// Sets the target tile for the player to move to (for mouse movement, might not be implemented)
        /// </summary>
        /// <param name="tile">The tile to move to</param>
        public void SetTargetTile(TileSprite tile)
        {
            targetTile = tile;
        }

        /// <summary>
        /// Loads the texture arrays for the animations.  This is used as a work-around.  XNA does not allow for texture sizes greater than
        /// 4096x4096 pixels.  Some of our textures are larger, so they must be separated at run time into arrays.
        /// </summary>
        /// <param name="s">The name of the texture to load</param>
        /// <returns>The array of textures</returns>
        private Texture2D[] LoadTextureArray(string s)
        {
            Texture2D[] textures;
            ImageMetaData meta = ImageDecoder.GetMetaData(s);
            animationData = meta;
            walkframes = meta.frames;
            walkfps = meta.fps;
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(s);
            textures = new Texture2D[meta.frames];
            for (int i = 0; i < meta.frames; i++)
            {
                Bitmap bmp = new Bitmap(meta.width, 333);
                Graphics g = Graphics.FromImage(bmp);

                g.DrawImage(bitmap, 0, 0, new RectangleF(1 + (i * meta.width), 0, meta.width, 333), GraphicsUnit.Pixel);

                MemoryStream str = new MemoryStream();
                bmp.Save(str, ImageFormat.Png);
                textures[i] = Texture2D.FromStream(TPGame.graphics.GraphicsDevice, str);
            }
            return textures;
        }
    }
}
