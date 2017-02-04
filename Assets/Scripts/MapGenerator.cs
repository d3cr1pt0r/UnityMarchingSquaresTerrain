using UnityEngine;
using System;

namespace MapGenerator {
	
	public class MapGenerator : MonoBehaviour {

		[Header("Map")]
		[SerializeField] private Texture2D mapTexture = null;
		[SerializeField] private Material mapMaterial = null;
		[SerializeField] private MeshFilter meshFilter = null;
		[SerializeField] private MeshRenderer meshRenderer = null;

		[Header("Tile settings")]
		[SerializeField] private Sprite[] spriteTiles = null;

		[Header("Debug")]
		[SerializeField] private bool enableDebug = false;
		[Range(0, 15)] [SerializeField] private int configurationNumber = 0;

		private Map map = null;
		private MeshGenerator meshGenerator = null;

		public void GenerateMap() {
			if (mapTexture == null || meshFilter == null) return;

			//map = MapReader.ReadMap (mapTexture);
			map = MapReader.GenerateRandomMap(50, 50, 50, UnityEngine.Random.Range(0, 100000));
			map = MapReader.SmoothMap (map, 8);
			meshGenerator = new MeshGenerator (map);

			Mesh mesh = meshGenerator.GenerateMesh (spriteTiles);
			meshFilter.mesh = mesh;

			meshRenderer.material = mapMaterial;
		}

		private void OnDrawGizmos() {
			if (map == null || meshGenerator == null || !enableDebug) return;

			SquareGrid squareGrid = meshGenerator.GetSquareGrid ();

			for (int y = 0; y < squareGrid.height; y++) {
				for (int x = 0; x < squareGrid.width; x++) {
					
					int configuration = squareGrid.GetSquare (x, y).GetConfiguration ();
					Gizmos.color = Color.black;
					if (configuration == configurationNumber) Gizmos.color = Color.red;

					Gizmos.DrawSphere (squareGrid.GetSquare(x, y).GetPosition(), 0.1f);

					Gizmos.color = squareGrid.GetSquare (x, y).TopLeft.value == 0 ? Color.white : Color.black;
					Gizmos.DrawCube (squareGrid.GetSquare (x, y).TopLeft.GetPosition(), Vector3.one * 0.4f);

					Gizmos.color = squareGrid.GetSquare (x, y).TopRight.value == 0 ? Color.white : Color.black;
					Gizmos.DrawCube (squareGrid.GetSquare (x, y).TopRight.GetPosition(), Vector3.one * 0.4f);

					Gizmos.color = squareGrid.GetSquare (x, y).BottomRight.value == 0 ? Color.white : Color.black;
					Gizmos.DrawCube (squareGrid.GetSquare (x, y).BottomRight.GetPosition(), Vector3.one * 0.4f);

					Gizmos.color = squareGrid.GetSquare (x, y).BottomLeft.value == 0 ? Color.white : Color.black;
					Gizmos.DrawCube (squareGrid.GetSquare (x, y).BottomLeft.GetPosition(), Vector3.one * 0.4f);
				}
			}
		}
	}

}