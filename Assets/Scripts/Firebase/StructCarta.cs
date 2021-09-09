using Firebase.Firestore;

namespace Trunfo
{
    [FirestoreData]
    public struct StructCarta
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public int Criterio { get; set; }
        [FirestoreProperty]
        public bool JogadorDoTurnoGanha { get; set; }
    }
}