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
using DBCommService;
using Game.States;
using Game.Saving;
using System.Collections;

namespace Game
{
    class EndingState : TPState
    {
        /// <summary>
        /// Layer : 1
        /// </summary>
        TPLayer[] endLayers;
        Int16 NumLayers = 1;
        /// <summary>
        /// Backfround Texture
        /// </summary>
        TPSprite backgroundTexture;
        /// <summary>
        ///Variable for user data
        /// </summary>
        User user;
        DBCommServiceClient server;
        /// <summary>
        /// Character data from user
        /// </summary>
        Character character;
        /// <summary>
        /// User's total score
        /// </summary>
        int totalScore;
        /// <summary>
        /// variable to get starting time
        /// </summary>
        int startTime;
        /// <summary>
        /// variable to get ending time
        /// </summary>
        int EndingPlayingTime = 10;
        /// <summary>
        /// boolean to check the first time run
        /// </summary>
        bool endingfirstRun = true;
        /// <summary>
        ///true: pass courses false: fail
        /// </summary>
        bool isSuccess;

        /// <summary>
        /// Load function. Get user data and character information to get total score.
        /// And check whether user pass or not the courses.
        /// And display relative result.
        /// </summary>
        protected override void Load()
        {
            base.Load();
            isSuccess = false;
            endLayers = new TPLayer[NumLayers];

            // To get user's information. 
            user = GameLauncher_LoginButtonSprite.getUser();
            if (user == null || user.id == 0)
                return;
            server = new DBCommServiceClient();
            character = server.LoadCharacterData(user);
            int a = AreaState.character.global_score;
            // instance data to test ending. it will be deleted after connect main game.
            //character.global_score = 400;

            totalScore = character.global_score;
            // 4 weeks schedule: 1 week has 4 lab and 1 lab has 100 maximum point
            // So 4 * 4 * 100 == 1600, and 50% is 800 point
            // each minigame is not suitable to send score so I didnt use database category to calcaulate this.
            // some minigame and some situation character doesn't have correct data, so
            //check AreaState.character.global_score too.

            if (character.global_score >= 800)
                isSuccess = true;
            if(a >= 800)
                isSuccess = true;
            if (a > totalScore)
                totalScore = a;
            for (int x = 0; x < NumLayers; x++)
            {
                endLayers[x] = new TPLayer(this.layers);
            }

            //set texture and audio according to total socre
            if (isSuccess)
            {
                backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/endingSuccess"));
                TPEngine.Get().Audio.LoadSong("result", @"sfx/applause");
            }
            else
            {
                backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/endingFail"));
                TPEngine.Get().Audio.LoadSong("result", @"sfx/screamLong");
            }
            endLayers[0].AddEntity(backgroundTexture);
        }
        /// <summary>
        /// Esc or mouse left click let skip this layer
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Get the start gametime and set audio.
            if (endingfirstRun)
            {
                endingfirstRun = false;
                startTime = gameTime.TotalGameTime.Seconds;
                TPEngine.Get().Audio.PlaySong("result", true);
            }
            //Mouse left click and keyboard Esacpe and 30 sec later, ending will be expired
            if (gameTime.TotalGameTime.Seconds - startTime > EndingPlayingTime
                || Keyboard.GetState().IsKeyDown(Keys.Escape)
                || Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                TPEngine.Get().Audio.StopAllMusic();

                if (isSuccess)
                {
                    TPEngine.Get().State.PopState();
                    TPEngine.Get().State.PushState(new EndingPopUpState(
                        "You graduate CST." + Environment.NewLine +
                        "Your final score is " + totalScore + "." + Environment.NewLine +
                        successResult()), true);
                }
                else
                {
                    TPEngine.Get().State.PopState();
                    TPEngine.Get().State.PushState(new EndingPopUpState(
                        "Unfortunatly, You need to come back next year." + Environment.NewLine +
                        "Your final score is " + totalScore + "." + Environment.NewLine +
                        "Study Hard!"), true);
                }
                //TPEngine.Get().State.PopState();
            }
        }
        /// <summary>
        /// Generate result. 16 kinds of result is prepared.
        /// </summary>
        /// <returns> string result to display about the job</returns>
        public string successResult()
        {
            string result;
            switch (TPEngine.Get().Rand.Next(4))
            {
                case 0:
                    result = "You get Senior Programmer position in ";
                    break;
                case 1:
                    result = "You get Project Mannager position in ";
                    break;
                case 2:
                    result = "You get Jr. Programmer position in ";
                    break;
                default:
                    result = "You get Tester/Quality Assurence position in ";
                    break;
            }
            switch (TPEngine.Get().Rand.Next(4))
            {
                case 0:
                    result += Environment.NewLine + "Game Developing Company.";
                    break;
                case 1:
                    result += Environment.NewLine + "Wep Developing Company.";
                    break;
                case 2:
                    result += Environment.NewLine + "Security Company.";
                    break;
                default:
                    result += Environment.NewLine + "Mobile App Developing Company.";
                    break;
            }
            return result;
        }
    }
}