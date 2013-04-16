using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.States.TDsrc.Environment;
using Game.States.TDsrc.Character;
using Microsoft.Xna.Framework;
using Engine.StateManagement;
using Game.States.TDsrc.Towers;
using Microsoft.Xna.Framework.Input;
using Game.States.TDsrc.Projectiles;
using Game.States.TDsrc.UI;
using Game.States.TDsrc.Stats;
using Engine;
using Game.States.TDsrc.TDStates;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.States.TDsrc.Management
{

    /// <summary>
    /// Main manager for the Tower Defense Game
    /// </summary>
    static class TowerDefenseManager
    {
        /// <summary>
        /// Layers of the game to draw on
        /// </summary>
        public static TPLayer[] TDLayers;

        /// <summary>
        /// Number of layers
        /// </summary>
        public static int numLayers;

        /// <summary>
        /// Reference to the current wave being spawned.
        /// </summary>
        static EnemyWave currentWave;

        /// <summary>
        /// Time since the last wave was updated
        /// </summary>
        private static TimeSpan LastWaveUpdate;

        /// <summary>
        /// time between one wave ending and the next one spawning
        /// </summary>
        private static TimeSpan SpawnNextWaveIn = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Checks if X was pressed.
        /// </summary>
        private static bool clicked;

        /// <summary>
        /// Initalizes the map with the loaded file 
        /// </summary>
        /// <param name="map"></param>
        public static void Start(Map map, TPLayer[] layers)
        {
            MapCreator mc = new MapCreator(map);
            mc.load("CurrentMapData.txt");
            Enemy.findend();
            TDLayers = layers;
            UICreator.CreateUI();
            LastWaveUpdate = TimeSpan.Zero;
           
        }

        /// <summary>
        /// Sets the layers
        /// </summary>
        /// <param name="layers">layers to set</param>
        public static void setLayers(TPLayer[] layers)
        {
            TDLayers = layers;
        }


        /// <summary>
        /// This is where all the Tower defense update calls should be placed so that it is easy to transfer our
        /// project over to the main one without having to add a lot of code to the main program.
        /// </summary>
        /// <param name="gametime">current game time</param>
        /// <param name="keyboard">keyboard state</param>
        /// <param name="mouse">mouse state</param>
        public static void Update(GameTime gametime, KeyboardState keyboard, MouseState mouse)
        {
            Random random = new Random();
            if (LastWaveUpdate == TimeSpan.Zero)
            {
                LastWaveUpdate = gametime.TotalGameTime;
            }

            if (currentWave != null && !currentWave.complete )
            {
                currentWave.update(gametime);
                LastWaveUpdate = gametime.TotalGameTime;
            }
            else if((gametime.TotalGameTime - LastWaveUpdate) > SpawnNextWaveIn)
            {
                if (TDPlayerStats.currentWave == TDPlayerStats.maxWaves)
                {
                    TPEngine.Get().State.PopState();
                    TPEngine.Get().State.PushState(new TDScoreScreenState());
                }

                TDPlayerStats.Money += 5;
                TDPlayerStats.currentWave++;
                currentWave = new EnemyWave(random.Next(8,12), 1);
                
                //LastWaveUpdate = gametime.TotalGameTime;
            }

            if (TDPlayerStats.Grade <= 45)
            {
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PushState(new TDScoreScreenState());
            }

            if(Keyboard.GetState().IsKeyDown(Keys.X) && !clicked)
            {
                TDPlayerStats.Grade = 15;
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PushState(new TDScoreScreenState());
                clicked = true;
            }
            TowerBuilder.UpdateKeyboardInput(keyboard, mouse);
            Enemy.Update(gametime);
            Tower.Update(gametime);
            Projectile.UpdateProjectiles(gametime);
            Window.Update(gametime, keyboard, mouse);
        }

    }
}
