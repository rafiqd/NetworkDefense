using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.States.TDsrc.Environment;
using Game.Sprites;
using Game.States.TDsrc.Stats;
using Game.States.TDsrc.UI;
using Engine.Objects;
using Engine;
using Game.States.TDsrc.Management;


namespace Game.States.TDsrc.Towers
{
    /// <summary>
    /// TowerBuilder class is used to build towers
    /// </summary>
    class TowerBuilder
    {
        /// <summary>
        /// if the user is currently holding a tower
        /// </summary>
        private static bool holdingTower;

        /// <summary>
        /// type of tower that is being held
        /// </summary>
        private static string towertype;

        /// <summary>
        /// current game map
        /// </summary>
        private static Map gameMap;

        /// <summary>
        /// if the mouse was clicked
        /// </summary>
        private static bool mouseclick;


        /// <summary>
        /// sprite of the tower under the mouse while it's being dragged onto the map
        /// </summary>
        private static TPSprite TowerHeld;

        /// <summary>
        /// hanldes all input from keyboard and mouse related to this class
        /// </summary>
        /// <param name="state">Keyboard state</param>
        /// <param name="mousestate">Mouse state</param>
        public static void UpdateKeyboardInput(KeyboardState state, MouseState mousestate)
        {
            if (holdingTower)
            {
                TowerHeld.Position.X = mousestate.X - TowerHeld.Width /2;
                TowerHeld.Position.Y = mousestate.Y - TowerHeld.Height /2;
            }

            if (holdingTower && mousestate.LeftButton == ButtonState.Pressed && !mouseclick)
            {
                mouseclick = true;
                Update(Mouse.GetState().X, Mouse.GetState().Y);
            }

            if (mousestate.LeftButton == ButtonState.Released)
                mouseclick = false;

        }

        /// <summary>
        /// Updates this instance of the builder
        /// tower building is done here
        /// </summary>
        /// <param name="mousex">mouse x position</param>
        /// <param name="mousey">mouse y position</param>
        public static void Update(int mousex, int mousey)
        {
            Tower newtower;


            switch (towertype)
            {
                case "normal":
                    if(NormalTower.Unlocked && isValidLoc_NormalTower(mousex, mousey))
                    {
                        newtower = new NormalTower(mousex, mousey, UICreator.CurrentColor);
                        cliamLocation_NormalTower(mousex, mousey);
                        holdingTower = false;
                        TowerHeld.Kill();
                        TowerDefenseManager.TDLayers[TowerDefenseManager.numLayers - 1].RemoveEntity(TowerHeld);
                        TowerHeld = null;
                    }
                    break;
                case "fast":
                    if (FastTower.Unlocked  && isValidLoc_NormalTower(mousex, mousey))
                    {
                        newtower = new FastTower(mousex, mousey, UICreator.CurrentColor);
                        cliamLocation_NormalTower(mousex, mousey);
                        holdingTower = false;
                        TowerHeld.Kill();
                        TowerDefenseManager.TDLayers[TowerDefenseManager.numLayers - 1].RemoveEntity(TowerHeld);
                        TowerHeld = null;
                    }
                    break;

                case "strong":
                    if (StrongTower.Unlocked  && isValidLoc_NormalTower(mousex, mousey))
                    {
                        newtower = new StrongTower(mousex, mousey, UICreator.CurrentColor);
                        cliamLocation_NormalTower(mousex, mousey);
                        holdingTower = false;
                        TowerHeld.Kill();
                        TowerDefenseManager.TDLayers[TowerDefenseManager.numLayers - 1].RemoveEntity(TowerHeld);
                        TowerHeld = null;
                    }
                    break;

                case "spray":
                    if (SprayTower.Unlocked  && isValidLoc_NormalTower(mousex, mousey))
                    {
                        newtower = new SprayTower(mousex, mousey, UICreator.CurrentColor);
                        cliamLocation_NormalTower(mousex, mousey);
                        holdingTower = false;
                        TowerHeld.Kill();
                        TowerDefenseManager.TDLayers[TowerDefenseManager.numLayers - 1].RemoveEntity(TowerHeld);
                        TowerHeld = null;
                    }
                    break;
            }
            
        }

        /// <summary>
        /// claims a portion of the map for the tower so that no other towers can be built overlapping it.
        /// </summary>
        /// <param name="mousex">mouse x position</param>
        /// <param name="mousey">mouse y position</param>
        private static void cliamLocation_NormalTower(int mousex, int mousey)
        {
            
            int mapX = mousex / gameMap.size;
            int mapY = mousey / gameMap.size;
            int width = TDNormalTowerSprite.NormalTowerTexture.Width/5;
            width += 5;

            for (int y = -width / 2; y < width-5; ++y)
                for (int x = -width / 2; x < width-5; ++x)
                    gameMap.map[mapY + y] [mapX + x] = 3;
                        
        }

        /// <summary>
        /// Checks if a tower can be planted at the loction the player clicked
        /// </summary>
        /// <param name="mousex">x mouse coords</param>
        /// <param name="mousey">y mouse coords</param>
        /// <returns>true if location is valid, false if it is not.</returns>
        private static bool isValidLoc_NormalTower(int mousex, int mousey)
        {
            int mapX = mousex / gameMap.size;
            int mapY = mousey / gameMap.size;
            int width = TDNormalTowerSprite.NormalTowerTexture.Width/5;
            width += 5;
            for (int y = -width / 2; y < width-5; ++y)
                for (int x = -width / 2; x < width-5; ++x)
                {
                    if (mapX + x <= 0 || mapX + x >= gameMap.map[0].Count() ||
                        mapY + y <= 0 || mapY + y >= gameMap.map.Count() )
                        return false;

                    if (gameMap.map[mapY + y][mapX + x] != 0)
                        return false;
                }
            return true;
        }

        /// <summary>
        /// Occurs when a tower button is clicked, makes the user hold a tower
        /// and ready for planting.
        /// </summary>
        /// <param name="tower">type of tower</param>
        public static void TowerBuildingClicked(string tower)
        {
            int cost = 0;
            switch (tower)
            {
                case "normal":
                    cost = NormalTower.Cost;
                    break;
                case "fast":
                    cost = FastTower.Cost;
                    break;
                case "strong":
                    cost = StrongTower.Cost;
                    break;
                case "spray":
                    cost = SprayTower.Cost;
                    break;
            }

            if (cost > TDPlayerStats.Money)
                return;

            holdingTower = true;
            towertype = tower;
            mouseclick = true;
            TowerHeld = new TDNormalTowerSprite();
            TowerDefenseManager.TDLayers[TowerDefenseManager.numLayers - 1].AddEntity(TowerHeld);
        }


        /// <summary>
        /// loads resources used by this class
        /// </summary>
        /// <param name="map">current map</param>
        public static void Load(Map map)
        {
            gameMap = map;
        }

    }
}
