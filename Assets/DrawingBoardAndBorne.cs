using UnityEngine;
using System.Collections.Generic;

public class DrawingBoard : MonoBehaviour
{   

    public Canvas canvas;
    public RectTransform card;
    public RectTransform Borne;

    public Canvas canvas_1;
    public Canvas canvas_2;
    public Canvas canvas_3;

    public List<RectTransform> ListOfBorne;


    public void DrawingBorneOnBoard(List<List<RectTransform>> Board){        

        float w = canvas.GetComponent<RectTransform>().rect.width;

        for (int i = 0;i<9;i++){
            RectTransform obj = Instantiate(Borne);
            obj.transform.SetParent(canvas.transform);
            obj.localPosition =  new Vector3((i-4)*w/9,0,0);
            obj.localScale = new Vector3(1,1,1);
            ListOfBorne.Add(obj);
        }
        

    }
    public void DrawingCardOnBoard( RectTransform card, int index, List<RectTransform> SubBoard){

        float w = canvas.GetComponent<RectTransform>().rect.width;
        float h = canvas.GetComponent<RectTransform>().rect.height;
        Canvas SetCanvas = canvas;

        tag  = card.tag;
        int CardAlreadyIn = 0;
        for (int i = 0;i<SubBoard.Count-1;i++){if(SubBoard[i].tag == tag){CardAlreadyIn++;}}

        if (CardAlreadyIn ==0){Debug.Log("canvas 1");SetCanvas = canvas_1;}
        if (CardAlreadyIn ==1){Debug.Log("canvas 2");SetCanvas = canvas_2;}
        if (CardAlreadyIn ==2){Debug.Log("canvas 3");SetCanvas = canvas_3;}

        if(card.tag == "Blue"){
            card.localPosition =  new Vector3((index-4)*w/9,-h/6 - CardAlreadyIn*h/10,CardAlreadyIn);
                        }
        if(card.tag == "Red"){
            card.localPosition =  new Vector3((index-4)*w/9,+h/6 + CardAlreadyIn*h/10,CardAlreadyIn);
                        }
        card.localScale = new Vector3(1,1,1);
        card.SetParent(SetCanvas.transform);
               
    }
}

