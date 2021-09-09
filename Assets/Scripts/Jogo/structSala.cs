using Firebase.Firestore;

namespace Trunfo
{
    [FirestoreData]
    public struct structSala
    {
        [FirestoreProperty]
        public string Criador { get; set; }
        [FirestoreProperty]
        public string Adversario { get; set; }
        [FirestoreProperty]
        public bool MesaCriada { get; set; }
    }
}