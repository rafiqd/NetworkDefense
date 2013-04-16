using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Game.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Game.States.TDsrc.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.Objects;
using Game.States.TDsrc.Management;
using Game.States.TDsrc.Stats;

namespace Game.States.TDsrc.Character
{
    /// <summary>
    /// Class represents the enemies that are spawned.
    /// </summary>
    public class Enemy
    {
        /// <summary>
        /// List of all the enemies
        /// </summary>
        public static List<Enemy> enemyList;

        /// <summary>
        /// Number of enemies that have been created
        /// </summary>
        private static int count;

        /// <summary>
        /// Temporary variable to hold a reference to the enemies' sprite
        /// </summary>
        private TdMobSprite temp;

        /// <summary>
        /// Static initalization for the list and count
        /// </summary>
        static Enemy()
        {
            count = 0;
            enemyList = new List<Enemy>();
        }

        /// <summary>
        /// Health of the enemy
        /// </summary>
        public int health;

        /// <summary>
        /// Current Xposition of the enemy, in Tiles not pixels
        /// </summary>
        public int xpos;

        /// <summary>
        /// Current Yposition of the enemy, in Tiles not pixels
        /// </summary>
        public int ypos;

        /// <summary>
        /// Tells if the enemy is dead or alive
        /// </summary>
        public bool dead;

        /// <summary>
        /// Unique ID for each enemy spawned.
        /// </summary>
        public int id;

        /// <summary>
        /// The dollar value the enemy is worth when killed.
        /// </summary>
        public int worth;

        /// <summary>
        /// Sets the health of the enemy, also sets it's worth
        /// </summary>
        /// <param name="health">health amount</param>
        public void SetHealth(int health)
        {
            temp.maxHealth = this.health = health;
            worth = health / 100;
        }

        /// <summary>
        /// this array is meant to hold the last X Y positions the mob was at so that it does
        /// not end up going backwards at certian points. it's a 2D array because the 2nd dimension
        /// needs to hold both the X and Y location. eg it will look like this
        /// {  {x1,y1},  {x2,y2},  {x3,y3},  {x4,y4},  {x5, y5}  }
        /// </summary>
        int[][] prevpos; 
        
        /// <summary>
        /// this one is used as a counter for the prevpos[][] array
        /// </summary>
        int prevposCounter; 

        /// <summary>
        /// Current Map
        /// </summary>
        Map map;

        /// <summary>
        /// List of spawn locations
        /// </summary>
        int[][] spawnlocs;

        /// <summary>
        /// Area of the end location.
        /// </summary>
        static int[] endlocs;

        /// <summary>
        /// Sprite that represents the enemy
        /// </summary>
        public TPSprite enemySprite;


        /// <summary>
        /// Update Frequency
        /// </summary>
        public TimeSpan movetime = TimeSpan.FromSeconds(.05f);

        /// <summary>
        /// Time that the mob previously moved
        /// </summary>
        private TimeSpan previousmove = TimeSpan.Zero;

        /// <summary>
        /// Enemy Constructor
        /// Takes in a map for the enemy to traverse
        /// adds the enemy to the list of all enemies
        /// sets the enemy to the start location
        /// </summary>
        /// <param name="map">map for the enemy to traverse</param>
        public Enemy(Map map)
        {
            id = count++;
            int i;
            prevpos = new int[9][];
            spawnlocs = new int[3][];
            for (i = 0; i < 9; ++i)
            {
                prevpos[i] = new int[2];
            }
            this.map = map;
            spawnlocs[0] = findspawnLocs();
            ypos = spawnlocs[0][0];
            xpos = spawnlocs[0][1];
            addToPositions(xpos, ypos);
            Enemy.enemyList.Add(this);
            health = 100;
            enemySprite = new TdMobSprite();
            TowerDefenseManager.TDLayers[2].AddEntity(enemySprite);
            temp = (TdMobSprite)enemySprite;
            temp.maxHealth = temp.currHealth = health;
        }

        /// <summary>
        /// Finds the end of the map.
        /// </summary>
        public static void findend()
        {
            int y, x;
            endlocs = new int[2];
            for (y = 0; y < Map.currentMap.map.Length; ++y)
            {
                for (x = 0; x < Map.currentMap.map[y].Length; ++x)
                {
                    if (Map.currentMap.map[y][x] == -2)
                    {
                        endlocs[0] = y;
                        endlocs[1] = x;
                        return;
                    }
                }
            }
            endlocs = null;

        }

        /// <summary>
        /// Finds the areas that mobs can spawn at.
        /// </summary>
        /// <returns>xy coords of the place the mob can spawn, 
        /// 0 = y pos
        /// 1 = x pos</returns>
        protected int[] findspawnLocs()
        {
            int y, x;
            int[] loc = new int[2];
            for (y = 0; y < map.map.Length; ++y)
            {
                for (x = 0; x < map.map[y].Length; ++x)
                {
                    if (map.map[y][x] == -1)
                    {
                        loc[0] = y;
                        loc[1] = x;
                        return loc;
                    }
                }
            }
            return null;
        }



        /// <summary>
        /// Adds the current position to previously used positions
        /// works like a circular buffer that holds up to 8 previous positions.
        /// </summary>
        /// <param name="x">current x pos</param>
        /// <param name="y">current y pos</param>
        protected void addToPositions(int x, int y)
        {
            prevpos[prevposCounter][0] = y;
            prevpos[prevposCounter][1] = x;

            if (prevposCounter == 8)
                prevposCounter = 0;
            else
                prevposCounter++;
        }

        /// <summary>
        /// Checks if the mob has been to the tile
        /// </summary>
        /// <param name="x">x pos </param>
        /// <param name="y">y pos </param>
        /// <returns> true if it has been to this spot, false if it has not</returns>
        protected bool isPreviousPos(int x, int y)
        {
            for (int i = 0; i < 9; ++i)
                if (prevpos[i][0] == y && prevpos[i][1] == x)
                    return true;
            return false;
        }

        /// <summary>
        /// Jumps the mob to the next tile by moving it either 1 up/down or 1 left/right
        /// only one param should be used as shown below 
        /// jumptoNext( 1, 0);   down
        /// jumptoNext(-1, 0);   up 
        /// jumptoNext( 0, 1);   right
        /// jumptoNext( 0,-1);   left
        /// </summary>
        /// <param name="x">should be 1 or -1, 
        ///                  1 is down 
        ///                  -1 is up</param>
        /// <param name="y">should be 1 or -1, 
        ///                  1 is right 
        ///                 -1 is left</param>
        protected void jumptoNext(int x, int y)
        {

            // paints tile currently at back to old colour
            // currently at as in the one before the jump
            if (map.map[ypos][xpos] != -1)
                map.map[ypos][xpos] = 2;

            xpos += x;
            ypos += y;

            if (map.map[ypos][xpos] == -2) // finds -2 at the end of the path
            {
                MobReachedEnd();
                return;
            }

            //map.map[ypos][xpos] = 5; // paints the tile currently at to black
            // currently at as in the one after the jump
            addToPositions(xpos, ypos);
        }

        /// <summary>
        /// Figures out where the next plot of path is for the mob to follow.
        /// </summary>
        protected void goToNextTile()
        {
            // Mob moves down
            if ((ypos + 1) < map.map.Length  &&
                Math.Abs(map.map[ypos + 1][xpos]) == 2 &&
                !isPreviousPos(xpos, ypos + 1)) 
            {
                jumptoNext(0, 1);
            }

            // Mob moves up
            if ((ypos - 1) >= 0 &&
                Math.Abs(map.map[ypos - 1][xpos]) == 2 &&
                !isPreviousPos(xpos, ypos - 1)) 
            {
                jumptoNext(0, -1);
            }

            // Mob moves right
            if (((xpos + 1) < map.map[0].Length ) &&
                Math.Abs(map.map[ypos][xpos + 1]) == 2 &&
                !isPreviousPos(xpos + 1, ypos)) 
            {
                jumptoNext(1, 0);
            }

            // Mob moves left
            if ((xpos - 1) >= 0 &&
                Math.Abs(map.map[ypos][xpos - 1]) == 2 &&
                !isPreviousPos(xpos - 1, ypos)) 
            {
                jumptoNext(-1, 0);
            }

        }
        
        /// <summary>
        /// Do logic involved with the mob reaching the end of the map here.
        /// </summary>
        protected void MobReachedEnd()
        {
            dead = true;
            map.map[ypos][xpos] = -2;
            enemySprite.Kill();
            TowerDefenseManager.TDLayers[2].RemoveEntity(enemySprite);
            enemyList.Remove(this);
            TDPlayerStats.Grade -= 5;
        }

        /// <summary>
        /// Do logic involved with the mob being killed here.
        /// </summary>
        protected void triggerDeath()
        {
            map.map[ypos][xpos] = 2;
            dead = true;
            enemySprite.Kill();
            TowerDefenseManager.TDLayers[2].RemoveEntity(enemySprite);
            enemyList.Remove(this);
            TDPlayerStats.KillCount++;
            TDPlayerStats.Money += (int)(1 + worth * 1.5);
        }


        /// <summary>
        /// Updates this instance of the Enemy
        /// </summary>
        /// <param name="gametime">current gametime</param>
        public void update(GameTime gametime)
        {
                temp.currHealth = health;
                previousmove = gametime.TotalGameTime;
                goToNextTile();
                if (health <= 0)
                    triggerDeath();
                enemySprite.Position.X = xpos * Map.currentMap.size;
                enemySprite.Position.Y = ypos * Map.currentMap.size;
        }


        /// <summary>
        /// Updates all the enemies, this is the only method that should be called from outside that
        /// effects this anything of this class
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < enemyList.Count; ++i)
            {
                if (gameTime.TotalGameTime - enemyList[i].previousmove > enemyList[i].movetime)
                {
                    enemyList[i].update(gameTime);
                }  
            }
                 
        }
    }
}
