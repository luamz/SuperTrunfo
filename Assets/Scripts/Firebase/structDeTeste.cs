
using Firebase.Firestore;

namespace Trunfo{
    [FirestoreData]
    public struct Teste{
        [FirestoreProperty]
        public int Valor { get; set; }

        public override string ToString()
        {
            return Valor.ToString();
        }
    }
}