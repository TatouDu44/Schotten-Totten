using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMov : MonoBehaviour
{

    public GameManager gameManager;
    public int pointer = 0;
    public int pointer_board = 0;

    public bool Selected = false;

    private List<RectTransform> BlueHands;

    // Update is called once per frame
    void Update()
    {   
        List<List<RectTransform>> Board = gameManager.GetComponent<GameManager>().Board;
        List<int> indice = new List<int>();
        List<RectTransform> BlueDeck = gameManager.GetComponent<GameManager>().BlueDeck;
        int NumberOfBlueCard = BlueDeck.Count;
        if(pointer> NumberOfBlueCard-1){pointer = 0;}

        //on check les colonnes avec 3 Blues

        for(int i = 0;i<9;i++){
            int count = 0;
            for (int j = 0;j<Board[i].Count;j++){
                if(Board[i][j].tag == "Blue"){count++;}
            }
            if(count !=3){indice.Add(i);}
        }

        if(Input.GetKeyDown("a")){
            if(pointer < NumberOfBlueCard -1){pointer++;}
            else{pointer = 0;}
            Change();
        }
        if(Input.GetKeyDown("d")){
            if(pointer >0){pointer--;}
            else{pointer = NumberOfBlueCard -1;}
            
            Change();
        }
        if(Input.GetKeyDown("s")){Selected = true;}
        if(Selected){
            
            if(Input.GetKeyDown("1")& indice.Contains(0)){pointer_board = 0;gameManager.GetComponent<GameManager>().tour();}
            if(Input.GetKeyDown("2")& indice.Contains(1)){pointer_board = 1;gameManager.GetComponent<GameManager>().tour();}
            if(Input.GetKeyDown("3")& indice.Contains(2)){pointer_board = 2;gameManager.GetComponent<GameManager>().tour();}
            if(Input.GetKeyDown("4")& indice.Contains(3)){pointer_board = 3;gameManager.GetComponent<GameManager>().tour();}
            if(Input.GetKeyDown("5")& indice.Contains(4)){pointer_board = 4;gameManager.GetComponent<GameManager>().tour();}
            if(Input.GetKeyDown("6")& indice.Contains(5)){pointer_board = 5;gameManager.GetComponent<GameManager>().tour();}
            if(Input.GetKeyDown("7")& indice.Contains(6)){pointer_board = 6;gameManager.GetComponent<GameManager>().tour();}
            if(Input.GetKeyDown("8")& indice.Contains(7)){pointer_board = 7;gameManager.GetComponent<GameManager>().tour();}
            if(Input.GetKeyDown("9")& indice.Contains(8)){pointer_board = 8;gameManager.GetComponent<GameManager>().tour();}
            
        }
        
    }
    void Change(){
        
        BlueHands = gameManager.GetComponent<GameManager>().BlueDeck;
        BlueHands[pointer].transform.localScale = new UnityEngine.Vector3(3,3,1);
        for (int i = 0;i<BlueHands.Count;i++){
            if(i != pointer){
                BlueHands[i].transform.localScale = new UnityEngine.Vector3(1,1,1);
            }
        }
    }
    
}


