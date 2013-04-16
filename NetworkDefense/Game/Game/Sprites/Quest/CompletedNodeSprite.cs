using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Microsoft.Xna.Framework;

namespace Game.Sprites
{

    class CompletedNodeSprite : TPSprite
    {
        public bool IsItAlive = false;
        double startTime;
        private bool firstRun = true;
        int lifeSpan = 4;
        public CompletedNodeSprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/Quest/QuestNodeCompleted"))
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.IsItAlive && firstRun)
            {
                startTime = gameTime.TotalGameTime.Seconds;
                firstRun = false;
            }
            if (this.IsItAlive && startTime + lifeSpan < gameTime.TotalGameTime.Seconds)
            {
                this.IsItAlive = false;
                firstRun = true;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (IsItAlive)
            {
                base.Draw(batch);
            }
        }
    }
}
