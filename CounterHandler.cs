using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterHandler : MonoBehaviour
{
    [SerializeField] private GameObject pointPanel; // The panel that appears after each location index
    [SerializeField] private GameObject ImageFramePanel1; // Frame1
    [SerializeField] private GameObject ImageFramePanel2; // Frame2
    [SerializeField] private GameObject ImageFramePanel3; // Frame3

    [SerializeField] private Button resetButton;
    [SerializeField] private Button stButton;
    // [SerializeField] private Button cheatButton;

    [SerializeField] private TextMeshProUGUI FrameText; // Amount of frame removed
    [SerializeField] private TextMeshProUGUI contextText; // The context of location when the object is pressed

    [SerializeField] private TextMeshProUGUI FrameScoreText; // Frame x/3
    [SerializeField] private TextMeshProUGUI landmarkText; // Landmark x/3

    [SerializeField] private TextMeshProUGUI TokenText;
    [SerializeField] private TextMeshProUGUI logText; // Log message display

    private int totalFrameScore = 0;
    private int maxFrameScore = 3;
    private int TotalLandmarks = 3;

    private int landmarksCompleted = 0;

    private int frameRemoved = 0;
    private string contextMessage = "";

    private GameController gameController;

    void Start()
    {
        // Use the singleton instance of GameController
        gameController = GameController.Instance;


        resetButton.onClick.AddListener(ResetScore);

        // Initialize UI
        FrameScoreText.text = $"{totalFrameScore}/{maxFrameScore}";
        landmarkText.text = $"{landmarksCompleted}/{TotalLandmarks}";

        pointPanel.SetActive(false);
        ImageFramePanel1.SetActive(true);
        ImageFramePanel2.SetActive(true);
        ImageFramePanel3.SetActive(true);
        logText.text = ""; // Initialize log text

     //   cheatButton.onClick.AddListener(CheatScore);

    }

    void Update()
    {
        // Check if the player clicked on an object
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickedObject = GetClickedObject(out RaycastHit hit);
            if (clickedObject != null)
            {
                AssignPointsBasedOnLocation(clickedObject);
            }
        }
    }

    // Raycast to find clicked object
    GameObject GetClickedObject(out RaycastHit hit)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }


    // Assign points based on the clicked location
    private void AssignPointsBasedOnLocation(GameObject clickedObject)
    {
        int currentLocationIndex = GameController.CurrentLocationIndex;

        // Assign points and messages based on location index
        switch (currentLocationIndex)
        {
            case 1:
                frameRemoved = 1;
                ImageFramePanel1.SetActive(false);
                 contextMessage = "The building on the west:\n"+"Royal Selangor Club\n" +
                 "Established in 1884 as a recreational club for British colonial officers";
                break;
            case 2:
                frameRemoved = 1;
                ImageFramePanel2.SetActive(false);
                contextMessage = "The British Union Jack flag was lowered & the Malaysian flag was hoisted for the first time";                break;
            case 3:
                frameRemoved = 1;
                ImageFramePanel3.SetActive(false);
                contextMessage = "The building on the east:\n"+"Sultan Abdul Samad Building\n" + 
                "Established in 1897 as a symbol of British authority in the Malay States";
                break;
            default:
                Debug.LogWarning("Unknown location index");
                return;
        }

        // Check if points for this location have already been earned
        if (gameController.locationPoints[currentLocationIndex] == 0) // Points haven't been awarded yet
        {
            // Award points and update data
            gameController.locationPoints[currentLocationIndex] = frameRemoved;
            totalFrameScore += frameRemoved;
            landmarksCompleted++;

            // Update UI
            FrameScoreText.text = $"{totalFrameScore}/{maxFrameScore}";
            FrameText.text = $"{frameRemoved} Frame";
            contextText.text = contextMessage;
            landmarkText.text = $"{landmarksCompleted}/{TotalLandmarks}";

            // Update log text
            logText.text = ""; // Clear previous text
            logText.text = $"Clicked on {clickedObject.name} at location {currentLocationIndex}. Removed {frameRemoved} frame.\n";
            pointPanel.SetActive(true);

            // Hide buttons for completed location
            gameController.locationButtons[currentLocationIndex].gameObject.SetActive(false);
            gameController.changeLocationButton.gameObject.SetActive(false);

            // Check if all frames are collected and show token
            if (totalFrameScore == maxFrameScore)
            {
                logText.text = ""; // Clear previous text
                logText.text = "Congratulations! You earned a historical stamp!";
                TokenText.gameObject.SetActive(true);
            }
        }
        else
        {
            logText.text = ""; // Clear previous text
            logText.text = $"You have already explored this location: {currentLocationIndex}.\n";
        }
    }

    private void ResetScore()
{
    stButton.gameObject.SetActive(true);
    TokenText.gameObject.SetActive(false);

    totalFrameScore = 0;
    landmarksCompleted = 0;
    ImageFramePanel1.SetActive(true); ImageFramePanel2.SetActive(true); ImageFramePanel3.SetActive(true);
 
    FrameScoreText.text = $"{totalFrameScore}/{maxFrameScore}";
    landmarkText.text = $"{landmarksCompleted}/{TotalLandmarks}";

    logText.text = ""; // Clear previous text


        gameController.locationPoints[1] = 0;
        gameController.locationButtons[1].gameObject.SetActive(true); 
        
        gameController.locationPoints[2] = 0;
        gameController.locationButtons[2].gameObject.SetActive(true); 

        gameController.locationPoints[3] = 0;
        gameController.locationButtons[3].gameObject.SetActive(true); 

    logText.text = "Scores and landmarks have been reset.\n";


     
    
}

// private void CheatScore()
// {
//     int currentLocationIndex = GameController.CurrentLocationIndex;

   
//         // Assign points and messages based on location index
//         switch (currentLocationIndex)
//         {
//             case 1:
//                 frameRemoved = 1;
//                 ImageFramePanel1.SetActive(false);
//                  contextMessage = "The building on the west:\n"+"Royal Selangor Club\n" +
//                  "Established in 1884 as a recreational club for British colonial officers";
//                 break;
//             case 2:
//                 frameRemoved = 1;
//                 ImageFramePanel2.SetActive(false);
//                 contextMessage = "The British Union Jack flag was lowered & the Malaysian flag was hoisted for the first time";                break;
//             case 3:
//                 frameRemoved = 1;
//                 ImageFramePanel3.SetActive(false);
//                 contextMessage = "The building on the east:\n"+"Sultan Abdul Samad Building\n" + 
//                 "Established in 1897 as a symbol of British authority in the Malay States";
//                 break;
//             default:
//                 Debug.LogWarning("Unknown location index");
//                 return;
//         }

//         // Check if points for this location have already been earned
//         if (gameController.locationPoints[currentLocationIndex] == 0) // Points haven't been awarded yet
//         {
//             // Award points and update data
//             gameController.locationPoints[currentLocationIndex] = frameRemoved;
//             totalFrameScore += frameRemoved;
//             landmarksCompleted++;

//             // Update UI
//             FrameScoreText.text = $"{totalFrameScore}/{maxFrameScore}";
//             FrameText.text = $"{frameRemoved} Frame";
//             contextText.text = contextMessage;
//             landmarkText.text = $"{landmarksCompleted}/{TotalLandmarks}";

//             // Update log text
//             logText.text = ""; // Clear previous text
//             logText.text = $"Clicked at location {currentLocationIndex}. Removed {frameRemoved} frame.\n";
//             pointPanel.SetActive(true);

//             // Hide buttons for completed location
//             gameController.locationButtons[currentLocationIndex].gameObject.SetActive(false);
//             gameController.changeLocationButton.gameObject.SetActive(false);

//             // Check if all frames are collected and show token
//             if (totalFrameScore == maxFrameScore)
//             {
//                 logText.text = ""; // Clear previous text
//                 logText.text = "Congratulations! You earned a historical stamp!";
//                 TokenText.gameObject.SetActive(true);
//             }
//         }
//         else
//         {
//             logText.text = ""; // Clear previous text
//             logText.text = $"You have already explored this location: {currentLocationIndex}.\n";
//         }
// }


  
}

