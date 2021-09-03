using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class RotacaoCartas : MonoBehaviour
    {
        RectTransform carta;
        public float vel = 0.1f;
        Vector3 mover;
        Vector3 pos_inicial;
        Vector3 pos_final;
        float cont =0;
        public float vel_giro=1.7f;
        CardDisplay card_display;
        bool frente = false;
        // Start is called before the first frame update
        void Start()
        {
            carta = GetComponent<RectTransform>();
            mover =  carta.localPosition;
            pos_inicial = carta.localPosition;
            pos_final = new Vector3(carta.localPosition.x-78,carta.localPosition.y,carta.localPosition.z);
            card_display = GetComponent<CardDisplay>();
            card_display.SetaVerso();
        }

        // Update is called once per frame
        void Update()
        {
            vai_carta();
        }

        public void vai_carta(){
            mover.x -= vel * Time.deltaTime;
            Debug.Log(carta.localRotation.w);
            carta.localPosition = Vector3.Lerp(pos_inicial,pos_final,cont);
            if(cont<1){
                cont += vel;
                carta.Rotate(0,1.7f,0);
            }else {
                carta.rotation = new Quaternion(0,0,0,0);
            }

            if(cont>0.9f && !frente){
                frente = true;
                card_display.SetaFrente();
            }
        }
    }
}
