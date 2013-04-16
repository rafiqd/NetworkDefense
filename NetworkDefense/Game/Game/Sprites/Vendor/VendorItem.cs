/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Sprites.Vendor
{
    /// <summary>
    /// VendorItem represents a food item that can be purchased from the vendor/vending machine.
    /// </summary>
    class VendorItem : TPSprite
    {
        /// <summary>
        /// The price of the item
        /// </summary>
        public int price { get; set;  }

        /// <summary>
        /// The amount of energy the item gives
        /// </summary>
        public int energy { get; set; }

        /// <summary>
        /// The amount of health the item gives
        /// </summary>
        public int health { get; set;  }

        /// <summary>
        /// Whether or not the item is currently selected
        /// </summary>
        public bool selected { get; set; }

        /// <summary>
        /// The frame that surrounds the item if it is currently selected
        /// </summary>
        static Texture2D frame;

        /// <summary>
        /// The type of item.  Used for printing to the screen.
        /// </summary>
        public string itemType { set; get; }

        /// <summary>
        /// Constructor forVendorItem
        /// </summary>
        /// <param name="pos">The screen position of the item</param>
        /// <param name="p">The price of the item</param>
        /// <param name="e">The amount of energy the item gives</param>
        /// <param name="h">The amount of health the item gives</param>
        /// <param name="type">The type of item (determines the texture to draw)</param>
        public VendorItem(Vector2 pos, int p, int e, int h, string type)
            :base(TPEngine.Get().TextureManager.LoadTexture(@"art/Vendor/" + type))
        {
            Position = pos;
            price = p;
            energy = e;
            health = h;
            selected = false;
            frame = TPEngine.Get().TextureManager.LoadTexture(@"art/Vendor/outline");
            itemType = type;
        }

        /// <summary>
        /// Draws the item, and (if it is selected) the highlighting frame
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);
            if (selected)
            {
                batch.Draw(frame, Position, Color.White);
            }
        }
    }
}
