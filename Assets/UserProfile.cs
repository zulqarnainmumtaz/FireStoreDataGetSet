using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public class UserProfile
{
    [FirestoreProperty]
    public string Id { get; set; }

    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public List<Car> Cars { get; set; } = new List<Car>();

    public UserProfile() { }

    public UserProfile(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
