using Firebase.Firestore;

//Struct que representa os baralhos no firestore
namespace Trunfo{
    [FirestoreData]
    public struct StructBaralho{
        [FirestoreProperty]
        public string[] BaralhoCriador{get; set;}
        [FirestoreProperty]
        public string[] BaralhoAdversario{get; set;}

    }
}