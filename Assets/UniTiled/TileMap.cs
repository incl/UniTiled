﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UniTiled.Data;
using UniTiled.Util;

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
            CreateMesh();
        }

        public void LoadTMX(TextAsset tmx)
        {
            tilesets = new List<Tileset>();
            layers = new List<Layer>();
            objectGroups = new List<ObjectGroup>();

            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(tmx.text)); 

            int layerIdx = 0;
            XmlNodeList nodeList = doc.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                switch (node.Name)
                {
                    case "tileset":
                        CreateTileset(node);
                        break;
                    case "layer":
                        CreateLayer(node, layerIdx);
                        break;
                    case "objectgroup":
                        CreateObjectGroup(node);
                        break;
                }
            }
        }

        public void CreateMesh()
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<int> triangles = new List<int>();

            foreach (Layer layer in layers)
            {
                int verticesCount = vertices.Count;
                vertices.AddRange(layer.CreateVertices());
                uv.AddRange(layer.CreateUV());
                triangles.AddRange(layer.CreateTriangles(verticesCount, vertices.Count));
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = triangles.ToArray();

            MeshFilter filter = GetComponent<MeshFilter>();
            filter.mesh = mesh;
            
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            Material[] materials = {
                Resources.Load<Material>("TestMat")
            };
            renderer.materials = materials;
        }

        public Tileset FindTilesetByGID(string GID)
        {
            return tilesets[0];
        }

        public void CreateTileset(XmlNode node)
        {
            Tileset tileset = new Tileset();
            
            tileset.firstGID = XmlHelper.GetAttribute<int>(node, "firstgid", 0);
            tileset.tileWidth = XmlHelper.GetAttribute<int>(node, "tilewidth", 0);
            tileset.tileHeight = XmlHelper.GetAttribute<int>(node, "tileheight", 0);
            tileset.spacing = XmlHelper.GetAttribute<int>(node, "spacing", 0);
            tileset.margin = XmlHelper.GetAttribute<int>(node, "margin", 0);
            
            XmlNode imageNode = node.SelectSingleNode("image");
            tileset.imageSource = XmlHelper.GetAttribute<string>(imageNode, "source", "");
            tileset.imageWidth = XmlHelper.GetAttribute<int>(imageNode, "width", 0);
            tileset.imageHeight = XmlHelper.GetAttribute<int>(imageNode, "height", 0);

            tilesets.Add(tileset);
        }

        public void CreateLayer(XmlNode node, int layerIdx)
        {
            Layer layer = new Layer();

            layer.tileMap = this;
            layer.index = layerIdx;
            layer.name = XmlHelper.GetAttribute<string>(node, "name", "");
            layer.width = XmlHelper.GetAttribute<int>(node, "width", 0);
            layer.height = XmlHelper.GetAttribute<int>(node, "height", 0);

            XmlNode dataNode = node.SelectSingleNode("data");
            layer.data = dataNode.InnerText.Split(',');

            layers.Add(layer);
        }

        public void CreateObjectGroup(XmlNode node)
        {
            ObjectGroup objectGroup = new ObjectGroup();

            objectGroups.Add(objectGroup);
        }
    }
}