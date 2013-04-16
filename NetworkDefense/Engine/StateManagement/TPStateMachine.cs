using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.StateManagement
{
    public class TPStateMachine : TPVisibleEntity
    {
        public List<TPState> stateStack = new List<TPState>();
        private bool m_bChangeState = false;
        private TPState m_newState = null;
        public Viewport ScreenSize { get; set; }
        /// <summary>
        /// If this is true then all layers except the top layer are paused.
        /// </summary>
        private bool m_bTopLayerPriority;

        /// <summary>
        /// This is a rendertarget the size of the screen which essentially acts as the back buffer. It gets passed to 
        /// all the states the state manager handles and each state paints to the buffer. 
        /// </summary>
        public RenderTarget2D m_ScreenBufferRenderTarget;

        private GraphicsDevice m_GDevice = TPEngine.Get().GameRef.GraphicsDevice;
        /// <summary>
        /// Pushes a new state onto the top of the stack.
        /// </summary>
        /// <param name="newState"></param>
        public void PushState(TPState newState)
        {
            stateStack.Add(newState);
            newState.BackBuffer = this.m_ScreenBufferRenderTarget;
        }

        protected override void Load()
        {
            base.Load();
            ScreenSize = TPEngine.Get().ScreenSize;
            m_ScreenBufferRenderTarget = new RenderTarget2D(m_GDevice,
                                              ScreenSize.Width, ScreenSize.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.PreserveContents);

        }

        /// <summary>
        /// Pushes a new state on the stack with a specified priority.
        /// </summary>
        /// <param name="newState">The new state to push onto the stack.</param>
        /// <param name="isAbsolute">Determins whether or not all states below this one should be paused or not.</param>
        public void PushState(TPState newState, bool priority)
        {
            m_bTopLayerPriority = priority;
            newState.BackBuffer = this.m_ScreenBufferRenderTarget;
            //PushState(new HpPauseStateOverlay(Color.Black));
            PushState(newState);
        }

        /// <summary>
        /// Removes the top state from the state stack leaving us to render the previous state.
        /// </summary>
        public void PopState()
        {
            stateStack.RemoveAt(stateStack.Count - 1);
        }

        public TPState PeekState()
        {
            return stateStack[stateStack.Count - 1];
        }

        public TPState PeekState(int offset)
        {
            return stateStack[stateStack.Count - offset];
        }



        /// <summary>
        /// Sets a boolean to safely change the state at the start of the next update. 
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(TPState newState)
        {
            this.m_newState = newState;
            m_bChangeState = true;
        }

        private void m_ChangeState(TPState newState)
        {
            stateStack.Clear();
            stateStack.Add(newState);
            m_bTopLayerPriority = false;
        }

        public override void Draw(SpriteBatch batch)
        {

            //Reset the viewport in case it has been altered and not fixed by another state.
            TPEngine.Get().GameRef.GraphicsDevice.Viewport = ScreenSize;
            m_GDevice.SetRenderTarget(m_ScreenBufferRenderTarget);
            m_GDevice.Clear(Color.Black);
            for (int counter = 0; counter < stateStack.Count; counter++)
            {
                stateStack[counter].Draw(batch);
            }
            m_GDevice.SetRenderTarget(null);
            batch.Begin();
            batch.Draw(m_ScreenBufferRenderTarget, Vector2.Zero, Color.White);
            batch.End();
            base.Draw(batch);
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); 

            if (m_bChangeState)
            {
                m_ChangeState(m_newState);
                m_bChangeState = false;
            }

            if (m_bTopLayerPriority)
            {
                stateStack[stateStack.Count - 1].Update(gameTime);
            }
            else
            {
                for (int counter = 0; counter < stateStack.Count; counter++)
                {
                    stateStack[counter].Update(gameTime);
                }
            }
        }

    }
}

