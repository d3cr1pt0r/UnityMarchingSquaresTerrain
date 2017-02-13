using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
	public class ControlNode : Node
	{
		public byte value;
		public Node nodeUp;
		public Node nodeRight;

		public ControlNode (Vector3 position, byte value) : base (position)
		{
			this.value = value;
			nodeUp = new Node (position + Vector3.forward * 0.5f);
			nodeRight = new Node (position + Vector3.right * 0.5f);
		}
	}
}