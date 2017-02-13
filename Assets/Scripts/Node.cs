using UnityEngine;

namespace MapGenerator
{

	public class Node
	{
		private Vector3 position;
		public int vertexIndex;

		public Node (Vector3 position)
		{
			this.position = position;
			this.vertexIndex = -1;
		}

		public Vector3 GetPosition ()
		{
			return position;
		}
	}
}