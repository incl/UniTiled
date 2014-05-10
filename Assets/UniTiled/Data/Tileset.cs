using UnityEngine;
using System.Collections;

namespace UniTiled.Data
{
    public class Tileset
    {
        public string imageSource;
        public int imageWidth;
        public int imageHeight;
        public int firstGID;
        public int tileWidth;
        public int tileHeight;
        public int spacing;
        public int margin;

        public string ToString()
        {
            return string.Format("[Tileset] {0}", imageSource);
        }
    }
}