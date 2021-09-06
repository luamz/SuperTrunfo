using Firebase.Firestore;

namespace Trunfo{
    [FirestoreData]
    public struct StructBaralho{
        [FirestoreProperty]
        public string[] Cartas{get; set;}
    }
}