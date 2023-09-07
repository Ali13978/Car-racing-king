using UnityEngine;

namespace EarthFX
{
	public class SurfaceTextureDedector
	{
		public static float[] GetTextureMix(Vector3 worldPos)
		{
			try
			{
				Terrain activeTerrain = Terrain.activeTerrain;
				TerrainData terrainData = activeTerrain.terrainData;
				Vector3 position = activeTerrain.transform.position;
				float num = worldPos.x - position.x;
				Vector3 size = terrainData.size;
				int num2 = (int)(num / size.x * (float)terrainData.alphamapWidth);
				float num3 = worldPos.z - position.z;
				Vector3 size2 = terrainData.size;
				int num4 = (int)(num3 / size2.z * (float)terrainData.alphamapHeight);
				float num5 = num2;
				Vector3 size3 = terrainData.size;
				if (num5 >= size3.x)
				{
					Vector3 size4 = terrainData.size;
					num2 = (int)size4.x - 1;
				}
				float num6 = num4;
				Vector3 size5 = terrainData.size;
				if (num6 >= size5.z)
				{
					Vector3 size6 = terrainData.size;
					num4 = (int)size6.z - 1;
				}
				float[,,] alphamaps = terrainData.GetAlphamaps(num2, num4, 1, 1);
				float[] array = new float[alphamaps.GetUpperBound(2) + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = alphamaps[0, 0, i];
				}
				return array;
			}
			catch
			{
				return null;
			}
		}

		public static int GetMainTexture(Vector3 worldPos)
		{
			try
			{
				float[] textureMix = GetTextureMix(worldPos);
				float num = 0f;
				int result = 0;
				if (textureMix != null)
				{
					for (int i = 0; i < textureMix.Length; i++)
					{
						if (textureMix[i] > num)
						{
							result = i;
							num = textureMix[i];
						}
					}
				}
				return result;
			}
			catch
			{
				return 0;
			}
		}
	}
}
