using UnityEngine;
using System;

namespace MapGenerator
{
	
	public class MapGenerator : MonoBehaviour
	{

		[Header ("Map")]
		[SerializeField] private Texture2D mapTexture = null;
		[SerializeField] private Material mapMaterial = null;
		[SerializeField] private MeshFilter meshFilter = null;
		[SerializeField] private MeshRenderer meshRenderer = null;

		[Header ("Generate settings")]
		[SerializeField] private int width = 25;
		[SerializeField] private int height = 25;
		[SerializeField] private int smoothIterations = 8;
		[SerializeField] private int fillPercent = 50;
		[SerializeField] private string seed = "";

		[Header ("Tile settings")]
		[SerializeField] private Sprite[] spriteTiles = null;

		[Header ("Debug")]
		[SerializeField] private bool squaresDebug = false;
		[SerializeField] private bool controlNodesDebug = false;
		[SerializeField] private bool sideNodesDebug = false;
		[Range (0, 15)] [SerializeField] private int configurationNumber = 0;

		private Map map = null;
		private MeshGenerator meshGenerator = null;

		public void GenerateFromTexture ()
		{
			if (mapTexture == null || meshFilter == null)
				return;

			map = new Map (mapTexture);
			meshGenerator = new MeshGenerator (map);
			Mesh mesh = meshGenerator.GenerateMeshRounded (spriteTiles);

			meshFilter.mesh = mesh;
			meshRenderer.material = mapMaterial;
		}

		public void GenerateRandom ()
		{
			if (meshFilter == null)
				return;

			map = new Map (width, height, fillPercent, seed, smoothIterations);
			meshGenerator = new MeshGenerator (map);
			Mesh mesh = meshGenerator.GenerateMesh (spriteTiles);

			meshFilter.mesh = mesh;
			meshRenderer.material = mapMaterial;
		}

		private void OnDrawGizmos ()
		{
			if (map == null || meshGenerator == null)
				return;

			SquareGrid squareGrid = meshGenerator.GetSquareGrid ();

			for (int y = 0; y < squareGrid.height; y++) {
				for (int x = 0; x < squareGrid.width; x++) {
					int configuration = squareGrid.GetSquare (x, y).GetConfiguration ();

					if (squaresDebug) {
						Gizmos.color = Color.black;
						if (configuration == configurationNumber)
							Gizmos.color = Color.red;

						Gizmos.DrawSphere (squareGrid.GetSquare (x, y).GetPosition (), 0.1f);
					}

					if (controlNodesDebug) {
						Gizmos.color = squareGrid.GetSquare (x, y).TopLeft.value == 0 ? Color.white : Color.black;
						Gizmos.DrawCube (squareGrid.GetSquare (x, y).TopLeft.GetPosition (), Vector3.one * 0.4f);

						Gizmos.color = squareGrid.GetSquare (x, y).TopRight.value == 0 ? Color.white : Color.black;
						Gizmos.DrawCube (squareGrid.GetSquare (x, y).TopRight.GetPosition (), Vector3.one * 0.4f);

						Gizmos.color = squareGrid.GetSquare (x, y).BottomRight.value == 0 ? Color.white : Color.black;
						Gizmos.DrawCube (squareGrid.GetSquare (x, y).BottomRight.GetPosition (), Vector3.one * 0.4f);

						Gizmos.color = squareGrid.GetSquare (x, y).BottomLeft.value == 0 ? Color.white : Color.black;
						Gizmos.DrawCube (squareGrid.GetSquare (x, y).BottomLeft.GetPosition (), Vector3.one * 0.4f);
					}

					if (sideNodesDebug) {
						Gizmos.color = Color.green;
						Gizmos.DrawCube (squareGrid.GetSquare (x, y).TopCenter.GetPosition (), Vector3.one * 0.1f);
						Gizmos.DrawCube (squareGrid.GetSquare (x, y).RightCenter.GetPosition (), Vector3.one * 0.1f);
						Gizmos.DrawCube (squareGrid.GetSquare (x, y).BottomCenter.GetPosition (), Vector3.one * 0.1f);
						Gizmos.DrawCube (squareGrid.GetSquare (x, y).LeftCenter.GetPosition (), Vector3.one * 0.1f);
					}
				}
			}
		}
	}

}