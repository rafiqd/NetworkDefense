using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Game.Sprites;
using System.Runtime.InteropServices;
using DBCommService;
using Game.States;

namespace Game
{
    /// <summary>
    /// Cyril, Peter
    /// this states displays a list of objectives for a request that is available for a particular day 
    /// </summary>
    class EmailViewState : TPState
    {
        /// <summary>
        /// an array type TPLayer that keeps track how many layers will be displayed on the screen
        /// </summary>
        TPLayer[] myLayers;

        /// <summary>
        /// number of layers to be displayed
        /// </summary>
        Int16 NumLayers = 3;

        /// <summary>
        /// player's cursor sprite
        /// </summary>
        GameMouse mouse;

        /// <summary>
        /// sprite indicates where the user clicks
        /// </summary>
        BulletHole bulletHole = new BulletHole();

        /// <summary>
        /// background sprite
        /// </summary>
        TPSprite backgroundTexture;

        /// <summary>
        /// list of checkbox for every subject item
        /// </summary>
        List<CheckBox> checkbox = new List<CheckBox>();

        /// <summary>
        /// list of subjects for a request
        /// </summary>
        List<TPString> Items = new List<TPString>();

        /// <summary>
        /// the flag indicates if the state is loaded for the first time
        /// </summary>
        static bool FirstRunEmail = true;
        
        /// <summary>
        /// list of "read more" buttons for every subject item
        /// </summary>
        List<ReadMoreButton> buttons = new List<ReadMoreButton>();

        /// <summary>
        /// stores the previous state of the mouse
        /// </summary>
        MouseState prevMouseState;

        /// <summary>
        /// stores the previous state of the keyboard
        /// </summary>
        KeyboardState prevKeyboardState;

        /// <summary>
        /// Cyril, Peter
        /// load the necessary items that needed to display on screen
        /// </summary>
        protected override void Load()
        {
            base.Load();
            int nodenumber = 0;
            if (AreaState.character.Quests != null)
            {
                foreach (Quest q in AreaState.character.Quests)
                {
                    foreach (QuestNode node in q.Nodes)
                    {
                        Items.Add(new TPString(node.QuestDescription.Subject));
                        if (nodenumber < q.current_node)
                            checkbox.Add(new CheckBox("checkboxticked"));
                        else
                            checkbox.Add(new CheckBox("checkboxunticked"));
                        nodenumber++;
                    }
                    break;
                }
            }
            

            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }

            //mySprite = new Bullseye(new Vector2(200, 200));
            mouse = new GameMouse(new Vector2(200, 200));
            myLayers[2].AddEntity(mouse);
            myLayers[2].AddEntity(bulletHole);

            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/Quest/emailview"));
            backgroundTexture.Position = new Vector2(backgroundTexture.Position.X, backgroundTexture.Position.Y);
            myLayers[0].AddEntity(backgroundTexture);


            if (Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    //buttons.Add(new ReadMoreButton(Items[i].ToString()));
                    buttons.Add(new ReadMoreButton(i));
                }
                for (int i = 0; i < Items.Count; i++)
                {
                    myLayers[1].AddEntity(Items[i]);
                    myLayers[1].AddEntity(buttons[i]);
                    myLayers[1].AddEntity(checkbox[i]);
                }
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Position.X = 150;
                    Items[i].Position.Y = 200+ i*50;
                    Items[i].RenderColor = Color.Black;

                    buttons[i].Position.X = 1050;
                    buttons[i].Position.Y = 190 + i*50;

                    checkbox[i].Position.X = 75;
                    checkbox[i].Position.Y = 200 + i * 50;
                }
            }


            mouse = new GameMouse(new Vector2(200, 200));
            if (FirstRunEmail)
            {
                
                TPEngine.Get().SpriteDictionary.Add("MinigameMySpriteEmail", mouse);
                TPEngine.Get().SpriteDictionary.Add("bulletHoleEmail", bulletHole);
                FirstRunEmail = false;
                if (Items.Count > 0)
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        TPEngine.Get().StringDictionary.Add(string.Format("Email{0}", i), (TPString)Items[i]);
                        TPEngine.Get().StringDictionary.Add(string.Format("Button{0}", i), new TPString(buttons[i].ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Cyril
        /// Update the sprites with each iteration. When the time's up. add new score into the database
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //if the Esc key is pressed, return to the main game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                TPEngine.Get().State.PopState();
            }

            if (prevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
                if (Mouse.GetState().X >= 1210 && Mouse.GetState().X <= 1277 && Mouse.GetState().Y >= 3 && Mouse.GetState().Y <= 53)
                    TPEngine.Get().State.PopState();

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }
    }
}
