using UnityEngine;

namespace MarchingSquaresGenerator
{
	
	public class SquareGrid
	{

		private Square[,] squares = null;

		public int width { get { return squares.GetLength (0); } private set { } }

		public int height { get { return squares.GetLength (1); } private set { } }

		public SquareGrid (Map map)
		{
			squares = new Square[map.width - 1, map.height - 1];

			for (int y = 0; y < width; y++) {
				for (int x = 0; x < height; x++) {
					float xPos = map.GetPosition (x, y).x + 0.5f;
					float yPos = map.GetPosition (x, y).y + 0.5f;

					ControlNode topLeft = new ControlNode (map.GetPosition (x, y + 1), map.GetValue (x, y + 1));
					ControlNode topRight = new ControlNode (map.GetPosition (x + 1, y + 1), map.GetValue (x + 1, y + 1));
					ControlNode bottomRight = new ControlNode (map.GetPosition (x + 1, y), map.GetValue (x + 1, y));
					ControlNode bottomLeft = new ControlNode (map.GetPosition (x, y), map.GetValue (x, y));

					Vector3 position = new Vector3 (xPos, yPos, 0);

					squares [x, y] = new Square (topLeft, topRight, bottomRight, bottomLeft, position);
				}
			}
		}

		public Square GetSquare (int x, int y)
		{
			if (squares == null)
				return null;

			return squares [x, y];
		}
	}
}