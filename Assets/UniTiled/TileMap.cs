using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UniTiled.Data;

namespace UniTiled
{
    [RequireComponent(typeof(MeshRenderer))]
    public class TileMap : MonoBehaviour
    {
        private List<Tileset> tilesets;
        private List<Layer> layers;
        private List<ObjectGroup> objectGroups;

        void Start()
        {
            LoadTMX(Resources.Load<TextAsset>("TestMap"));
        }

        public void LoadTMX(TextAsset tmx)
        {
            tilesets = new List<Tileset>();
            layers = new List<Layer>();
            objectGroups = new List<ObjectGroup>();

            CreateMesh(tmx);
        }

        public Mesh CreateMesh(TextAsset tmx)
        {
            Mesh mesh = new Mesh();
            int usedVertices = 0;
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<int> triangles = new List<int>();

            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(tmx.text)); 

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
            
            mesh.vertices = vertices.ToArray ();
            mesh.uv = uv.ToArray ();
            mesh.triangles = triangles.ToArray ();
            return mesh;
        }

        public string GetStringAttr(XmlNode node, string name, string defaultValue)
        {
            if (node.Attributes[name] == null)
                return defaultValue;

            return node.Attributes[name].InnerText;
        }
        
        public int GetIntAttr(XmlNode node, string name, int defaultValue)
        {
            if (node.Attributes[name] == null)
                return defaultValue;
            
            return int.Parse(node.Attributes[name].InnerText);
        }

        public void CreateTileset(XmlNode node)
        {
            Tileset tileset = new Tileset();
            
            XmlNode imageNode = node.SelectSingleNode("image");
            tileset.imageSource = GetStringAttr(imageNode, "source", "");
            tileset.imageWidth = GetIntAttr(imageNode, "width", 0);
            tileset.imageHeight = GetIntAttr(imageNode, "height", 0);
            
            tileset.firstGID = GetIntAttr(node, "firstgid", 0);
            tileset.tileWidth = GetIntAttr(node, "tilewidth", 0);
            tileset.tileHeight = GetIntAttr(node, "tileheight", 0);
            tileset.spacing = GetIntAttr(node, "spacing", 0);
            tileset.margin = GetIntAttr(node, "margin", 0);

            tilesets.Add(tileset);
        }

        public void CreateLayer(XmlNode node)
        {
        }

        public void CreateObjectGroup(XmlNode node)
        {
        }
    }
}