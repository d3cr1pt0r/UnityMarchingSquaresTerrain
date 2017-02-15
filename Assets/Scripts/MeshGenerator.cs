using System.Collections.Generic;
using UnityEngine;

namespace MarchingSquaresGenerator
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

		private int roundSteps;
		private float radius = 0.5f;

		private List<List<Vector2>> edgeColliderPoints = new List<List<Vector2>> ();
		private HashSet<Vector3> checkedVertices = new HashSet<Vector3> ();

		public MeshGenerator (Map map)
		{
			squareGrid = new SquareGrid (map);
		}

		public Mesh GenerateMeshRounded (int roundSteps)
		{
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

		public List<List<Vector2>> GetEdgeColliderPoints() {
			return edgeColliderPoints;
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

		private void TriangulateQuadRounded (Mesh mesh, Square square)
		{
			int configuration = square.GetConfiguration ();

			if (configuration == 1) {
				checkedVertices.Add (square.TopLeft.GetPosition());

				CreateRoundCornerConvex (square, CornerSide.BottomRight, radius, roundSteps, configuration);
			}
			if (configuration == 2) {
				checkedVertices.Add (square.TopRight.GetPosition());

				CreateRoundCornerConvex (square, CornerSide.BottomLeft, radius, roundSteps, configuration);
			}
			if (configuration == 3) {
				checkedVertices.Add (square.TopLeft.GetPosition());
				checkedVertices.Add (square.TopRight.GetPosition());

				TriangulateMesh (square, configuration, square.TopLeft, square.TopRight, square.RightCenter, square.LeftCenter);
			}
			if (configuration == 4) {
				checkedVertices.Add (square.BottomRight.GetPosition());

				CreateRoundCornerConvex (square, CornerSide.TopLeft, radius, roundSteps, configuration);
			}
			if (configuration == 5) {
				checkedVertices.Add (square.TopRight.GetPosition());
				checkedVertices.Add (square.BottomLeft.GetPosition());

				TriangulateMesh (square, configuration, square.TopLeft, square.TopCenter, square.RightCenter, square.BottomRight, square.BottomCenter, square.LeftCenter);
			}
			if (configuration == 6) {
				checkedVertices.Add (square.TopRight.GetPosition());
				checkedVertices.Add (square.BottomRight.GetPosition());

				TriangulateMesh (square, configuration, square.BottomRight, square.BottomCenter, square.TopCenter, square.TopRight);
			}
			if (configuration == 7) {
				checkedVertices.Add (square.TopLeft.GetPosition());
				checkedVertices.Add (square.TopRight.GetPosition());
				checkedVertices.Add (square.BottomRight.GetPosition());

				CreateRoundCornerConcave (square, CornerSide.TopRight, radius, roundSteps, configuration);
			}
			if (configuration == 8) {
				checkedVertices.Add (square.BottomLeft.GetPosition());

				CreateRoundCornerConvex (square, CornerSide.TopRight, radius, roundSteps, configuration);
			}
			if (configuration == 9) {
				checkedVertices.Add (square.TopLeft.GetPosition());
				checkedVertices.Add (square.BottomLeft.GetPosition());

				TriangulateMesh (square, configuration, square.TopLeft, square.TopCenter, square.BottomCenter, square.BottomLeft);
			}
			if (configuration == 10) {
				checkedVertices.Add (square.TopLeft.GetPosition());
				checkedVertices.Add (square.BottomRight.GetPosition());

				TriangulateMesh (square, configuration, square.TopRight, square.RightCenter, square.BottomCenter, square.BottomLeft, square.LeftCenter, square.TopCenter);
			}
			if (configuration == 11) {
				checkedVertices.Add (square.BottomLeft.GetPosition());
				checkedVertices.Add (square.TopLeft.GetPosition());
				checkedVertices.Add (square.TopRight.GetPosition());

				CreateRoundCornerConcave (square, CornerSide.TopLeft, radius, roundSteps, configuration);
			}
			if (configuration == 12) {
				checkedVertices.Add (square.BottomLeft.GetPosition());
				checkedVertices.Add (square.BottomRight.GetPosition());

				TriangulateMesh (square, configuration, square.LeftCenter, square.RightCenter, square.BottomRight, square.BottomLeft);
			}
			if (configuration == 13) {
				checkedVertices.Add (square.TopLeft.GetPosition());
				checkedVertices.Add (square.BottomLeft.GetPosition());
				checkedVertices.Add (square.BottomRight.GetPosition());

				CreateRoundCornerConcave (square, CornerSide.BottomLeft, radius, roundSteps, configuration);
			}
			if (configuration == 14) {
				checkedVertices.Add (square.TopRight.GetPosition());
				checkedVertices.Add (square.BottomRight.GetPosition());
				checkedVertices.Add (square.BottomLeft.GetPosition());

				CreateRoundCornerConcave (square, CornerSide.BottomRight, radius, roundSteps, configuration);
			}
			if (configuration == 15) {
				checkedVertices.Add (square.TopLeft.GetPosition());
				checkedVertices.Add (square.TopRight.GetPosition());
				checkedVertices.Add (square.BottomRight.GetPosition());
				checkedVertices.Add (square.BottomLeft.GetPosition());

				TriangulateMesh (square, configuration, square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
			}
		}

		private void TriangulateMesh (Square square, int configuration, params Node[] nodes)
		{
			if (nodes.Length < 3) {
				Debug.LogError ("TriangulateMesh: need at least 3 nodes!");
				return;
			}

			List<Vector2> edgeColliders = new List<Vector2> ();
			
			for (int i = 0; i < nodes.Length; i++) {
				Vector2 uv = new Vector2 (nodes [i].GetPosition ().x - square.BottomLeft.GetPosition ().x, nodes [i].GetPosition ().y - square.BottomLeft.GetPosition ().y);
				Vector4 tilingAndOffset = TileMapData.GetTilingAndOffsetFromConfiguration (configuration);
				uv.x = uv.x * tilingAndOffset.x + tilingAndOffset.z;
				uv.y = uv.y * tilingAndOffset.y + tilingAndOffset.w;

				CreateVertex (nodes [i]);
				CreateUvs (uv);

				// edge collider
				if (!checkedVertices.Contains (nodes [i].GetPosition ())) {
					edgeColliders.Add (new Vector2 (nodes [i].GetPosition ().x, nodes [i].GetPosition ().y));
				}
			}

			for (int i = 0; i < nodes.Length-2; i++) {
				CreateTriangle (nodes [0].vertexIndex, nodes [i+1].vertexIndex, nodes [i+2].vertexIndex);
			}

			if (edgeColliders.Count >= 2) {
				edgeColliderPoints.Add (edgeColliders);
			}
		}

		private void CreateRoundCornerConvex (Square square, CornerSide cornerSide, float radius, int steps, int configuration)
		{
			List<Node> nodes = new List<Node> ();
			Vector3 position = Vector3.one;
			Node targetNode = null;

			if (cornerSide == CornerSide.TopLeft) {
				position = square.BottomRight.GetPosition ();
				targetNode = square.BottomRight;
			}
			if (cornerSide == CornerSide.TopRight) {
				position = square.BottomLeft.GetPosition ();
				targetNode = square.BottomLeft;
			}
			if (cornerSide == CornerSide.BottomRight) {
				position = square.TopLeft.GetPosition ();
				targetNode = square.TopLeft;
			}
			if (cornerSide == CornerSide.BottomLeft) {
				position = square.TopRight.GetPosition ();
				targetNode = square.TopRight;
			}

			nodes.Add (targetNode);

			List<Vector3> arcVertices = GetArcVertices (position, radius, steps, cornerSide);
			arcVertices.Reverse ();
			for (int i = 0; i < arcVertices.Count; i++) {
				nodes.Add (new Node (arcVertices [i]));
			}

			TriangulateMesh (square, configuration, nodes.ToArray ());
		}

		private void CreateRoundCornerConcave (Square square, CornerSide cornerSide, float radius, int steps, int configuration)
		{
			List<Node> nodes = new List<Node> ();
			Vector3 arcPosition = Vector3.zero;
			Node n0 = null;
			Node n1 = null;
			Node n2 = null;

			if (cornerSide == CornerSide.TopLeft) {
				arcPosition = square.BottomRight.GetPosition ();

				n0 = square.TopLeft;
				n1 = square.BottomLeft;
				n2 = square.TopRight;
			}
			if (cornerSide == CornerSide.TopRight) {
				arcPosition = square.BottomLeft.GetPosition ();

				n0 = square.TopRight;
				n1 = square.TopLeft;
				n2 = square.BottomRight;
			}
			if (cornerSide == CornerSide.BottomRight) {
				arcPosition = square.TopLeft.GetPosition ();

				n0 = square.BottomRight;
				n1 = square.TopRight;
				n2 = square.BottomLeft;
			}
			if (cornerSide == CornerSide.BottomLeft) {
				arcPosition = square.TopRight.GetPosition ();

				n0 = square.BottomLeft;
				n1 = square.BottomRight;
				n2 = square.TopLeft;
			}

			nodes.Add (n0);
			nodes.Add (n2);

			List<Vector3> arcVertices = GetArcVertices (arcPosition, radius, roundSteps, cornerSide);
			for (int i = 0; i < arcVertices.Count; i++) {
				nodes.Add (new Node(arcVertices [i]));
			}

			nodes.Add (n1);

			TriangulateMesh (square, configuration, nodes.ToArray());
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
				float y = position.y + radius * Mathf.Sin (angleFrom + i * angleStep);
				Vector3 p = new Vector3 (x, y, 0);

				arcVertices.Add (p);
			}

			return arcVertices;
		}
	}
}