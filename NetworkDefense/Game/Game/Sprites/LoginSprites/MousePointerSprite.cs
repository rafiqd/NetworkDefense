using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;

namespace Game
{
    /// <summary>
    /// The player sprite.
    /// </summary>
    class MousePointerSprite : TPSprite
    {
        //The movement speed
        int MoveSpeed = 5;

        //The total score
        public int TotalScore { get; set; }

        /// <summary>
        /// The constructor sets the initial position and initial score.
        /// </summary>
        /// <param name="pos">The initial position to set</param>
        public MousePointerSprite(Vector2 pos) : base(TPEngine.Get().TextureManager.LoadTexture(@"art/MousePointer"))
        {
            Position = pos;
            TotalScore = 0;
        }

        /// <summary>
        /// Updates the sprite.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running.</param>
        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Handle all input that directly relates to the sprite.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        private void HandleInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Position.Y += MoveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Position.Y -= MoveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Position.X += MoveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Position.X -= MoveSpeed;
            }

            Position.X = Mouse.GetState().X;
            Position.Y = Mouse.GetState().Y;
        }

        /// <summary>
        /// Add or subtract from the score.
        /// </summary>
        /// <param name="points">The adjustment value.</param>
        public void AdjustScore(int points)
        {
            TotalScore += points;

            // If you don't want to have a negative score...
            if (TotalScore < 0)
            {
                TotalScore = 0;
            }
            TPString score = TPEngine.Get().StringDictionary["MinigameScore"];
            score.Clear();
            score.Append("Score: " + TotalScore);
        }
    }
}
