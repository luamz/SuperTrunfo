using Firebase.Firestore;

namespace Trunfo{
    [FirestoreData]
    public struct StructCarta{
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string[] Baralho{get; set;} // apagar dps
        [FirestoreProperty]
        public bool JogadorDoTurnoGanha { get; set; }
    }
}