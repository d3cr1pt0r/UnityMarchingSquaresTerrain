using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarchingSquaresGenerator {
	
	public class Triangle {

		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;

		public Triangle(int vertexIndexA, int vertexIndexB, int vertexIndexC) {
			this.vertexIndexA = vertexIndexA;
			this.vertexIndexB = vertexIndexB;
			this.vertexIndexC = vertexIndexC;
		}

		public int GetIndex(int i) {
			if (i == 0)
				return vertexIndexA;
			if (i == 1)
				return vertexIndexB;
			if (i == 2)
				return vertexIndexC;
			return -1;
		}

		public bool Contains(int vertexIndex) {
			return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
		}

	}

}