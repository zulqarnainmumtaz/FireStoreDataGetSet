using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class UserProfileManager : MonoBehaviour
{
    public InputField idInput;
    public InputField nameInput;
    public Button createProfileButton;
    public GameObject carSelectionPanel;

    private FirebaseManager firebaseManager;
    private CarSelection carSelection;

    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        carSelection = carSelectionPanel.GetComponent<CarSelection>();

        if (carSelection == null)
        {
            Debug.LogError("CarSelection component is not attached to carSelectionPanel.");
        }

        createProfileButton.onClick.AddListener(CreateUserProfile);

        // Initialize Google Play Games
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private async void CreateUserProfile()
    {
        string userId = await GetUserId();
        string userName = nameInput.text;

        // Automatically assign the user ID to the input field
        idInput.text = userId;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
        {
            Debug.LogWarning("ID and Name cannot be empty.");
            return;
        }

        // Check if the user profile already exists
        bool profileExists = await firebaseManager.CheckUserProfileExists(userId);

        if (profileExists)
        {
            Debug.Log("User profile already exists. Showing car selection.");
            await ShowCarSelection(userId);
        }
        else
        {
            UserProfile newProfile = new UserProfile(userId, userName);
            bool success = await firebaseManager.UpdateUserProfile(newProfile);

            if (success)
            {
                Debug.Log("User profile created successfully.");
                await ShowCarSelection(userId);
            }
            else
            {
                Debug.LogWarning("Failed to create user profile.");
            }
        }
    }

    private async Task<string> GetUserId()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            // Get user ID from Google Play Games
            return PlayGamesPlatform.Instance.GetUserId();
        }
        else
        {
            // Handle getting user ID from Gmail or fallback
            // Implement your own logic here to get the ID from Gmail or any other source
            // For now, return a dummy ID or handle as needed
            return "fallback_user_id";
        }
    }

    private async Task ShowCarSelection(string userId)
    {
        if (carSelection == null)
        {
            Debug.LogError("CarSelection is null in ShowCarSelection.");
            return;
        }

        await carSelection.Initialize(userId);

        // Hide the user profile creation panel and show the car selection panel
        gameObject.SetActive(false);
        carSelectionPanel.SetActive(true);
    }
}
