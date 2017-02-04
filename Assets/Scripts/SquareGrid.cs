using UnityEngine;

namespace MapGenerator {
	
	public class SquareGrid {

		private Square[,] squares = null;

		public int width { get { return squares.GetLength (0); } private set { } }

		public int height { get { return squares.GetLength (1); } private set { } }

		public SquareGrid(Map map) {
			squares = new Square[map.width - 1, map.height - 1];

			for (int y = 0; y < width; y++) {
				for (int x = 0; x < height; x++) {
					float xPos = width * 0.5f - x;
					float yPos = height * 0.5f - y;

					Node topLeft = new Node (map.GetPosition(x, y), map.GetValue(x, y));
					Node topRight = new Node (map.GetPosition(x+1, y), map.GetValue(x+1, y));
					Node bottomRight = new Node (map.GetPosition(x+1, y+1), map.GetValue(x+1, y+1));
					Node bottomLeft = new Node (map.GetPosition(x, y+1), map.GetValue(x, y+1));

					Vector3 position = new Vector3 (xPos, 0, yPos);

					squares [x, y] = new Square (topLeft, topRight, bottomRight, bottomLeft, position);
				}
			}
		}

		public Square GetSquare(int x, int y) {
			if (squares == null) return null;

			return squares [x, y];
		}
	}
}