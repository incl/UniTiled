using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniTiled
{
    public class Layer
    {
        public TileMap tileMap;
        public string name;
        public int index;
        public int width;
        public int height;
        public int[] data;

        public void Build()
        {
            float z = -index;
            int dataIdx = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int GID = data[dataIdx];
                    if (GID == 0)
                    {
                        dataIdx++;
                        continue;
                    }

                    Tileset tileset = tileMap.FindTilesetByGID(GID);

                    int verticesCount = tileset.VerticesCount;

                    float w = (float)tileset.TileWidth / (float)tileMap.pixelsToUnits;
                    float h = (float)tileset.TileHeight / (float)tileMap.pixelsToUnits;
                    tileset.AddVertices(new List<Vector3>() {
                        new Vector3(w * x, h * -y, z),
                        new Vector3(w * x, h * (-y + 1), z),
                        new Vector3(w * (x + 1), h * (-y + 1), z),
                        new Vector3(w * (x + 1), h * -y, z)
                    });

                    int idx = GID - tileset.FirstGID;
                    int posY = idx / tileset.Rows;
                    int posX = idx % tileset.Cols;
                    float u = ((tileset.CellWidth + tileset.SpacingWidth) * posX) + tileset.SpacingWidth / 2f;
                    float v = 1f - ((tileset.CellHeight + tileset.SpacingHeight) * posY) - tileset.SpacingHeight / 2f;
//                    float u = tileset.MarginWidth + ((tileset.CellWidth + tileset.SpacingWidth) * posX);
//                    float v = 1f - tileset.MarginHeight + ((tileset.CellHeight + tileset.SpacingHeight) * posY);
                    tileset.AddUV(new List<Vector2>() {
                        new Vector2(u, v - tileset.CellHeight),
                        new Vector2(u, v),
                        new Vector2(u + tileset.CellWidth, v),
                        new Vector2(u + tileset.CellWidth, v - tileset.CellHeight)
                    });

                    for (int i = verticesCount; i < tileset.VerticesCount; i += 4)
                    {
                        tileset.AddTriangles(new List<int>() {
                            i, i + 1, i + 2,
                            i, i + 2, i + 3,
                        });
                    }

                    dataIdx++;
                }
            }
        }
    }
}