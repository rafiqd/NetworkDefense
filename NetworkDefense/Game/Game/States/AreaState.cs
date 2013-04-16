/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Game.Area;
using Game.Saving;
using Game.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework.Input;
using DBCommService;
using Game.UI;
using Game.Quests;

namespace Game.States
{
    /// <summary>
    /// Data describing an area state
    /// </summary>
    struct AreaStateData
    {
        /// <summary>
        /// The layers that will be rendered in order
        /// </summary>
        public TPLayer[] areaLayers;

        /// <summary>
        /// Data describing the particular area
        /// </summary>
        public AreaInfo areaInfo;

        /// <summary>
        /// The player sprite within the current area
        /// </summary>
        public PlayerSprite playerSprite;

        /// <summary>
        /// The name of the current area
        /// </summary>
        public string areaName;

        /// <summary>
        /// The mouse pointer (not currently implemented)
        /// </summary>
        public MousePointer mousePointer;

        /// <summary>
        /// The clock
        /// </summary>
        public Clock clock;

        /// <summary>
        /// The schedule
        /// </summary>
        public Schedule schedule;
    }
    /// <summary>
    /// The areastate defines an area that represents a room or hallway and the player can navigate and access usable objects like computers or doors.
    /// </summary>
    class AreaState : TPState
    {
        /// <summary>
        /// The positions that a player will start at given the current area
        /// </summary>
        Dictionary<string, Point> startPositions = new Dictionary<string, Point>();

        /// <summary>
        /// Whether or not the hall has already been loaded
        /// </summary>
        static bool loadedHall = false;

        /// <summary>
        /// Whether or not a subroom needs to be loaded
        /// </summary>
        static bool loadSubroom = false;

        /// <summary>
        /// Whether or not this is the first time any area has been initialized
        /// </summary>
        static bool firstInit = true;

        /// <summary>
        /// The name of the player's current room
        /// </summary>
        public static string playerRoom;

        /// <summary>
        /// The data for the current area
        /// </summary>
        AreaStateData data;

        /// <summary>
        /// The previous input state of the keyboard
        /// </summary>
        private KeyboardState prevKeyState;

        /// <summary>
        /// Determines if the player is meant to load in a special position not found in the dictionary
        /// </summary>
        public static bool IsSpecialPosition = false;

        /// <summary>
        /// The data for the current character
        /// </summary>
        public static Character character = null;

        /// <summary>
        /// The hour previous to starting an event
        /// </summary>
        public int prevHour;

        /// <summary>
        /// The character attributes such as health and money
        /// </summary>
        public AttributeMeters attributeMeters;

        /// <summary>
        /// The sanity level of the character
        /// </summary>
        public int currentSanity;

        /// <summary>
        /// The minute previous to starting and event
        /// </summary>
        public double prevMin;

        /// <summary>
        /// Sprite to signify completed qust
        /// </summary>
        CompletedNodeSprite completedNodeSprite;

        /// <summary>
        /// Constructor for AreaState
        /// </summary>
        /// <param name="area">The name of the area to build</param>
        public AreaState(string area)
        {
            //prevUpdate = TimeSpan.Zero;
            //healthUpdateTimeSpan = TimeSpan.FromSeconds(10);
            if (firstInit)
            {
                data.areaName = character.position;
                playerRoom = character.position;
            }
            else
            {
                data.areaName = area;
                playerRoom = area;

                //Saves character position
                character.position = area;
                SaveCharacterData.Save();
            }

            if (playerRoom != "hall" && firstInit)
            {
                IsSpecialPosition = true;
                firstInit = false;
                loadSubroom = true;
            }
            firstInit = false;
        }

        /// <summary>
        /// Loads all the objects used in the state
        /// </summary>
        protected override void Load()
        {
            base.Load();

            if(!loadedHall)
            {
                data = AreaBuilder.BuildArea(layers, "hall");
                data.areaName = "hall";
                loadedHall = true;
            }
            else
            {
                data = AreaBuilder.BuildArea(layers, playerRoom);
            }

            // Init the schedule
            data.schedule = Schedule.Get();

            // Check if a clock exists, if not make one.
            TPSprite temp;
            if (!TPEngine.Get().SpriteDictionary.TryGetValue("MainGameClock", out temp))
            {
                if (character.week >= 5)
                {
                    character.week = 1;
                }
                int hour = (character.hour > 12) ? character.hour - 12 : character.hour;
                data.clock = new Clock(character.week, character.hour, character.minute, character.day);
                TPEngine.Get().SpriteDictionary.Add("MainGameClock", data.clock);
            }
            else
            {
                data.clock = (Clock)temp;
            }

            // Add that clock!
            layers[data.areaInfo.areaX + data.areaInfo.areaY].AddEntity(data.clock);

            //create the attribute meters and add it to layers
            attributeMeters = new AttributeMeters();
            layers[data.areaInfo.areaX + data.areaInfo.areaY].AddEntity(attributeMeters);

            //Create the view quest button and add it to the layer
            ButtonSprite questButton = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/UI/CellphoneOff"),
                    TPEngine.Get().TextureManager.LoadTexture(@"art/UI/CellphoneOn"),
                    new Rectangle(1280 - 90, 100, 70, 118), "View quests", layers[data.areaInfo.areaX + data.areaInfo.areaY]);
            
            questButton.TextScale = new Vector2(0.7f, 0.7f);
            questButton.TextColor = Color.White;
            questButton.DrawColor = Color.White;
            questButton.CenterText = true;
            questButton.TextPosition = questButton.TextPosition + new Vector2(0, 70);
            questButton.ButtonAction = delegate { TPEngine.Get().State.PushState(new EmailViewState()); };
            
            layers[data.areaInfo.areaX + data.areaInfo.areaY].AddEntity(questButton);

            if (!TPEngine.Get().SpriteDictionary.ContainsKey("completedNodeSprite"))
            {
                completedNodeSprite = new CompletedNodeSprite();
                completedNodeSprite.Position = new Vector2(1280 - 180, 125);
                completedNodeSprite.Scale = new Vector2(.2f, .2f);
                completedNodeSprite.IsItAlive = false;
                layers[data.areaInfo.areaX + data.areaInfo.areaY].AddEntity(completedNodeSprite);
                TPEngine.Get().SpriteDictionary.Add("completedNodeSprite", completedNodeSprite);
            }
            else
            {
                layers[data.areaInfo.areaX + data.areaInfo.areaY].AddEntity(TPEngine.Get().SpriteDictionary["completedNodeSprite"]);
            }

            data.mousePointer = new MousePointer(data.playerSprite, data.areaInfo, data.areaName);
            data.areaLayers[data.areaInfo.areaX + data.areaInfo.areaY - 1].AddEntity(data.mousePointer);
            if (TPEngine.Get().SpriteDictionary.ContainsKey(data.areaName + "PlayerSprite"))
            {
                TPEngine.Get().SpriteDictionary.Remove(data.areaName + "PlayerSprite");
            }

            TPEngine.Get().SpriteDictionary[data.areaName + "PlayerSprite"] = data.playerSprite;

            prevKeyState = Keyboard.GetState();

            if (data.areaName != "hall")
            {
                TPEngine.Get().State.PushState(this, true);
            }

            if (loadSubroom)
            {
                loadSubroom = false;
                TPEngine.Get().State.PushState(new AreaState(playerRoom), true);
            }
            Clock clock = (Clock)TPEngine.Get().SpriteDictionary["MainGameClock"];
            prevHour = clock.GetHour();
            prevMin = clock.GetMinute();
            PlayerMeters.PlayerMeters.Load();
        }

        /// <summary>
        /// Updates the area according to the gameTime
        /// </summary>
        /// <param name="gameTime">The amount of time that has passed</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            
            PlayerMeters.PlayerMeters.Update(gameTime);

            if (data.playerSprite.changeLayer)
            {
                data.playerSprite.ChangeLayer();
            }

            bool isdown = Keyboard.GetState().IsKeyDown(Keys.E);
            bool wasup = prevKeyState.IsKeyUp(Keys.E);

            // Check for an active object and attempt to activate it when the player pushes E
            if (data.playerSprite.ActiveObject != null && data.playerSprite.ActiveObject.isAvailable && Keyboard.GetState().IsKeyUp(Keys.E) && prevKeyState.IsKeyDown(Keys.E))
            {
                TPState state = data.playerSprite.ActiveObject.GetNewState();
                // If its null, we clicked a door back, so pop state back.
                if (state == null)
                {
                    TPEngine.Get().State.PopState();
                    
                    //Any pop-age means positon is hall
                    character.position = "hall";
                }
                // If its not null, push the new state
                else
                {
                    
                    TPEngine.Get().State.PushState(state);
                    QuestChecking.UsingItem();
                    prevKeyState = Keyboard.GetState();
                }
                
                
                SaveCharacterData.Save();
            }
            else
            {
                prevKeyState = Keyboard.GetState();
            }
            // Modified by Jin to display EndingState anytime
            if(Keyboard.GetState().IsKeyDown(Keys.F12))
            {

                TPEngine.Get().State.PushState(new EndingState(), true);
            }
        }

        /// <summary>
        /// Draw the area
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(SpriteBatch batch)
        {
            if (Alive)
            {
                batch.GraphicsDevice.Clear(Color.Black);
                base.Draw(batch);
            }
        }
    }
}
