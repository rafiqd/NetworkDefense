using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.States.TDsrc.Projectiles;

namespace Game.States.TDsrc.Management
{
    /// <summary>
    /// Used to convert damage a tower does to the proper value with their colour factored in
    /// </summary>
    static class TDDamageManager
    {
        /// <summary>
        /// colour of the strengthed tower
        /// </summary>
        public static string TowerColor1;

        /// <summary>
        /// colour of the weakend tower
        /// </summary>
        public static string TowerColor2;

        /// <summary>
        /// Bonus damage amount for the strengthed tower
        /// </summary>
        public static double TowerBonus1;

        /// <summary>
        /// Reduced damage amount for the weakend tower
        /// </summary>
        public static double TowerBonus2;

        /// <summary>
        /// static initalization, sets both damage amounts to 100% as default
        /// </summary>
        static TDDamageManager()
        {
            TowerBonus1 = 100;
            TowerBonus2 = 100;
        }

        /// <summary>
        /// Sets the damage for the towers
        /// </summary>
        /// <param name="tower1">Tower to Increase damage for</param>
        /// <param name="tower2">Tower to Lower damage for</param>
        /// <param name="damage1">Increase amount</param>
        /// <param name="damage2">Weaken amount</param>
        public static void setDamage(string tower1, string tower2, int damage1, int damage2)
        {
            TowerColor1 = tower1;
            TowerColor2 = tower2;
            TowerBonus1 = damage1;
            TowerBonus2 = damage2;
        }

        /// <summary>
        /// Modifies damage done by a projectile and returns the proper value with
        /// colour factored in.
        /// </summary>
        /// <param name="projectile">projectile damage to modify</param>
        /// <returns>proper damage amount</returns>
        public static int ModifiedDamage(Projectile projectile)
        {
            double damage = projectile.damage;
            if (TowerColor1 == projectile.Color)
            {
                damage *= (TowerBonus1 / 100); 
            }

            if (TowerColor2 == projectile.Color)
            {
                damage *= (TowerBonus2 / 100);
            }

            return (int)damage;
        }
    }
}
