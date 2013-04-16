using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Engine.Collision;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.States;
using DBCommService;

namespace Game.Sprites
{

    /// <summary>
    /// Cyril, Peter
    /// Read more button sprite. provide the email detail when the user clicks
    /// </summary>
    class ReadMoreButton : TPSprite
    {
        /// <summary>
        /// stores the previous state of the mouse
        /// </summary>
        MouseState prevMouseState;

        /// <summary>
        /// stores the previous state of the keyboard
        /// </summary>
        KeyboardState prevKeyboardState;

        /// <summary>
        /// sprite indicates where the user clicks
        /// </summary>
        BulletHole bulletHole;

        /// <summary>
        /// user's cursor sprite
        /// </summary>
        GameMouse mouse;

        /// <summary>
        /// variable stores the current quest node
        /// </summary>
        private int currentNode;

        /// <summary>
        /// sender name
        /// </summary>
        private string from;

        public string From
        {
            get { return from; }
            set { from = value; }
        }

        /// <summary>
        /// email subject
        /// </summary>
        private string subject;

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// email body
        /// </summary>
        private List<string> body;

        public List<string> Body
        {
            get { return body; }
            set { body = value; }
        }

        //private string selectedEmail;

        //public string SelectedEmail
        //{
        //    get { return selectedEmail; }
        //    set { selectedEmail = value; }
        //}
        
        /// <summary>
        /// constructor used to initialize the read more button's properties
        /// </summary>
        /// <param name="node">the current quest node</param>
        public ReadMoreButton(int node)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/Quest/readmore"))
        {
            this.currentNode = node;
        }

        /// <summary>
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            mouse = (GameMouse)TPEngine.Get().SpriteDictionary["MinigameMySpriteEmail"];
            if (prevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

                bulletHole = (BulletHole)TPEngine.Get().SpriteDictionary["bulletHoleEmail"];
                bulletHole.Position = new Vector2(Mouse.GetState().X - (bulletHole.Width * bulletHole.Scale.X / 2),
                                                Mouse.GetState().Y - (bulletHole.Height * bulletHole.Scale.Y / 2));
                bulletHole.Alive = true;
                bulletHole.Scale = new Vector2(0.0001f, 0.0001f);
                if (TPCollider.Test(this, bulletHole))
                {
                    if (AreaState.character.Quests != null)
                    {
                        from = AreaState.character.Quests[0].Nodes[currentNode].QuestDescription.Sender;
                        subject = AreaState.character.Quests[0].Nodes[currentNode].QuestDescription.Subject;
                        body = AreaState.character.Quests[0].Nodes[currentNode].QuestDescription.Body;
                    }
                    TPEngine.Get().State.PushState(new EmailDetailState(from, subject, body));
                    Globals.Paused = false;
                }
            }

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        /// <summary>
        /// load the necessary items that needed to display on screen
        /// </summary>
        protected override void Load()
        {
            base.Load();
            AffectedByDrag = false;
        }
    }
}
