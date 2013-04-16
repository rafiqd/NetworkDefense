using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.Events
{
    /// <summary>
    /// Early skeleteon for Quest and NPC dialogs. Not currently implemented.
    /// </summary>
    class Dialog
    {
        //Event text to be communicated to the Player on the window
        public String RawText { get; set; }
        
        //window (this needs to be changed)
        public String Window { get; set; }

        //response choices given to the player
        public List<String> PlayerResponse { get; set; }

        public Dialog(String rawtext, String window, List<String> playerresponse)
        {
            RawText = rawtext;
            Window = window;
            PlayerResponse = playerresponse;
        }

        public void Show()
        {
            
        }
        
        public void Dismiss()
        {
            
        }

        public void Update(String updatedText)
        {
            RawText = updatedText;
        }
    }
}
