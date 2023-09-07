using UnityEngine;

public class RayBehavior : MonoBehaviour
{
	public GameObject BeginLocation;

	public GameObject EndLocation;

	public Color BeginColor = Color.white;

	public Color EndColor = Color.white;

	public Vector3 PositionRange;

	public float WidthA = 1f;

	public float WidthB = 1f;

	public float RadiusA = 1f;

	public float RadiusB = 1f;

	private LineRenderer Line;

	private Animation Anim;

	private bool changed = true;

	private Vector3 Offset;

	public float AlphaCurve;

	public float FadeSpeed = 1f;

	public void ResetRay()
	{
		Offset = new Vector3(UnityEngine.Random.Range(0f - PositionRange.x, PositionRange.x), UnityEngine.Random.Range(0f - PositionRange.y, PositionRange.y), UnityEngine.Random.Range(0f - PositionRange.z, PositionRange.z));
		changed = true;
	}

	public void UpdateLineData()
	{
		Line.SetPosition(0, BeginLocation.transform.position + Offset * RadiusA);
		Line.SetPosition(1, EndLocation.transform.position + Offset * RadiusB);
		Line.SetWidth(WidthA, WidthB);
	}

	private void Start()
	{
		Line = GetComponent<LineRenderer>();
		Anim = GetComponent<Animation>();
		Anim["RayAlphaCurve"].speed = FadeSpeed;
	}

	private void Update()
	{
		if (changed)
		{
			changed = false;
			UpdateLineData();
		}
		Line.SetColors(new Color(BeginColor.r, BeginColor.g, BeginColor.b, AlphaCurve), new Color(EndColor.r, EndColor.g, EndColor.b, AlphaCurve));
	}
}
