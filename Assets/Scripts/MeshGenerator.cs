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

		private void CreateVertex(Vector3 position) {
			vertices.Add (position);
		}

		private void CreateTriangle(int i0, int i1, int i2) {
			triangles.Add (i0);
			triangles.Add (i1);
			triangles.Add (i2);
		}

		private void CreateUvs(Vector2 i0, Vector2 i1, Vector2 i2, Vector2 i3) {
			uvs.Add (i0);
			uvs.Add (i1);
			uvs.Add (i2);
			uvs.Add (i3);
		}

		private void TriangulateQuad(Mesh mesh, Square square) {
			int configuration = square.GetConfiguration ();
			int spriteIndex = Mathf.Max (0, configuration);

			Sprite sprite = spriteTiles [spriteIndex];

			if (configuration != 0) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex(square.TopLeft.GetPosition());

				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex(square.TopRight.GetPosition());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex(square.BottomRight.GetPosition());

				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex(square.BottomLeft.GetPosition());

				CreateTriangle(square.TopLeft.vertexIndex, square.BottomRight.vertexIndex, square.TopRight.vertexIndex);
				CreateTriangle(square.TopLeft.vertexIndex, square.BottomLeft.vertexIndex, square.BottomRight.vertexIndex);

				switch (configuration) {
				case 1:
						CreateUvs (sprite.uv [3], sprite.uv [1], sprite.uv [0], sprite.uv [2]);
						break;
					case 2:
						CreateUvs (sprite.uv [0], sprite.uv [2], sprite.uv [3], sprite.uv [1]);
						break;
					case 3:
						CreateUvs (sprite.uv [2], sprite.uv [3], sprite.uv [1], sprite.uv [0]);
						break;
					case 4:
						CreateUvs (sprite.uv [0], sprite.uv [2], sprite.uv [3], sprite.uv [1]);
						break;
					case 6:
						CreateUvs (sprite.uv [0], sprite.uv [2], sprite.uv [3], sprite.uv [1]);
						break;
					case 7:
						CreateUvs (sprite.uv [0], sprite.uv [2], sprite.uv [3], sprite.uv [1]);
						break;
					case 8:
						CreateUvs (sprite.uv [3], sprite.uv [1], sprite.uv [0], sprite.uv [2]);
						break;
					case 9:
						CreateUvs (sprite.uv [3], sprite.uv [1], sprite.uv [0], sprite.uv [2]);
						break;
					case 11:
						CreateUvs (sprite.uv [2], sprite.uv [3], sprite.uv [1], sprite.uv [0]);
						break;
					case 12:
						CreateUvs (sprite.uv [1], sprite.uv [0], sprite.uv [2], sprite.uv [3]);
						break;
					case 13:
						CreateUvs (sprite.uv [3], sprite.uv [1], sprite.uv [0], sprite.uv [2]);
						break;
					default:
						CreateUvs (sprite.uv [1], sprite.uv [0], sprite.uv [2], sprite.uv [3]);
						break;
				}
			}
		}
	}
}