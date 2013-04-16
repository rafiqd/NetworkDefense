using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Game.States.TDsrc.Environment;
using Game.Sprites;
using Game.States.TDsrc.Management;
using Game.States.TDsrc.Stats;

namespace Game.States.TDsrc.Character
{

    /// <summary>
    /// Represents "Wave" of enemies being spawned, controls things like
    /// how fast they'll move, their hp, and their spawn rate.
    /// </summary>
    class EnemyWave
    {
        /// <summary>
        /// Amount of enemies to spawn
        /// </summary>
        int waveSize;

        /// <summary>
        /// If the wave is complete or not
        /// </summary>
        public bool complete;

        /// <summary>
        /// Rate to spawn the enemies at
        /// </summary>
        TimeSpan spawnRate;

        /// <summary>
        /// Previous time the wave was updated
        /// </summary>
        TimeSpan previousUpdateTime;

        /// <summary>
        /// Consturctor that sets the amount of enemies spawned in a wave and the
        /// rate at which they are spawned.
        /// </summary>
        /// <param name="size">Amount of enemies to spawn</param>
        /// <param name="spawnRateSeconds">Rate at which to spawn enemies</param>
        public EnemyWave(int size, double spawnRateSeconds)
        {
            waveSize = size;
            spawnRate = TimeSpan.FromSeconds(spawnRateSeconds);
            previousUpdateTime = TimeSpan.Zero;
        }


        /// <summary>
        /// Updates this instance of EnemyWave class
        /// </summary>
        /// <param name="gameTime">current gametime</param>
        public void update(GameTime gameTime)
        {
            // doesn't update if a certian time hasn't passed
            if (gameTime.TotalGameTime - previousUpdateTime > spawnRate)
            {
                Random random = new Random();
                if (waveSize <= 0 && Enemy.enemyList.Count == 0)
                {
                    complete = true;
                    return;
                }

                if (waveSize-- > 0)
                {
                    Enemy e = new Enemy(Map.currentMap);
                    int movementspeed = random.Next(20, 100);
                    e.movetime = TimeSpan.FromMilliseconds(movementspeed);
                    

                    double newhealth = (70 * TDPlayerStats.currentWave) + movementspeed * (Math.Pow(TDPlayerStats.currentWave, 2));
                    e.SetHealth((int)newhealth);
                    
                }

                spawnRate = TimeSpan.FromMilliseconds(random.Next(500, 4000));
                previousUpdateTime = gameTime.TotalGameTime;
            }
        }
    }
}
