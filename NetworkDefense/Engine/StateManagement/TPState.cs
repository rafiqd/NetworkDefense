using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.StateManagement
{
    public class TPState : TPVisibleEntity
    {
        /// <summary>
        /// The State's list of layers. 
        /// </summary>
        protected TPLayerList layers;
        /// <summary>
        /// The default draw layer for the state. Whenever you use the AddVisibleEntity it goes here.
        /// </summary>
        private TPLayer m_DefaultLayer;

        public RenderTarget2D BackBuffer { get; set; }

        /// <summary>
        /// Our viewport rendering bounds.
        /// </summary>
        protected Viewport viewportBounds;
        public bool UseRenderTarget { get; set; }
        public RenderTarget2D m_RenderTarget;

        public TPState()
        {
            //set default viewport to entire window
            viewportBounds = TPEngine.Get().GameRef.GraphicsDevice.Viewport;

            layers = new TPLayerList();

            m_DefaultLayer = new TPLayer(layers); 
            //m_DrawLayer = new TPLayer(layers);
            UseRenderTarget = false;
        }

        public void AddLayer(TPLayer layer)
        {
            layers.Add(layer);
        }

        protected override void Load()
        {
            base.Load();
            if (UseRenderTarget)
            {
                m_RenderTarget = new RenderTarget2D(TPEngine.Get().GameRef.GraphicsDevice,
                                                  viewportBounds.Width, viewportBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.DiscardContents);
            } 
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].Update(gameTime);
            }
            m_DefaultLayer.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            SpriteSortMode mode = SpriteSortMode.Deferred;
            batch.Begin(mode, BlendState.AlphaBlend);
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].Draw(batch);
            }
            m_DefaultLayer.Draw(batch);
            batch.End();
        }

        /// <summary>
        /// Adds a visible entity to the states default render layer. Allows small classes and lazy programmeres
        /// to add visible entities to a state without having to create a layer and add it. 
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(TPVisibleEntity entity)
        {
            m_DefaultLayer.AddEntity(entity);
        }

    }
}
