using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Game.Area;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Game.States;

namespace Game.Sprites
{
    public enum PlayerMotionState
    {
        Moving,
        Still,
    }

    public class DemoPlayerSprite : TPSprite
    {
        private Vector2 targetPosition;
        DemoTileSprite targetTile = null;
        public Vector2 roomPosition { set; get; }
        public Point currentPosition;
        private int maxMoveFrames = 20;
        private int currentMoveFrame;
        private KeyboardState previousKBState, currentKBState;
        public AreaInfo[] areaInfo;
        public PlayerMotionState motionState { set; get; }
        public int currentArea { set; get; }
        public int currentLayer { set; get; }
        private TPLayer[][] layers;
        public bool changeLayer { set; get; }
        public bool changeLayerDelay { set; get; }
        private bool changeDirUp = true;
        public UsableIsometricSprite ActiveObject = null;

        public DemoPlayerSprite(AreaInfo[] info, TPLayer[][] lay)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/demoperson"))
        {
            Position = new Vector2(640 - m_Texture.Width / 2, 360 - m_Texture.Height / 2);
            currentMoveFrame = maxMoveFrames;

            areaInfo = info;
            layers = lay;

            motionState = PlayerMotionState.Still;

            previousKBState = Keyboard.GetState();
        }

        public void Load(Vector2 pos, int curArea, int curLayer, Point curPos)
        {
            roomPosition = pos;
            currentPosition = curPos;
            currentArea = curArea;
            currentLayer = curLayer;
            changeLayer = false;
            changeLayerDelay = false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            HandleInput(gameTime);

            if (motionState == PlayerMotionState.Moving)
            {
                roomPosition += (targetPosition - roomPosition) / currentMoveFrame;
                currentMoveFrame--;
                if (currentMoveFrame == 0)
                {
                    motionState = PlayerMotionState.Still;
                    if (changeLayerDelay)
                    {
                        changeLayerDelay = false;
                        changeLayer = true;
                    }
                }
            }

            base.Update(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            currentKBState = Keyboard.GetState();
            if (currentKBState.IsKeyDown(Keys.Left)
                && motionState == PlayerMotionState.Still && currentPosition.Y > 0
                && areaInfo[currentArea].tileTypeArray[(int)currentPosition.Y - 1][(int)(currentPosition.X)] == 't')
            {
                targetPosition = roomPosition + new Vector2(-92, -35);
                currentMoveFrame = maxMoveFrames;
                if (currentPosition.X - 1 >= 0 && areaInfo[currentArea].tileTypeArray[(int)(currentPosition.Y)][(int)(currentPosition.X - 1)] != 't')
                {
                    changeLayerDelay = true;
                }
                else
                {
                    changeLayer = true;
                }
                changeDirUp = true;
                currentPosition.Y--;
                motionState = PlayerMotionState.Moving;
            }
            if (currentKBState.IsKeyDown(Keys.Right)
                && motionState == PlayerMotionState.Still && currentPosition.Y < areaInfo[currentArea].areaY - 1
                && areaInfo[currentArea].tileTypeArray[(int)currentPosition.Y + 1][(int)(currentPosition.X)] == 't')
            {
                targetPosition = roomPosition + new Vector2(92, 35);
                //roomPosition.Y += 35;
                //roomPosition.X += 92;
                currentMoveFrame = maxMoveFrames;
                if (currentPosition.X - 1 >= 0 && currentPosition.Y + 1 < areaInfo[currentArea].areaY && areaInfo[currentArea].tileTypeArray[(int)(currentPosition.Y + 1)][(int)(currentPosition.X - 1)] != 't')
                {
                    changeLayer = true;
                }
                else
                {
                    changeLayerDelay = true;
                }
                changeDirUp = false;
                currentPosition.Y++;
                motionState = PlayerMotionState.Moving;
            }
            if (currentKBState.IsKeyDown(Keys.Down)
                && motionState == PlayerMotionState.Still && currentPosition.X < areaInfo[currentArea].areaX - 1
                && areaInfo[currentArea].tileTypeArray[(int)currentPosition.Y][(int)(currentPosition.X + 1)] == 't')
            {
                targetPosition = roomPosition + new Vector2(-55, 58);
                currentMoveFrame = maxMoveFrames;
                changeLayer = true;
                changeDirUp = false;
                currentPosition.X++;
                motionState = PlayerMotionState.Moving;
            }
            if (currentKBState.IsKeyDown(Keys.Up)
                && motionState == PlayerMotionState.Still && currentPosition.X > 0
                && areaInfo[currentArea].tileTypeArray[(int)currentPosition.Y][(int)(currentPosition.X - 1)] == 't')
            {
                targetPosition = roomPosition + new Vector2(55, -58);
                currentMoveFrame = maxMoveFrames;
                changeLayer = true;
                changeDirUp = true;
                currentPosition.X--;
                motionState = PlayerMotionState.Moving;
            }
            previousKBState = currentKBState;
        }

        public void ChangeLayer()
        {
            if (changeDirUp)
            {
                if (currentLayer > 0)
                {
                    layers[currentArea][currentLayer].RemoveEntity(this);
                    currentLayer--;
                    layers[currentArea][currentLayer].AddEntityToFront(this);
                }
            }
            else if( currentLayer < areaInfo[currentArea].areaX + areaInfo[currentArea].areaY - 1)
            {
                layers[currentArea][currentLayer].RemoveEntity(this);
                currentLayer++;
                layers[currentArea][currentLayer].AddEntityToFront(this);
            }

            changeLayer = false;
        }

        public void SetTargetTile(DemoTileSprite tile)
        {
            targetTile = tile;
        }
    }
}
