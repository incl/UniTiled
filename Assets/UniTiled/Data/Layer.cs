using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniTiled.Data
{
    public class Layer
    {
        public TileMap tileMap;
        public string name;
        public int index;
        public int width;
        public int height;
        public string[] data;

        public List<Vector3> CreateVertices()
        {
            var vertices = new List<Vector3>();

            float z = index * 0.1f;
            int dataIdx = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    string GID = data[dataIdx].ToString().Trim();
                    if (GID == "0")
                    {
                        dataIdx++;
                        continue;
                    }

                    Tileset tileset = tileMap.FindTilesetByGID(GID);
                    float w = tileset.tileWidth / (float)tileMap.pixelsToUnits;
                    float h = tileset.tileHeight / (float)tileMap.pixelsToUnits;


                    vertices.Add(new Vector3(w * x, h * -y, z));
                    vertices.Add(new Vector3(w * x, h * (-y + 1), z));
                    vertices.Add(new Vector3(w * (x + 1), h * (-y + 1), z));
                    vertices.Add(new Vector3(w * (x + 1), h * -y, z));

                    dataIdx++;
                }
            }

            return vertices;
        }
        
        public List<Vector2> CreateUV()
        {
            var uv = new List<Vector2>();

            Tileset tileset = tileMap.FindTilesetByGID("0");
            int cols = (int)(tileset.imageWidth / (tileset.tileWidth + tileset.spacing));
            int rows = (int)(tileset.imageHeight / (tileset.tileHeight + tileset.spacing));
            float cellWidth = ((float)tileset.tileWidth / tileset.imageWidth);
            float cellHeight = ((float)tileset.tileHeight / tileset.imageHeight);      
            float spacingWidth = ((float)tileset.spacing / tileset.imageWidth);
            float spacingHeight = ((float)tileset.spacing / tileset.imageHeight);
            int totalCells = width * height;
            int GID;
            for (int i = 0; i < totalCells; i++)
            {
                GID = int.Parse(data[i].ToString().Trim());
                if (GID != 0)
                {
                    GID = GID - tileset.firstGID;
                    int posY = GID / rows;
                    int posX = GID % cols;                     
                    float u = ((cellWidth + spacingWidth) * posX) + spacingWidth / 2;
                    float v = 1.0f - ((cellHeight + spacingHeight) * posY) - spacingHeight / 2;             
                    uv.AddRange(new Vector2[] {
                        new Vector2(u, v - cellHeight),
                        new Vector2(u, v),
                        new Vector2(u + cellWidth, v),
                        new Vector2(u + cellWidth, v - cellHeight),                    
                    });
                }
            }

            return uv;
        }
        
        public List<int> CreateTriangles(int begin, int end)
        {
            var triangles = new List<int>();

            for (int i = begin; i < end; i += 4)
            {
                triangles.AddRange(new int[] {
                    i, i + 1, i + 2,
                    i, i + 2, i + 3,
                });
            }

            return triangles;
        }
    }
}