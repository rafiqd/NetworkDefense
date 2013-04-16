using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.SubSystems;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;

namespace Engine
{
    public class TPEngine
    {
        public TPFontManager FontManager { get; set; }
        public TPTextureManager TextureManager { get; set; }
        public TPGame GameRef { get; set; }
        public TPStateMachine State { get; set; }
        public TPAudioManager Audio { get; set; }
        public TPInput Input { get; set; }
        public Random Rand { get; set; }
        /// <summary>
        /// Our game resolution. Not encapuslated in a property beacuse of a flaw with .NET and not being able to 
        /// modify members of a struct through a public property.
        /// </summary>
        public Viewport ScreenSize;

        public Dictionary<String, TPSprite> SpriteDictionary;
        public Dictionary<String, TPString> StringDictionary;


        /// <summary>
        /// Used for initializing the engine. 
        /// </summary>
        /// <param name="gameref">A reference to the game class.</param>
        public void Initialize(TPGame gameref)
        {
            GameRef = gameref;
            TextureManager = new TPTextureManager(gameref);
            FontManager = new TPFontManager(gameref);
            Audio = new TPAudioManager(gameref);
            State = new TPStateMachine();
            Input = new TPInput();
            Rand = new Random();

            SpriteDictionary = new Dictionary<string, TPSprite>();
            StringDictionary = new Dictionary<string, TPString>();
        }

        public void Update(GameTime gameTime)
        {
            State.Update(gameTime);
            Input.Update(gameTime);
        }

        #region Singleton code
        private static TPEngine m_Singleton;
        private TPEngine() { }
        public static TPEngine Get()
        {
            if (m_Singleton == null)
            {
                m_Singleton = new TPEngine();
            }
            return m_Singleton;
        }
        #endregion
    }
}
