using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
	
	public class MeshGenerator
	{
		enum CornerSide
		{
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
		private int roundSteps;
		private float radius = 0.5f;

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

		public Mesh GenerateMeshRounded (Sprite[] spriteTiles, int roundSteps)
		{
			this.spriteTiles = spriteTiles;
			this.roundSteps = roundSteps;

			mesh = new Mesh ();
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

		private int CreateVertex (Vector3 position)
		{
			int index = vertices.Count;
			vertices.Add (position);

			return index;
		}

		private int CreateVertex (Node node)
		{
			node.vertexIndex = vertices.Count;
			CreateVertex (node.GetPosition ());

			return node.vertexIndex;
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
				CreateVertex (square.TopLeft);
				CreateVertex (square.TopRight);
				CreateVertex (square.BottomRight);
				CreateVertex (square.BottomLeft);

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
				CreateRoundCornerConvex (square.TopLeft.GetPosition (), radius, roundSteps, CornerSide.BottomRight);
			}
			if (configuration == 2) {
				CreateRoundCornerConvex (square.TopRight.GetPosition (), radius, roundSteps, CornerSide.BottomLeft);
			}
			if (configuration == 3) {
				TriangulateMesh (square.TopLeft, square.TopRight, square.RightCenter, square.LeftCenter);
			}
			if (configuration == 4) {
				CreateRoundCornerConvex (square.BottomRight.GetPosition (), radius, roundSteps, CornerSide.TopLeft);
			}
			if (configuration == 5) {
				TriangulateMesh (square.TopLeft, square.TopCenter, square.RightCenter, square.BottomRight, square.BottomCenter, square.LeftCenter);
			}
			if (configuration == 6) {
				TriangulateMesh (square.TopCenter, square.TopRight, square.BottomRight, square.BottomCenter);
			}
			if (configuration == 7) {
				TriangulateMesh (square.TopRight, square.BottomRight, square.BottomCenter);
				TriangulateMesh (square.TopRight, square.LeftCenter, square.TopLeft);

				CreateVertex (square.TopRight);
				CreateUvs (Vector2.zero);

				List<Vector3> arcVertices = GetArcVertices (square.BottomLeft.GetPosition (), 0.5f, roundSteps, CornerSide.TopRight);
				for (int i = 0; i < arcVertices.Count; i++) {
					CreateVertex (arcVertices [i]);
					CreateUvs (Vector2.zero);

					if (i > 0) {
						CreateTriangle (square.TopRight.vertexIndex, vertices.Count - 2, vertices.Count - 1);
					}
				}
			}
			if (configuration == 8) {
				CreateRoundCornerConvex (square.BottomLeft.GetPosition (), radius, roundSteps, CornerSide.TopRight);
			}
			if (configuration == 9) {
				TriangulateMesh (square.TopLeft, square.TopCenter, square.BottomCenter, square.BottomLeft);
			}
			if (configuration == 10) {
				TriangulateMesh (square.TopRight, square.RightCenter, square.BottomCenter, square.BottomLeft, square.LeftCenter, square.TopCenter);
			}
			if (configuration == 11) {
				TriangulateMesh (square.TopLeft, square.TopRight, square.RightCenter);
				TriangulateMesh (square.TopLeft, square.BottomCenter, square.BottomLeft);

				CreateVertex (square.TopLeft);
				CreateUvs (Vector2.zero);

				List<Vector3> arcVertices = GetArcVertices (square.BottomRight.GetPosition (), radius, roundSteps, CornerSide.TopLeft);
				for (int i = 0; i < arcVertices.Count; i++) {
					CreateVertex (arcVertices [i]);
					CreateUvs (Vector2.zero);

					if (i > 0) {
						CreateTriangle (square.TopLeft.vertexIndex, vertices.Count - 2, vertices.Count - 1);
					}
				}
			}
			if (configuration == 12) {
				TriangulateMesh (square.LeftCenter, square.RightCenter, square.BottomRight, square.BottomLeft);
			}
			if (configuration == 13) {
				TriangulateMesh (square.BottomLeft, square.TopLeft, square.TopCenter);
				TriangulateMesh (square.BottomLeft, square.RightCenter, square.BottomRight);

				CreateVertex (square.BottomLeft);
				CreateUvs (Vector2.zero);

				List<Vector3> arcVertices = GetArcVertices (square.TopRight.GetPosition (), radius, roundSteps, CornerSide.BottomLeft);
				for (int i = 0; i < arcVertices.Count; i++) {
					CreateVertex (arcVertices [i]);
					CreateUvs (Vector2.zero);

					if (i > 0) {
						CreateTriangle (square.BottomLeft.vertexIndex, vertices.Count - 2, vertices.Count - 1);
					}
				}
			}
			if (configuration == 14) {
				TriangulateMesh (square.BottomRight, square.TopCenter, square.TopRight);
				TriangulateMesh (square.BottomRight, square.BottomLeft, square.LeftCenter);

				CreateVertex (square.BottomRight);
				CreateUvs (Vector2.zero);

				List<Vector3> arcVertices = GetArcVertices (square.TopLeft.GetPosition (), radius, roundSteps, CornerSide.BottomRight);
				for (int i = 0; i < arcVertices.Count; i++) {
					CreateVertex (arcVertices [i]);
					CreateUvs (Vector2.zero);

					if (i > 0) {
						CreateTriangle (square.BottomRight.vertexIndex, vertices.Count - 2, vertices.Count - 1);
					}
				}
			}
			if (configuration == 15) {
				TriangulateMesh (square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
			}
		}

		private void TriangulateMesh (params Node[] nodes)
		{
			for (int i = 0; i < nodes.Length; i++) {
				CreateVertex (nodes [i]);
				CreateUvs (Vector2.zero);
			}

			if (nodes.Length >= 3) {
				CreateTriangle (nodes [0].vertexIndex, nodes [1].vertexIndex, nodes [2].vertexIndex);
			}
			if (nodes.Length >= 4) {
				CreateTriangle (nodes [0].vertexIndex, nodes [2].vertexIndex, nodes [3].vertexIndex);
			}
			if (nodes.Length >= 5) {
				CreateTriangle (nodes [0].vertexIndex, nodes [3].vertexIndex, nodes [4].vertexIndex);
			}
			if (nodes.Length >= 6) {
				CreateTriangle (nodes [0].vertexIndex, nodes [4].vertexIndex, nodes [5].vertexIndex);
			}
		}

		private void CreateRoundCornerConvex (Vector3 position, float radius, int steps, CornerSide cornerSide)
		{
			List<Vector3> arcVertices = GetArcVertices (position, radius, steps, cornerSide);
			int cornerIndex = CreateVertex (position);

			CreateUvs (Vector2.zero);

			for (int i = 0; i < arcVertices.Count; i++) {
				CreateVertex (arcVertices [i]);
				CreateTriangle (cornerIndex, vertices.Count - 1, vertices.Count - 2);
				CreateUvs (Vector2.zero);
			}
		}

		private void CreateRoundCornerConcave (Vector3 position, float radius, int steps, CornerSide cornerSide)
		{
			
		}

		private List<Vector3> GetArcVertices (Vector3 position, float radius, int steps, CornerSide cornerSide)
		{
			List<Vector3> arcVertices = new List<Vector3> ();
			float angleFrom = 0;
			float angleTo = 0;

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
			}
			if (cornerSide == CornerSide.BottomLeft) {
				angleFrom = Mathf.PI;
				angleTo = (3.0f * Mathf.PI) / 2.0f;
			}
				
			float angleStep = Mathf.Abs (angleFrom - angleTo) / (float)steps;

			for (int i = 0; i <= steps; i++) {
				float x = position.x + radius * Mathf.Cos (angleFrom + i * angleStep);
				float y = position.z + radius * Mathf.Sin (angleFrom + i * angleStep);
				Vector3 p = new Vector3 (x, 0, y);

				arcVertices.Add (p);
			}

			return arcVertices;
		}
	}
}