using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Niantic.Lightship.AR.LocationAR;

public class GameController : MonoBehaviour
{
    // Singleton instance
    public static GameController Instance { get; private set; }

    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button changeLocationButton;
    public Button sTButton;
    public Button distanceButton;
    [SerializeField] private TextMeshProUGUI logText; // Log message display
    private float distanceToPhone;
    public GameObject rawImage1; 
    public GameObject rawImage2; 
    public GameObject rawImage3; 
    private ARLocationManager _arLocationManager;
    private ARLocation[] _locations;
    public static int CurrentLocationIndex { get; set; }

    public Dictionary<int, Button> locationButtons;
    public Dictionary<int, int> locationPoints = new Dictionary<int, int>();

    [SerializeField] private GameObject mapPanel;

    void Awake()
    {
        // Ensure the instance is set up correctly
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep instance across scenes
    }

    void Start()
    {
        logText.text = ""; // Initialize log text
        _arLocationManager = FindObjectOfType<ARLocationManager>();
        List<ARLocation> locationList = new List<ARLocation>(_arLocationManager.ARLocations);
        _locations = locationList.ToArray();


        for (int i = 0; i < _locations.Length; i++)
        {
            locationPoints[i] = 0;
        }

        locationButtons = new Dictionary<int, Button>
        {
            { 1, Button1 },
            { 2, Button2 },
            { 3, Button3 }
        };

        Button1.onClick.AddListener(() => SwitchToLocation(1));
        Button2.onClick.AddListener(() => SwitchToLocation(2));
        Button3.onClick.AddListener(() => SwitchToLocation(3));


        sTButton.gameObject.SetActive(true);
        changeLocationButton.gameObject.SetActive(false);

        changeLocationButton.onClick.AddListener(ChangeLocation);
        sTButton.onClick.AddListener(StopTrack);

        distanceButton.gameObject.SetActive(true);
        distanceButton.onClick.AddListener(printDistance);

        rawImage1.SetActive(false);
        rawImage2.SetActive(false);
        rawImage3.SetActive(false);

        
    }

    private void OnDisable()
    {
        Button1.onClick.RemoveAllListeners();
        Button2.onClick.RemoveAllListeners();
        Button3.onClick.RemoveAllListeners();
        changeLocationButton.onClick.RemoveAllListeners();
        sTButton.onClick.RemoveAllListeners();
        distanceButton.onClick.RemoveAllListeners();
    }

    
    private void StopTrack()
    {
        _arLocationManager.StopTracking();
        logText.text="";
        logText.text = "Stop Tracking and ready for reset.\n";
        sTButton.gameObject.SetActive(false);

        rawImage1.SetActive(false);
        rawImage2.SetActive(false);
        rawImage3.SetActive(false);
    }

    public void SwitchToLocation(int locationIndex)
    {

         mapPanel.SetActive(false);
        if (locationIndex < 0 || locationIndex >= _locations.Length)
        {
            logText.text = "Invalid location index\n";
            return;
        }
        // Measure the distance to the selected location
        GameObject selectedLocation = _locations[locationIndex].gameObject;
        distanceToPhone = MeasureDistanceToPhone(selectedLocation);

        CurrentLocationIndex = locationIndex;
        _arLocationManager.StopTracking();
        _arLocationManager.SetARLocations(new[] { _locations[locationIndex] });
        _arLocationManager.StartTracking();

        logText.text = $"Stay still to scan!\nScanning location {locationIndex}: {_locations[locationIndex].gameObject.name}";
        changeLocationButton.gameObject.SetActive(true);

        switch (locationIndex)
    {
        case 1:
            rawImage1.SetActive(true);
            rawImage2.SetActive(false);
            rawImage3.SetActive(false);
            break;
        case 2:
            rawImage2.SetActive(true);
            rawImage1.SetActive(false);
            rawImage3.SetActive(false);
            break;
        case 3:
            rawImage3.SetActive(true);
            rawImage1.SetActive(false);
            rawImage2.SetActive(false);
            break;
        default:
            logText.text += "No associated panel for this location.\n";
            break;
    }
    }

    private void ChangeLocation()
    {
        _arLocationManager.StopTracking();
        logText.text="";
        logText.text = "Tracking stopped and reset.\n";
        changeLocationButton.gameObject.SetActive(false);
    }

        private void printDistance()
    {
        logText.text = "";
        logText.text = $"{_locations[CurrentLocationIndex].gameObject.name} is within {distanceToPhone:F2} meters\n";

    }

    private float MeasureDistanceToPhone(GameObject arObject)
    {
        Vector3 phonePosition = Camera.main.transform.position;
        Vector3 objectPosition = arObject.transform.position;
        return Vector3.Distance(phonePosition, objectPosition);
    }


}





