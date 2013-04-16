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
namespace Game
{
    class WashRoomState : TPState
    {
        /// <summary>
        /// Total layers
        /// </summary>
        TPLayer[] myLayers;
        Int16 NumLayers = 7;
        TargetSprite TargetSprite;
        /// <summary>
        /// Background Texture
        /// </summary>
        TPSprite backgroundTexture;
        /// <summary>
        /// Fly sprites. Total 5 flies.
        /// </summary>
        FlySprite[] FlySprite = new FlySprite[5];
        /// <summary>
        /// Flag sprite. Total 30 flags.
        /// </summary>
        FlagSprite[] FlagSprite = new FlagSprite[30];
        /// <summary>
        /// Water sprite. Display remained water amount. 30 ticks
        /// </summary>
        WaterSprite[] waterSprite = new WaterSprite[30];
        /// <summary>
        /// Dead flies sprite. Total 5.
        /// </summary>
        DeadFlySprite[] deadFlySprite = new DeadFlySprite[5];
        /// <summary>
        /// Score sprite.
        /// </summary>
        TPString score = new TPString("Flies Caught: 0");
        /// <summary>
        /// Shoot sprite
        /// </summary>
        ShootSprite shootSprite;
        /// <summary>
        /// FirstRun, set other states
        /// </summary>
        bool firstRun = true;
        /// <summary>
        /// Flag index between 0~29
        /// </summary>
        int flagIndex;

        /// <summary>
        /// This has total 7 layers for washroom.
        /// </summary>
        protected override void Load()
        {
            base.Load();
            flagIndex = 0;
            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }
            //for layer 0
            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/toilet"));
            myLayers[0].AddEntity(backgroundTexture);

            //for layer 1
            for (int i = 0; i < 30; i++)
            {
                FlagSprite[i] = new FlagSprite();
                myLayers[1].AddEntity(FlagSprite[i]);
                TPEngine.Get().SpriteDictionary.Remove("MinigameFlagSprite" + i);
                TPEngine.Get().SpriteDictionary.Add("MinigameFlagSprite" + i, FlagSprite[i]);
            }

            //for layer 2
            for (int i = 0; i < 5; i++)
            {
                FlySprite[i] = new FlySprite();
                myLayers[2].AddEntity(FlySprite[i]);
            }

            //for layers 3
            TargetSprite = new TargetSprite(new Vector2(200, 200));
            TPEngine.Get().SpriteDictionary.Remove("MinigameTargetSprite");
            TPEngine.Get().SpriteDictionary.Add("MinigameTargetSprite", TargetSprite);
            myLayers[3].AddEntity(TargetSprite);

            //for layer 4
            for (int i = 0; i < 30; i++)
            {
                waterSprite[i] = new WaterSprite();
                myLayers[4].AddEntity(waterSprite[i]);
                waterSprite[i].Position = new Vector2(60, 610 - (5 * i));
                waterSprite[i].onScreen = true;
                TPEngine.Get().SpriteDictionary.Remove("MinigameWaterSprite" + i);
                TPEngine.Get().SpriteDictionary.Add("MinigameWaterSprite" + i, waterSprite[i]);
            }
            //for layer 5
            shootSprite = new ShootSprite(new Vector2(-100, -100));
            myLayers[5].AddEntity(shootSprite);
            TPEngine.Get().SpriteDictionary.Remove("MinigameShootSprite");
            TPEngine.Get().SpriteDictionary.Add("MinigameShootSprite", shootSprite);


            //for layer 6
            for (int i = 0; i < 5; i++)
            {
                deadFlySprite[i] = new DeadFlySprite(new Vector2(2000, 2000));
                myLayers[6].AddEntity(deadFlySprite[i]);
                TPEngine.Get().SpriteDictionary.Remove("MinigameDeadFlySprite" + i);
                TPEngine.Get().SpriteDictionary.Add("MinigameDeadFlySprite" + i, deadFlySprite[i]);
            }

            // for score layer 
            score.Position.X = 130;
            score.Position.Y = 70;

            score.RenderColor = Color.Green;
            myLayers[2].AddEntity(score);

            TPEngine.Get().StringDictionary.Remove("MinigameScore");
            TPEngine.Get().StringDictionary.Add("MinigameScore", score);
            TPEngine.Get().Audio.LoadSong("flysound", @"sfx/fly");

        }
        /// <summary>
        /// In this Update, check flies, and if they are catched change texture for dead flies.
        /// And they check whether drawing flag is start or not.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandleInput(gameTime);
            if (firstRun)
            {
                firstRun = false;
                TPEngine.Get().Audio.PlaySong("flysound", true);

            }
            for (int i = 0; i < 5; i++)
            {
                if (!FlySprite[i].IsLive)
                {
                    deadFlySprite[i].Position.X = FlySprite[i].Position.X;
                    deadFlySprite[i].Position.Y = FlySprite[i].Position.Y;
                }
            }

            if (shootSprite.IsFinish)
            {
                TPEngine.Get().Audio.StopAllMusic();
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PushState(new ResultState(), true);
            }
        }

        /// <summary>
        /// Handle all input that directly relates to the sprite.
        /// Espatially this input is for flag generation.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        private void HandleInput(GameTime gameTime)
        {
            if (!shootSprite.IsStart && Mouse.GetState().LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.Milliseconds % 100 == 0)
            {
                //To draw flag finish, start to shoot
                if (flagIndex == 30)
                {
                    shootSprite.IsStart = true;

                    flagIndex = 0;
                }
                if (!FlagSprite[flagIndex].onScreen && flagIndex < 30)
                {
                    FlagSprite[flagIndex].onScreen = true;
                    FlagSprite[flagIndex].Position.X = Mouse.GetState().X;
                    FlagSprite[flagIndex].Position.Y = Mouse.GetState().Y;

                    if (waterSprite[29 - flagIndex].onScreen)
                    {
                        waterSprite[29 - flagIndex].Position = new Vector2(-50, -50);
                    }
                    flagIndex++;
                }
            }
        }
    }
}
