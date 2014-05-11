using UnityEngine;
using System.Collections;

namespace UniTiled.Data
{
    public class Tileset
    {
        public string imageSource;
        public float imageWidth;
        public float imageHeight;
        public int firstGID;
        public float tileWidth;
        public float tileHeight;
        public float spacing;
        public float margin;

        public string ToString()
        {
            return string.Format("[Tileset] {0}", imageSource);
        }
    }
}