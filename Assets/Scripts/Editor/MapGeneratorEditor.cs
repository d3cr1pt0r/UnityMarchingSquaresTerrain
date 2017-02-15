using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MarchingSquaresGenerator
{
	[CustomEditor (typeof(MapGenerator))]
	public class MapGeneratorEditor : Editor
	{

		public override void OnInspectorGUI ()
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
			if (GUILayout.Button ("Clear")) {
				mapGenerator.Clear ();
				SceneView.RepaintAll ();
			}
		}

	}
}