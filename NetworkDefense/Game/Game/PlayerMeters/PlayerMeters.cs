using System.Collections.Generic;
using Engine;
using Game.States;
using Microsoft.Xna.Framework;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.PlayerMeters
{
    /// <summary>
    /// PlayerMeters handles all logic relating to updating the values of the Player Meters
    /// including Energy, Health, Sanity and Money. Works with the AreaState.character values.
    /// It also loads the data from the database.
    /// </summary>
    static class PlayerMeters
    {
        /// <summary>
        /// Energy used for various meter logic, UI and interaction with database.
        /// Represents the players Energy.
        /// </summary>
        public static int? Energy;

        /// <summary>
        /// Money used for purchasing food, UI and interaction with database.
        /// Represents the players Money.
        /// </summary>
        public static int? Money;

        /// <summary>
        /// Health used for various meter logic, UI and interaction with database.
        /// Represents the players Health.
        /// </summary>
        public static int? Health;

        /// <summary>
        /// Sanity used for various meter logic, UI and interaction with database.
        /// Reprsents the players Sanity. May effect minigame difficulties.
        /// </summary>
        public static int? Sanity;

        /// <summary>
        /// Used in logic for decreasing or increasing values based on current time in game.
        /// </summary>
        public static int prevHour;

        /// <summary>
        /// Used in logic for decreasing or increasing values based on current time in game.
        /// </summary>
        public static double prevMin;

        /// <summary>
        /// Used in logic for decreasing or increasing sanity based on current time in game.
        /// </summary>
        public static int? currentSanity;

        /// <summary>
        /// Currently unused.
        /// </summary>
        public static Dictionary<string, int> Grades;
        
        /// <summary>
        /// Static constructor that calls Load()
        /// </summary>
        static PlayerMeters(){Load();}
        
        /// <summary>
        /// Loads any data from the database and if any of the values are null ie. the first character load
        /// the values are set to 100.
        /// </summary>
        public static void Load()
        {
            LoadFromDb();
            if (Energy == null)
                Energy = 100;
            if (Health == null || Health == 0)
                Health = 100;
            if (Sanity == null)
                Sanity = 100;
            Grades = new Dictionary<string, int>();
            Clock clock = (Clock)TPEngine.Get().SpriteDictionary["MainGameClock"];
            prevHour = clock.GetHour();
            prevMin = clock.GetMinute();
        }

        /// <summary>
        /// Modifies Sanity as desired. Value will not go below 100.
        /// </summary>
        /// <param name="amount">Positive amount for increase and negative for decrease.</param>
        public static void SetSanity(int amount)
        {
            Sanity += amount;

            if(Sanity > 100)
            {
                Sanity = 100;
            }

            if (Sanity < 0)
                Sanity = 0;

            AreaState.character.sanity = Sanity;
        }

        /// <summary>
        /// Modifies Energy as desired. Value will not go below 100.
        /// </summary>
        /// <param name="amount">Positive amount for increase and negative for decrease.</param>
        public static void SetEnergy(int amount)
        {
            Energy += amount;

            if (Energy > 100)
            {
                Energy = 100;
            }

            if (Energy < 0)
                Energy = 0;

            AreaState.character.energy = Energy;
        }

        /// <summary>
        /// Modifies Health as desired. Value will not go below 100.
        /// </summary>
        /// <param name="amount">Positive amount for increase and negative for decrease.</param>
        public static void SetHealth(int amount)
        {
            Health += amount;

            if (Health > 100)
            {
                Health = 100;
            }

            if (Health < 0)
                Health = 0;

            AreaState.character.hunger = Health;
        }

        /// <summary>
        /// Gives money to the character.
        /// </summary>
        /// <param name="amount">amount of increase.</param>
        public static void IncreaseMoney(int amount)
        {
            Money += amount;
            AreaState.character.money = Money;
        }

        /// <summary>
        /// Subtracts money from the character.
        /// </summary>
        /// <param name="amount">amount of increase.</param>
        public static void DecreaseMoney(int amount)
        {
            Money -= amount;

            if (Money < 0)
                Money = 0;

            AreaState.character.money = Money;
        }

        /// <summary>
        /// Used to modify Energy and Sanity based on the amount of time slept.
        /// </summary>
        /// <param name="amount">hours slept.</param>
        public static void Sleep(int amount)
        {
            SetEnergy(2 * amount);
            SetSanity(2 * amount);
            if (Sanity > 75 && Energy > 70)
            {
                SetHealth(amount);
            }
        }

        /// <summary>
        /// Gets called from AreaState so that there is constant change when playing the game.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            ConstantDecreaseSanity(2);
            ConstantDecreaseEnergy(5);
            //If sanity falls below 70, the players health should never be greater than 75
            if (Sanity <= 75 && Energy < 70 && Health > 75)
            {
                int difference = (int)Health - 75;
                SetHealth(-difference);
            }
        }

        /// <summary>
        /// Not yet implemented.
        /// </summary>
        public static void UpdateHealth()
        {

        }

        /// <summary>
        /// Decreases sanity by a constant amount every hours.
        /// </summary>
        /// <param name="amount"></param>
        public static void ConstantDecreaseSanity(int amount)
        {
            //Decreases sanity by 2 every hour
            currentSanity = Sanity;
            Clock clock = (Clock)TPEngine.Get().SpriteDictionary["MainGameClock"];
            if (clock.GetHour() > prevHour)
            {
                prevHour = clock.GetHour();
                SetSanity(-amount);
            }
        }

        /// <summary>
        /// Decreases Energy by a constant amount every 30 mins of game time.
        /// </summary>
        /// <param name="amount"></param>
        public static void ConstantDecreaseEnergy(int amount)
        {
            Clock clock = (Clock)TPEngine.Get().SpriteDictionary["MainGameClock"];
            //Decreases energy by 5 Every 30 minutes
            if (clock.GetMinute() > 30 && clock.GetMinute() < 30.04)
            {
                SetEnergy(-amount);
            }
        }

        /// <summary>
        /// Load Character attributes from the database.
        /// </summary>
        public static void LoadFromDb()
        {
            Energy = AreaState.character.energy;
            Health = AreaState.character.hunger;
            Sanity = AreaState.character.sanity;
            Money = AreaState.character.money;
        }

        /// <summary>
        /// Unused at this time.
        /// </summary>
        /// <returns></returns>
        public static double GetGpa()
        {
            double total = 0;
            foreach (var className in Grades.Keys)
            {
                total += Grades[className];
            }
            return total/Grades.Count;
        }
    }
}
