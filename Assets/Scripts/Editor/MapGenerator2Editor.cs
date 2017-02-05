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

			if (GUILayout.Button ("Generate from texture")) {
				mapGenerator.GenerateFromTexture ();
				SceneView.RepaintAll ();
			}
			if (GUILayout.Button ("Generate random")) {
				mapGenerator.GenerateRandom ();
				SceneView.RepaintAll ();
			}
		}

	}
}