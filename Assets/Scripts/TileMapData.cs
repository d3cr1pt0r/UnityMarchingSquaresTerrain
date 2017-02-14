using UnityEngine;

public class TileMapData {

	public static Vector4 GetTilingAndOffsetFromConfiguration(int configuration) {
		int index = GetIndexFromConfiguration (configuration);
		return GetTilingAndOffset (index);
	}

	// gets tiling and offset values from index
	private static Vector4 GetTilingAndOffset(int index) {
		float aspect = 0.25f;
		float x = (index % 4) * aspect;
		float y = ((int)(index / 4)) * aspect;

		return new Vector4 (aspect, aspect, x, y);
	}

	// get correct index from configuration (predefined tileset, make this better soon!)
	private static int GetIndexFromConfiguration(int configuration) {
		switch (configuration) {
			case 1:
				return 13;
			case 2:
				return 14;
			case 3:
				return 15;
			case 4:
				return 8;
			case 5:
				return 9;
			case 6:
				return 10;
			case 7:
				return 11;
			case 8:
				return 4;
			case 9:
				return 5;
			case 10:
				return 6;
			case 11:
				return 7;
			case 12:
				return 0;
			case 13:
				return 1;
			case 14:
				return 2;
			case 15:
				return 3;
			default:
				return 12;
		}
	}

}
