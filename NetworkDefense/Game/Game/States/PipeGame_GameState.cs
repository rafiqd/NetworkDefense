using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Game.Sprites;
using System.Collections;
using Microsoft.Xna.Framework.Input;
using Game.States;
using DBCommService;

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game.PipeGame
{
    /// <summary>
    /// Game state of the pipe game. It creates a grid of tiles. It shows the game time and 
    /// a giveup button too.
    /// </summary>
    class PipeGame_GameState : TPState
    {
        /// <summary>
        /// The layer of the state
        /// </summary>
        private TPLayer[] layer;
        /// <summary>
        /// Number of tile on a row
        /// </summary>
        private int tileNumberRow = 5;
        /// <summary>
        /// Number of tile on a column
        /// </summary>
        private int tileNumberCol = 5;
        /// <summary>
        /// The width of a tile
        /// </summary>
        private int tileWidth;
        /// <summary>
        /// The height of a tile
        /// </summary>
        private int tileHeight;
        /// <summary>
        /// The space between two tiles
        /// </summary>
        private int tileGap = 5;
        /// <summary>
        /// The x position of the leftmost tile
        /// </summary>
        private int tileStartX = 50;
        /// <summary>
        /// The y position of the topmost tile
        /// </summary>
        private int tileStartY = 50;
        /// <summary>
        /// The screen's width
        /// </summary>
        private int screenWidth = 0;
        /// <summary>
        /// The screen's height
        /// </summary>
        private int screenHeight = 0;
        /// <summary>
        /// The array of tiles
        /// </summary>
        private PipeGame_Tile[] tiles;
        /// <summary>
        /// The colour of the tiles
        /// </summary>
        private Color tileColour = new Color(141,166,195);
        /// <summary>
        /// The colour of the tiles that are connected to the start tile
        /// </summary>
        private Color connectedColour = new Color(147, 233, 184);
        /// <summary>
        /// The colour of the pipes
        /// </summary>
        private Color pipeColour = new Color(255, 170, 170);
        /// <summary>
        /// The colour of the border of the tiles
        /// </summary>
        private Color borderColour = new Color(227, 170, 214);
        /// <summary>
        /// The colour of the border of the selected tile
        /// </summary>
        private Color selectedBorderColour = Color.White;
        /// <summary>
        /// The arraylist of all the end tiles
        /// </summary>
        private ArrayList endTiles;
        /// <summary>
        /// The mouse cursor
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(200, 200));
        /// <summary>
        /// Current state of the mouse
        /// </summary>
        private MouseState currentMouseState;
        /// <summary>
        /// Previous state of the mouse
        /// </summary>
        private MouseState lastMouseState;
        /// <summary>
        /// Starting time
        /// </summary>
        private int StartTime = 0;
        /// <summary>
        /// First run of the game state
        /// </summary>
        private bool firstRun = true;
        /// <summary>
        /// The string that displays the game time
        /// </summary>
        private TPString timeText = new TPString("Time: 0");
        /// <summary>
        /// The x position of the string that displays the game time 
        /// </summary>
        private int timeTextX;
        /// <summary>
        /// The y position of the string that displays the game time
        /// </summary>
        private int timeTextY;
        /// <summary>
        /// The colour of the string that displays the game time
        /// </summary>
        private Color timeTextColour = Color.DarkBlue;
        /// <summary>
        /// The number of seconds that the game has run
        /// </summary>
        private int numSecond;
        /// <summary>
        /// Player's score for the game
        /// </summary>
        private int score;

        /// <summary>
        /// Connecting direction of a tile
        /// </summary>
        public enum direction
        {
            right,
            down,
            left,
            up
        };

        /// <summary>
        /// Load everything the minigame needed.
        /// </summary>
        protected override void Load()
        {
            base.Load();

            //Get the screen size
            screenHeight = TPEngine.Get().ScreenSize.Height;
            screenWidth = TPEngine.Get().ScreenSize.Width;

            //Set the tile numbers accordingly to the sanity of the character 
            tileNumberCol = tileNumberRow = 9 - (int)(PlayerMeters.PlayerMeters.Sanity / 25);

            //Setting the width and height of the tiles
            if(tileNumberRow < tileNumberCol)
                tileWidth = tileHeight = (screenHeight - 2 * tileStartY - tileNumberCol * tileGap) / (tileNumberCol);
            else
                tileWidth = tileHeight = (screenHeight - 2 * tileStartY - tileNumberRow * tileGap) / (tileNumberRow);

            //Create the layer
            layer = new TPLayer[3];
            for (int x = 0; x < 3; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }
            layer[0].AddEntity(new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/pipe_background")));

            //Create a grid of tiles and connect them randomly
            //Rotate every used tile for a random times
            //Update the connecting relationship
            createTiles();
            createNetWork();
            scramble();
            updateConnection();
            layer[2].AddEntity(mousePointer);

            //Timer
            timeTextX = screenHeight + (screenWidth - screenHeight) / 2;
            timeTextY = screenHeight * 1 / 4;
            timeText.Position.X = timeTextX;
            timeText.Position.Y = timeTextY;
            timeText.RenderColor = timeTextColour;
            timeText.centerText = true;

            layer[1].AddEntity(timeText);
            TPEngine.Get().StringDictionary.Add("PipeGame_Time", timeText);

            //Instruction
            TPString instruction = new TPString("Connect the wifi to all of the computers." + Environment.NewLine
                + "Click on a tile to rotate it.");
            instruction.Position = new Vector2(timeTextX, timeTextY + timeTextY);
            instruction.RenderColor = Color.DarkBlue;
            instruction.centerText = true;
            instruction.Scale = new Vector2(0.7f, 0.7f);
            layer[1].AddEntity(instruction);

            //Give up button
            ButtonSprite giveupButton = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
                TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                new Rectangle(screenHeight + (screenWidth - screenHeight) / 2 - 100, screenHeight - 50 - 40, 200, 40), "Give Up", layer[2]);
            giveupButton.ButtonAction = delegate { TPEngine.Get().State.PopState(); score = 0; updateScore(); };
            giveupButton.CenterText = true;
            giveupButton.DrawColor = Color.White;
            layer[1].AddEntity(giveupButton);

            //Score
            score = 200;
        }

        /// <summary>
        /// Update the state 60 times.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Tile being clicked
            PipeGame_Tile clickedTile  = null;

            // The active state from the last frame is now old 
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame 
            currentMouseState = Mouse.GetState();

            //Recognize a single click of the left mouse button
            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                clickedTile = mouseClick(Mouse.GetState().X, Mouse.GetState().Y);

            mouseOver(Mouse.GetState().X, Mouse.GetState().Y);

            //Set the start time
            if (firstRun)
            {
                StartTime = (int)gameTime.TotalGameTime.TotalSeconds;
                firstRun = false;
            }

            //Update the time
            numSecond = (int)Math.Floor(gameTime.TotalGameTime.TotalSeconds - StartTime);
            timeText = new TPString(numSecond.ToString());
            TPString tempTime = TPEngine.Get().StringDictionary["PipeGame_Time"];
            tempTime.Clear();
            tempTime.Append("Time: " + timeText);  
        }

        /// <summary>
        /// Create an array of tiles.
        /// </summary>
        public void createTiles()
        {
            tiles = new PipeGame_Tile[tileNumberRow * tileNumberCol];
            int posY = tileStartY;
            int tileNum = 0;
            for (int i = 0; i < tileNumberRow; i++)
            {
                int posX = tileStartX;
                for (int j = 0; j < tileNumberCol; j++)
                {
                    tiles[tileNum] = new PipeGame_Tile(new Vector2(posX, posY), tileWidth, tileHeight, tileColour, new PipeGame_Tile.connection(false, false, false, false), this);
                    layer[0].AddEntity(tiles[tileNum]);
                    posX += tileGap + tileWidth;
                    tileNum++;
                }
                posY += tileGap + tileHeight;
            }
        }

        /// <summary>
        /// Create the tile network on the grid.
        /// </summary>
        /// <returns></returns>
        public int createNetWork()
        {
            ArrayList list = new ArrayList();
            //Randomly choose a start tile
            int randTileNum = TPEngine.Get().Rand.Next(tileNumberRow * tileNumberCol);

            //Set the chosen tile as the start tile and connected
            tiles[randTileNum].Connected = true;
            tiles[randTileNum].StartTile = true;

            //Add the chosen tile to the working list
            list.Add(tiles[randTileNum]);

            if (TPEngine.Get().Rand.Next(2) == 0)//50% chance
            {
                addRandomDir(list);
            }
            
            //Add directions
            while (list.Count > 0)
            {
                
                if (TPEngine.Get().Rand.Next(2) == 0)//50% chance
                {
                    addRandomDir(list);
                    if(TPEngine.Get().Rand.Next(2) == 0)//50% chance
                        addRandomDir(list);
                    if (TPEngine.Get().Rand.Next(3) == 0)//33% chance
                        addRandomDir(list);
                }
                else
                    list.Add(list[0]);
                //Remove first element of the list
                list.RemoveAt(0);
            }
            
            //Count the number of connected tiles in the grid and put all end tiles
            //into endTiles array
            endTiles = new ArrayList();
            int tileCount = 0;
            for (int i = 0; i < (tileNumberRow * tileNumberCol); i++)
            {
                if (tiles[i].Connected)
                    tileCount++;
                if (isEndTile(tiles[i]) && !tiles[i].StartTile)
                {
                    endTiles.Add(tiles[i]);
                    tiles[i].EndTile = true;
                }
            }
            return tileCount;
        }

        /// <summary>
        /// Randomly add directions onto a tile and connect it with the previous tile.
        /// </summary>
        /// <param name="list"></param>
        public void addRandomDir(ArrayList list)
        {
            PipeGame_Tile tile = (PipeGame_Tile) list[0];
            ArrayList freeTileConPair = new ArrayList();

            //Find all the neighbour tiles and check if it is connected
            //Add the non-connected neighbour to the free tile list 
            for (int i = 0; i < tiles.Length; i++)
            {               
                //Left neighbour tile
                if ((tiles[i].Position.X + tileWidth + tileGap) == tile.Position.X && tiles[i].Position.Y == tile.Position.Y)
                {
                    tile.LeftNeighbour = tiles[i];
                    if (!tiles[i].Connected)
                        freeTileConPair.Add(new KeyValuePair<PipeGame_Tile, direction>(tiles[i], direction.left));                        
                }
                //Right neighbour tile
                else if ((tiles[i].Position.X - tileWidth - tileGap) == tile.Position.X && tiles[i].Position.Y == tile.Position.Y)
                {
                    tile.RightNeighbour = tiles[i];
                    if (!tiles[i].Connected)
                        freeTileConPair.Add(new KeyValuePair<PipeGame_Tile, direction>(tiles[i], direction.right));                        
                }
                //Top neighbour tile
                else if (tiles[i].Position.X == tile.Position.X && (tiles[i].Position.Y + tileHeight + tileGap) == tile.Position.Y)
                {
                    tile.TopNeighbour = tiles[i];
                    if (!tiles[i].Connected)
                        freeTileConPair.Add(new KeyValuePair<PipeGame_Tile, direction>(tiles[i], direction.up));                   
                }
                //Bottom neighbour tile
                else if (tiles[i].Position.X == tile.Position.X && (tiles[i].Position.Y - tileHeight - tileGap) == tile.Position.Y)
                {
                    tile.BottomNeighbour = tiles[i];
                    if (!tiles[i].Connected)
                        freeTileConPair.Add(new KeyValuePair<PipeGame_Tile, direction>(tiles[i], direction.down));                
                }
                
            }

            //Return when there is no free tile
            if (freeTileConPair.Count == 0)
                return;

            //Randomly choose one of the tile in the free tile list and
            //add the connections to the tile and the reverse direction to its
            //destination tile
            int randNum = TPEngine.Get().Rand.Next(freeTileConPair.Count);
            switch (((KeyValuePair<PipeGame_Tile, direction>)freeTileConPair[randNum]).Value)
            {
                case direction.left:
                    tile.Left = true;
                    ((KeyValuePair<PipeGame_Tile, direction>)freeTileConPair[randNum]).Key.Right = true;
                    break;
                case direction.right:
                    tile.Right = true;
                    ((KeyValuePair<PipeGame_Tile, direction>)freeTileConPair[randNum]).Key.Left = true;
                    break;
                case direction.up:
                    tile.Up = true;
                    ((KeyValuePair<PipeGame_Tile, direction>)freeTileConPair[randNum]).Key.Down = true;
                    break;
                case direction.down:
                    tile.Down = true;
                    ((KeyValuePair<PipeGame_Tile, direction>)freeTileConPair[randNum]).Key.Up = true;
                    break;
            }
            //Set the connected flag of the tile to be true
            ((KeyValuePair<PipeGame_Tile, direction>)freeTileConPair[randNum]).Key.Connected = true;

            //Add the chosen neighbour tile to the working list
            list.Add(((KeyValuePair<PipeGame_Tile, direction>)freeTileConPair[randNum]).Key);
        }

        /// <summary>
        /// Check if the tile is an end tile
        /// </summary>
        /// <param name="tile">tile</param>
        /// <returns>boolean indicate whether the tile is a end tile or not</returns>
        public bool isEndTile(PipeGame_Tile tile)
        {
            if ((tile.Left == true && tile.Right == false && tile.Up == false && tile.Down == false) ||
                (tile.Left == false && tile.Right == true && tile.Up == false && tile.Down == false) ||
                (tile.Left == false && tile.Right == false && tile.Up == true && tile.Down == false) ||
                (tile.Left == false && tile.Right == false && tile.Up == false && tile.Down == true))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Randomly rotate the tiles.
        /// </summary>
        public void scramble()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < TPEngine.Get().Rand.Next(4); j++)
                    tiles[i].rotate();
            }
        }

        /// <summary>
        /// Updates the boolean connected value in the tiles. Call this 
        /// every time a tile is rotated.
        /// </summary>
        public void updateConnection()
        {
            ArrayList connectingCells = new ArrayList();
            PipeGame_Tile startTile;

            //Reset every tile to be non-connected
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].Connected = false;
                tiles[i].PipeColour = pipeColour;
                //Find the start tile and add it to the connecting arraylist
                if (tiles[i].StartTile)
                {
                    connectingCells.Add(tiles[i]);
                    startTile = tiles[i];
                    startTile.Connected = true;
                    startTile.PipeColour = connectedColour;
                }
            }

            //Exit if the start tile is rotating
            //SOMETHING

            //Breadth First Search!!!
            while (connectingCells.Count != 0)
            {
                PipeGame_Tile tile = (PipeGame_Tile)connectingCells[0];
                connectingCells.RemoveAt(0);

                //Check for left neighbour tile
                if (tile.LeftNeighbour != null && !tile.LeftNeighbour.Connected)
                {
                    if (tile.Left == true && tile.LeftNeighbour.Right == true)
                    {
                        tile.LeftNeighbour.Connected = true;                        
                        tile.LeftNeighbour.PipeColour = connectedColour;
                        connectingCells.Add(tile.LeftNeighbour);
                    }
                }
                //Check for right neighbour tile
                if (tile.RightNeighbour != null && !tile.RightNeighbour.Connected)
                {
                    if (tile.Right == true && tile.RightNeighbour.Left == true)
                    {
                        tile.RightNeighbour.Connected = true;                      
                        tile.RightNeighbour.PipeColour = connectedColour;
                        connectingCells.Add(tile.RightNeighbour);
                    }
                }
                //Check for top neighbour tile
                if (tile.TopNeighbour != null && !tile.TopNeighbour.Connected)
                {
                    if (tile.Up == true && tile.TopNeighbour.Down == true)
                    {
                        tile.TopNeighbour.Connected = true;                        
                        tile.TopNeighbour.PipeColour = connectedColour;
                        connectingCells.Add(tile.TopNeighbour);
                    }
                }
                //Check for bottom neighbour tile
                if (tile.BottomNeighbour != null && !tile.BottomNeighbour.Connected)
                {
                    if (tile.Down == true && tile.BottomNeighbour.Up == true)
                    {
                        tile.BottomNeighbour.Connected = true;                        
                        tile.BottomNeighbour.PipeColour = connectedColour;
                        connectingCells.Add(tile.BottomNeighbour);
                    }
                }
            }
        }

        /// <summary>
        /// Check for victory. If all the end tiles are connected, user has won the game.
        /// </summary>
        /// <returns>Return true if the user has solved the game</returns>
        public bool isVictory()
        {
            for (int i = 0; i < endTiles.Count; i++)
                if (!((PipeGame_Tile)endTiles[i]).Connected)
                    return false;
            return true;
        }

        /// <summary>
        /// Update the score if the user has won the game. Show the score and let the user to quit the game.
        /// </summary>
        public void checkVictory()
        {
            if (isVictory())
            {
                score -= numSecond;
                if (score < 0)
                    score = 0;
                else if (score > 100)
                    score = 100;
                updateScore();
                TPEngine.Get().State.PushState(new ConfirmationPopup("You have won!" + Environment.NewLine + "Your score: " + score, true, true), true);
            }
        }

        /// <summary>
        /// Rotate the tile if the user clicks on a tile
        /// </summary>
        public PipeGame_Tile mouseClick(int x, int y)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (x >= tiles[i].Position.X && x <= (tiles[i].Position.X + tileWidth))
                    if (y >= tiles[i].Position.Y && y <= (tiles[i].Position.Y + tileHeight))
                    {
                        tiles[i].rotate();
                        updateConnection();
                        return tiles[i];
                    }
            }
            return null;
        }

        /// <summary>
        /// Indicate the tile by changing the border colour of the tile if the mouse pointer is on it.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void mouseOver(int x, int y)
        {
            for(int i = 0; i < tiles.Length; i++){
                tiles[i].BorderColour = borderColour;
                if (x > tiles[i].Position.X && x < (tiles[i].Position.X + tileWidth))
                    if (y > tiles[i].Position.Y && y < (tiles[i].Position.Y + tileHeight))
                    {
                        tiles[i].BorderColour = selectedBorderColour;
                    }
            }
        }

        /// <summary>
        /// Update the score to the database via the web server.
        /// </summary>
        private void updateScore()
        {
            User user = GameLauncher_LoginButtonSprite.getUser();
            if (user == null || user.id == 0)
                return;
            DBCommServiceClient server = new DBCommServiceClient();
            MinigameScore minigame = new MinigameScore();
            Character character = server.LoadCharacterData(user);
            minigame.CharacterID = character.id;
            minigame.CharacterName = character.name;
            minigame.Lecture_Attended = false;
            minigame.Score = score;
            minigame.MinigameID = 2;
            server.SaveMinigameScore(user, minigame);
        }
    }
}