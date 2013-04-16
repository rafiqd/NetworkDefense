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

namespace Game.Sprites
{

    /// <summary>
    /// A non-player sprite.
    /// </summary>
    class BackButton : TPSprite
    {
        /// <summary>
        /// Previous mouse state
        /// </summary>
        MouseState prevMouseState;
        /// <summary>
        /// Previose Keyboard state
        /// </summary>
        KeyboardState prevKeyboardState;
        /// <summary>
        /// Sprite used to determine mouse location
        /// </summary>
        BulletHole bulletHole;
        /// <summary>
        /// Bullseye sprite
        /// </summary>
        Bullseye mySprite;

        /// <summary>
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        public BackButton()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/BullseyeButtonTrio"))
        {
            prevMouseState = Mouse.GetState();
            prevKeyboardState = Keyboard.GetState();
            this.Position = new Vector2(500, 500);
        }

        /// <summary>
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (prevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                TPEngine.Get().Audio.PlaySFX("@sfx/xbuster");
                if ((Bullseye)TPEngine.Get().SpriteDictionary["MinigameMySpriteMenu"] != null)
                {
                    mySprite = (Bullseye)TPEngine.Get().SpriteDictionary["MinigameMySpriteMenu"];
                }
                else
                {
                    mySprite = (Bullseye)TPEngine.Get().SpriteDictionary["MinigameMySprite"];
                }
                //Ammobar.ShotFired();
                if ((BulletHole)TPEngine.Get().SpriteDictionary["bulletHoleMenu"] != null)
                {
                    bulletHole = (BulletHole)TPEngine.Get().SpriteDictionary["bulletHoleMenu"];
                    bulletHole.Position = new Vector2(Mouse.GetState().X + (mySprite.Width / 2) - (bulletHole.Width * bulletHole.Scale.X / 2),
                                                    Mouse.GetState().Y + (mySprite.Height / 2) - (bulletHole.Height * bulletHole.Scale.Y / 2));
                    bulletHole.Alive = true;
                }
                else
                {
                    bulletHole = (BulletHole)TPEngine.Get().SpriteDictionary["bulletHole"];
                    bulletHole.Position = new Vector2(Mouse.GetState().X + (mySprite.Width / 2) - (bulletHole.Width * bulletHole.Scale.X / 2),
                                                    Mouse.GetState().Y + (mySprite.Height / 2) - (bulletHole.Height * bulletHole.Scale.Y / 2));
                    bulletHole.Alive = true;
                }
                if (TPCollider.Test(this, bulletHole))
                {
                    Explosion explosion = (Explosion)TPEngine.Get().SpriteDictionary["explosionMenu"];
                    explosion.SetStartPos(this.Position);
                    //BirdFall fall = (BirdFall)TPEngine.Get().SpriteDictionary["fall"];
                    //fall.SetSprite(this.Position);
                    //ResetSprite();
                    TPEngine.Get().State.PopState();
                    Globals.Paused = false;

                }
            }

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }


        protected override void Load()
        {
            base.Load();
            AffectedByDrag = false;
        }
    }
}
