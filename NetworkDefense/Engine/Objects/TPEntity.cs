using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine.Objects
{
    public class TPEntity
    {
        /// <summary>
        /// Represents the world space position of the entity.
        /// </summary>
        public Vector2 Position = Vector2.Zero;
        /// <summary>
        /// Represents the velocity of the entity.
        /// </summary>
        public Vector2 Velocity = Vector2.Zero;
        /// <summary>
        /// Represents the acceleration of the entity.
        /// </summary>
        public Vector2 Acceleration = Vector2.Zero;
        /// <summary>
        /// The origin of the entity. This is defined as the worldspace center of the entity. 
        /// </summary>
        public Vector2 Origin = Vector2.Zero;
        /// <summary>
        /// The velocity drag of the entity. This is the amount of velocity lost every update.
        /// </summary>
        public Vector2 DragVelocity = new Vector2(0.05f,0.05f);
        /// <summary>
        /// The acceleration drag of the entity. This is the amount of acceleration lost every update.
        /// </summary>
        public Vector2 DragAcceleration = Vector2.Zero;
        /// <summary>
        /// The draw scale of the entity. NOTE: Since this only affects draw scale it should be moved to the 
        /// VisualEntity class.
        /// </summary>
        public Vector2 Scale = new Vector2(1, 1);
        /// <summary>
        /// The max acceleration limit of this entity.
        /// </summary>
        public Vector2 LimitAcceleration = new Vector2(100, 100);
        /// <summary>
        /// The max velocity limit of this entity.
        /// </summary>
        public Vector2 LimitVelocity = new Vector2(100,100);
        /// <summary>
        /// Used to determine the point of origin when a sprite is rotated. 
        /// (0,0) is the top left and (1,1) is the top right.
        /// </summary>
        public Vector2 RotationOrigin = Vector2.Zero;
        /// <summary>
        /// The current rotaion of the entity. In radian format.
        /// </summary>
        public float Rotation = 0.0f;
        /// <summary>
        /// Width of the entity.
        /// </summary>
        public int Width = 0;
        /// <summary>
        /// Height of the entity.
        /// </summary>
        public int Height = 0;

        /// <summary>
        /// Poll this to see if the entity has just woken up.
        /// </summary>
        public bool JustWokeUp { get; set; }
        /// <summary>
        /// Used to tell if JustWokeUp has been true for exactly 1 update cycle. 
        /// </summary>
        protected bool JustWokeUpLast { get; set; }
        /// <summary>
        /// Used to determine whether or not to call the load function.
        /// </summary>
        private bool m_FirstUpdate = true;
        /// <summary>
        /// Alive property. Will not be drawn or updated when false. Additionally
        /// container classes use the Alive property to determine whether elements 
        /// in collections can be replaced with new elements.
        /// </summary>
        public bool Alive { get; set; }
        /// <summary>
        /// Private sleep member.
        /// </summary>
        private bool m_Asleep;
        /// <summary>
        /// If this is true then the entity will not update. 
        /// </summary>
        public bool Asleep
        {
            get
            {
                return m_Asleep;
            }
            set
            {
                if (value == true)
                {
                    JustWokeUp = true;
                }
                m_Asleep = value;
            }
        }
        /// <summary>
        /// Determines whether or not the entity should be affected by drag values. 
        /// </summary>
        public bool AffectedByDrag { get; set; }


        public void Kill()
        {
            Alive = false;
            Asleep = false;
        }
        /// <summary>
        /// Load function
        /// </summary>
        protected virtual void Load() 
        {
            //This to prevent some divide-by-zero cases when calculating the rotation origin.
            if (Width == 0)
            {
                Width = 10;
            }
            if (Height == 0)
            {
                Height = 10;
            }
            AffectedByDrag = true;
            Alive = true;
        }

        /// <summary>
        /// The main update function. In this function we handle entity dynamics mainly. We
        /// also perform some logic to detect if the entity has just woken up or needs to 
        /// call its load function.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            //Check if we need to call the entities load function.
            if (m_FirstUpdate)
            {
                m_FirstUpdate = false;
                Load();
            }
            // Handle logic for testing whether the entity just woke up.
            if (JustWokeUp)
            {
                if (JustWokeUp && JustWokeUpLast)
                {
                    JustWokeUp = false;
                    JustWokeUpLast = false;
                }
                else
                {
                    JustWokeUpLast = true;
                }
            }
            //Perform entity dynamics calculations.
            if (!Asleep)
            {
                ApplyLimits(ref Acceleration, ref LimitAcceleration);
                if (AffectedByDrag)
                {
                    ApplyDrag(ref Acceleration, ref DragAcceleration);
                }
                Velocity += Acceleration;
                ApplyLimits(ref Velocity, ref LimitVelocity);
                if (AffectedByDrag)
                {
                    ApplyDrag(ref Velocity, ref DragVelocity);
                }
                Position += Velocity;
                Origin.X = Position.X + (Width / 2);
                Origin.Y = Position.Y + (Height / 2);
                RotationOrigin.X = (Position.X / Origin.X) / Width;
                RotationOrigin.Y = (Position.Y / Origin.Y) / Height;
            }

            //m_CollisionMesh.UpdatePosition(this.position.X, this.position.Y, (int)this.width, (int)this.height, this.scale);

        }

        /// <summary>
        /// Takes in a vector and makes sure that its values lie within a limit. If they do not then they 
        /// are clamped.
        /// </summary>
        /// <param name="value">The value vector</param>
        /// <param name="limit">The limit vector</param>
        protected void ApplyLimits(ref Vector2 value, ref Vector2 limit)
        {
            if (Math.Abs(value.X) > limit.X)
            {
                if (value.X < 0)
                {
                    value.X = -limit.X;
                }
                if (value.X > 0)
                {
                    value.X = limit.X;
                }
            }

            if (Math.Abs(value.Y) > limit.Y)
            {
                if (value.Y < 0)
                {
                    value.Y = -limit.Y;
                }
                if (value.Y > 0)
                {
                    value.Y = limit.Y;
                }
            }
        }
        /// <summary>
        /// Used to apply drag to an entity. This is like degredation or friction.
        /// </summary>
        /// <param name="value">The value vector</param>
        /// <param name="drag">The drag vector</param>
        protected void ApplyDrag(ref Vector2 value, ref Vector2 drag)
        {

            if (value.X > drag.X)
            {
                value.X -= drag.X;
            }
            else if (value.X < -drag.X)
            {
                value.X += drag.X;
            }
            else
            {
                value.X = 0;
            }

            if (value.Y > drag.Y)
            {
                value.Y -= drag.Y;
            }
            else if (value.Y < -drag.Y)
            {
                value.Y += drag.Y;
            }
            else
            {
                value.Y = 0;
            }
        }
    }
}
