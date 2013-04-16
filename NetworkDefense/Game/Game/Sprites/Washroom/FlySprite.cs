using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Engine.Collision;
using Microsoft.Xna.Framework.Input;

namespace Game.Sprites
{
    /// <summary>
    /// A non-player sprite.
    /// </summary>
    class FlySprite : TPSprite
    {
        /// <summary>
        /// Variable to decide flies alive
        /// </summary>
        public bool IsLive { set; get; }
        //dynamic fly movement variables
        uint remainingPathDistance;
        float currentDirectionX;
        float currentDirectionY;
        float currentCurveX;
        float currentCurveY;
        int maxMovement = 10;
        /// <summary>
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        public FlySprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/fly"))
        {
            Position.X = 290 + TPEngine.Get().Rand.Next(600);
            Position.Y = 180 + TPEngine.Get().Rand.Next(400 - Height);
            IsLive = true;
        }

        /// <summary>
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsLive)
            {
                //getCircleMoving(moveIndex);
                updateDynamicMovement();
                if (TPCollider.Test(this, TPEngine.Get().SpriteDictionary["MinigameShootSprite"]))
                {
                    ShootSprite shootSprite = (ShootSprite)TPEngine.Get().SpriteDictionary["MinigameShootSprite"];

                    shootSprite.AdjustScore(5);
                    IsLive = false;
                }
            }
        }
        /// <summary>
        /// Dynamic Fly Movement Code, This was Abdon's alternative to Jin's fly movement.
        /// Flies move in a direction for a certain amount of time with a slight curve.
        /// </summary>
        private void updateDynamicMovement()
        {
            //bounds of fly movement area
            uint leftBoundary = 290;
            uint rightBoundary = 920;
            uint topBoundary = 180;
            uint bottomBoundary = 570;

            int maxNewPathDistance = 64; //maximum distance of a fly's movement on a path, counts down then new path is chosen.
            float curveFactor = 1.0125f; //this increases the curve value's distortion of the path with each move

            if (remainingPathDistance > 0)
            {
                if (((Position.X + currentDirectionX + currentCurveX) < rightBoundary)
                    && ((Position.X + currentDirectionX) > leftBoundary))
                {
                    Position.X += currentDirectionX;
                    Position.X += currentCurveX;
                    currentCurveX *= curveFactor;
                }
                else
                {
                    currentDirectionX *= -1;
                    Position.X += currentDirectionX;
                    currentCurveX *= -0.1f;
                }

                if (((Position.Y + currentDirectionY + currentCurveY) < bottomBoundary)
                    && ((Position.Y + currentDirectionY) > topBoundary))
                {
                    Position.Y += currentDirectionY;
                    Position.Y += currentCurveY;
                    currentCurveY *= curveFactor;
                }
                else
                {
                    currentDirectionY *= -1;
                    Position.Y += currentDirectionY;
                    currentCurveY *= -0.1f;
                }
                remainingPathDistance--;
            }
            else
            {
                remainingPathDistance = (uint)TPEngine.Get().Rand.Next(maxNewPathDistance);
                currentDirectionX = TPEngine.Get().Rand.Next(maxMovement);
                currentDirectionY = TPEngine.Get().Rand.Next(maxMovement);
                currentCurveX = (float)TPEngine.Get().Rand.NextDouble();
                currentCurveY = (float)TPEngine.Get().Rand.NextDouble();
                if (TPEngine.Get().Rand.Next(2) == 1) currentDirectionX *= -1;
                if (TPEngine.Get().Rand.Next(2) == 1) currentDirectionY *= -1;
            }
        }
    }
}
