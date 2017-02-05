using UnityEngine;
using UnityEditor;

namespace MapGenerator {
	
	public class Map {
		
		private byte[,] values;
		private Vector3[,] positions;

		public int width { get { return values.GetLength (0); } private set { } }
		public int height { get { return values.GetLength (1); } private set { } }

		public Map() {
			
		}

		public Map(Texture2D mapTexture) {
			LoadFromTexture (mapTexture);
		}

		public Map(Texture2D mapTexture, int smoothIterations) {
			LoadFromTexture (mapTexture);
			SmoothMap (smoothIterations);
		}

		public Map(int width, int height, int fillPercent, string seed) {
			LoadRandomMap (width, height, fillPercent, seed);
		}

		public Map(int width, int height, int fillPercent, string seed, int smoothIterations) {
			LoadRandomMap (width, height, fillPercent, seed);
			SmoothMap (smoothIterations);
		}

		public void Init(int width, int height) {
			values = new byte[width, height];
			positions = new Vector3[width, height];
		}

		public void SetValue(int x, int y, byte value) {
			values [x, y] = value;
		}

		public void SetPosition(int x, int y, Vector3 position) {
			positions [x, y] = position;
		}

		public byte GetValue(int x, int y) {
			return values [x, y];
		}

		public Vector3 GetPosition(int x, int y) {
			return positions [x, y];
		}

		public void LoadFromTexture(Texture2D mapTexture) {
			Init (mapTexture.width, mapTexture.height);

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					Color color = mapTexture.GetPixel (x, y);

					SetValue (x, y, ColorToByte (color));
					SetPosition (x, y, new Vector3 (width * 0.5f - x, 0, height * 0.5f - y));
				}
			}
		}

		public void LoadRandomMap(int width, int height, int fillPercent, string seed) {
			Init (width, height);

			if (seed != "") {
				Random.InitState (seed.GetHashCode ());
			} else {
				Random.InitState (EditorApplication.timeSinceStartup.ToString().GetHashCode());
			}

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					int isWall = Random.Range (0, 100) < fillPercent ? 1 : 0;

					SetValue (x, y, (byte) isWall);
					SetPosition (x, y, new Vector3 (width * 0.5f - x, 0, height * 0.5f - y));
				}
			}
		}

		public void SmoothMap(int iterations) {
			for (int i = 0; i < iterations; i++) {
				for (int y = 0; y < height; y++) {
					for (int x = 0; x < width; x++) {
						int neighbourCellWallCount = GetNeighbourCellWallCount (x, y);

						if (neighbourCellWallCount > 4) {
							SetValue (x, y, 1);
						} else if (neighbourCellWallCount < 4) {
							SetValue (x, y, 0);
						}
					}
				}
			}
		}

		private int GetNeighbourCellWallCount(int x, int y) {
			int neighboutCellWallCount = 0;
			Vector2[] neighboursToCheck = new Vector2[] {
				new Vector2(0, 1), new Vector2(1, 1),
				new Vector2(1, 0), new Vector2(1, -1),
				new Vector2(0, -1), new Vector2(-1, -1),
				new Vector2(-1, 0), new Vector2(-1, 1),
			};

			for (int i = 0; i < neighboursToCheck.Length; i++) {
				int xCheck = x + (int) neighboursToCheck [i].x;
				int yCheck = y + (int) neighboursToCheck [i].y;

				if (GetCellValue(xCheck, yCheck) == 1) {
					neighboutCellWallCount++;
				}
			}

			return neighboutCellWallCount;
		}

		private bool IsCellInBounds(int x, int y) {
			if (x < 0 || x >= width || y < 0 || y >= height) {
				return false;
			}
			return true;
		}

		private int GetCellValue(int x, int y) {
			if (!IsCellInBounds (x, y)) {
				return int.MinValue;
			}
			return GetValue (x, y);
		}

		private byte ColorToByte(Color color) {
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