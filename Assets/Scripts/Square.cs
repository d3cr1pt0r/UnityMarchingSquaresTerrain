using UnityEngine;

namespace MapGenerator {

	public class Square {

		public Node TopLeft;
		public Node TopRight;
		public Node BottomRight;
		public Node BottomLeft;

		private Vector3 position;
		private int configuration;

		public Square(Node topLeft, Node topRight, Node bottomRight, Node bottomLeft, Vector3 position = default(Vector3)) {
			this.position = position;

			TopLeft = topLeft;
			TopRight = topRight;
			BottomRight = bottomRight;
			BottomLeft = bottomLeft;

			configuration = topLeft.value | (topRight.value << 1) | (bottomRight.value << 2) | (bottomLeft.value << 3);
		}

		public Vector3 GetPosition() {
			return position;
		}

		public int GetConfiguration() {
			return configuration;
		}
	}
}