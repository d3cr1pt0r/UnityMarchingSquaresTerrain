using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarchingSquaresGenerator {
	public class MapDrawController : MonoBehaviour {

		[SerializeField] private MapGenerator mapGenerator = null;

		private Map map;
		private int mapWidth;
		private int mapHeight;

		private void Awake() {
			mapWidth = (int) ScreenToMapPosition (new Vector3(Screen.width, 0, 0)).x;
			mapHeight = (int) ScreenToMapPosition (new Vector3(0, Screen.height, 0)).y;

			map = new Map (mapWidth, mapHeight);

			map.SetValue (2, 1, 1);
			map.SetValue (4, 3, 1);
			map.SetValue (6, 5, 1);
			map.SetValue (8, 7, 1);

			map.SetPosition (2, 1, new Vector3 (0, 0, 0));
			map.SetPosition (4, 3, new Vector3 (1, 0, 0));
			map.SetPosition (6, 5, new Vector3 (0, 1, 0));
			map.SetPosition (8, 7, new Vector3 (1, 1, 0));

			mapGenerator.GenerateFromMap (map);
		}

		private void Update () {
			if (Input.GetMouseButtonDown (0)) {
				Vector2 mapPosition = ScreenToMapPosition (Input.mousePosition);
				Vector3 worldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				worldPosition.x = Mathf.Floor (worldPosition.x);
				worldPosition.y = Mathf.Floor (worldPosition.y);

				map.SetValue (mapPosition, 1);
				map.SetPosition (mapPosition, new Vector3 (worldPosition.x, worldPosition.y, 0));

				mapGenerator.GenerateFromMap (map);
			}
		}

		private Vector2 ScreenToMapPosition(Vector2 screenPos) {
			return new Vector2(Mathf.Floor(screenPos.x / 20.0f), Mathf.Floor(screenPos.y / 20.0f));
		}
	}
}