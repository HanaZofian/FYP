using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Niantic.Lightship.AR.LocationAR;

public class GameInterface : MonoBehaviour
{
    // Declare the buttons and panels
    [SerializeField] private Button closeButton; // Map panel
    [SerializeField] private Button closeButton2; // badge frame panel
    [SerializeField] private Button closeButton3; // Image hint panel

    [SerializeField] private Button nextButton; // Next button

    [SerializeField] private Button imageButton; // badge button
     [SerializeField] private Button imageHintButton; // imagehint button
    [SerializeField] private Button mapButton; // Map button

    [SerializeField] private GameObject mapPanel; // Map panel
    [SerializeField] private GameObject imageFramePanel; // Image frame panel
    [SerializeField] private GameObject pointPanel; // Image frame panel
    [SerializeField] private GameObject imageHintPanel; // Image frame panel

    [SerializeField] private TextMeshProUGUI logText;

    // Start is called before the first frame update
    void Start()
    {
        mapButton.onClick.AddListener(ShowMapPanel);
        closeButton.onClick.AddListener(HideMapPanel);
        closeButton2.onClick.AddListener(HideImageFramePanel);
        closeButton3.onClick.AddListener(HideImageHintPanel);

        imageButton.onClick.AddListener(DisplayImage);
        imageHintButton.onClick.AddListener(ShowImageHintPanel);

        nextButton.onClick.AddListener(NextButtonClicked);

        // Initial state of panels and buttons
        mapPanel.SetActive(false);
        pointPanel.SetActive(false);
        imageFramePanel.SetActive(false);
       // changeLocationButton.gameObject.SetActive(false);
    }

 
    // Show the map panel
    void ShowMapPanel()
    {
        mapPanel.SetActive(true); // Activate the map panel
    }

    // Hide the map panel
    void HideMapPanel()
    {
        mapPanel.SetActive(false); // Deactivate the map panel
    }

    // Hide the image frame panel
    void HideImageFramePanel()
    {
        imageFramePanel.SetActive(false); // Deactivate the image frame panel
    }

    void HideImageHintPanel()
    {
        imageHintPanel.SetActive(false);
    }

    // Open the image frame panel when next button is clicked
    void NextButtonClicked()
    {
        pointPanel.SetActive(false);
        imageFramePanel.SetActive(true);  // Show the image frame panel
        
    }

    void ShowImageHintPanel(){
        imageHintPanel.SetActive(true);
    }

  
    void DisplayImage()
    {
    imageFramePanel.SetActive(true);
    }
}
