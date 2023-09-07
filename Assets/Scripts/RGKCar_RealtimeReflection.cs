using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("Racing Game Kit/Helpers/Car Realtime Reflection")]
[ExecuteInEditMode]
public class RGKCar_RealtimeReflection : MonoBehaviour
{
	public int cubemapSize = 128;

	public bool oneFacePerFrame;

	public Camera CubeCamera;

	private RenderTexture Renter2Texture;

	public GameObject cubeCamLocation;

	public LayerMask CullingMask;

	public float FarClipPlane = 250f;

	public Material Skybox;

	private void Start()
	{
		UpdateCubemap(63);
	}

	private void LateUpdate()
	{
		if (base.enabled)
		{
			if (oneFacePerFrame)
			{
				int num = Time.frameCount % 6;
				int faceMask = 1 << num;
				UpdateCubemap(faceMask);
			}
			else
			{
				UpdateCubemap(63);
			}
		}
	}

	private void UpdateCubemap(int faceMask)
	{
		if (CubeCamera == null)
		{
			GameObject gameObject = new GameObject("CubemapCamera", typeof(Camera));
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			CubeCamera = gameObject.GetComponent<Camera>();
		}
		if (CubeCamera != null)
		{
			CubeCamera.farClipPlane = FarClipPlane;
			CubeCamera.enabled = false;
			CubeCamera.cullingMask = CullingMask;
			if (cubeCamLocation != null)
			{
				CubeCamera.transform.parent = cubeCamLocation.transform.parent;
			}
			if (Skybox != null)
			{
				CubeCamera.gameObject.AddComponent(typeof(Skybox));
				Skybox skybox = CubeCamera.GetComponent(typeof(Skybox)) as Skybox;
				skybox.material = Skybox;
			}
		}
		if (cubeCamLocation != null)
		{
			CubeCamera.transform.localPosition = cubeCamLocation.transform.localPosition;
		}
		if (Renter2Texture == null)
		{
			Renter2Texture = new RenderTexture(cubemapSize, cubemapSize, 16);
			Renter2Texture.isPowerOfTwo = true;
			Renter2Texture.dimension = TextureDimension.Cube;
			Renter2Texture.hideFlags = HideFlags.HideAndDontSave;
			Material[] sharedMaterials = GetComponent<Renderer>().sharedMaterials;
			Material[] array = sharedMaterials;
			foreach (Material material in array)
			{
				if (material.HasProperty("_Cube"))
				{
					material.SetTexture("_Cube", Renter2Texture);
				}
			}
		}
		CubeCamera.RenderToCubemap(Renter2Texture, faceMask);
	}

	private void OnDisable()
	{
		UnityEngine.Object.DestroyImmediate(Renter2Texture);
	}
}
