using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DBCommService;
using Engine;
using Engine.Objects;
using Engine.StateManagement;
using Engine.SubSystems;
using Game.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Game.States;

namespace Game
{
    /// <summary>
    /// Cyril, Peter
    /// Bird Hunt game state. This screen displays and allows the user to play bird hunt game. Within the time limit, the user has to
    /// score a certain score.
    /// </summary>
    class BirdHuntState : TPState
    {
        /// <summary>
        /// an array type TPLayer that keeps track how many layers will be displayed on the screen
        /// </summary>
        TPLayer[] myLayers;

        /// <summary>
        /// number of layers to be displayed
        /// </summary>
        Int16 NumLayers = 3;

        /// <summary>
        /// crosshair sprite indicate where the user's aiming
        /// </summary>
        Bullseye bullseye;

        /// <summary>
        /// background image
        /// </summary>
        TPSprite backgroundTexture;

        /// <summary>
        /// ammo bar sprite. represents the amount of ammo the player currently has
        /// </summary>
        Ammobar ammobar;

        /// <summary>
        /// first bird's sprite
        /// </summary>
        Bird bird1;

        /// <summary>
        /// second bird's sprite
        /// </summary>
        Bird bird2;

        /// <summary>
        /// the label for the score counter
        /// </summary>
        TPString score = new TPString("Score: 0");

        /// <summary>
        /// the label for the level counter
        /// </summary>
        TPString level = new TPString("Level: 1");

        /// <summary>
        /// the initial game time when the game is started
        /// </summary>
        int startTime;

        /// <summary>
        /// a flag to keep track if the game is first run to set the timer
        /// </summary>
        bool firstRun = true;

        /// <summary>
        /// a variable to track if the state is opened for the 2nd time. If it is, don't add any new dictionary items that already in there
        /// </summary>
        static bool BirdHuntFirstRun = true;

        /// <summary>
        /// the total elapsed time in second
        /// </summary>
        private int numSecond;

        /// <summary>
        /// the countdown timer label
        /// </summary>
        TPString time = new TPString("Time: 30");

        /// <summary>
        /// bullet hole sprite, indicate where the player shoots
        /// </summary>
        BulletHole bulletHole = new BulletHole();

        /// <summary>
        /// explosion sprite, store a list of sprites with similar behaviour to illustrate a hit
        /// </summary>
        Explosion explosion = new Explosion();

        /// <summary>
        /// a queue of sprites to represent when the bird falls down
        /// </summary>
        BirdFall fall = new BirdFall("deadup", "deadright", "deaddown", "deadleft");

        /// <summary>
        /// stores the previous state of the mouse
        /// </summary>
        MouseState prevMouseState;

        /// <summary>
        /// stores the previous state of the keyboard
        /// </summary>
        KeyboardState prevKeyboardState;

        /// <summary>
        /// load the necessary items that needed to display on screen
        /// </summary>
        protected override void Load()
        {
            base.Load();
            //difficulty = Areastate...
            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }
            //initialize the position of the crosshair
            bullseye = new Bullseye(new Vector2(200, 200));
            myLayers[2].AddEntity(bullseye);

            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/parkinglot"));
            backgroundTexture.Position = new Vector2(backgroundTexture.Position.X, backgroundTexture.Position.Y - 400);
            myLayers[0].AddEntity(backgroundTexture);
            bird1 = new Bird("bird2goingrightwingup", "bird2goingrightwingdown", "bird2goingleftwingdown", "bird2goingleftwingup", 2);
            bird2 = new Bird("bird2goingrightwingup", "bird2goingrightwingdown", "bird2goingleftwingdown", "bird2goingleftwingup", 4);

            ammobar = new Ammobar();

            myLayers[1].AddEntity(bird1);
            myLayers[1].AddEntity(bird2);
            myLayers[1].AddEntity(bulletHole);
            myLayers[1].AddEntity(explosion);
            myLayers[1].AddEntity(fall);
            if (!TPEngine.Get().SpriteDictionary.ContainsKey("bulletHole"))
            {
                TPEngine.Get().SpriteDictionary.Add("bulletHole", bulletHole);
            }
            if (!TPEngine.Get().SpriteDictionary.ContainsKey("explosion"))
            {
                TPEngine.Get().SpriteDictionary.Add("explosion", explosion);
            }
            if (!TPEngine.Get().SpriteDictionary.ContainsKey("fall"))
            {
                TPEngine.Get().SpriteDictionary.Add("fall", fall);
            }
            if (!TPEngine.Get().SpriteDictionary.ContainsKey("MinigameBullseye"))
            {
                TPEngine.Get().SpriteDictionary.Add("MinigameBullseye", bullseye);
            }
            TPEngine.Get().Audio.LoadSFX(@"sfx/xbuster");
            TPEngine.Get().Audio.LoadSFX(@"sfx/shotgunreload");
            TPEngine.Get().Audio.LoadSFX(@"sfx/mariocoin");
            

            score.Position.X = 80;
            score.Position.Y = TPEngine.Get().ScreenSize.Height - 75;
            score.RenderColor = Color.AliceBlue;

            ammobar.Position.X = 250;
            ammobar.Position.Y = TPEngine.Get().ScreenSize.Height - 110;


            level.Position.X = TPEngine.Get().ScreenSize.Width - 170;
            level.Position.Y = TPEngine.Get().ScreenSize.Height - 55;
            level.RenderColor = Color.AliceBlue;

            time.Position.X = TPEngine.Get().ScreenSize.Width - 170;
            time.Position.Y = TPEngine.Get().ScreenSize.Height - 100;
            time.RenderColor = Color.Firebrick;

            myLayers[1].AddEntity(score);
            myLayers[1].AddEntity(ammobar);
            myLayers[1].AddEntity(level);
            myLayers[1].AddEntity(time);
            if (BirdHuntFirstRun)
            {
                TPEngine.Get().StringDictionary.Add("MinigameScore", score);
                TPEngine.Get().StringDictionary.Add("MinigameLevel", level);
                TPEngine.Get().StringDictionary.Add("MinigameTime", time);
                BirdHuntFirstRun = false;
            }
            TPEngine.Get().State.PushState(new BirdMenuState(), true);
        }

        /// <summary>
        /// Update the sprites with each iteration. When the time's up. add new score into the database
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (firstRun)
            {
                firstRun = false;
                startTime = (int)gameTime.TotalGameTime.TotalSeconds;
            }

            //Update the time
            numSecond = (int)Math.Floor(31 - (gameTime.TotalGameTime.TotalSeconds - startTime));
            time = new TPString(numSecond.ToString());
            TPString tempTime = TPEngine.Get().StringDictionary["MinigameTime"];
            tempTime.Clear();
            tempTime.Append("Time: " + time);

            //time limit for the game is 30 seconds, after 30 seconds, add whatever score the user earned during the game into database
            if (numSecond < 0)
            {
                using (DBCommServiceClient service = new DBCommServiceClient())
                {
                    MinigameScore thescore = new MinigameScore()
                    {
                        CharacterID = AreaState.character.id,
                        Score = Bullseye.TotalScore,
                        MinigameID = 1,
                        Lecture_Attended = service.GetLectureAttended(GameLauncher_LoginButtonSprite.getUser(), AreaState.character, 1)
                    };
                    service.SaveMinigameScore(GameLauncher_LoginButtonSprite.getUser(), thescore);
                }
                TPEngine.Get().State.PushState(new HighScoreState(), true);
            }

            if (prevMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Ammobar.Ammo > 0)
            {
                Ammobar.ShotFired();
            }
            if (Mouse.GetState().X <= 0)
                Mouse.SetPosition(0, Mouse.GetState().Y);

            if (Mouse.GetState().X >= TPEngine.Get().ScreenSize.Width)
                Mouse.SetPosition(TPEngine.Get().ScreenSize.Width, Mouse.GetState().Y);

            if (Mouse.GetState().Y <= 0)
                Mouse.SetPosition(Mouse.GetState().X, 0);

            if (Mouse.GetState().Y >= TPEngine.Get().ScreenSize.Height)
                Mouse.SetPosition(Mouse.GetState().X, TPEngine.Get().ScreenSize.Height);

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }
    }
}
