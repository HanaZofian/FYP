using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script stores data about an object in a persistent structure so it can be saved and reloaded with its properties.

public class PersistantObject : MonoBehaviour
{
    // The ID of the prefab that this object is based on, set in the Unity Inspector.
    [SerializeField] private string prefabId;
    
    // The unique location ID for tracking this object's specific AR location, set in the Inspector.
    [SerializeField] private string locationID;

    // Property for accessing and setting the prefab ID.
    public string PrefabID
    {
        get => prefabId;
        set => prefabId = value;
    }

    // Stores a unique UUID for this object, which is set when the object is created.
    private string objectUUID;

    // Property for accessing and setting the UUID.
    public string ObjectUUID
    {
        set => objectUUID = value;
        get => objectUUID;
    }

    // Event to broadcast the object's data when it is saved. Other scripts can subscribe to this event to handle the data.
    public static event Action<PersistantObjectData> myPersistantData; 

    // Called when the object is first initialized in the scene.
    void Start()
    {
        // Generate a UUID for the object if one hasn't been assigned already.
        if (string.IsNullOrEmpty(objectUUID))
            objectUUID = CreateUUID();

        // Immediately save the object's data, passing the location ID as the parameter.
        SaveData(locationID);
    }

    // Method for creating a new UUID for this object using the System.Guid class.
    string CreateUUID()
    {
       return Guid.NewGuid().ToString();
    }

    // Method to save the object's data into a structure and trigger an event to store it.
    void SaveData(string ARLocation)
    {
        // Create a new instance of PersistantObjectData, storing key information about the object (location, ID, transform data).
        PersistantObjectData objectData = new PersistantObjectData(
            ARLocation,
            prefabId,
            objectUUID,
            transform.position,
            transform.localScale,
            transform.rotation
        );
        
        // Trigger the myPersistantData event, passing the newly created objectData to listeners.
        myPersistantData?.Invoke(objectData);
    }
}

// Struct to hold the persistent data for an object, which will be serialized and stored.
public struct PersistantObjectData
{
    // Variables for storing key data of the object (location, ID, transform properties).
    public string _locationID;      // The ID for the AR location of this object.
    public string _prefabID;        // The ID of the prefab this object was created from.
    public string _uuid;            // Unique UUID for this instance of the object.
    public Vector3 _position;       // Position of the object in the scene.
    public Vector3 _localScale;     // Scale of the object.
    public Quaternion _rotation;    // Rotation of the object.

    // Constructor to initialize a new instance of PersistantObjectData with values for each field.
    public PersistantObjectData(string locationID, string prefabID, string uuid, Vector3 position, Vector3 localScale,
        Quaternion rotation)
    {
        _locationID = locationID;
        _prefabID = prefabID;
        _uuid = uuid;
        _position = position;
        _localScale = localScale;
        _rotation = rotation;
    }
}
