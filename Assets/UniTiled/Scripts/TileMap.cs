using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UniTiled.Util;

namespace UniTiled
{
    public class TileMap : MonoBehaviour
    {
        public static string OrientalOrthogonal = "orthogonal";
        public static string ISOMetric = "isometric";

        [Range(1, 100)]
        public int pixelsToUnits;
        public TextAsset tmx;
        private string orientation;
        private int width;
        private int height;
        private int tileWidth;
        private int tileHeight;
        private List<Tileset> tilesets;
        private List<Layer> layers;
        private List<ObjectGroup> objectGroups;

        void Start()
        {
            LoadTMX(tmx);
            Build();
        }

        public void LoadTMX(TextAsset tmx)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(tmx.text));

            XmlNode mapNode = doc.SelectSingleNode("map");
            orientation = XmlHelper.GetAttribute<string>(mapNode, "orientation", OrientalOrthogonal);
            width = XmlHelper.GetAttribute<int>(mapNode, "width", 0);
            height = XmlHelper.GetAttribute<int>(mapNode, "width", 0);
            tileWidth = XmlHelper.GetAttribute<int>(mapNode, "tilewidth", 0);
            tileHeight = XmlHelper.GetAttribute<int>(mapNode, "tileheight", 0);

            tilesets = new List<Tileset>();
            layers = new List<Layer>();
            objectGroups = new List<ObjectGroup>();

            XmlNodeList nodeList = doc.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                switch (node.Name)
                {
                    case "tileset":
                        CreateTileset(node);
                        break;
                    case "layer":
                        CreateLayer(node);
                        break;
                    case "objectgroup":
                        CreateObjectGroup(node);
                        break;
                }
            }
        }

        public void Build()
        {
            foreach (Layer layer in layers)
            {
                layer.Build();
            }
            foreach (Tileset tileset in tilesets)
            {
                tileset.Build();
            }
        }

        public Tileset FindTilesetByGID(int GID)
        {
            Tileset tileset = tilesets[0];
            foreach (Tileset t in tilesets)
            {
                if (tileset.FirstGID < t.FirstGID
                    && t.FirstGID < GID)
                    tileset = t;
            }
            return tileset;
        }

        public void CreateTileset(XmlNode node)
        {
            var firstGID = XmlHelper.GetAttribute<int>(node, "firstgid", 0);
            var tileWidth = XmlHelper.GetAttribute<int>(node, "tilewidth", 0);
            var tileHeight = XmlHelper.GetAttribute<int>(node, "tileheight", 0);
            var spacing = XmlHelper.GetAttribute<int>(node, "spacing", 0);
            var margin = XmlHelper.GetAttribute<int>(node, "margin", 0);
            
            XmlNode imageNode = node.SelectSingleNode("image");
            var imageSource = XmlHelper.GetAttribute<string>(imageNode, "source", "");
            imageSource = imageSource.Substring(0, imageSource.IndexOf("."));
            var imageWidth = XmlHelper.GetAttribute<int>(imageNode, "width", 0);
            var imageHeight = XmlHelper.GetAttribute<int>(imageNode, "height", 0);

            var obj = new GameObject("Tileset (" + imageSource + ")");
            obj.transform.parent = transform;
            Tileset tileset = obj.AddComponent<Tileset>();
            tileset.SetData(imageSource, imageWidth, imageHeight, 
                            firstGID, tileWidth, tileHeight, 
                            spacing, margin);
            tilesets.Add(tileset);
        }

        public void CreateLayer(XmlNode node)
        {
            Layer layer = new Layer();

            layer.tileMap = this;
            layer.index = layers.Count;
            layer.name = XmlHelper.GetAttribute<string>(node, "name", "");
            layer.width = XmlHelper.GetAttribute<int>(node, "width", 0);
            layer.height = XmlHelper.GetAttribute<int>(node, "height", 0);

            XmlNode dataNode = node.SelectSingleNode("data");
            string[] data = dataNode.InnerText.Split(',');
            layer.data = Array.ConvertAll<string, int>(data, int.Parse);

            layers.Add(layer);
        }

        public void CreateObjectGroup(XmlNode node)
        {
            ObjectGroup objectGroup = new ObjectGroup();

            objectGroups.Add(objectGroup);
        }
    }
}