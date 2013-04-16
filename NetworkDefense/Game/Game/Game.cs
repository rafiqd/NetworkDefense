using System;
using System.Collections.Generic;
using System.Linq;
using Game.Events;
using Game.Saving;
using Game.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Engine;

namespace Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : TPGame
    {
        public static GifAnimation.GifAnimation anim1;
        public static GifAnimation.GifAnimation anim2;
        public Game()
        {
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            base.LoadContent();

            anim1 = Content.Load<GifAnimation.GifAnimation>("chip4");
            anim2 = Content.Load<GifAnimation.GifAnimation>("mob1");
            // TODO: use this.Content to load your game content here

            EventInfoManager LectureManager = new BirdHuntLectureInfoWriter();
            LectureManager.Write();
            LectureManager = new BirdHuntLabInfoWriter();
            LectureManager.Write();
            LectureManager = new PipeGameLectureInfoWriter();
            LectureManager.Write();
            LectureManager = new PipeGameLabInfoWriter();
            LectureManager.Write();
            LectureManager = new TdLectureInfoWriter();
            LectureManager.Write();
            LectureManager = new TdLabInfoWriter();
            LectureManager.Write();
            LectureManager = new WordSearchLectureInfoWriter();
            LectureManager.Write();
            LectureManager = new WordSearchLabInfoWriter();
            LectureManager.Write();
            TPEngine.Get().State.PushState(new OpenningState(), true);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            if(AreaState.character != null)
                SaveCharacterData.Save();
            // Stop the threads
        }
    }
}
