﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Engine.Collision;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game.Sprites
{
    /// <summary>
    /// Cyril
    /// skip button sprite. When clicked, it takes the player directly to the highscore screen and skip the game. 
    /// </summary>
    class SkipButton : TPSprite
    {
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
        /// represents the crosshair sprite
        /// </summary>
        Bullseye bullseye;

        /// <summary>
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        public SkipButton()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/skipButton"))
        {
            prevMouseState = Mouse.GetState();
            prevKeyboardState = Keyboard.GetState();
            this.Position = new Vector2(1180, 30);
            Scale = new Vector2(0.5f, 0.5f);
        }

        /// <summary>
        /// Updates the sprite's association with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (prevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                TPEngine.Get().Audio.LoadSFX(@"sfx/xbuster");
                if ((Bullseye)TPEngine.Get().SpriteDictionary["MinigameBullseyeMenu"] != null)
                {
                    bullseye = (Bullseye)TPEngine.Get().SpriteDictionary["MinigameBullseyeMenu"];
                }
                else
                {
                    bullseye = (Bullseye)TPEngine.Get().SpriteDictionary["MinigameBullseye"];
                }
                if ((BulletHole)TPEngine.Get().SpriteDictionary["bulletHoleMenu"] != null)
                {
                    bulletHole = (BulletHole)TPEngine.Get().SpriteDictionary["bulletHoleMenu"];
                    bulletHole.Position = new Vector2(Mouse.GetState().X - (bulletHole.Width * bulletHole.Scale.X / 2),
                                                    Mouse.GetState().Y - (bulletHole.Height * bulletHole.Scale.Y / 2));
                    bulletHole.Alive = true;
                }
                else
                {
                    bulletHole = (BulletHole)TPEngine.Get().SpriteDictionary["bulletHole"];
                    bulletHole.Position = new Vector2(Mouse.GetState().X - (bulletHole.Width * bulletHole.Scale.X / 2),
                                                    Mouse.GetState().Y - (bulletHole.Height * bulletHole.Scale.Y / 2));
                    bulletHole.Alive = true;
                }
                if (TPCollider.Test(this, bulletHole))
                {
                    TPEngine.Get().State.PopState();
                    TPEngine.Get().State.PushState(new HighScoreState());
                    Globals.Paused = false;
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
        /// loads all data related to the class
        /// </summary>
        protected override void Load()
        {
            base.Load();
            AffectedByDrag = false;
        }
    }
}
