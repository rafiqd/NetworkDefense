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
    /// The target sprite.
    /// </summary>
    class TargetSprite : TPSprite
    {
        public int TotalScore { get; set; }
        /// <summary>
        /// Boolean variable to know whether game starts or not
        /// </summary>
        public bool IsGameStart { get; set; }
        /// <summary>
        /// The constructor sets the initial position and initial score.
        /// </summary>
        /// <param name="pos">The initial position to set</param>
        public TargetSprite(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/target"))
        {
            Position = pos;
            TotalScore = 0;
            IsGameStart = false;
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
            Position.X = Mouse.GetState().X;
            Position.Y = Mouse.GetState().Y;
        }
    }
}
