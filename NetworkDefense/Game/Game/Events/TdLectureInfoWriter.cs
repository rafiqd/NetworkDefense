using System;
using Microsoft.Win32;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.Events
{
    /// <summary>
    /// Tower Defense implementation of LectureInfoManager. The Tower Defense LectureInfoWriter creates and stores
    /// all of the information required for each Lecture.
    /// </summary>
    class TdLectureInfoWriter : EventInfoManager
    {
        /// <summary>
        /// Key for the registry for the folder
        /// </summary>
        public static string Key_Folder = "c4952_towerdefense";

        /// <summary>
        /// Key for the registry for first color
        /// </summary>
        public static string Key_Color1 = "TD_colour1";

        /// <summary>
        /// key for the registry for the second color
        /// </summary>
        public static string Key_Color2 = "TD_colour2";

        /// <summary>
        /// key for the registry for the first damage mod
        /// </summary>
        public static string Key_Dmg1= "TD_Dmg1";

        /// <summary>
        /// key for the registry for the second damage mod
        /// </summary>
        public static string Key_Dmg2 = "TD_Dmg2";

        private string[] colors = { "blue", "green", "yellow", "red" };
        private string BonusTowerColor;
        private string ColorMod1;
        private string ColorMod2;
        private int DmgMod1;
        private int DmgMod2;

        /// <summary>
        /// Allows the LectureInfoWriter to generate new difficulty modifiers for each week.
        /// </summary>
        private void Generate()
        {
            Random rand = new Random();
            BonusTowerColor = colors[rand.Next(4)];
            ColorMod1 = colors[rand.Next(4)];
            ColorMod2 = colors[rand.Next(4)];
            while (ColorMod1 == ColorMod2)
            {
                ColorMod2 = colors[rand.Next(4)];
            }
            DmgMod1 = 100 + rand.Next(20, 30);
            DmgMod2 = 100 - rand.Next(1, 30);

        }

        /// <summary>
        /// Writes the Lecture details to the Lecture state. Data that should be written includes
        /// text and images. This is the stuff that needs to be communicated to the Player for 
        /// the minigame.
        /// </summary>
        /// <returns>Returns the lecture</returns>
        public override EventInfo Write()
        {
            Generate();
            EventInfo lectureInfo = new EventInfo();
            lectureInfo.Type = "lecture";
            lectureInfo.Data = BonusTowerColor + ", " + ColorMod1 + ", "
                               + ", " + DmgMod1 + ", " + DmgMod2;
            lectureInfo.Name = "towerDefense";
            
            lectureInfo.Text = "Welcome to Programming Methods! \n" +
                               "How To Play: Click on the buttons on the lower bar then plant\n" +
                               "the towers on the map to buy new towers click on the $ button\n" + 
                               "and click on the tower class to unlock it, once unlocked it\n" + 
                               "will appear on the bottom bar where you can click the button\n" + 
                               "and place that tower type on the map\n\n" +
                               "Tower Color: " + ColorMod1 + " Will recieve a Bonus Damage of " + DmgMod1 + "%\n" +
                               "Tower Color: " + ColorMod2 + " Will recieve a Damage reduction of " + DmgMod2 + "%\n";

            RegistryKey mykey;
            mykey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\" + Key_Folder);
            mykey.SetValue(Key_Color1, ColorMod1);
            mykey.SetValue(Key_Color2, ColorMod2);
            mykey.SetValue(Key_Dmg1, DmgMod1);
            mykey.SetValue(Key_Dmg2, DmgMod2);

            lectureInfo.Day = 0;
            lectureInfo.StartTime = 900;
            lectureInfo.EndTime = 1100;
            MinigameEvents.Add(lectureInfo);
            return lectureInfo;
        }

        public override EventInfo Read(EventInfo evenInfo)
        {
            throw new NotImplementedException();
        }

        public override void WriteDb(EventInfo lectureInfo)
        {
            throw new NotImplementedException();
        }

        public override EventInfo ReadDb(EventInfo evenInfo)
        {
            throw new NotImplementedException();
        }
    }
}
