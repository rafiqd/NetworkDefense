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
    class ShootSprite : TPSprite
    {
        /// <summary>
        /// index variable for flags change.
        /// </summary>
        int flagIndex;
        /// <summary>
        /// Float variable to calculate the movement between two dos.
        /// </summary>
        float movePoint;
        /// <summary>
        /// Sprites which have information about 2 locations where shoot will move.
        /// </summary>
        FlagSprite startWaterSprite;
        FlagSprite endWaterSprite;
        /// <summary>
        /// Total score. It has dead flies amount
        /// </summary>
        public int TotalScore { get; set; }
        /// <summary>
        /// Boolean variable which decide whether shoot is started or not
        /// </summary>
        public bool IsStart { get; set; }
        /// <summary>
        /// Boolean variable which decide whether shoot is finished or not
        /// </summary>
        public bool IsFinish { get; set; }
        /// <summary>
        /// The constructor sets the initial position and initial score.
        /// </summary>
        /// <param name="pos">The initial position to set</param>
        public ShootSprite(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/waterhead"))
        {
            Position = pos;
            TotalScore = 0;
            flagIndex = 0;
            movePoint = 1;
            IsStart = false;
            IsFinish = false;
        }

        /// <summary>
        /// Updates the sprite.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running.</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            if (IsStart && flagIndex != 29 && movePoint >= 1.0f)
            {
                startWaterSprite = (FlagSprite)TPEngine.Get().SpriteDictionary["MinigameFlagSprite" + flagIndex];
                endWaterSprite = (FlagSprite)TPEngine.Get().SpriteDictionary["MinigameFlagSprite" + (flagIndex + 1)];
                movePoint = 0;
                flagIndex++;
            }
            if (flagIndex == 29)
            {
                IsFinish = true;
                return;
            }
            if (IsStart && gameTime.TotalGameTime.Milliseconds % 10 == 0)
            {
                movePoint += 0.2f;

                Position.X = startWaterSprite.Position.X + (movePoint * (endWaterSprite.Position.X - startWaterSprite.Position.X));
                Position.Y = startWaterSprite.Position.Y + (movePoint * (endWaterSprite.Position.Y - startWaterSprite.Position.Y));
            }
        }
        /// <summary>
        /// Add or subtract from the score.
        /// </summary>
        /// <param name="points">The adjustment value.</param>
        public void AdjustScore(int points)
        {
            TotalScore += 1;

            // If you don't want to have a negative score...
            if (TotalScore < 0)
            {
                TotalScore = 0;
            }
            TPString score = TPEngine.Get().StringDictionary["MinigameScore"];
            score.Clear();
            score.Append("Flies Caught: " + TotalScore);
        }
    }
}
