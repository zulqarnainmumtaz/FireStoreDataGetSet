using System.Threading.Tasks;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    private UserProfileManager userProfileManager;
    private FirebaseManager firebaseManager;

    void Start()
    {
        userProfileManager = FindObjectOfType<UserProfileManager>();
        firebaseManager = FindObjectOfType<FirebaseManager>();

        // Example of how to add a car to Firestore
        //Car newCar = new Car("car123", "Ferrari", "488 GTB", 2020);
        //Task<bool> carAdded = firebaseManager.AddCar(newCar);

        // Other examples can go here, such as deleting a car, buying a car, etc.
    }
}
