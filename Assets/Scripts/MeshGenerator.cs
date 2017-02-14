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
				CreateRoundCornerConvex (square, CornerSide.BottomRight, radius, roundSteps, configuration);
			}
			if (configuration == 2) {
				CreateRoundCornerConvex (square, CornerSide.BottomLeft, radius, roundSteps, configuration);
			}
			if (configuration == 3) {
				TriangulateMesh (square, configuration, square.TopLeft, square.TopRight, square.RightCenter, square.LeftCenter);
			}
			if (configuration == 4) {
				CreateRoundCornerConvex (square, CornerSide.TopLeft, radius, roundSteps, configuration);
			}
			if (configuration == 5) {
				TriangulateMesh (square, configuration, square.TopLeft, square.TopCenter, square.RightCenter, square.BottomRight, square.BottomCenter, square.LeftCenter);
			}
			if (configuration == 6) {
				TriangulateMesh (square, configuration, square.TopCenter, square.TopRight, square.BottomRight, square.BottomCenter);
			}
			if (configuration == 7) {
				CreateRoundCornerConcave (square, CornerSide.TopRight, radius, roundSteps, configuration);
			}
			if (configuration == 8) {
				CreateRoundCornerConvex (square, CornerSide.TopRight, radius, roundSteps, configuration);
			}
			if (configuration == 9) {
				TriangulateMesh (square, configuration, square.TopLeft, square.TopCenter, square.BottomCenter, square.BottomLeft);
			}
			if (configuration == 10) {
				TriangulateMesh (square, configuration, square.TopRight, square.RightCenter, square.BottomCenter, square.BottomLeft, square.LeftCenter, square.TopCenter);
			}
			if (configuration == 11) {
				CreateRoundCornerConcave (square, CornerSide.TopLeft, radius, roundSteps, configuration);
			}
			if (configuration == 12) {
				TriangulateMesh (square, configuration, square.LeftCenter, square.RightCenter, square.BottomRight, square.BottomLeft);
			}
			if (configuration == 13) {
				CreateRoundCornerConcave (square, CornerSide.BottomLeft, radius, roundSteps, configuration);
			}
			if (configuration == 14) {
				CreateRoundCornerConcave (square, CornerSide.BottomRight, radius, roundSteps, configuration);
			}
			if (configuration == 15) {
				TriangulateMesh (square, configuration, square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
			}
		}

		private void TriangulateMesh (Square square, int configuration, params Node[] nodes)
		{
			for (int i = 0; i < nodes.Length; i++) {
				Vector2 uv = new Vector2 (nodes [i].GetPosition ().x - square.BottomLeft.GetPosition ().x, nodes [i].GetPosition ().z - square.BottomLeft.GetPosition ().z);
				Vector4 tilingAndOffset = TileMapData.GetTilingAndOffsetFromConfiguration (configuration);
				uv.x = uv.x * tilingAndOffset.x + tilingAndOffset.z;
				uv.y = uv.y * tilingAndOffset.y + tilingAndOffset.w;

				CreateVertex (nodes [i]);
				CreateUvs (uv);
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

		private void CreateRoundCornerConvex (Square square, CornerSide cornerSide, float radius, int steps, int configuration)
		{
			Vector3 position = Vector3.one;

			if (cornerSide == CornerSide.TopLeft) {
				position = square.BottomRight.GetPosition ();
			}
			if (cornerSide == CornerSide.TopRight) {
				position = square.BottomLeft.GetPosition ();
			}
			if (cornerSide == CornerSide.BottomRight) {
				position = square.TopLeft.GetPosition ();
			}
			if (cornerSide == CornerSide.BottomLeft) {
				position = square.TopRight.GetPosition ();
			}

			List<Vector3> arcVertices = GetArcVertices (position, radius, steps, cornerSide);
			int cornerIndex = CreateVertex (position);

			Vector4 tilingAndOffset = TileMapData.GetTilingAndOffsetFromConfiguration (configuration);
			Vector2 cornerUv = new Vector2 (position.x - square.BottomLeft.GetPosition ().x, position.z - square.BottomLeft.GetPosition ().z);
			cornerUv.x = cornerUv.x * tilingAndOffset.x + tilingAndOffset.z;
			cornerUv.y = cornerUv.y * tilingAndOffset.y + tilingAndOffset.w;

			CreateUvs (cornerUv);

			for (int i = 0; i < arcVertices.Count; i++) {
				Vector2 uv = new Vector2 (arcVertices [i].x - square.BottomLeft.GetPosition ().x, arcVertices [i].z - square.BottomLeft.GetPosition ().z);
				uv.x = uv.x * tilingAndOffset.x + tilingAndOffset.z;
				uv.y = uv.y * tilingAndOffset.y + tilingAndOffset.w;

				CreateVertex (arcVertices [i]);
				CreateTriangle (cornerIndex, vertices.Count - 1, vertices.Count - 2);
				CreateUvs (uv);
			}
		}

		private void CreateRoundCornerConcave (Square square, CornerSide cornerSide, float radius, int steps, int configuration)
		{
			Vector3 arcPosition = Vector3.zero;
			Node n0 = null;
			Node n1 = null;
			Node n2 = null;
			Node n3 = null;
			Node n4 = null;

			if (cornerSide == CornerSide.TopLeft) {
				arcPosition = square.BottomRight.GetPosition ();

				n0 = square.TopLeft;
				n1 = square.BottomCenter;
				n2 = square.BottomLeft;
				n3 = square.TopRight;
				n4 = square.RightCenter;
			}
			if (cornerSide == CornerSide.TopRight) {
				arcPosition = square.BottomLeft.GetPosition ();

				n0 = square.TopRight;
				n1 = square.LeftCenter;
				n2 = square.TopLeft;
				n3 = square.BottomRight;
				n4 = square.BottomCenter;
			}
			if (cornerSide == CornerSide.BottomRight) {
				arcPosition = square.TopLeft.GetPosition ();

				n0 = square.BottomRight;
				n1 = square.TopCenter;
				n2 = square.TopRight;
				n3 = square.BottomLeft;
				n4 = square.LeftCenter;
			}
			if (cornerSide == CornerSide.BottomLeft) {
				arcPosition = square.TopRight.GetPosition ();

				n0 = square.BottomLeft;
				n1 = square.TopLeft;
				n2 = square.TopCenter;
				n3 = square.RightCenter;
				n4 = square.BottomRight;
			}

			TriangulateMesh (square, configuration, n0, n1, n2);
			TriangulateMesh (square, configuration, n0, n3, n4);

			CreateVertex (n0);

			Vector4 tilingAndOffset = TileMapData.GetTilingAndOffsetFromConfiguration (configuration);
			Vector2 cornerUv = new Vector2 (n0.GetPosition ().x - square.BottomLeft.GetPosition ().x, n0.GetPosition ().z - square.BottomLeft.GetPosition ().z);
			cornerUv.x = cornerUv.x * tilingAndOffset.x + tilingAndOffset.z;
			cornerUv.y = cornerUv.y * tilingAndOffset.y + tilingAndOffset.w;

			CreateUvs (cornerUv);

			List<Vector3> arcVertices = GetArcVertices (arcPosition, radius, roundSteps, cornerSide);
			for (int i = 0; i < arcVertices.Count; i++) {
				Vector2 uv = new Vector2 (arcVertices [i].x - square.BottomLeft.GetPosition ().x, arcVertices [i].z - square.BottomLeft.GetPosition ().z);
				uv.x = uv.x * tilingAndOffset.x + tilingAndOffset.z;
				uv.y = uv.y * tilingAndOffset.y + tilingAndOffset.w;
				CreateVertex (arcVertices [i]);
				CreateUvs (uv);

				if (i > 0) {
					CreateTriangle (n0.vertexIndex, vertices.Count - 2, vertices.Count - 1);
				}
			}

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