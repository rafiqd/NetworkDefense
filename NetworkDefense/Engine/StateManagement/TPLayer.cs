using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine.Exceptions;

namespace Engine.StateManagement
{
    public class TPLayer : TPVisibleEntity
    {
        private List<TPVisibleEntity> m_Entity = new List<TPVisibleEntity>();

        public TPLayer(TPLayerList layers)
        {
            layers.Add(this);
        }

        public List<TPVisibleEntity> GetCollection()
        {
            return m_Entity;
        }

        public TPVisibleEntity this[int i]
        {
            get
            {
                // This indexer is very simple, and just returns or sets
                // the corresponding element from the internal array.
                return m_Entity[i];
            }
            set
            {
                m_Entity[i] = value;
            }
        }

        public void AddEntity(TPVisibleEntity entity)
        {
            //for (int i = 0; i < m_Entity.Count; i++)
            //{
            //    if (m_Entity[i].Alive == false)
            //    {
            //        m_Entity[i] = entity;
            //        return;
            //    }
            //}
            m_Entity.Add(entity);
        }

        public void AddEntityToFront(TPVisibleEntity entity)
        {
            //for (int i = 0; i < m_Entity.Count; i++)
            //{
            //    if (m_Entity[i].Alive == false)
            //    {
            //        m_Entity[i] = entity;
            //        return;
            //    }
            //}
            m_Entity.Insert(0, entity);
        }

        public void RemoveEntity(TPVisibleEntity entity)
        {
            m_Entity.Remove(entity);
        }

        protected override void Load()
        {
            base.Load();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (TPVisibleEntity e in m_Entity)
            {
#if DEBUG
                if (e == null)
                {
                    //throw new TPException("null object in TPLayer");
                }
#endif
                if (e != null)
                {
                    e.Update(gameTime);
                }
            }
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        { 
            foreach (TPVisibleEntity e in m_Entity)
            {
                if (e != null)
                {
                    e.Draw(batch);
                }
            }
        }
    }
}
