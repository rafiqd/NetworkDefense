using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Game.Sprites;
using Microsoft.Xna.Framework.Input;
using Game.States;
using Game.States.TDsrc.TDStates;

namespace Game
{
    class OpenningState : TPState
    {
        /// <summary>
        /// Layers : 4
        /// </summary>
        TPLayer[] myLayers;
        Int16 NumLayers = 4;
        /// <summary>
        /// Sprites to display in openning
        /// </summary>
        BusSprite busSprite = new BusSprite();
        LogoSprite logoSprite = new LogoSprite();

        /// <summary>
        /// Opening Texture
        /// </summary>
        TPSprite backgroundTexture;
        /// <summary>
        /// Start time to get from gametime
        /// </summary>
        int startTime;
        // Openning playing time
        int GamePlayingTime = 25;
        bool firstRun = true;
        /// <summary>
        /// Variable for thunder blending
        /// </summary>
        float increase = 0;

        protected override void Load()
        {
            base.Load();
            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }

            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/openning/darkbcit"));

            myLayers[1].AddEntity(backgroundTexture);
            myLayers[2].AddEntity(busSprite);
            TPEngine.Get().SpriteDictionary.Add("MyBusSprite", busSprite);
            myLayers[3].AddEntity(logoSprite);
            TPEngine.Get().SpriteDictionary.Add("MyLogoSprite", logoSprite);

            TPEngine.Get().Audio.LoadSong("thunder", @"sfx/thunder");
        }
        /// <summary>
        /// For 25 sec, openning is displayed. ESC and Mouse left click is way to skip
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (firstRun)
            {
                firstRun = false;
                startTime = gameTime.TotalGameTime.Seconds;
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PushState(new TDBaseState(), true);
               // TPEngine.Get().State.PushState(new GameLauncherState(), true); ;
            }
            /*old code
            base.Update(gameTime);
            if (firstRun)
            {
                firstRun = false;
                startTime = gameTime.TotalGameTime.Seconds;
                //set sound
                TPEngine.Get().Audio.PlaySong("thunder", true);
            }
            if (gameTime.TotalGameTime.Seconds - startTime > GamePlayingTime
                || Keyboard.GetState().IsKeyDown(Keys.Escape)
                || Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                TPEngine.Get().Audio.StopAllMusic();
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PushState(new GameLauncherState(), true); ;
            }
            */

        }
        /// <summary>
        /// for thinder alpha blender effect
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            int tileMapWidth = Width;
            int tileMapHeight = Height;

            batch.Begin(SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                SamplerState.PointWrap,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null);
            Texture2D texture = TPEngine.Get().TextureManager.LoadTexture(@"art/openning/thunder");

            while (increase < 10.0f)
            {

                increase += 0.1f;

                float alpha = increase;
                batch.Draw(texture,
                            new Rectangle(
                                0,
                                0,
                                1280,
                                720),
                            new Color(new Vector4(255.0f, 255.0f, 255.0f, 1.0f - (increase % 1.0f))));
            }
            batch.End();
        }
    }
}
