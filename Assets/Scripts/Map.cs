using UnityEngine;

namespace MapGenerator {
	
	public class Map {
		
		public byte[,] values;
		public Vector3[,] positions;

		public int width { get { return values.GetLength (0); } }

		public int height { get { return values.GetLength (1); } }

		public Map(int width, int height) {
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
	}
}