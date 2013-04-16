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
    /// class that represents an individual email data
    /// </summary>
    class EmailDetailState : TPState
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
        /// user's cursor sprite
        /// </summary>
        GameMouse mouse;

        /// <summary>
        /// background sprite
        /// </summary>
        TPSprite backgroundTexture;

        /// <summary>
        /// indicates if the state is loaded for the first time during that play time
        /// </summary>
        static bool FirstRunDetail = true;
        
        /// <summary>
        /// the sender name data
        /// </summary>
        TPString from;

        /// <summary>
        /// stores subject of the email
        /// </summary>
        TPString subject;

        /// <summary>
        /// list of string used to describe the email
        /// </summary>
        List<TPString> body = new List<TPString>();

        /// <summary>
        /// stores the previous state of the mouse
        /// </summary>
        MouseState prevMouseState;

        /// <summary>
        /// stores the previous state of the keyboard
        /// </summary>
        KeyboardState prevKeyboardState;

        /// <summary>
        /// Peter
        /// constructor used to set the email data
        /// </summary>
        /// <param name="from">the sender name</param>
        /// <param name="subject">subject of the email</param>
        /// <param name="body">the description of the task</param>
        public EmailDetailState(string from, string subject, List<string> body)
        {
            this.from = new TPString(from);
            this.subject = new TPString(subject);
            for(int i = 0; i < body.Count; i++)
            {
                this.body.Add(new TPString(body[i]));
            }
        }

        /// <summary>
        /// Cyril, Peter
        /// load the necessary items that needed to display on screen
        /// </summary>
        protected override void Load()
        {
            base.Load();
            
            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }

            mouse = new GameMouse(new Vector2(200, 200));
            myLayers[2].AddEntity(mouse);

            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/Quest/emaildetail"));
            backgroundTexture.Position = new Vector2(backgroundTexture.Position.X, backgroundTexture.Position.Y);
            myLayers[0].AddEntity(backgroundTexture);

            from.Position.X = 205;
            from.Position.Y = 145;
            from.RenderColor = Color.Black;

            subject.Position.X = 210;
            subject.Position.Y = 225;
            subject.RenderColor = Color.Black;

            myLayers[1].AddEntity(from);
            myLayers[1].AddEntity(subject);
            if (body.Count > 0)
            {
                for (int i = 0; i < body.Count; i++)
                {
                    body[i].Position.X = 100;
                    body[i].Position.Y = 375 + (i * 50);
                    body[i].RenderColor = Color.Black;
                    myLayers[1].AddEntity(body[i]);
                }
            }
            if (FirstRunDetail)
            {
                TPEngine.Get().SpriteDictionary.Add("MinigameMySpriteDetail", mouse);
                TPEngine.Get().StringDictionary.Add("fromDetail", from);
                TPEngine.Get().StringDictionary.Add("subjectDetail", subject);
                FirstRunDetail = false;
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
