using System.Collections.Generic;
using UnityEngine;

public class DrawingHands : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float Offset_x = -200;
    public float BlueOffset_y = -200;
    public float RedOffset_y = 200;

    public float CardSpace = 80;

    public RectTransform card;
    public Canvas canvas;


    public void DrawRedHands(List<RectTransform> Red){

        int RedNumberOfCard = Red.Count;

        for(int i = 0; i<RedNumberOfCard;i++){
            RectTransform obj  = Red[i];
            obj.transform.SetParent(canvas.transform);
            obj.localPosition =  new Vector3(Offset_x + i*CardSpace,RedOffset_y,0);
            obj.localScale = new Vector3(1,1,1);
        }
    }
    public void DrawBlueHands(List<RectTransform> Blue){

        int BlueNumberOfCard = Blue.Count;

        for(int i = 0; i<BlueNumberOfCard;i++){
            RectTransform obj  = Blue[i];
            obj.transform.SetParent(canvas.transform);
            obj.localPosition =  new Vector3(Offset_x + i*CardSpace,BlueOffset_y,0);
            obj.localScale = new Vector3(1,1,1);
        }
    }
}
