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
    class Lab4State : TPState
    {
        TPString s = new TPString("Lab 4");
        TPLayer layer;
        KeyboardState prevState;

        public Lab4State()
        {
            prevState = Keyboard.GetState();
            layer = new TPLayer(this.layers);
            layer.AddEntity(s);
            s.Position = new Vector2(200, 200);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.X) && prevState.IsKeyUp(Keys.X))
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
