using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using DBCommService;
using Game.States;

namespace Game.Sprites
{
    /// <summary>
    /// Class that will display the end game state of the wordSearch
    /// </summary>
    class WordSearch_EndGameSprite : TPSprite
    {
        /// <summary>
        /// Variables to store the string that will be displayed on the screen
        /// </summary>
        TPString label,
                 scoreLabel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameState">the game state, if the player lost or won</param>
        /// <param name="score">the players score</param>
        public WordSearch_EndGameSprite(string gameState, int score)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/1x1WhitePixel"))
        {
            label = new TPString(gameState);
            label.Scale = new Vector2(1.5f, 1.5f);
            label.Position = new Vector2(650, 300);
            label.RenderColor = Color.Black;


            scoreLabel = new TPString("Your Final Grade is: " + score);
            scoreLabel.Scale = new Vector2(0.8f, 0.8f);
            scoreLabel.Position = new Vector2(650, 350);
            scoreLabel.RenderColor = Color.Black;

            using (DBCommServiceClient srv = new DBCommServiceClient())
            {
                MinigameScore gameScore = new MinigameScore()
                {
                    CharacterID = AreaState.character.id,
                    MinigameID = 3,
                    Score = score,
                    Lecture_Attended = srv.GetLectureAttended(GameLauncher_LoginButtonSprite.getUser(), AreaState.character, 3)

                };
                srv.SaveMinigameScore(GameLauncher_LoginButtonSprite.getUser(), gameScore);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(Keyboard.GetState().IsKeyDown(Keys.X))
            {
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PopState();
            }
        }
        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch">used to draw the sprite</param>
        public override void Draw(SpriteBatch batch)
        {
            label.Draw(batch);
            scoreLabel.Draw(batch);
        }
    }
}
