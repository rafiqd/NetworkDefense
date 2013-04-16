using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Input;
using Engine;
using Engine.Objects;
using Microsoft.Xna.Framework;

namespace Game.States
{
    class TestState : TPState
    {
        TPString s = new TPString("This is a test state.  Press backspace to return.");
        TPLayer layer;
        KeyboardState prevState;

        public TestState()
        {
            prevState = Keyboard.GetState();
            layer = new TPLayer(this.layers);
            layer.AddEntity(s);
            s.Position = new Vector2(200, 200);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Back) && prevState.IsKeyUp(Keys.Back))
            {
                TPEngine.Get().State.PopState();
            }
            prevState = Keyboard.GetState();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.GraphicsDevice.Clear(Color.Black);
            base.Draw(batch);
        }
    }
}
