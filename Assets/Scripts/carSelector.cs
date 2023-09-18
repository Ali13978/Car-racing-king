using RacingGameKit;
using UnityEngine;

public class carSelector : MonoBehaviour
{
    public static carSelector Instance;

    public Race_Manager raceManagerInstance;

	public GameObject[] cars;

	private void Awake()
	{
        Instance = this;
		raceManagerInstance = UnityEngine.Object.FindObjectOfType<Race_Manager>();
		raceManagerInstance.HumanRacerPrefab = cars[PlayerPrefs.GetInt("SelectedVehicle")];
	}

	public GameObject GetSelectedCarGameObject(int SelectedVehicle)
    {
        return cars[SelectedVehicle];
    }
}
