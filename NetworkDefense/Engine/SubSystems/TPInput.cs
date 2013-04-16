using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.SubSystems
{
    public class TPInput
    {

        private KeyboardHandler m_Keyboard;
        private GamePadHandler m_Gamepads;
        public TPInput()
        {
            m_Keyboard = new KeyboardHandler();
            m_Gamepads = new GamePadHandler();
        }


        public GamePadHandler GamePads
        {
            get { return m_Gamepads; }
        }


        public KeyboardHandler Keyboard
        {
            get { return m_Keyboard; }
        }


        public void Update(GameTime gameTime)
        {
            m_Gamepads.Update(gameTime);
            m_Keyboard.Update(gameTime);
        }
    }

    public class GamePadHandler
    {
        private GamePadState[] prevGamePadsState = new GamePadState[4];
        private GamePadState[] gamePadsState = new GamePadState[4];

        public GamePadState[] Player
        {
            get
            {
                return (gamePadsState);
            }
        }

        public GamePadHandler()
        {
            prevGamePadsState[0] = GamePad.GetState(PlayerIndex.One);
            prevGamePadsState[1] = GamePad.GetState(PlayerIndex.Two);
            prevGamePadsState[2] = GamePad.GetState(PlayerIndex.Three);
            prevGamePadsState[3] = GamePad.GetState(PlayerIndex.Four);
        }

        public void Update(GameTime gameTime)
        {
            //set our previous state to our new state
            prevGamePadsState[0] = gamePadsState[0];
            prevGamePadsState[1] = gamePadsState[1];
            prevGamePadsState[2] = gamePadsState[2];
            prevGamePadsState[3] = gamePadsState[3];

            //get our new state
            gamePadsState[0] = GamePad.GetState(PlayerIndex.One);
            gamePadsState[1] = GamePad.GetState(PlayerIndex.Two);
            gamePadsState[2] = GamePad.GetState(PlayerIndex.Three);
            gamePadsState[3] = GamePad.GetState(PlayerIndex.Four);
        }

        public bool WasButtonPressed(int playerIndex, Buttons button)
        {
            return (gamePadsState[playerIndex].IsButtonDown(button) &&
                prevGamePadsState[playerIndex].IsButtonUp(button));
        }

        public bool IsButtonDown(int playerIndex, Buttons button)
        {
            return (gamePadsState[playerIndex].IsButtonDown(button));
        }
    }


    public class KeyboardHandler
    {
        private KeyboardState keyboardState;
        private KeyboardState prevKeyboardState;

        public KeyboardHandler()
        {
            prevKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return (keyboardState.IsKeyDown(key));
        }

        public bool IsAnyKeyDown()
        {
            if (keyboardState.GetPressedKeys().Length > 0)
            {
                return true;
            }
            else { return false; }
        }

        public bool IsHoldingKey(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyDown(key));
        }

        public bool HasReleasedKey(Keys key)
        {
            return (keyboardState.IsKeyUp(key) && prevKeyboardState.IsKeyDown(key));
        }

        public void Update(GameTime gameTime)
        {
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        } 
    }
}
