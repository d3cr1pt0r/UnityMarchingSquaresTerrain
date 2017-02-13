using UnityEngine;

namespace MapGenerator
{

	public class Square
	{

		public ControlNode TopLeft;
		public ControlNode TopRight;
		public ControlNode BottomRight;
		public ControlNode BottomLeft;

		public Node TopCenter;
		public Node RightCenter;
		public Node BottomCenter;
		public Node LeftCenter;

		private Vector3 position;
		private int configuration;

		public Square (ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft, Vector3 position = default(Vector3))
		{
			this.position = position;

			TopLeft = topLeft;
			TopRight = topRight;
			BottomRight = bottomRight;
			BottomLeft = bottomLeft;

			TopCenter = topLeft.nodeRight;
			RightCenter = bottomRight.nodeUp;
			BottomCenter = bottomLeft.nodeRight;
			LeftCenter = bottomLeft.nodeUp;

			configuration = topLeft.value | (topRight.value << 1) | (bottomRight.value << 2) | (bottomLeft.value << 3);
		}

		public Vector3 GetPosition ()
		{
			return position;
		}

		public int GetConfiguration ()
		{
			return configuration;
		}
	}
}