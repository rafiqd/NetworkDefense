using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using Game.States.TDsrc.Towers;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.States.TDsrc.UI
{

    /// <summary>
    /// Creates the UI for the TD game
    /// </summary>
    static class UICreator
    {
        /// <summary>
        /// Bottom bar with the towers on it
        /// </summary>
        public static Window BottomBar;

        /// <summary>
        /// Buy menu 
        /// </summary>
        public static Window BuyMenu;

        /// <summary>
        /// score bar at the top
        /// </summary>
        public static Window ScoreBar;

        /// <summary>
        /// menu with the colours
        /// </summary>
        public static Window ColorMenu;

        /// <summary>
        /// currently selected tower
        /// </summary>
        public static string CurrentColor;

        /// <summary>
        /// creates the UI
        /// </summary>
        public static void CreateUI()
        {
            CreateBottomBar();
            CreateBuyWindow();
            createColorMenu();
            CurrentColor = "Red";
        }

        /// <summary>
        /// Everything included with building the bottom menu bar should go here.
        /// </summary>
        private static void CreateBottomBar()
        {
            BottomBar = new Window(0, 620, true, TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/BotBar"));


            BottomBar.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/BuyButton"),
                                "BuyButton",
                                MakeBuyWindowVisible);

            BottomBar.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/Bot_NormalTower"),
                                "normal",
                                TowerBuilder.TowerBuildingClicked);

            NormalTower.UnlockTowerClass();
        }

        /// <summary>
        /// creates the colour menu
        /// </summary>
        public static void createColorMenu()
        {
            string[] colors = { "Red", "Green", "Yellow", "Blue" };
            ColorMenu = new Window(0, 550, true, TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/ColorMenu"));
            for (int i = 0; i < 4; ++i)
            {
                ColorMenu.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/" + colors[i] + "button"), colors[i], setColor);
            }
        }

        /// <summary>
        /// sets the current colour
        /// </summary>
        /// <param name="color">color you want to set it to</param>
        public static void setColor(string color)
        {
            CurrentColor = color;
        }

        /// <summary>
        /// toggles the buymenu visible/invisible
        /// </summary>
        /// <param name="name">name of the button calling</param>
        private static void MakeBuyWindowVisible(string name)
        {
            BuyMenu.Visible ^= true;
        }

        /// <summary>
        /// Creates the buy menu
        /// </summary>
        private static void CreateBuyWindow()
        {
            BuyMenu = new Window(740, 100, false, TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/BuyMenu"));

                BuyMenu.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/Bot_FastTower"),
                                       "fast",
                                       UnlockTowerButton);
                BuyMenu.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/Bot_StrongTower"),
                                       "strong",
                                       UnlockTowerButton);
                BuyMenu.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/Bot_SprayTower"),
                                       "spray",
                                       UnlockTowerButton);      
        }

        /// <summary>
        /// Unlocks a class of towers
        /// </summary>
        /// <param name="towerName">name of the tower class</param>
        public static void UnlockTowerButton(string towerName)
        {
            switch (towerName)
            {

                case "fast":
                    if(!FastTower.Unlocked && FastTower.UnlockTowerClass())
                    {
                        BottomBar.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/Bot_FastTower"),
                                            "fast",
                                            TowerBuilder.TowerBuildingClicked);
                    }
                    break;

                case "strong":
                    if (!StrongTower.Unlocked && StrongTower.UnlockTowerClass())
                    {
                        BottomBar.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/Bot_StrongTower"),
                                            "strong",
                                            TowerBuilder.TowerBuildingClicked);
                    }
                    break;

                case "spray":
                    if (!SprayTower.Unlocked && SprayTower.UnlockTowerClass())
                    {
                        BottomBar.addButton(TPEngine.Get().TextureManager.LoadTexture(@"art/TowerDefense/Bot_SprayTower"),
                                            "spray",
                                            TowerBuilder.TowerBuildingClicked);
                    }
                    break;

            }
        }

    }
}
