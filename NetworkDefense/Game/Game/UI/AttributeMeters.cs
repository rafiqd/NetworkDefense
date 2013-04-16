using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Microsoft.Xna.Framework;

namespace Game.UI
{
    class AttributeMeters : TPVisibleEntity
    {
        /// <summary>
        /// If this object has finished loading or not
        /// </summary>
        private bool loaded = false;
        /// <summary>
        /// The array holding the values for the bars
        /// </summary>
        private int[] barValues;
        /// <summary>
        /// The amount of money the player has
        /// </summary>
        private int money;
        /// <summary>
        /// The width of the background image
        /// </summary>
        private const int backgroundWidth = 300;
        /// <summary>
        /// The height of the background image
        /// </summary>
        private const int backgroundHeight = 130;
        /// <summary>
        /// The x position of the background image
        /// </summary>
        private const int backgroundX = 10;
        /// <summary>
        /// The y position of the background image
        /// </summary>
        private const int backgroundY = 580;
        /// <summary>
        /// The size of the gap between the bars
        /// </summary>
        private const int gap = 10;
        /// <summary>
        /// The transparency of the entire object
        /// </summary>
        private float transparency = 0.7f;
        /// <summary>
        /// The color of the background image
        /// </summary>
        private Color backgroundColor = Color.LightBlue;
        /// <summary>
        /// The texture of the background image
        /// </summary>
        private Texture2D backgroundTexture;
        /// <summary>
        /// The size of the border surrounding the object
        /// </summary>
        private const int borderSize = 2;
        /// <summary>
        /// The color of the border
        /// </summary>
        private Color borderColor = Color.White;
        /// <summary>
        /// The width of the meter bars
        /// </summary>
        private const int barWidth = 260;
        /// <summary>
        /// The height of the meter bars
        /// </summary>
        private const int barHeight = 20;
        /// <summary>
        /// The texture of the meter bar background
        /// </summary>
        private Texture2D barBackgroundTexture;
        /// <summary>
        /// The texture of the meter bar foreground
        /// </summary>
        private Texture2D barForegroundTexture;
        /// <summary>
        /// The color of the bar background
        /// </summary>
        private Color barBackgroundColor = Color.MidnightBlue;
        /// <summary>
        /// The array of strings for the meter bars
        /// </summary>
        private string[] barText = {"Health", "Energy", "Sanity"};
        /// <summary>
        /// The array of colors of the meter bars
        /// </summary>
        private Color[] barColors;
        /// <summary>
        /// The color of the meter bar text color
        /// </summary>
        private Color barTextColor = Color.White;
        /// <summary>
        /// The font for the text
        /// </summary>
        private SpriteFont font;
        /// <summary>
        /// The text color for the money
        /// </summary>
        private Color moneyTextColor = Color.DarkBlue;

        /// <summary>
        /// Sets the health. Cannot be lower than 0 or greater than 100.
        /// </summary>
        public int Health
        {
            get { return barValues[0]; }
            set { if(value >= 0 && value <= 100) barValues[0] = value; }
        }
        /// <summary>
        /// Sets the hunger. Cannot be lower than 0 or greater than 100.
        /// </summary>
        public int Energy
        {
            get { return barValues[1]; }
            set { if (value >= 0 && value <= 100) barValues[1] = value; }
        }
        /// <summary>
        /// Sets the sanity. Cannot be lower than 0 or greater than 100.
        /// </summary>
        public int Sanity
        {
            get { return barValues[2]; }
            set { if (value >= 0 && value <= 100) barValues[2] = value; }
        }
        /// <summary>
        /// Sets the money. Cannot be lower than 0.
        /// </summary>
        public int Money
        {
            get { return money; }
            set { if (value >= 0 && value <= int.MaxValue) money = value; }
        }

        /// <summary>
        /// Sets the transparency of the entire meter display. Range: 0f to 1f.
        /// </summary>
        public float Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        public AttributeMeters()
        {
            money = (int)PlayerMeters.PlayerMeters.Money;
            barValues = new int[3];
            barValues[0] = (int)PlayerMeters.PlayerMeters.Health;
            barValues[1] = (int) PlayerMeters.PlayerMeters.Energy;
            barValues[2] = (int)PlayerMeters.PlayerMeters.Sanity;
        }

        /// <summary>
        /// Constructor to set the values. Values cannot be less than 0. Money can be greater than 100, but the other values cannot.
        /// </summary>
        /// <param name="money">Set the money</param>
        /// <param name="health">Set the health</param>
        /// <param name="energy">Set the energy</param>
        /// <param name="sanity">Set the sanity</param>
        /// <param name="influence">Set the influence</param>
        public AttributeMeters(int money, int health, int energy, int sanity) : base()
        {
            Money = money;
            barValues = new int[4];
            Health = health;
            Energy = energy;
            Sanity = sanity;

        }

        /// <summary>
        /// Update the object
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Alive && !Asleep)
            {
                Health = (int)PlayerMeters.PlayerMeters.Health;
                Sanity = (int)PlayerMeters.PlayerMeters.Sanity;
                Energy = (int)PlayerMeters.PlayerMeters.Energy;
                Money = (int) PlayerMeters.PlayerMeters.Money;
            }

        }

        /// <summary>
        /// Load everything this class needs.
        /// </summary>
        protected override void Load()
        {
            base.Load();
            

            //Colors of the barForegrounds
            barColors = new Color[3];
            barColors[0] = Color.Red;
            barColors[1] = Color.Orange;
            barColors[2] = Color.Green;

            font = new TPString("").Font;

            //Texture
            backgroundTexture = TPEngine.Get().TextureManager.CreateFilledRectangle(1, 1, Color.White);
            barBackgroundTexture = TPEngine.Get().TextureManager.CreateFilledRectangle(1, 1, Color.White);
            barForegroundTexture = TPEngine.Get().TextureManager.CreateFilledRectangle(1, 1, Color.White);

            loaded = true;
        }


        /// <summary>
        /// Draw the meters and money text
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            if (Alive && loaded)
            {
                //Background box
                batch.Draw(backgroundTexture, new Rectangle(backgroundX, backgroundY, backgroundWidth, backgroundHeight), backgroundColor * transparency);
                //Background bar
                for(int i = 0; i < 3; i++)
                    batch.Draw(barBackgroundTexture, new Rectangle(backgroundX + 2 * gap, backgroundY + (i + 1) * gap + i * barHeight + 20 + gap, barWidth, barHeight), barBackgroundColor * transparency);
                //Foreground bar
                for (int i = 0; i < 3; i++)
                {
                    batch.Draw(barForegroundTexture, new Rectangle(backgroundX + 2 * gap, backgroundY + (i + 1) * gap + i * barHeight + 20 + gap, barWidth * barValues[i] / 100, barHeight), barColors[i] * transparency);
                    batch.Draw(TPEngine.Get().TextureManager.LoadTexture(@"art/UI/healthOverlay"), new Rectangle(backgroundX + 2 * gap, backgroundY + (i + 1) * gap + i * barHeight + 20 + gap, barWidth, barHeight), Color.White * transparency);
                    batch.DrawString(font, barText[i] + ": " + barValues[i], new Vector2(backgroundX + 3 * gap, backgroundY + (i + 1) * gap + i * barHeight + 20 + gap), barTextColor * transparency, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
                }
                //Money text
                batch.DrawString(font, "Money: $" + money.ToString(), new Vector2(backgroundX + 2 * gap, backgroundY + gap), moneyTextColor * transparency, 0f, new Vector2(0,0), 0.6f, SpriteEffects.None, 0);
                //Borders (top, left, right, bot)
                batch.Draw(backgroundTexture, new Rectangle(backgroundX, backgroundY, backgroundWidth, borderSize), borderColor * transparency);
                batch.Draw(backgroundTexture, new Rectangle(backgroundX, backgroundY, borderSize, backgroundHeight), borderColor * transparency);
                batch.Draw(backgroundTexture, new Rectangle(backgroundX + backgroundWidth, backgroundY, borderSize, backgroundHeight), borderColor * transparency);
                batch.Draw(backgroundTexture, new Rectangle(backgroundX, backgroundY + backgroundHeight, backgroundWidth, borderSize), borderColor * transparency);
            }
        }

    }
}
