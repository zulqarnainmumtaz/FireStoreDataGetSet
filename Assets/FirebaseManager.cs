using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    // Add a car to a user's profile
    public async Task<bool> AddCarToUserProfile(string userId, Car car)
    {
        var docRef = db.Collection("users").Document(userId);

        try
        {
            // Fetch user profile
            var snapshot = await docRef.GetSnapshotAsync();
            UserProfile profile;

            if (snapshot.Exists)
            {
                profile = snapshot.ConvertTo<UserProfile>();
            }
            else
            {
                // If the profile does not exist, create a new one
                profile = new UserProfile(userId, "DefaultName"); // Replace "DefaultName" with actual name
            }

            // Add the new car to the user's profile
            profile.Cars.Add(car);

            // Update the user document in Firestore
            await docRef.SetAsync(profile, SetOptions.MergeAll);
            Debug.Log("Car added to user's profile.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to add car and associate with user: " + e.Message);
            return false;
        }
    }

    // Get all cars owned by a user
    public async Task<List<Car>> GetCarsOwnedByUser(string userId)
    {
        var docRef = db.Collection("users").Document(userId);
        var snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            var profile = snapshot.ConvertTo<UserProfile>();
            return profile.Cars;
        }
        else
        {
            Debug.LogWarning("User profile not found.");
            return new List<Car>();
        }
    }
    public async Task<bool> CheckUserProfileExists(string userId)
    {
        // Implement the logic to check if a user profile with the given userId exists
        // This will involve querying your Firestore database for the user document
        try
        {
            var userDoc = await FirebaseFirestore.DefaultInstance
                .Collection("users")
                .Document(userId)
                .GetSnapshotAsync();

            return userDoc.Exists;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error checking user profile existence: " + e.Message);
            return false;
        }
    }
    // Update a user profile
    public async Task<bool> UpdateUserProfile(UserProfile profile)
    {
        var docRef = db.Collection("users").Document(profile.Id);

        try
        {
            await docRef.SetAsync(profile, SetOptions.MergeAll);
            Debug.Log("User profile updated.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to update user profile: " + e.Message);
            return false;
        }
    }
}
