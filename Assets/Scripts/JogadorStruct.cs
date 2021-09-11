using Firebase.Firestore;
//Struct que representa o jogador
namespace Trunfo
{
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
}