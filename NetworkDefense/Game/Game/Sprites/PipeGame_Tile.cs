using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game.PipeGame
{
    /// <summary>
    /// Tile that has connections related to other tiles.
    /// </summary>
    class PipeGame_Tile : TPSprite
    {
        /// <summary>
        /// The thickness of the pipe on the tile
        /// </summary>
        private const int pipeThickness = 5;
        /// <summary>
        /// The thickness of the border of the tile
        /// </summary>
        private const int borderThickness = 3;
        /// <summary>
        /// The rotation speed of the tile
        /// </summary>
        private const float rotateSpeed = (float)(0.05 * Math.PI);
        /// <summary>
        /// The width of the tile
        /// </summary>
        private int tileWidth;
        /// <summary>
        /// The height of the tile
        /// </summary>
        private int tileHeight;
        /// <summary>
        /// Colour of the pipe on the tile
        /// </summary>
        private Color pipeColour = Color.Blue;

        /// <summary>
        /// Colour of the pipe
        /// </summary>
        public Color PipeColour
        {
            get { return pipeColour; }
            set { pipeColour = value; }
        }
        /// <summary>
        /// Colour of the border of the tile
        /// </summary>
        private Color borderColour = Color.DarkSlateBlue;

        /// <summary>
        /// Colour of the border of the tile
        /// </summary>
        public Color BorderColour
        {
            get { return borderColour; }
            set { borderColour = value; }
        }

        /// <summary>
        /// The textures of the horizontal pipes
        /// </summary>
        private Texture2D horizontalPipe;
        /// <summary>
        /// The textures of the vertical pipes
        /// </summary>
        private Texture2D verticalPipe;
        /// <summary>
        /// The textures of the horizontal borders
        /// </summary> 
        private Texture2D horizontalBorder;
        /// <summary>
        /// The textures of the vertical borders
        /// </summary>
        private Texture2D verticalBorder;
        /// <summary>
        /// A boolean flag indicates whether the tile is a start tile or not
        /// </summary>
        private bool startTile = false;

        /// <summary>
        /// The start tile
        /// </summary>
        public bool StartTile
        {
            get { return startTile; }
            set { startTile = value; }
        }

        /// <summary>
        /// A boolean flag indicates whether the tile is an end tile or not
        /// </summary>
        private bool endTile = false;
        /// <summary>
        /// The end tile
        /// </summary>
        public bool EndTile
        {
            get { return endTile; }
            set { endTile = value; }
        }

        /// <summary>
        /// A boolean flag indicates whether the tile is connected to another tile
        /// </summary>
        private bool connected = false;

        /// <summary>
        /// A boolean flag indicates whether the tile is connected to another or not
        /// </summary>
        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }
        /// <summary>
        /// A struct that stores boolean flags representing the four pipe connection
        /// </summary>
        private connection connect;
        /// <summary>
        /// A struct that stores boolean flags representing the previous four pipe connection
        /// </summary>
        private connection oldConnect;

        /// <summary>
        /// Connections between this tile and the neighbour tiles
        /// </summary>
        public struct connection
        {
            public connection(bool r, bool d, bool l, bool u)
            {
                right = r;
                down = d;
                left = l;
                up = u;
            }
            public bool right;
            public bool down;
            public bool left;
            public bool up;
        };
        
        /// <summary>
        /// Connection state of the tile
        /// </summary>
        public connection Connect
        {
            get { return connect; }
            set { connect = value; }
        }

        /// <summary>
        /// Indicates whether this tile is connected to the tile on the left
        /// </summary>
        public bool Left
        {
            get { return connect.left; }
            set { connect.left = value; }
        }
        /// <summary>
        /// Indicates whether this tile is connected to the tile on the right
        /// </summary>
        public bool Right
        {
            get { return connect.right; }
            set { connect.right = value; }
        }
        /// <summary>
        /// Indicates whether this tile is connected to the tile on the top
        /// </summary>
        public bool Up
        {
            get { return connect.up; }
            set { connect.up = value; }
        }
        /// <summary>
        /// Indicates whether this tile is connected to the tile on the down
        /// </summary>
        public bool Down
        {
            get { return connect.down; }
            set { connect.down = value; }
        }

        /// <summary>
        /// Left neighbour tile
        /// </summary>
        private PipeGame_Tile leftNeighbour = null;

        /// <summary>
        /// Left neighbour tile
        /// </summary>
        public PipeGame_Tile LeftNeighbour
        {
            get { return leftNeighbour; }
            set { leftNeighbour = value; }
        }

        /// <summary>
        /// Right neighbour tile
        /// </summary>
        private PipeGame_Tile rightNeighbour = null;

        /// <summary>
        /// Right neighbour tile
        /// </summary>
        public PipeGame_Tile RightNeighbour
        {
            get { return rightNeighbour; }
            set { rightNeighbour = value; }
        }

        /// <summary>
        /// Top neighbour tile
        /// </summary>
        private PipeGame_Tile topNeighbour = null;

        /// <summary>
        /// Top neighbour tile
        /// </summary>
        public PipeGame_Tile TopNeighbour
        {
            get { return topNeighbour; }
            set { topNeighbour = value; }
        }

        /// <summary>
        /// Bottom neighbour tile
        /// </summary>
        private PipeGame_Tile bottomNeighbour = null;

        /// <summary>
        /// Bottom neighbour tile
        /// </summary>
        public PipeGame_Tile BottomNeighbour
        {
            get { return bottomNeighbour; }
            set { bottomNeighbour = value; }
        }

        /// <summary>
        /// The game state of the game
        /// </summary>
        private PipeGame_GameState pipegame_state;
        /// <summary>
        /// The rotation angle
        /// </summary>
        private float rotationAngle = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pos">Position of the tile</param>
        /// <param name="width">Width of the tile</param>
        /// <param name="height">Height of the tile</param>
        /// <param name="col">Colour of the tile</param>
        /// <param name="con">Connection state of the tile</param>
        public PipeGame_Tile(Vector2 pos, int width, int height, Color col, connection con, PipeGame_GameState pg_state)
            : base(TPEngine.Get().TextureManager.CreateFilledRectangle(width, height, col))
        {
            Position = pos;
            tileHeight = height;
            tileWidth = Width;
            connect = con;
            pipegame_state = pg_state;
            //Create the textures for the pipes
            horizontalPipe = TPEngine.Get().TextureManager.CreateFilledRectangle(tileWidth / 2, pipeThickness, Color.White);
            verticalPipe = TPEngine.Get().TextureManager.CreateFilledRectangle(pipeThickness, tileHeight / 2, Color.White);
            //Create the textures for the borders
            horizontalBorder = TPEngine.Get().TextureManager.CreateFilledRectangle(tileWidth, borderThickness, Color.White);
            verticalBorder = TPEngine.Get().TextureManager.CreateFilledRectangle(borderThickness, tileHeight, Color.White);
        }

        /// <summary>
        /// Draw the tile
        /// </summary>
        /// <param name="batch">The sprite batch for drawing</param>
        public override void Draw(SpriteBatch batch)
        {
            //Draw the tile
            batch.Draw(this.m_Texture, new Rectangle((int)Position.X + tileWidth / 2, (int)Position.Y + tileHeight / 2, tileWidth, tileHeight), null, Color.White, rotationAngle, new Vector2(tileWidth / 2, tileHeight / 2), SpriteEffects.None, 0.0f);
            Vector2 pos;
            Vector2 topLeftBorderPos = new Vector2(Position.X, Position.Y);

            //Save the old connection for the rotation animation
            connection connect;
            if(rotationAngle != 0)
                connect = oldConnect;
            else
                connect = this.connect;

            //Draw the pipes 
            if (connect.right)
            {
                pos = new Vector2(Position.X + tileWidth / 2, Position.Y + tileHeight / 2);
                batch.Draw(horizontalPipe, new Rectangle((int)pos.X, (int)pos.Y, tileWidth / 2, pipeThickness), null, pipeColour, rotationAngle, new Vector2(0, pipeThickness / 2), SpriteEffects.None, 0.0f);
            }
            if (connect.down)
            {
                pos = new Vector2(Position.X + tileWidth / 2, Position.Y + tileHeight / 2);
                batch.Draw(verticalPipe, new Rectangle((int)pos.X, (int)pos.Y, pipeThickness, tileWidth / 2), null, pipeColour, rotationAngle, new Vector2(pipeThickness / 2, 0), SpriteEffects.None, 0.0f);
            }
            if (connect.left)
            {
                pos = new Vector2(Position.X + tileHeight / 2, Position.Y + tileHeight / 2);
                batch.Draw(horizontalPipe, new Rectangle((int)pos.X, (int)pos.Y, tileWidth / 2, pipeThickness), null, pipeColour, rotationAngle, new Vector2(tileWidth / 2, pipeThickness / 2), SpriteEffects.None, 0.0f);
            }
            if (connect.up)
            {
                pos = new Vector2(Position.X + tileWidth / 2, Position.Y + tileWidth / 2);
                batch.Draw(verticalPipe, new Rectangle((int)pos.X, (int)pos.Y, pipeThickness, tileWidth / 2), null, pipeColour, rotationAngle, new Vector2(pipeThickness / 2, tileWidth / 2), SpriteEffects.None, 0.0f);
            }

            //Draw the borders
            batch.Draw(horizontalBorder, topLeftBorderPos, borderColour);
            batch.Draw(horizontalBorder, new Vector2(Position.X, Position.Y + tileHeight), borderColour);
            batch.Draw(verticalBorder, new Vector2(Position.X, Position.Y), borderColour);
            batch.Draw(verticalBorder, new Vector2(Position.X + tileWidth, Position.Y), borderColour);
           
            //Draw the icons on the tile
            if (startTile)
                batch.Draw(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/pipe_wifi"), new Rectangle((int)Position.X + tileWidth / 2, (int)Position.Y + tileHeight / 2, (int)(80 * ((float)tileWidth/119)), (int)(80 * ((float)tileHeight/119))), null, Color.White, 0, new Vector2(50, 50), SpriteEffects.None, 0.0f);
            if (endTile)
                if(Connected)
                    batch.Draw(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/pipe_endon"), new Rectangle((int)Position.X + tileWidth / 2, (int)Position.Y + tileHeight / 2, (int)(60* ((float)tileWidth/119)) , (int)(60* ((float)tileHeight/119))), null, Color.White, 0, new Vector2(50, 50), SpriteEffects.None, 0.0f);
                else
                    batch.Draw(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/pipe_endoff"), new Rectangle((int)Position.X + tileWidth / 2, (int)Position.Y + tileHeight / 2, (int)(60* ((float)tileWidth/119)), (int)(60* ((float)tileHeight/119))), null, Color.White, 0, new Vector2(50, 50), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Update the rotation radian, update the connections check for victory
        /// </summary>
        /// <param name="gameTime">The time of the game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (rotationAngle != 0 && rotationAngle < (0.5 * Math.PI))
                rotationAngle += rotateSpeed;
            else if (rotationAngle > (0.5 * Math.PI))
            {
                rotationAngle = 0;
                pipegame_state.updateConnection();
                pipegame_state.checkVictory();
            }
        }

        /// <summary>
        /// Rotate the tile.
        /// </summary>
        public void rotate()
        {
            rotationAngle = rotateSpeed;
            rotateConnectedDirections();
        }

        /// <summary>
        /// Rotate the connected directions
        /// </summary>
        public void rotateConnectedDirections()
        {
            //save old connection values
            oldConnect.right = connect.right;
            oldConnect.left = connect.left;
            oldConnect.up = connect.up;
            oldConnect.down = connect.down;

            PipeGame_Tile.connection con;
            con.right = connect.right;
            con.left = connect.left;
            con.up = connect.up;
            con.down = connect.down;

            connect.right = false;
            connect.down = false;
            connect.left = false;
            connect.up = false;

            if (con.right)
                connect.down = true;
            if (con.down)
                connect.left = true;
            if (con.left)
                connect.up = true;
            if (con.up)
                connect.right = true;
        }
    }
}
