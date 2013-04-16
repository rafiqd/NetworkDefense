using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Engine.Collision;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.States;

namespace Game.Sprites
{

    /// <summary>
    /// A bird sprite
    /// </summary>
    class Bird : TPSprite
    {
        /// <summary>
        /// keep track whether the bird's wings are currently up or down
        /// </summary>
        public bool wingup = true;

        /// <summary>
        /// the field for bird's flying direction
        /// </summary>
        public Vector2 direction;

        /// <summary>
        /// the sprite represents the bird going left with wings up
        /// </summary>
        public Texture2D goingLeftWingUp;

        /// <summary>
        /// the sprite represents the bird going left with wings down
        /// </summary>
        public Texture2D goingLeftWingDown;

        /// <summary>
        /// the sprite represents the bird going right with wings up
        /// </summary>
        public Texture2D goingRightWingUp;

        /// <summary>
        /// the sprite represents the bird going right with wings down
        /// </summary>
        public Texture2D goingRightWingDown;

        /// <summary>
        /// stores the previous state of the mouse
        /// </summary>
        MouseState prevMouseState;

        /// <summary>
        /// stores the previous state of the keyboard
        /// </summary>
        KeyboardState prevKeyboardState;

        /// <summary>
        /// represents bullet hole sprite
        /// </summary>
        BulletHole bulletHole;

        /// <summary>
        /// difficulty multiplier
        /// </summary>
        float difficulty;
        
        /// <summary>
        /// represents the crosshair sprite
        /// </summary>
        Bullseye bullseye;

        /// <summary>
        /// the value of the bird
        /// </summary>
        public int pointsWorth;

        /// <summary>
        /// the direction where the bird flies
        /// </summary>
        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        /// <summary>
        /// Peter
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        /// <param name="rightWingsUpFileName">the sprite's facing right with wings up name</param>
        /// <param name="rightWingsDownFileName">the sprite's facing right with wings down name</param>
        /// <param name="leftWingsDownFileName">the sprite's facing left with wings up name</param>
        /// <param name="leftWingsUpFileName">the sprite's facing left with wings down name</param>
        /// <param name="pointsWorth">the value of the bird</param>
        public Bird(string rightWingsUpFileName, string rightWingsDownFileName, string leftWingsDownFileName, string leftWingsUpFileName, int pointsWorth)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + rightWingsDownFileName))
        {
            prevMouseState = Mouse.GetState();
            prevKeyboardState = Keyboard.GetState();
            Direction = new Vector2(GetRand(-8,5) , GetRand(-8,5));
            goingRightWingUp = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + rightWingsUpFileName);
            goingRightWingDown = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + rightWingsDownFileName);
            goingLeftWingUp = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + leftWingsUpFileName);
            goingLeftWingDown = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + leftWingsDownFileName);
            this.pointsWorth = pointsWorth;
            Scale = new Vector2(2, 2);
            this.difficulty = 1;// +(1 - ((float)(AreaState.character.sanity + AreaState.character.hunger + AreaState.character.energy / 3) / 100));
        }

        /// <summary>
        /// Peter, Cyril
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Globals.Paused)
            {
                base.Update(gameTime);
                Position += direction * difficulty;
                if ((Position.Y + Height) > 590 || Position.Y < 0) //TPEngine.Get().ScreenSize.Height
                {
                    direction.Y = -direction.Y;
                }
                if ((Position.X + Width) > TPEngine.Get().ScreenSize.Width || Position.X < 0)
                {

                    direction.X = -direction.X;
                    if (direction.X < 0)
                    {
                        this.m_Texture = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/bird2goingleftwingup");
                    }
                    else
                    {
                        this.m_Texture = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/bird2goingrightwingup");
                    }
                }
                if (prevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed && Ammobar.Ammo > 0)
                {
                    TPEngine.Get().Audio.PlaySFX(@"sfx/xbuster");
                    if ((Bullseye)TPEngine.Get().SpriteDictionary["MinigameBullseye"] != null)
                    {
                        bullseye = (Bullseye)TPEngine.Get().SpriteDictionary["MinigameBullseye"];
                    }
                    else
                    {
                        bullseye = (Bullseye)TPEngine.Get().SpriteDictionary["MinigameBullseyeMenu"];
                    }
                    if ((BulletHole)TPEngine.Get().SpriteDictionary["bulletHole"] != null)
                    {
                        bulletHole = (BulletHole)TPEngine.Get().SpriteDictionary["bulletHole"];
                        bulletHole.Position = new Vector2(Mouse.GetState().X - (bulletHole.Width * bulletHole.Scale.X / 2),
                                                        Mouse.GetState().Y - (bulletHole.Height * bulletHole.Scale.Y / 2));
                        bulletHole.Alive = true;
                    }
                    else
                    {
                        bulletHole = (BulletHole)TPEngine.Get().SpriteDictionary["bulletHoleMenu"];
                        bulletHole.Position = new Vector2(Mouse.GetState().X - (bulletHole.Width * bulletHole.Scale.X / 2),
                                                        Mouse.GetState().Y - (bulletHole.Height * bulletHole.Scale.Y / 2));
                        bulletHole.Alive = true;
                    }
                    if (TPCollider.Test(this, bulletHole))
                    {
                        Explosion explosion = (Explosion)TPEngine.Get().SpriteDictionary["explosion"];
                        explosion.SetStartPos(this.Position);
                        BirdFall fall = (BirdFall)TPEngine.Get().SpriteDictionary["fall"];
                        fall.SetSprite(this.Position);
                        ResetSprite();
                        bullseye.AdjustScore(pointsWorth);
                    }
                    else
                    {
                        direction.X = -(direction.X++);
                        direction.Y = -GetRand(-9, 6);
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.R) || Mouse.GetState().RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                {
                    TPEngine.Get().Audio.PlaySFX(@"sfx/shotgunreload");
                    Ammobar.Reload();
                }
                if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                {
                    if (direction.X > 0)
                    {
                        this.m_Texture = (wingup) ? goingRightWingDown : goingRightWingUp;
                        wingup = !wingup;
                    }
                    else
                    {
                        this.m_Texture = (wingup) ? goingLeftWingDown : goingLeftWingUp;
                        wingup = !wingup;
                    }
                }
            }
            //if (Keyboard.GetState().IsKeyUp(Keys.P))
            //{
            //    Globals.Paused = !Globals.Paused;
            //}

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        /// <summary>
        /// method that executes when the sprite is loaded on screen
        /// </summary>
        protected override void Load()
        {
            base.Load();
            AffectedByDrag = false;
        }

        /// <summary>
        /// Peter
        /// Resets the sprite to the top of the screen at a random distance from the left side without going over the right side.
        /// </summary>
        private void ResetSprite()
        {
            Position.Y = 0;
            Direction = new Vector2(GetRand(-8, 10), GetRand(-8, 10));
            Position.X = TPEngine.Get().Rand.Next(1280 - Width);
            //Position.X = 0;
        }

        /// <summary>
        /// Peter
        /// a function the get a random number between the min and max value 
        /// </summary>
        /// <param name="min">minimum value</param>
        /// <param name="max">maximum value</param>
        /// <returns>a random value between min and max</returns>
        private int GetRand(int min, int max)
        {
            int value;//took out the assigned int here
            bool isNegative = (TPEngine.Get().Rand.Next(1, 2) == 1) ? true : false;
            value = TPEngine.Get().Rand.Next(min, max);
            value = (value == 0) ? 1 : value;
            return (isNegative) ? value * -1 : value;
        }
   
    }
}
