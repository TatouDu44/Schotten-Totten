using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Adversary : MonoBehaviour
{

    public int pointer = 0;
    public int pointer_board = 0;

    private List<int> indice;
    
    public void PickCard(List<List<RectTransform>> Board, List<RectTransform> RedDeck){

        indice = new List<int>();
        for (int i  = 0;i< RedDeck.Count;i++){indice.Add(i);}
        System.Random rng = new System.Random();
        indice = indice.OrderBy(c => rng.Next()).ToList();
        pointer = indice[0];

        indice = new List<int>();
        for(int i = 0;i<9;i++){
            int count = 0;
            for (int j = 0;j<Board[i].Count;j++){
                if(Board[i][j].tag == "Red"){count++;}
            }
            if(count !=3){indice.Add(i);}
        }
        System.Random rng_2 = new System.Random();
        indice = indice.OrderBy(c => rng_2.Next()).ToList();
        pointer_board = indice[0];

    }
}
