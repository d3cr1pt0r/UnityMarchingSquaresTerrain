using UnityEngine;

namespace MapGenerator {

	public class Node {

		private Vector3 position;
		public int vertexIndex;
		public byte value;

		public Node(Vector3 position, byte value) {
			this.position = position;
			this.vertexIndex = -1;
			this.value = value;
		}

		public Vector3 GetPosition() {
			return position;
		}
	}
}