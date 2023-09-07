using RacingGameKit;
using UnityEngine;

public class carSelector : MonoBehaviour
{
	public Race_Manager raceManagerInstance;

	public GameObject[] cars;

	private void Awake()
	{
		raceManagerInstance = UnityEngine.Object.FindObjectOfType<Race_Manager>();
		raceManagerInstance.HumanRacerPrefab = cars[PlayerPrefs.GetInt("SelectedVehicle")];
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
