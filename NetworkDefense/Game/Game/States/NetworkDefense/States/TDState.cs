using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Objects;
using Engine.StateManagement;
using Game.Sprites;
using Game.States.TDsrc.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.States.TDsrc.Character;
using Game.States.TDsrc.Towers;
using Game.States.TDsrc.Management;
using Game.States.TDsrc.Projectiles;
using Game.States.TDsrc.Stats;
using Game.States.TDsrc.UI;

namespace Game.States
{
    /// <summary>
    /// Main game state for tower defense
    /// </summary>
    class TDState: TPState
    {
        /// <summary>
        /// Layers of this state
        /// </summary>
        TPLayer[] TDLayers;

        /// <summary>
        /// Number of layers
        /// </summary>
        private int numLayers = 6;

        /// <summary>
        /// Mouse object
        /// </summary>
        private TDPointer tdMouse;

        /// <summary>
        /// Background texture
        /// </summary>
        private TPSprite backgroundTexture;

        /// <summary>
        /// Current map
        /// </summary>
        Map map;

        /// <summary>
        /// Current map creator
        /// </summary>
        MapCreator mp;

        /// <summary>
        /// Font used for the text
        /// </summary>
        SpriteFont Font;

        /// <summary>
        /// Loads the resources used by this state
        /// </summary>
        protected override void Load()
        {
            base.Load();

            tdMouse = new TDPointer(new Vector2(200, 200));
            TDLayers = new TPLayer[numLayers];
            for (int i = 0; i < numLayers; i++)
            {
                TDLayers[i] = new TPLayer(layers);
            }
            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/CurrentMap"));
            
            //backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/mob"));
            TDLayers[0].AddEntity(backgroundTexture);
            TDLayers[5].AddEntity(tdMouse);

            map = new Map(1280, 720);
            TowerBuilder.Load(map);
            mp = new MapCreator(map);
            TowerDefenseManager.numLayers = numLayers;
            TowerDefenseManager.Start(map, TDLayers);
            Font = TPEngine.Get().FontManager.LoadFont(@"fonts/testfont");
        }

        /// <summary>
        /// Draws elements on the screen.
        /// </summary>
        /// <param name="batch">current batchsprite</param>
        public override void Draw(SpriteBatch batch)
        {
            if (Alive)
            {
                batch.Begin();
                backgroundTexture.Draw(batch);
                batch.End();
                batch.GraphicsDevice.Clear(Color.Black);    
                base.Draw(batch);
                batch.Begin();
                drawHud(batch);
                batch.End();
            }
        }

        /// <summary>
        /// Draws the HUD elements
        /// </summary>
        /// <param name="batch"></param>
        public void drawHud(SpriteBatch batch)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Money: $" + TDPlayerStats.Money + "     ");
            sb.Append("Grade: " + TDPlayerStats.Grade + "/100" + "     ");
            sb.Append("Current Wave: " + TDPlayerStats.currentWave + "/" + TDPlayerStats.maxWaves);
            batch.DrawString(Font, sb.ToString(), new Vector2(50, 50), new Color(0, 240, 240));
            sb.Clear();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            TowerDefenseManager.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
          
            base.Update(gameTime);
        }
    }
}
