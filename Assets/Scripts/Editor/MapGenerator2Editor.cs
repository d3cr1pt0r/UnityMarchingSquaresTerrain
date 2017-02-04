using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapGenerator {
	
	[CustomEditor(typeof(MapGenerator))]
	public class MapGenerator2Editor : Editor {

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector ();

			MapGenerator mapGenerator = target as MapGenerator;

			if (GUILayout.Button ("Generate")) {
				mapGenerator.GenerateMap ();
				SceneView.RepaintAll ();
			}
		}

	}
}