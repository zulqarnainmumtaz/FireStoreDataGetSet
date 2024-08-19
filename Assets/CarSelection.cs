using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    public FirebaseManager firebaseManager;
    public string currentUserId;

    [SerializeField] private List<Car> availableCars = new List<Car>();
    [SerializeField] private List<Car> ownedCars = new List<Car>();

    [SerializeField] private InputField carIdInputField;
    [SerializeField] private InputField carNameInputField;
    [SerializeField] private InputField carModelInputField;
    [SerializeField] private InputField carYearInputField;

    void Start()
    {
        if (firebaseManager == null)
        {
            firebaseManager = FindObjectOfType<FirebaseManager>();
        }
    }

    public async Task Initialize(string userId)
    {
        currentUserId = userId;

        if (string.IsNullOrEmpty(currentUserId))
        {
            Debug.LogWarning("Current user ID is not set.");
            return;
        }

        await LoadAvailableCars();
        await LoadOwnedCars();
    }

    private async Task LoadAvailableCars()
    {
        //try
        //{
        //    // Fetch available cars from Firestore
        //    List<Car> fetchedCars = await firebaseManager.GetAvailableCars();

        //    // Clear the existing list to avoid duplicates
        //    availableCars.Clear();

        //    // Add the fetched cars to the availableCars list
        //    availableCars.AddRange(fetchedCars);

        //    // Log the cars to ensure they are correctly loaded
        //    foreach (var car in availableCars)
        //    {
        //        Debug.Log($"Available Car: {car.Name} - {car.Model} ({car.Year})");
        //    }
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError("Failed to load available cars: " + e.Message);
        //}
    }

    private async Task LoadOwnedCars()
    {
        try
        {
            ownedCars = await firebaseManager.GetCarsOwnedByUser(currentUserId);
            foreach (var car in ownedCars)
            {
                Debug.Log($"Owned Car: {car.Name} - {car.Model} ({car.Year})");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load owned cars: " + e.Message);
        }
    }
    //assign all data in inspector


    public async void AddNewCar()
    {
        string id = carIdInputField.text;
        string name = carNameInputField.text;
        string model = carModelInputField.text;
        int year;

        if (!int.TryParse(carYearInputField.text, out year))
        {
            Debug.LogWarning("Invalid year input.");
            return;
        }

        Car newCar = new Car(id, name, model, year);

        try
        {
            bool success = await firebaseManager.AddCarToUserProfile(currentUserId, newCar);

            if (success)
            {
                Debug.Log("New car added to user profile.");
                availableCars.Add(newCar);
            }
            else
            {
                Debug.LogWarning("Failed to add car.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error adding new car: " + e.Message);
        }
    }
}
