using Firebase.Firestore;

namespace Trunfo{
    [FirestoreData]
    public struct StructBaralho{
        [FirestoreProperty]
        public string[] Baralho{get; set;}
    }
}