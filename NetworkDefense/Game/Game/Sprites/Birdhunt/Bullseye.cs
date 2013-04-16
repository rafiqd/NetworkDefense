using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    /// <summary>
    /// Cyril, Peter
    /// The player sprite. need to be scaled 
    /// </summary>
    class Bullseye : TPSprite
    {
        /// <summary>
        /// The score counter for the player. starting from 0
        /// </summary>
        public static int TotalScore { get; set; }

        /// <summary>
        /// The level system. starting from 1
        /// </summary>
        public int Level { get; set; }


        /// <summary>
        /// The constructor sets the initial position and initial score.
        /// </summary>
        /// <param name="pos">The initial position to set</param>
        public Bullseye(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/big-bullseye"))
        {
            Position = pos;
            TotalScore = 0;
            Level = 1;
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
            Position.X = Mouse.GetState().X - Width / 2;
            Position.Y = Mouse.GetState().Y - Height / 2;
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
            if (TotalScore != 0)
            {
                TPString level = TPEngine.Get().StringDictionary["MinigameLevel"];
                level.Clear();

                int levelNum = (int)(0.25 * Math.Sqrt(TotalScore) + 1);
                if (Level != levelNum)
                {
                    Level = levelNum;
                    TPEngine.Get().Audio.PlaySFX(@"sfx/mariocoin");
                }
                level.Append("Level: " + Level);
            }
        }
    }
}
