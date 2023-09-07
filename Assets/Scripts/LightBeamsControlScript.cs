using UnityEngine;

public class LightBeamsControlScript : MonoBehaviour
{
	public GameObject SourceObject;

	public GameObject TargetObject;

	public GameObject RayPrefab;

	public Color RayColor;

	public Vector3 PositionRange = Vector3.zero;

	public float RadiusA;

	public float RadiusB;

	public float WidthA;

	public float WidthB;

	public float FadeSpeed = 1f;

	public int NumRays = 10;

	private int Spawned;

	private float TimeToSpawnAll = 3f;

	private float spawnInterval = 1f;

	private float currentCountdown;

	private RayBehavior[] rays;

	private void setRayValues(RayBehavior ray)
	{
		ray.PositionRange = PositionRange;
		ray.BeginLocation = SourceObject;
		ray.EndLocation = TargetObject;
		ray.BeginColor = RayColor;
		ray.EndColor = RayColor;
		ray.WidthA = WidthA;
		ray.WidthB = WidthB;
		ray.RadiusA = RadiusA;
		ray.RadiusB = RadiusB;
		ray.FadeSpeed = FadeSpeed;
		ray.ResetRay();
	}

	private void SpawnRay()
	{
		if (Spawned < NumRays)
		{
			rays[Spawned] = UnityEngine.Object.Instantiate(RayPrefab).GetComponent<RayBehavior>();
			setRayValues(rays[Spawned]);
		}
		Spawned++;
		currentCountdown = spawnInterval;
	}

	private void Start()
	{
		spawnInterval = TimeToSpawnAll / (float)NumRays;
		rays = new RayBehavior[NumRays];
		SpawnRay();
	}

	private void Update()
	{
		if (Spawned < NumRays)
		{
			if (currentCountdown <= 0f)
			{
				SpawnRay();
			}
			currentCountdown -= Time.deltaTime;
		}
	}
}
