/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Game.Saving;
using Game.Sprites.Vendor;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Game.UI;

namespace Game.States
{
    /// <summary>
    /// VendorState represents the vending machine.  The player can purchase food for a cost. That food will increase energy,
    /// but at the cost of money, or (for cheaper foods) some combination of money and health.
    /// </summary>
    class VendorState : TPState
    {
        /// <summary>
        /// Array of the items available for purchase from the vendor
        /// </summary>
        VendorItem[,] items = new VendorItem[3, 4];

        /// <summary>
        /// Names of the items available for purchase
        /// </summary>
        string[,] itemNames = { 
                                        { "beer", "cheeseburger", "chips", "chocolatebar" },
                                        { "fruit", "icecream", "juice", "pretzel" },
                                        { "salad", "sandwich", "soda", "water" }
                                    };

        /// <summary>
        /// Prices of the available items
        /// </summary>
        int[,] prices = {
                                  { 2, 2, 1, 1 },
                                  { 2, 2, 1, 1 },
                                  { 3, 2, 1, 2 }
                              };

        /// <summary>
        /// Energy each available item gives you
        /// </summary>
        int[,] energy = {
                                  { 1, 2, 1, 2 },
                                  { 2, 2, 2, 1 },
                                  { 1, 2, 3, 0 }
                              };

        /// <summary>
        /// Health each item gives or takes away
        /// </summary>
        int[,] health = {
                                  { -2, 1, -1, -1 },
                                  { 3, -2, 2, -1 },
                                  { 4, 3, -1, 3 }
                              };

        /// <summary>
        /// The previous keyboard state
        /// </summary>
        KeyboardState prevKBState;

        /// <summary>
        /// The current x index of the item array
        /// </summary>
        int xIndex = 0;

        /// <summary>
        /// The current u index of the item array
        /// </summary>
        int yIndex = 0;

        /// <summary>
        /// The maximum number in a row
        /// </summary>
        const int MAX_X = 4;

        /// <summary>
        /// The maximum number in a column
        /// </summary>
        const int MAX_Y = 3;

        /// <summary>
        /// Reference to the current item.  Not necessary, but makes writing the code a little easier.
        /// </summary>
        VendorItem currentItem;

        /// <summary>
        /// String describing the currently selected item to the player
        /// </summary>
        TPString currentItemString;

        /// <summary>
        /// String telling the player how to purchase and how to leave.
        /// </summary>
        TPString usageString = new TPString("Press Enter to buy.  Press Esc to leave.");

        /// <summary>
        /// Constructor for VendorState
        /// </summary>
        public VendorState()
        {
            TPLayer backLayer = new TPLayer(layers);
            backLayer.AddEntity(new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/Vendor/vendstateback")));
            TPLayer sprites = new TPLayer(layers);
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    items[x, y] = new VendorItem(new Vector2(y * 165 + 300, x * 135 + 80), prices[x, y], energy[x, y], health[x, y], itemNames[x, y]);
                    sprites.AddEntity(items[x, y]);
                }
            }
            TPLayer attLayer = new TPLayer(layers);
            //PlayerMeters.PlayerMeters.Load();
            attLayer.AddEntity(new AttributeMeters());

            prevKBState = Keyboard.GetState();

            currentItem = items[yIndex, xIndex];
            currentItem.selected = true;

            currentItemString = new TPString("Buy " + currentItem.itemType + ": $" + currentItem.price + " (Energy: " + currentItem.energy + ", Health: " + currentItem.health + ")");
            currentItemString.Position = new Vector2(320, 500);
            usageString.Position = new Vector2(320, 550);

            attLayer.AddEntity(currentItemString);
            attLayer.AddEntity(usageString);

            // Add Money for testing purposed.
            // PlayerMeters.PlayerMeters.Money = 10;
        }

        /// <summary>
        /// Updates the state
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput(gameTime);
        }

        /// <summary>
        /// Handles the player's input to select and purchase items
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        private void HandleInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && prevKBState.IsKeyUp(Keys.Down))
            {
                yIndex++;
                if (yIndex == MAX_Y)
                {
                    yIndex = 0;
                }
                SetCurrentItem();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) && prevKBState.IsKeyUp(Keys.Right))
            {
                xIndex++;
                if (xIndex == MAX_X)
                {
                    xIndex = 0;
                }
                SetCurrentItem();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && prevKBState.IsKeyUp(Keys.Left))
            {
                xIndex--;
                if (xIndex == -1)
                {
                    xIndex = MAX_X - 1;
                }
                SetCurrentItem();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && prevKBState.IsKeyUp(Keys.Up))
            {
                yIndex--;
                if (yIndex == -1)
                {
                    yIndex = MAX_Y - 1;
                }
                SetCurrentItem();
            }

            //DEBUG Cheat to add money - Remove for final version
            else if (Keyboard.GetState().IsKeyDown(Keys.T) && prevKBState.IsKeyUp(Keys.T))
            {
                PlayerMeters.PlayerMeters.IncreaseMoney(50);
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
            {
                if (PlayerMeters.PlayerMeters.Money >= currentItem.price)
                {
                    PlayerMeters.PlayerMeters.DecreaseMoney(currentItem.price);
                    PlayerMeters.PlayerMeters.SetEnergy(currentItem.energy);
                    PlayerMeters.PlayerMeters.SetHealth(currentItem.health);
                    SetCurrentItem();
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Escape) && prevKBState.IsKeyUp(Keys.Escape))
            {
                SaveCharacterData.Save();
                TPEngine.Get().State.PopState();
            }

            prevKBState = Keyboard.GetState();
        }

        /// <summary>
        /// Sets the current item and updates the item description when the player changes the selected item or purchases.
        /// </summary>
        private void SetCurrentItem()
        {
            currentItem.selected = false;
            currentItem = items[yIndex, xIndex];
            currentItem.selected = true;
            currentItemString.Clear();
            if (currentItem.price <= PlayerMeters.PlayerMeters.Money)
            {
                currentItemString.Append("Buy " + currentItem.itemType + ": $" + currentItem.price + " (Energy: " + currentItem.energy + ", Health: " + currentItem.health + ")");
            }
            else
            {
                currentItemString.Append("Sorry, insufficient funds.");
            }
        }
    }
}
