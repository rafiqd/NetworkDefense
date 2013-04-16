using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Game.Sprites;
using System.Runtime.InteropServices;
using DBCommService;

namespace Game
{
    class EmailViewState : TPState
    {
        TPLayer[] myLayers;
        Int16 NumLayers = 3;
        Bullseye mySprite;
        TPSprite backgroundTexture;

        List<TPString> Items = new List<TPString>();

        static bool FirstRunEmail = true;

        int startTime;
        BulletHole bulletHole = new BulletHole();
        List<ReadMoreButton> buttons = new List<ReadMoreButton>();
        Explosion explosion = new Explosion();


        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        protected override void Load()
        {
            base.Load();
            //for (int i = 0; i < 10; i++)
            //{
            //    Items.Add(new TPString(scores[i].ToString()));
            //}
            Items.Add(new TPString("Email 1"));
            Items.Add(new TPString("Email 2"));
            Items.Add(new TPString("Email 3"));
            Items.Add(new TPString("Email 4"));
            Items.Add(new TPString("Email 5"));
            Items.Add(new TPString("Email 6"));
            Items.Add(new TPString("Email 7"));
            Items.Add(new TPString("Email 8"));
            Items.Add(new TPString("Email 9"));
            Items.Add(new TPString("Email 10"));

            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }

            mySprite = new Bullseye(new Vector2(200, 200));
            myLayers[2].AddEntity(mySprite);
            myLayers[2].AddEntity(bulletHole);

            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/Quest/emailview"));
            backgroundTexture.Position = new Vector2(backgroundTexture.Position.X, backgroundTexture.Position.Y);
            myLayers[0].AddEntity(backgroundTexture);

            if (Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    buttons.Add(new ReadMoreButton());
                }
                for (int i = 0; i < Items.Count; i++)
                {
                    myLayers[1].AddEntity(Items[i]);
                    myLayers[1].AddEntity(buttons[i]);
                }
                
                Items[0].Position.X = 150;
                Items[0].Position.Y = 200;
                Items[0].RenderColor = Color.Black;

                buttons[0].Position.X = 1050;
                buttons[0].Position.Y = 190;
                for (int i = 1; i < 10; i++)
                {
                    Items[i].Position.X = 150;
                    Items[i].Position.Y = Items[i - 1].Position.Y + 50;
                    Items[i].RenderColor = Color.Black;

                    buttons[i].Position.X = 1050;
                    buttons[i].Position.Y = buttons[i - 1].Position.Y + 50;
                }
            }

            if (FirstRunEmail)
            {
                TPEngine.Get().SpriteDictionary.Add("bulletHoleEmail", bulletHole);
                TPEngine.Get().SpriteDictionary.Add("MinigameMySpriteEmail", mySprite);
                FirstRunEmail = false;
                if (Items.Count > 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        TPEngine.Get().StringDictionary.Add(string.Format("Email{0}", i), (TPString)Items[i]);
                        TPEngine.Get().StringDictionary.Add(string.Format("Button{0}", i), new TPString(buttons[i].ToString()));
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //if the Esc key is pressed, return to the main game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                TPEngine.Get().State.PopState();
            }
        }
    }
}
