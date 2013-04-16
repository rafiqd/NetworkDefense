using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCommService;
using Engine.StateManagement;
using Game.Saving;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;
using Game.Sprites;
using Game.States.TDsrc.UI;
using Microsoft.Xna.Framework;
using Engine;
using Game.States.TDsrc.Management;
using Game.States.TDsrc.Stats;
using Microsoft.Xna.Framework.Input;


namespace Game.States.TDsrc.TDStates
{
    /// <summary>
    /// Score screen state that appears at the end of the game
    /// </summary>
    class TDScoreScreenState : TPState 
    {
        /// <summary>
        /// Background Image
        /// </summary>
        private TPSprite background;
        
        /// <summary>
        /// Array of Layers on the state
        /// </summary>
        TPLayer[] TDLayers;

        /// <summary>
        /// Number of layers
        /// </summary>
        private int numLayers = 6;

        /// <summary>
        /// Mouse for this state
        /// </summary>
        private TDPointer tdMouse;

        /// <summary>
        /// Window that has the exit button in it.
        /// </summary>
        private Window menuwindow;

        /// <summary>
        /// x Position of the window
        /// </summary>
        private const int menuxpos = 500;

        /// <summary>
        /// y Position of the window
        /// </summary>
        private const int menuypos = 400;

        /// <summary>
        /// Font used for the text
        /// </summary>
        SpriteFont Font;

        /// <summary>
        /// Constructor
        /// </summary>
        public TDScoreScreenState()
        {
            tdMouse = new TDPointer(new Vector2(200, 200));
            TDLayers = new TPLayer[numLayers];
            for (int i = 0; i < numLayers; i++)
            {
                TDLayers[i] = new TPLayer(layers);
            }
            background = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/ScoreScreenBackGround"));
            TDLayers[0].AddEntity(background);
            TDLayers[numLayers - 1].AddEntity(tdMouse);
            TowerDefenseManager.setLayers(TDLayers);

            menuwindow = new Window(menuxpos, menuypos, true, TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/startscreenWindow"));
            menuwindow.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/ExitButton"), "exit", quitGame);

            Font = TPEngine.Get().FontManager.LoadFont(@"fonts/testfont");
        }

        /// <summary>
        /// Called when the user clicks exit
        /// </summary>
        /// <param name="text"></param>
        public void quitGame(string text)
        {
            UpdateDbScore();
            TPEngine.Get().State.PopState();
            TPEngine.Get().State.PopState();
        }

        /// <summary>
        /// Updates the score stored in the database
        /// </summary>
        public void UpdateDbScore()
        {
            User user = GameLauncher_LoginButtonSprite.getUser();
            if (user == null || user.id == 0)
                return;
            DBCommServiceClient server = new DBCommServiceClient();
            MinigameScore minigame = new MinigameScore();
            DBCommService.Character character = server.LoadCharacterData(user);
            minigame.CharacterID = character.id;
            minigame.CharacterName = character.name;
            minigame.Lecture_Attended = false;
            minigame.Score = TDPlayerStats.Grade;
            minigame.MinigameID = 4;
            server.SaveMinigameScore(user, minigame);
            character.global_score += minigame.Score;
        }

        /// <summary>
        /// Draws the score screen.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            batch.Begin();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Lab Grade:           " + TDPlayerStats.Grade + "/100");
            sb.AppendLine("Virus' Killed:       " + TDPlayerStats.KillCount);
            sb.AppendLine("Remaining Money:     " + TDPlayerStats.Money);
            batch.DrawString(Font, sb.ToString(), new Vector2(TPEngine.Get().ScreenSize.Width / 2 - 100, 200), new Color(0, 240, 240));
            sb = null;
            batch.End();
        }

        /// <summary>
        /// Loads resources used by the state
        /// </summary>
        protected override void Load()
        {
            base.Load();
        }

        /// <summary>
        /// Updates this state
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Window.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
        }
    }
}
