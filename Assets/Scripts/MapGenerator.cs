using UnityEngine;
using System;
using System.Collections.Generic;

namespace MarchingSquaresGenerator
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
		[SerializeField] private int edgeSteps = 11;
		[SerializeField] private string seed = "";

		[Header("Collider")]
		[SerializeField] private GameObject edgeColliderPrefab = null;

		[Header ("Debug")]
		[SerializeField] private GameObject DebugNumberPrefab = null;
		[SerializeField] private bool configurationNumberDebug = false;
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
			Mesh mesh = meshGenerator.GenerateMeshRounded (edgeSteps);

			meshFilter.mesh = mesh;
			meshRenderer.material = mapMaterial;

			GenerateEdgeColliders (meshGenerator.GetEdgeColliderPoints ());

			if (configurationNumberDebug) {
				GenerateConfigurationNumberDebug ();
			}
		}

		public void GenerateRandom ()
		{
			if (meshFilter == null)
				return;

			map = new Map (width, height, fillPercent, seed, smoothIterations);
			meshGenerator = new MeshGenerator (map);
			Mesh mesh = meshGenerator.GenerateMeshRounded (edgeSteps);

			meshFilter.mesh = mesh;
			meshRenderer.material = mapMaterial;

			GenerateEdgeColliders (meshGenerator.GetEdgeColliderPoints ());

			if (configurationNumberDebug) {
				GenerateConfigurationNumberDebug ();
			}
		}

		private void GenerateConfigurationNumberDebug ()
		{
			SquareGrid squareGrid = meshGenerator.GetSquareGrid ();

			for (int y = 0; y < squareGrid.height; y++) {
				for (int x = 0; x < squareGrid.width; x++) {
					int configuration = squareGrid.GetSquare (x, y).GetConfiguration ();
					Vector3 position = squareGrid.GetSquare (x, y).GetPosition ();

					GameObject go = Instantiate (DebugNumberPrefab, position, Quaternion.Euler (new Vector3 (90, 0, 0)));
					go.GetComponent<TextMesh> ().text = configuration.ToString ();
				}
			}
		}

		private void GenerateEdgeColliders(List<List<Vector2>> edgeColliderPoints) {
			GameObject edgeColliderContainer = GameObject.Find("EdgeColliders");
			if (edgeColliderContainer != null) {
				DestroyImmediate (edgeColliderContainer);
			}

			GameObject edgeColliderContainerGO = new GameObject ("EdgeColliders");
			edgeColliderContainerGO.transform.SetParent (transform);
				
			for (int i = 0; i < edgeColliderPoints.Count; i++) {
				List<Vector2> edgePoints = edgeColliderPoints [i];
				GameObject edgeColliderGO = Instantiate (edgeColliderPrefab, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 0)));
				EdgeCollider2D edgeCollider = edgeColliderGO.GetComponent<EdgeCollider2D> ();

				edgeCollider.Reset ();
				edgeCollider.points = edgePoints.ToArray ();

				edgeColliderGO.transform.SetParent (edgeColliderContainerGO.transform);
				edgeColliderGO.transform.localPosition = Vector3.zero;
			}
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