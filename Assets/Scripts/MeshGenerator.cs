using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
	
	public class MeshGenerator
	{
		enum CornerSide {
			TopLeft,
			TopRight,
			BottomRight,
			BottomLeft,
		}

		private SquareGrid squareGrid = null;

		private Mesh mesh = null;
		private List<Vector3> vertices = new List<Vector3> (0);
		private List<Vector2> uvs = new List<Vector2> (0);
		private List<int> triangles = new List<int> (0);

		private Sprite[] spriteTiles;

		public MeshGenerator (Map map)
		{
			squareGrid = new SquareGrid (map);
		}

		public Mesh GenerateMesh (Sprite[] spriteTiles)
		{
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

		public Mesh GenerateMeshRounded (Sprite[] spriteTiles)
		{
			mesh = new Mesh ();
			this.spriteTiles = spriteTiles;

			vertices = new List<Vector3> ();
			uvs = new List<Vector2> ();
			triangles = new List<int> ();

			for (int y = 0; y < squareGrid.height; y++) {
				for (int x = 0; x < squareGrid.width; x++) {
					TriangulateQuadRounded (mesh, squareGrid.GetSquare (x, y));
				}
			}

			mesh.vertices = vertices.ToArray ();
			mesh.uv = uvs.ToArray ();
			mesh.triangles = triangles.ToArray ();

			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();

			return mesh;
		}

		public SquareGrid GetSquareGrid ()
		{
			return squareGrid;
		}

		private void CreateVertex (Vector3 position)
		{
			vertices.Add (position);
		}

		private void CreateTriangle (int i0, int i1, int i2)
		{
			triangles.Add (i0);
			triangles.Add (i1);
			triangles.Add (i2);
		}

		private void CreateUvs (params Vector2[] uv)
		{
			for (int i = 0; i < uv.Length; i++) {
				uvs.Add (uv [i]);
			}
		}

		private void TriangulateQuad (Mesh mesh, Square square)
		{
			int configuration = square.GetConfiguration ();
			int spriteIndex = Mathf.Max (0, configuration);

			Sprite sprite = spriteTiles [spriteIndex];

			if (configuration != 0) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex (square.TopLeft.GetPosition ());

				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex (square.TopRight.GetPosition ());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex (square.BottomRight.GetPosition ());

				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex (square.BottomLeft.GetPosition ());

				CreateTriangle (square.TopLeft.vertexIndex, square.TopRight.vertexIndex, square.BottomRight.vertexIndex);
				CreateTriangle (square.TopLeft.vertexIndex, square.BottomRight.vertexIndex, square.BottomLeft.vertexIndex);

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

		private void TriangulateQuadRounded (Mesh mesh, Square square)
		{
			int configuration = square.GetConfiguration ();

			if (configuration == 1) {
				CreateRoundCorner (square.TopLeft.GetPosition (), 0.5f, 11, CornerSide.BottomRight);
			}
			if (configuration == 2) {
				CreateRoundCorner (square.TopRight.GetPosition (), 0.5f, 11, CornerSide.BottomLeft);
			}
			if (configuration == 3) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex (square.TopLeft.GetPosition ());

				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex (square.TopRight.GetPosition ());

				square.LeftCenter.vertexIndex = vertices.Count;
				CreateVertex (square.LeftCenter.GetPosition ());

				square.RightCenter.vertexIndex = vertices.Count;
				CreateVertex (square.RightCenter.GetPosition ());

				CreateTriangle (square.TopLeft.vertexIndex, square.TopRight.vertexIndex, square.RightCenter.vertexIndex);
				CreateTriangle (square.TopLeft.vertexIndex, square.RightCenter.vertexIndex, square.LeftCenter.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
			}
			if (configuration == 4) {
				CreateRoundCorner (square.BottomRight.GetPosition (), 0.5f, 11, CornerSide.TopLeft);
			}
			if (configuration == 5) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex (square.TopLeft.GetPosition ());

				square.TopCenter.vertexIndex = vertices.Count;
				CreateVertex (square.TopCenter.GetPosition ());

				square.RightCenter.vertexIndex = vertices.Count;
				CreateVertex (square.RightCenter.GetPosition ());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex (square.BottomRight.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				square.LeftCenter.vertexIndex = vertices.Count;
				CreateVertex (square.LeftCenter.GetPosition ());

				CreateTriangle (square.TopLeft.vertexIndex, square.TopCenter.vertexIndex, square.LeftCenter.vertexIndex);
				CreateTriangle (square.RightCenter.vertexIndex, square.BottomRight.vertexIndex, square.BottomCenter.vertexIndex);
				CreateTriangle (square.TopCenter.vertexIndex, square.RightCenter.vertexIndex, square.LeftCenter.vertexIndex);
				CreateTriangle (square.LeftCenter.vertexIndex, square.RightCenter.vertexIndex, square.BottomCenter.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
			}
			if (configuration == 6) {
				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex (square.TopRight.GetPosition ());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex (square.BottomRight.GetPosition ());

				square.TopCenter.vertexIndex = vertices.Count;
				CreateVertex (square.TopCenter.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				CreateTriangle (square.TopCenter.vertexIndex, square.TopRight.vertexIndex, square.BottomRight.vertexIndex);
				CreateTriangle (square.TopCenter.vertexIndex, square.BottomRight.vertexIndex, square.BottomCenter.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
			}
			if (configuration == 7) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex (square.TopLeft.GetPosition ());

				square.TopCenter.vertexIndex = vertices.Count;
				CreateVertex (square.TopCenter.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex (square.BottomRight.GetPosition ());

				square.LeftCenter.vertexIndex = vertices.Count;
				CreateVertex (square.LeftCenter.GetPosition ());

				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex (square.TopRight.GetPosition ());

				square.RightCenter.vertexIndex = vertices.Count;
				CreateVertex (square.RightCenter.GetPosition ());

				square.vertexIndex = vertices.Count;
				CreateVertex (square.GetPosition ());

				CreateTriangle (square.TopLeft.vertexIndex, square.TopRight.vertexIndex, square.RightCenter.vertexIndex);
				CreateTriangle (square.TopLeft.vertexIndex, square.RightCenter.vertexIndex, square.LeftCenter.vertexIndex);
				CreateTriangle (square.vertexIndex, square.RightCenter.vertexIndex, square.BottomRight.vertexIndex);
				CreateTriangle (square.vertexIndex, square.BottomRight.vertexIndex, square.BottomCenter.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);

				List<Vector3> arcVertices = GetArcVertices (square.BottomLeft.GetPosition (), 0.5f, 11, CornerSide.TopRight);
				for (int i = 0; i < arcVertices.Count; i++) {
					CreateVertex (arcVertices [i]);
					CreateUvs (Vector2.zero);

					if (i > 0) {
						CreateTriangle (square.vertexIndex, vertices.Count - 2, vertices.Count - 1);
					}
				}
			}
			if (configuration == 8) {
				CreateRoundCorner (square.BottomLeft.GetPosition (), 0.5f, 11, CornerSide.TopRight);
			}
			if (configuration == 9) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex (square.TopLeft.GetPosition ());

				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex (square.BottomLeft.GetPosition ());

				square.TopCenter.vertexIndex = vertices.Count;
				CreateVertex (square.TopCenter.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				CreateTriangle (square.TopLeft.vertexIndex, square.TopCenter.vertexIndex, square.BottomCenter.vertexIndex);
				CreateTriangle (square.TopLeft.vertexIndex, square.BottomCenter.vertexIndex, square.BottomLeft.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
			}
			if (configuration == 10) {
				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex (square.TopRight.GetPosition ());

				square.TopCenter.vertexIndex = vertices.Count;
				CreateVertex (square.TopCenter.GetPosition ());

				square.RightCenter.vertexIndex = vertices.Count;
				CreateVertex (square.RightCenter.GetPosition ());

				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex (square.BottomLeft.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				square.LeftCenter.vertexIndex = vertices.Count;
				CreateVertex (square.LeftCenter.GetPosition ());

				CreateTriangle (square.TopCenter.vertexIndex, square.TopRight.vertexIndex, square.RightCenter.vertexIndex);
				CreateTriangle (square.LeftCenter.vertexIndex, square.BottomCenter.vertexIndex, square.BottomLeft.vertexIndex);
				CreateTriangle (square.TopCenter.vertexIndex, square.RightCenter.vertexIndex, square.LeftCenter.vertexIndex);
				CreateTriangle (square.LeftCenter.vertexIndex, square.RightCenter.vertexIndex, square.BottomCenter.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
			}
			if (configuration == 11) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex (square.TopLeft.GetPosition ());

				square.TopCenter.vertexIndex = vertices.Count;
				CreateVertex (square.TopCenter.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex (square.BottomLeft.GetPosition ());

				square.LeftCenter.vertexIndex = vertices.Count;
				CreateVertex (square.LeftCenter.GetPosition ());

				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex (square.TopRight.GetPosition ());

				square.RightCenter.vertexIndex = vertices.Count;
				CreateVertex (square.RightCenter.GetPosition ());

				square.vertexIndex = vertices.Count;
				CreateVertex (square.GetPosition ());

				CreateTriangle (square.TopLeft.vertexIndex, square.TopRight.vertexIndex, square.RightCenter.vertexIndex);
				CreateTriangle (square.TopLeft.vertexIndex, square.RightCenter.vertexIndex, square.LeftCenter.vertexIndex);
				CreateTriangle (square.LeftCenter.vertexIndex, square.vertexIndex, square.BottomCenter.vertexIndex);
				CreateTriangle (square.LeftCenter.vertexIndex, square.BottomCenter.vertexIndex, square.BottomLeft.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);

				List<Vector3> arcVertices = GetArcVertices (square.BottomRight.GetPosition (), 0.5f, 11, CornerSide.TopLeft);
				for (int i = 0; i < arcVertices.Count; i++) {
					CreateVertex (arcVertices [i]);
					CreateUvs (Vector2.zero);

					if (i > 0) {
						CreateTriangle (square.vertexIndex, vertices.Count - 2, vertices.Count - 1);
					}
				}
			}
			if (configuration == 12) {
				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex (square.BottomLeft.GetPosition ());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex (square.BottomRight.GetPosition ());

				square.LeftCenter.vertexIndex = vertices.Count;
				CreateVertex (square.LeftCenter.GetPosition ());

				square.RightCenter.vertexIndex = vertices.Count;
				CreateVertex (square.RightCenter.GetPosition ());

				CreateTriangle (square.LeftCenter.vertexIndex, square.BottomRight.vertexIndex, square.BottomLeft.vertexIndex);
				CreateTriangle (square.LeftCenter.vertexIndex, square.RightCenter.vertexIndex, square.BottomRight.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
			}
			if (configuration == 13) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex (square.TopLeft.GetPosition ());

				square.TopCenter.vertexIndex = vertices.Count;
				CreateVertex (square.TopCenter.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex (square.BottomLeft.GetPosition ());

				square.LeftCenter.vertexIndex = vertices.Count;
				CreateVertex (square.LeftCenter.GetPosition ());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex (square.BottomRight.GetPosition ());

				square.RightCenter.vertexIndex = vertices.Count;
				CreateVertex (square.RightCenter.GetPosition ());

				square.vertexIndex = vertices.Count;
				CreateVertex (square.GetPosition ());

				CreateTriangle (square.TopLeft.vertexIndex, square.TopCenter.vertexIndex, square.BottomCenter.vertexIndex);
				CreateTriangle (square.TopLeft.vertexIndex, square.BottomCenter.vertexIndex, square.BottomLeft.vertexIndex);
				CreateTriangle (square.vertexIndex, square.RightCenter.vertexIndex, square.BottomRight.vertexIndex);
				CreateTriangle (square.vertexIndex, square.BottomRight.vertexIndex, square.BottomCenter.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);

				List<Vector3> arcVertices = GetArcVertices (square.TopRight.GetPosition (), 0.5f, 11, CornerSide.BottomLeft);
				for (int i = 0; i < arcVertices.Count; i++) {
					CreateVertex (arcVertices [i]);
					CreateUvs (Vector2.zero);

					if (i > 0) {
						CreateTriangle (square.vertexIndex, vertices.Count - 2, vertices.Count - 1);
					}
				}
			}
			if (configuration == 14) {
				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex (square.TopRight.GetPosition ());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex (square.BottomRight.GetPosition ());

				square.TopCenter.vertexIndex = vertices.Count;
				CreateVertex (square.TopCenter.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				square.LeftCenter.vertexIndex = vertices.Count;
				CreateVertex (square.LeftCenter.GetPosition ());

				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex (square.BottomLeft.GetPosition ());

				square.BottomCenter.vertexIndex = vertices.Count;
				CreateVertex (square.BottomCenter.GetPosition ());

				square.vertexIndex = vertices.Count;
				CreateVertex (square.GetPosition ());

				CreateTriangle (square.TopCenter.vertexIndex, square.TopRight.vertexIndex, square.BottomRight.vertexIndex);
				CreateTriangle (square.TopCenter.vertexIndex, square.BottomRight.vertexIndex, square.BottomCenter.vertexIndex);
				CreateTriangle (square.LeftCenter.vertexIndex, square.vertexIndex, square.BottomCenter.vertexIndex);
				CreateTriangle (square.LeftCenter.vertexIndex, square.BottomCenter.vertexIndex, square.BottomLeft.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);

				List<Vector3> arcVertices = GetArcVertices (square.TopLeft.GetPosition (), 0.5f, 11, CornerSide.BottomRight);
				for (int i = 0; i < arcVertices.Count; i++) {
					CreateVertex (arcVertices [i]);
					CreateUvs (Vector2.zero);

					if (i > 0) {
						CreateTriangle (square.vertexIndex, vertices.Count - 2, vertices.Count - 1);
					}
				}
			}
			if (configuration == 15) {
				square.TopLeft.vertexIndex = vertices.Count;
				CreateVertex (square.TopLeft.GetPosition ());

				square.TopRight.vertexIndex = vertices.Count;
				CreateVertex (square.TopRight.GetPosition ());

				square.BottomRight.vertexIndex = vertices.Count;
				CreateVertex (square.BottomRight.GetPosition ());

				square.BottomLeft.vertexIndex = vertices.Count;
				CreateVertex (square.BottomLeft.GetPosition ());

				CreateTriangle (square.TopLeft.vertexIndex, square.TopRight.vertexIndex, square.BottomRight.vertexIndex);
				CreateTriangle (square.TopLeft.vertexIndex, square.BottomRight.vertexIndex, square.BottomLeft.vertexIndex);

				CreateUvs (Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
			}
		}

		private void CreateRoundCorner(Vector3 position, float radius, int steps, CornerSide corner) {
			float angleFrom = 0;
			float angleTo = 0;
			float fix = 0;

			if (corner == CornerSide.TopLeft) {
				angleFrom = Mathf.PI / 2.0f;
				angleTo = Mathf.PI;
			}
			if (corner == CornerSide.TopRight) {
				angleFrom = 0;
				angleTo = Mathf.PI / 2.0f;
			}
			if (corner == CornerSide.BottomRight) {
				angleFrom = (3.0f * Mathf.PI) / 2.0f;
				angleTo = Mathf.PI * 2.0f;
				fix = Mathf.Abs(angleFrom - angleTo) / (float)steps;
			}
			if (corner == CornerSide.BottomLeft) {
				angleFrom = Mathf.PI;
				angleTo = (3.0f * Mathf.PI) / 2.0f;
				fix = Mathf.Abs(angleFrom - angleTo) / (float)steps;
			}

			int cornerIndex = vertices.Count;
			float angleStep = Mathf.Abs(angleFrom - angleTo) / (float)steps;

			CreateVertex (position);
			CreateUvs (Vector2.zero);

			for (float i = angleFrom; i <= angleTo+fix; i += angleStep) {
				float x = position.x + radius * Mathf.Cos (i);
				float y = position.z + radius * Mathf.Sin (i);
				Vector3 p = new Vector3 (x, 0, y);

				CreateVertex (p);
				CreateTriangle (cornerIndex, vertices.Count - 1, vertices.Count - 2);
				CreateUvs (Vector2.zero);
			}
		}

		private List<Vector3> GetArcVertices(Vector3 position, float radius, int steps, CornerSide cornerSide) {
			List<Vector3> arcVertices = new List<Vector3> ();
			float angleFrom = 0;
			float angleTo = 0;
			float fix = 0;

			if (cornerSide == CornerSide.TopLeft) {
				angleFrom = Mathf.PI / 2.0f;
				angleTo = Mathf.PI;
			}
			if (cornerSide == CornerSide.TopRight) {
				angleFrom = 0;
				angleTo = Mathf.PI / 2.0f;
			}
			if (cornerSide == CornerSide.BottomRight) {
				angleFrom = (3.0f * Mathf.PI) / 2.0f;
				angleTo = Mathf.PI * 2.0f;
				fix = Mathf.Abs(angleFrom - angleTo) / (float)steps;
			}
			if (cornerSide == CornerSide.BottomLeft) {
				angleFrom = Mathf.PI;
				angleTo = (3.0f * Mathf.PI) / 2.0f;
				fix = Mathf.Abs(angleFrom - angleTo) / (float)steps;
			}

			int cornerIndex = vertices.Count;
			float angleStep = Mathf.Abs(angleFrom - angleTo) / (float)steps;

			for (float i = angleFrom; i <= angleTo+fix; i += angleStep) {
				float x = position.x + radius * Mathf.Cos (i);
				float y = position.z + radius * Mathf.Sin (i);
				Vector3 p = new Vector3 (x, 0, y);

				arcVertices.Add (p);
			}

			return arcVertices;
		}
	}
}