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
    class EmailDetailState : TPState
    {
        TPLayer[] myLayers;
        Int16 NumLayers = 3;
        Bullseye mySprite;
        TPSprite backgroundTexture;
        static bool FirstRunDetail = true;
        List<TPString> Items = new List<TPString>();
        int startTime;
        TPString from = new TPString("Hell");
        TPString subject = new TPString("Say aye!");
        TPString body = new TPString("Say aye to win reward.");
        BulletHole bulletHole = new BulletHole();

        Explosion explosion = new Explosion();


        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        protected override void Load()
        {
            base.Load();
            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }

            mySprite = new Bullseye(new Vector2(200, 200));
            myLayers[2].AddEntity(mySprite);

            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/emaildetail"));
            backgroundTexture.Position = new Vector2(backgroundTexture.Position.X, backgroundTexture.Position.Y);
            myLayers[0].AddEntity(backgroundTexture);

            myLayers[2].AddEntity(bulletHole);
            myLayers[2].AddEntity(explosion);

            from.Position.X = 240;
            from.Position.Y = 165;
            from.RenderColor = Color.Black;

            subject.Position.X = 240 + from.HalfSize.X;
            subject.Position.Y = 250;
            subject.RenderColor = Color.Black;

            body.Position.X = 300;
            body.Position.Y = 400;
            body.RenderColor = Color.Black;

            myLayers[1].AddEntity(from);
            myLayers[1].AddEntity(subject);
            myLayers[1].AddEntity(body);

            if (FirstRunDetail)
            {
                TPEngine.Get().SpriteDictionary.Add("bulletHoleDetail", bulletHole);
                TPEngine.Get().SpriteDictionary.Add("explosionDetail", explosion);
                TPEngine.Get().SpriteDictionary.Add("MinigameMySpriteDetail", mySprite);
                TPEngine.Get().StringDictionary.Add("fromDetail", from);
                TPEngine.Get().StringDictionary.Add("subjectDetail", subject);
                TPEngine.Get().StringDictionary.Add("bodyDetail", body);
                FirstRunDetail = false;
            }
            //TPEngine.Get().StringDictionary.Add("MinigameLevel", level);
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
