using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Sprites;
using Game.States;
using DBCommService;
using Engine;
using Game.Quests;

namespace Game.Saving
{
    /// <summary>
    /// Save methods for saving to database.
    /// </summary>
    static class SaveCharacterData
    {
        /// <summary>
        /// Collects all data related to the character and stores in the database.
        /// </summary>
        public static void Save()
        {
            using (DBCommServiceClient client = new DBCommServiceClient())
            {
                Clock maingameclock = (Clock)TPEngine.Get().SpriteDictionary["MainGameClock"];
                AreaState.character.minute = (int)maingameclock.currentMinute;
                AreaState.character.hour = maingameclock.GetHour();
                AreaState.character.day = maingameclock.currentDay;
                AreaState.character.week = maingameclock.currentWeek;
                AreaState.character.sanity = PlayerMeters.PlayerMeters.Sanity;
                AreaState.character.hunger = PlayerMeters.PlayerMeters.Health;
                AreaState.character.energy = PlayerMeters.PlayerMeters.Energy;
                AreaState.character.money = PlayerMeters.PlayerMeters.Money;
                client.SaveExistingCharacterData(AreaState.character, GameLauncher_LoginButtonSprite.getUser());
                QuestChecking.PerformCheck();
                AreaState.character.global_score = client.LoadCharacterData(GameLauncher_LoginButtonSprite.getUser()).global_score;
            }
        }
    }
}
