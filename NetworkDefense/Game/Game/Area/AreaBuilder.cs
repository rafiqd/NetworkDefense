/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Game.Sprites;
using Game.States;
using Microsoft.Xna.Framework;
using Engine.Objects;

namespace Game.Area
{
    /// <summary>
    /// AreaBuilder converts text files to maps for the main game
    /// </summary>
    static class AreaBuilder
    {
        /// <summary>
        /// BuildArea takes a string, loads the map of that name, and builds the area for the main game.
        /// </summary>
        /// <param name="layers">The layers that game objects are drawn in</param>
        /// <param name="areaName">The name of the text file to load</param>
        /// <returns>The data giving a detailed renderable description of the area</returns>
        public static AreaStateData BuildArea(TPLayerList layers, string areaName)
        {

            AreaStateData data = new AreaStateData();
            data.areaInfo.areaName = areaName;

            // Read in the text file data and instanciate the layers accordingly
            data.areaInfo = FileReader.GetAreaInfo(areaName);
            data.areaInfo.tileSearchArray = new TileSprite[data.areaInfo.areaY][];
            int maxJ = data.areaInfo.areaX + data.areaInfo.areaY;
            data.areaLayers = new TPLayer[maxJ + 1];
            for (int j = 0; j < maxJ; j++)
            {
                data.areaLayers[j] = new TPLayer(layers);
            }

            // Create a player and add the room backdrop.
            data.playerSprite = new PlayerSprite(data.areaInfo, data.areaLayers);
            TPSprite roomBackdrop = new IsometricSprite(StartPositions.Get().GetPosition(areaName), areaName, data.playerSprite, new Point());
            data.areaLayers[0].AddEntity(roomBackdrop);

            // Loop through the characters in the text file and add world objects according to the character values.
            for (int y = 0; y < data.areaInfo.areaY; y++)
            {
                data.areaInfo.tileSearchArray[y] = new TileSprite[data.areaInfo.areaX];
                for (int x = 0; x < data.areaInfo.areaX; x++)
                {
                    int layerNum = x + y;
                    Vector2 pos = new Vector2(476 - (layerNum) * 62 + x * 154 + y * 7, 105 + (layerNum) * 33 + y * 25 + x * 2);
                    TileSprite tileSprite;

                    switch (data.areaInfo.tileTypeArray[y][x])
                    {
                        case ('q'):
                            data.areaLayers[0].AddEntity(new IsometricSprite(pos + new Vector2(0, -200), "white_board", data.playerSprite, new Point(x, y)));
                            tileSprite = new TileSprite(pos, data.playerSprite, new Point(y, x));
                            data.areaInfo.tileTypeArray[y][x] = 't';
                            data.areaInfo.tileSearchArray[y][x] = tileSprite;
                            break;
                        case ('c'):
                            data.areaLayers[x + y + 1].AddEntity(new IsometricSprite(pos + new Vector2(-115, -160), "chair_toscale", data.playerSprite, new Point(x, y)));
                            break;
                        case ('g'):
                            data.areaLayers[x + y + 1].AddEntity(new IsometricSprite(pos + new Vector2(-115, -160), "chair_w_dude_ne", data.playerSprite, new Point(x, y)));
                            break;
                        case ('t'):
                            tileSprite = new TileSprite(pos, data.playerSprite, new Point(y, x));
                            tileSprite.Alive = false;
                            //data.areaLayers[0].AddEntity(tileSprite);
                            data.areaInfo.tileTypeArray[y][x] = 't';
                            data.areaInfo.tileSearchArray[y][x] = tileSprite;
                            break;
                        case ('d'):
                            //tileSprite = new DemoTileSprite(pos, playerSprite);
                            //tileSprite.Alive = false;
                            //areaLayers[0].AddEntity(tileSprite);
                            switch (areaName)
                            {
                                case "lab":
                                    data.areaLayers[x + y + 1].AddEntity(new LabDeskSprite(pos + new Vector2(-135, -175), data.playerSprite, new Point(x, y), true, data.areaName));
                                    break;
                                case "lecturehall":
                                    data.areaLayers[x + y + 1].AddEntity(new LectureDeskSprite(pos + new Vector2(-135, -175), data.playerSprite, new Point(x, y), true, data.areaName));
                                    break;
                                default:
                                    data.areaLayers[x + y + 1].AddEntity(new StudyDeskSprite(pos + new Vector2(-135, -175), data.playerSprite, new Point(x, y), true, data.areaName));
                                    break;
                            }
                            break;
                        case ('b'):
                            //tileSprite = new DemoTileSprite(pos, playerSprite);
                            //tileSprite.Alive = false;
                            //areaLayers[0].AddEntity(tileSprite);
                            data.areaLayers[x + y + 1].AddEntity(new BedSprite(pos + new Vector2(-135, -95), data.playerSprite, new Point(x, y), data.areaName));
                            break;
                        case ('v'):
                            data.areaLayers[x + y + 1].AddEntity(new VendorSprite(pos + new Vector2(-135, -185), data.playerSprite, new Point(x, y), data.areaName));
                            break;
                        case ('x'):
                        case ('X'):
                            tileSprite = new TileSprite(pos, data.playerSprite, new Point(y, x));
                            tileSprite.Alive = false;
                            //areaLayers[0].AddEntity(tileSprite);
                            break;
                        case ('w'):
                            tileSprite = new TileSprite(pos, data.playerSprite, new Point(y, x));
                            data.areaInfo.tileTypeArray[y][x] = 't';
                            data.areaInfo.tileSearchArray[y][x] = tileSprite;
                            //data.areaLayers[0].AddEntity(tileSprite);
                            string s = data.areaInfo.targets[0];
                            data.areaInfo.targets.Remove(s);
                            data.areaLayers[0].AddEntity(new DoorSouthSprite(pos + new Vector2(-170, -115), data.playerSprite, new Point(x, y - 1), s, areaName));
                            if (areaName != "hall")
                            {
                                data.playerSprite.Load(pos + new Vector2(25, -140), x + y + 1, new Point(x, y));
                                data.areaLayers[x + y + 1].AddEntity(data.playerSprite);
                            }
                            break;
                        case ('e'):
                            tileSprite = new TileSprite(pos, data.playerSprite, new Point(y, x));
                            data.areaInfo.tileTypeArray[y][x] = 't';
                            data.areaInfo.tileSearchArray[y][x] = tileSprite;
                            //data.areaLayers[0].AddEntity(tileSprite);
                            data.areaLayers[0].AddEntity(new DoorEastSprite(pos + new Vector2(-170, -115), data.playerSprite, new Point(x, y - 1), "null", areaName));
                            if (areaName != "hall")
                            {
                                data.playerSprite.Load(pos + new Vector2(25, -140), x + y + 1, new Point(x, y));
                                data.areaLayers[x + y + 1].AddEntity(data.playerSprite);
                            }
                            break;
                        case ('p'):
                            if (areaName == "hall" && AreaState.IsSpecialPosition)
                            {
                                Point start = StartPositions.Get().GetPoint(AreaState.playerRoom);
                                int layerNum2 = start.X + start.Y;
                                Vector2 pos2 = new Vector2(476 - (layerNum2) * 62 + start.X * 154 + start.Y * 7, 105 + (layerNum2) * 33 + start.Y * 25 + start.X * 2);
                                data.playerSprite.Load(pos2 + new Vector2(25, -140), start.X + start.Y + 1, start);
                            }
                            else
                            {
                                data.playerSprite.Load(pos + new Vector2(25, -140), x + y + 1, new Point(x, y));
                            }
                            tileSprite = new TileSprite(pos, data.playerSprite, new Point(y, x));
                            tileSprite.Alive = false;
                            //data.areaLayers[0].AddEntity(tileSprite);
                            data.areaLayers[x + y + 1].AddEntity(data.playerSprite);
                            data.areaInfo.tileTypeArray[y][x] = 't';
                            data.areaInfo.tileSearchArray[y][x] = tileSprite;
                            break;
                    }
                }
            }

            // Return the area
            return data;
        }
    }
}
