using Firebase.Firestore;

[FirestoreData]
public struct JogadorStruct
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string UserToken { get; set; }

    [FirestoreProperty]
    public bool Status { get; set; }
}
