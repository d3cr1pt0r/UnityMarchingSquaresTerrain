using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator {
	
	public class MeshGenerator {

		private SquareGrid squareGrid = null;

		private Mesh mesh = null;
		private List<Vector3> vertices = new List<Vector3> (0);
		private List<Vector2> uvs = new List<Vector2> (0);
		private List<int> triangles = new List<int> (0);

		private Sprite[] spriteTiles;

		public MeshGenerator(Map map) {
			squareGrid = new SquareGrid (map);
		}

		public Mesh GenerateMesh(Sprite[] spriteTiles) {
			mesh = new Mesh ();
			this.spriteTiles = spriteTiles;

			vertices = new List<Vector3> ();
			uvs = new List<Vector2> ();
			triangles = new List<int> ();

			for (int y = 0; y < squareGrid.height; y++) {
				for (int x = 0; x < squareGrid.width; x++) {
					TriangulateQuad (mesh, squareGrid.GetSquare (x, y));
				}
			}

			mesh.vertices = vertices.ToArray ();
			mesh.uv = uvs.ToArray ();
			mesh.triangles = triangles.ToArray ();

			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();

			return mesh;
		}

		public SquareGrid GetSquareGrid() {
			return squareGrid;
		}

		private void TriangulateQuad(Mesh mesh, Square square) {
			int configuration = square.GetConfiguration ();
			int spriteIndex = Mathf.Max (0, configuration);

			Sprite sprite = spriteTiles [spriteIndex];

			if (configuration != 0) {
				// top left
				square.TopLeft.vertexIndex = vertices.Count;
				vertices.Add (square.TopLeft.GetPosition());

				// top right
				square.TopRight.vertexIndex = vertices.Count;
				vertices.Add (square.TopRight.GetPosition());

				// bottom right
				square.BottomRight.vertexIndex = vertices.Count;
				vertices.Add (square.BottomRight.GetPosition());

				// bottom left
				square.BottomLeft.vertexIndex = vertices.Count;
				vertices.Add (square.BottomLeft.GetPosition());

				// right triangle
				triangles.Add (square.TopLeft.vertexIndex);
				triangles.Add (square.BottomRight.vertexIndex);
				triangles.Add (square.TopRight.vertexIndex);

				// left triangle
				triangles.Add (square.TopLeft.vertexIndex);
				triangles.Add (square.BottomLeft.vertexIndex);
				triangles.Add (square.BottomRight.vertexIndex);

				switch (configuration) {
					case 1:
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						break;
					case 2:
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						break;
					case 3:
						uvs.Add (sprite.uv [2]);
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						uvs.Add (sprite.uv [0]);
						break;
					case 4:
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						break;
					case 6:
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						break;
					case 7:
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						break;
					case 8:
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						break;
					case 9:
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						break;
					case 11:
						uvs.Add (sprite.uv [2]);
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						uvs.Add (sprite.uv [0]);
						break;
					case 12:
						uvs.Add (sprite.uv [1]);
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						uvs.Add (sprite.uv [3]);
						break;
					case 13:
						uvs.Add (sprite.uv [3]);
						uvs.Add (sprite.uv [1]);
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						break;
					default:
						uvs.Add (sprite.uv [1]);
						uvs.Add (sprite.uv [0]);
						uvs.Add (sprite.uv [2]);
						uvs.Add (sprite.uv [3]);
						break;
				}
			}
		}
	}
}