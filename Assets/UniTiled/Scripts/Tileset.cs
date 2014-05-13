using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniTiled
{
    public class Tileset : MonoBehaviour
    {
        private string imageSource;
        private int imageWidth;
        private int imageHeight;
        private int firstGID;
        private int tileWidth;
        private int tileHeight;
        private int spacing;
        private int margin;
        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector2> uv = new List<Vector2>();
        private List<int> triangles = new List<int>();

#region properties
        public int FirstGID
        {
            get { return firstGID; }
            set { firstGID = value; }
        }
        
        public int TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = value; }
        }
        
        public int TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        public int Cols
        {
            get { return imageWidth / (tileWidth + spacing); }
        }

        public int Rows
        {
            get { return imageHeight / (tileHeight + spacing); }
        }

        public float CellWidth
        {
            get { return (float)tileWidth / (float)imageWidth; }
        }

        public float CellHeight
        {
            get { return (float)tileHeight / (float)imageHeight; }
        }
        
        public float SpacingWidth
        {
            get { return (float)spacing / (float)imageWidth; }
        }
        
        public float SpacingHeight
        {
            get { return (float)spacing / (float)imageHeight; }
        }
        
        public float MarginWidth
        {
            get { return (float)margin / (float)imageWidth; }
        }
        
        public float MarginHeight
        {
            get { return (float)margin / (float)imageHeight; }
        }

        public int VerticesCount
        {
            get { return vertices.Count; }
        }
#endregion

        public void SetData(string imageSource,
                            int imageWidth,
                            int imageHeight,
                            int firstGID,
                            int tileWidth,
                            int tileHeight,
                            int spacing,
                            int margin)
        {
            this.imageSource = imageSource;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.firstGID = firstGID;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.spacing = spacing;
            this.margin = margin;

            gameObject.AddComponent<MeshFilter>();
            
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.materials = new Material[] { Resources.Load<Material>(imageSource) };
        }
        
        public void AddVertices(List<Vector3> vertices)
        {
            this.vertices.AddRange(vertices);
        }
        
        public void AddUV(List<Vector2> uv)
        {
            this.uv.AddRange(uv);
        }
        
        public void AddTriangles(List<int> triangles)
        {
            this.triangles.AddRange(triangles);
        }

        public void Clear()
        {
        }

        public void Build()
        {
            Mesh mesh = new Mesh();
            mesh.name = "TiledMesh";
            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = triangles.ToArray();

            MeshFilter filter = GetComponent<MeshFilter>();
            filter.mesh = mesh;
        }
    }
}