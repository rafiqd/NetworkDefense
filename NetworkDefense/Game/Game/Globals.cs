using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Globals
    {
        static bool paused = false;

        public static bool Paused
        {
            get
            {
                return paused;
            }
            set
            {
                paused = value;
            }
        }
        static bool firstRun = true;

        public static bool FirstRun
        {
            get
            {
                return firstRun;
            }
            set
            {
                firstRun = value;
            }
        }

        static bool firstRunMenu = true;

        public static bool FirstRunMenu
        {
            get
            {
                return firstRunMenu;
            }
            set
            {
                firstRunMenu = value;
            }
        }
    }
}
