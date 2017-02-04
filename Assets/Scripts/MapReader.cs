using UnityEngine;
using System.IO;

namespace MapGenerator {
	
	public class MapReader {
		
		public static Map ReadMap(Texture2D textureMap) {
			int width = textureMap.width;
			int height = textureMap.height;

			Map map = new Map (width, height);

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					Color color = textureMap.GetPixel (x, y);

					map.SetValue (x, y, ColorToByte (color));
					map.SetPosition (x, y, new Vector3 (width * 0.5f - x, 0, height * 0.5f - y));
				}
			}

			return map;
		}

		public static Map GenerateRandomMap(int width, int height, int randomFillPercent, int seed) {
			Map map = new Map (width, height);
			Random.InitState (seed);

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					int isWall = Random.Range (0, 100) < randomFillPercent ? 1 : 0;
					map.SetValue (x, y, (byte)isWall);
					map.SetPosition (x, y, new Vector3 (width * 0.5f - x, 0, height * 0.5f - y));
				}
			}

			return map;
		}

		public static Map SmoothMap(Map map, int iterations) {
			for (int i = 0; i < iterations; i++) {
				for (int y = 0; y < map.height; y++) {
					for (int x = 0; x < map.width; x++) {
						int neighbourCellWallCount = GetNeighbourCellWallCount (map, x, y);

						if (neighbourCellWallCount > 4) {
							map.SetValue (x, y, 1);
						} else if (neighbourCellWallCount < 4) {
							map.SetValue (x, y, 0);
						}
					}
				}
			}

			return map;
		}

		private static int GetNeighbourCellWallCount(Map map, int x, int y) {
			int neighboutCellWallCount = 0;
			Vector2[] neighboursToCheck = new Vector2[] {
				new Vector2(0, 1),
				new Vector2(1, 1),
				new Vector2(1, 0),
				new Vector2(1, -1),
				new Vector2(0, -1),
				new Vector2(-1, -1),
				new Vector2(-1, 0),
				new Vector2(-1, 1),
			};

			for (int i = 0; i < neighboursToCheck.Length; i++) {
				int xCheck = x + (int) neighboursToCheck [i].x;
				int yCheck = y + (int) neighboursToCheck [i].y;

				if (GetCellValue(map, xCheck, yCheck) == 1) {
					neighboutCellWallCount++;
				}
			}

			return neighboutCellWallCount;
		}

		private static bool IsCellInBounds(Map map, int x, int y) {
			if (x < 0 || x >= map.width || y < 0 || y >= map.height) {
				return false;
			}
			return true;
		}

		private static int GetCellValue(Map map, int x, int y) {
			if (!IsCellInBounds (map, x, y)) {
				return int.MinValue;
			}
			return map.GetValue (x, y);
		}

		public static byte ColorToByte(Color color) {
			if (color == Color.white) {
				return 0;
			} else if (color == Color.black) {
				return 1;
			} else {
				return 0;
			}
		}
	}
}