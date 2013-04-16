using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;
using Engine;
using Game.Sprites;
using Microsoft.Xna.Framework;
using Game.States.TDsrc.UI;
using Game.States.TDsrc.Management;
using Microsoft.Xna.Framework.Input;
using Game.States.TDsrc.Environment;
using Game.States.TDsrc.Towers;
using Game.States.TDsrc.Stats;
using Game.Events;
using Microsoft.Win32;

namespace Game.States.TDsrc.TDStates
{
    /// <summary>
    /// Starting screen state
    /// </summary>
    class TDStartState : TPState
    {
        /// <summary>
        /// Background sprite
        /// </summary>
        private TPSprite background;

        /// <summary>
        /// Layers for this state
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
        /// Window that holds the two buttons on this state
        /// </summary>
        private Window menuwindow;
        
        /// <summary>
        /// X position of menuwindow
        /// </summary>
        private const int menuxpos = 500;

        /// <summary>
        /// Y position of menuwindow
        /// </summary>
        private const int menuypos = 400;

        /// <summary>
        /// State is alive or dead
        /// </summary>
        private Boolean dead;
        
        /// <summary>
        /// Constructor of the TDStartState
        /// </summary>
        public TDStartState()
        {
            tdMouse = new TDPointer(new Vector2(200, 200));
            TDLayers = new TPLayer[numLayers];
            for (int i = 0; i < numLayers; i++)
            {
                TDLayers[i] = new TPLayer(layers);
            }
            background = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/StartScreenBackground"));
            TDLayers[0].AddEntity(background);
            TDLayers[numLayers-1].AddEntity(tdMouse);
            TowerDefenseManager.setLayers(TDLayers);
            
            menuwindow = new Window(menuxpos, menuypos, true, TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/startscreenWindow"));
            menuwindow.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/StartButton"), "start", startGame);
            menuwindow.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/ExitButton"), "exit", quitGame);

        }

        /// <summary>
        /// Starts the TDGame state
        /// </summary>
        /// <param name="name">variable passed from button</param>
        public void startGame(string name)
        {
            if(!dead)
            {
                Tower.ResetTowerLocks();
                TDPlayerStats.ResetStats();
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PushState(new TDState(), true);
                RegistryKey mykey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\" + TdLectureInfoWriter.Key_Folder);
                string color1 = (string)mykey.GetValue(TdLectureInfoWriter.Key_Color1);
                string color2 = (string)mykey.GetValue(TdLectureInfoWriter.Key_Color2);
                int dmg1 = (int)mykey.GetValue(TdLectureInfoWriter.Key_Dmg1);
                int dmg2 = (int)mykey.GetValue(TdLectureInfoWriter.Key_Dmg2);
                
                if(color1 != null && color1 != "")
                {
                    TDDamageManager.setDamage(color1,color2,dmg1,dmg2);
                }

                                            
                dead = true;
            }
            
        }

        /// <summary>
        /// Quits the TowerDefense game
        /// </summary>
        /// <param name="name">variable passed through by the button</param>
        public void quitGame(string name)
        {
            if(!dead)
            {
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PopState();
                dead = true;
            }
            
        }

        /// <summary>
        /// Updates this state
        /// </summary>
        /// <param name="gameTime">current gametime</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Window.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
            base.Update(gameTime);
        }

        /// <summary>
        /// draws this state
        /// </summary>
        /// <param name="batch">current batchsprite</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}
