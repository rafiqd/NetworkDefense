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
    class ResultState : TPState
    {
        /// <summary>
        /// variable for 2 layers
        /// </summary>
        TPLayer[] myLayers;
        Int16 NumLayers = 1;
        TPSprite scoreBoardSprite;
        /// <summary>
        /// location for the first string
        /// </summary>
        int xPos = 430;
        int yPos = 300;
        /// <summary>
        /// interval between strings
        /// </summary>
        int interval = 100;
        /// <summary>
        /// variable for rate to display in scoreboard
        /// </summary>
        int rate;
        /// <summary>
        /// variable to check mouse release
        /// </summary>
        bool isReady = false;
        /// <summary>
        /// variables to store string to display on scoreboard
        /// </summary>
        TPString string1, string2, string3;
        
        protected override void Load()
        {
            base.Load();
            myLayers = new TPLayer[NumLayers];

            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }
            scoreBoardSprite = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/scoreboard"));
            scoreBoardSprite.Position.X = 240;
            scoreBoardSprite.Position.Y = 60;
            myLayers[0].AddEntity(scoreBoardSprite);
            
            adjustKillFlies();
            adjustRate();
            adjustTimeLose();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                isReady = true;
            }
            if (isReady && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                TPEngine.Get().State.PopState();

            }
        }
        /// <summary>
        /// The function to set string's color and position, and add on myLayers[0]
        /// </summary>
        /// <param name="s">string to display</param>
        /// <param name="x">string's x-axis location</param>
        /// <param name="y">string's y-axis location</param>
        public void setString(TPString s, int x, int y)
        {
            s.Position.X = x;
            s.Position.Y = y;

            s.RenderColor = Color.Black;
            myLayers[0].AddEntity(s);
        }
        /// <summary>
        /// Make sentence to display about Dead Flies
        /// </summary>
        public void adjustKillFlies()
        {
            TPString score = TPEngine.Get().StringDictionary["MinigameScore"];
            ShootSprite shootSprite = (ShootSprite)TPEngine.Get().SpriteDictionary["MinigameShootSprite"];
            string1 = new TPString("Dead Flies: " + shootSprite.TotalScore);
            setString(string1, xPos, yPos);
        }
        /// <summary>
        /// Make sentence to display about Rate ( totalscore / fryamount(5) * 100)
        /// </summary>
        public void adjustRate()
        {
            TPString score = TPEngine.Get().StringDictionary["MinigameScore"];
            ShootSprite shootSprite = (ShootSprite)TPEngine.Get().SpriteDictionary["MinigameShootSprite"];
            rate = (shootSprite.TotalScore  * 20);
            string2 = new TPString("   Rate      : " + rate + " %" );
            setString(string2, xPos , yPos += interval);
        }
        /// <summary>
        /// Make sentece to display about Time Lose ( 2~ 10mins), and send time lose to main game's clock
        /// </summary>
        public void adjustTimeLose()
        {
            TPSprite temp;
            if (!TPEngine.Get().SpriteDictionary.TryGetValue("MainGameClock", out temp))
            {
                int hour = 8;
                temp = new Clock(1, 8, 0, 0);
            }
            int lose = 0;
            switch (rate)
            { 
                case 0:
                    lose = 10;
                    break;
                case 20:
                    lose = 8;
                    break;
                case 40:
                    lose = 6;
                    break;
                case 60:
                    lose = 5;
                    break;
                case 80:
                    lose = 4;
                    break;
                case 100:
                    lose = 2;
                    break;                
            }
            Clock clock = (Clock)temp;
            clock.Advance(0, lose);
            string3 = new TPString("    Time Lose : " + lose + " min");
            setString(string3, xPos , yPos += interval);
        }
    }
}
