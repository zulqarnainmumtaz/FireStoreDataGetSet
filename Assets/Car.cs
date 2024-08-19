using Firebase.Firestore;

[FirestoreData]
public class Car
{
    [FirestoreProperty]
    public string Id { get; set; }

    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Model { get; set; }

    [FirestoreProperty]
    public int Year { get; set; }

    public Car() { }

    public Car(string id, string name, string model, int year)
    {
        Id = id;
        Name = name;
        Model = model;
        Year = year;
    }
}
