//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Engine.StateManagement;
//using Game.Events;
//using Microsoft.Xna.Framework.Input;
//using Engine;
//using Engine.Objects;
//using Microsoft.Xna.Framework;

//namespace Game.States
//{
//    class LabState : TPState
//    {
//        TPString s;
//        TPLayer layer;
//        KeyboardState prevState;
//        public Clock clock;

//        public LabState(EventInfo info)
//        {
//            switch(info.Name)
//            {
//                case "birdhunt":
//                    TPEngine.Get().State.PushState(new MyState(), true);
//                    TPEngine.Get().State.PushState(new BirdMenuState(), true);
//                    break;
//            }
//            TPSprite temp;
//            if (!TPEngine.Get().SpriteDictionary.TryGetValue("MainGameClock", out temp))
//            {
//                clock = new Clock(8, 0.0d, 0);
//                TPEngine.Get().SpriteDictionary.Add("MainGameClock", clock);
//            }
//            else
//            {
//                clock = (Clock)temp;
//            }
//        }

//        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
//        {
//            base.Update(gameTime);
//            if (Keyboard.GetState().IsKeyDown(Keys.X) && prevState.IsKeyUp(Keys.X))
//            {
//                clock.Set(3,0,false);
//                TPEngine.Get().State.PopState();
//            }
//            prevState = Keyboard.GetState();
//        }

//        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
//        {
//            batch.GraphicsDevice.Clear(Color.Black);
//            base.Draw(batch);
//        }
//    }
//}
