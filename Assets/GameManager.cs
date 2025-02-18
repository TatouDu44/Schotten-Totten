using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor.PackageManager.Requests;
// Github version

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public RectTransform Card_template;
    public List<RectTransform> Deck;

    public List<RectTransform> RedDeck;
    public List<RectTransform> BlueDeck;

    public List<List<RectTransform>> Board;

    public GameObject DrawingHands;

    public Canvas EndgameCanvas;

    public bool BlueStart = true;
    public bool HasEnded = false;


    //variable for the testing part

    public bool testing = false;
    public int cycles = 10;

    public List<Color> result;

    public int cycle_index = 0;

    void Start()
    {
        if(cycle_index == 0){
            result = new List<Color>();
        }
        HasEnded = false;
        //function to create a list of card
        Deck = CreateDeck();
        //mix them up
        System.Random rng = new System.Random();
        Deck = Deck.OrderBy(c => rng.Next()).ToList();

        Debug.Log(Deck.Count);
        //Create Starting Hands
        RedDeck = CreateHands(Deck,"Red");
        BlueDeck = CreateHands(Deck,"Blue");

        DrawingHands.GetComponent<DrawingHands>().DrawBlueHands(BlueDeck);
        DrawingHands.GetComponent<DrawingHands>().DrawRedHands(RedDeck);

        DrawingHands.GetComponent<DrawingBoard>().DrawingBorneOnBoard(Board);

        Board = CreateBoard();

        if(testing){          
            
            while(HasEnded == false){
                tour();
                }
            
            if(cycle_index<cycles){
                cycle_index++;
                reset();
                Start();
                }
            else{
                int blue = 0;
                int red = 0;
                int draw = 0;
                for (int i = 0;i<cycles;i++){
                    if(result[i]==Color.blue){blue++;}
                    if(result[i]==Color.red){red++;}
                    if(result[i]==Color.gray){draw++;}
                }
                Debug.Log("Blue : " + blue +" Red : "+ red+" Draw : "+draw);
            }
            
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<RectTransform> CreateDeck(){

        Color[] colors = { Color.red, Color.green, Color.blue, Color.cyan, Color.yellow, Color.magenta  };

        List<RectTransform> Cards = new List<RectTransform>();

        foreach (Color color in colors)
        {
            for (int number = 1; number <= 9; number++)
            {
                RectTransform obj = Instantiate(Card_template);
                obj.GetComponent<Properties>().number = number;
                obj.GetComponent<Properties>().color = color;
                obj.GetComponent<Properties>().Set_properties();

                Cards.Add(obj);
            }
        }

        return Cards;
    }
    public List<RectTransform>  CreateHands(List<RectTransform> fromDeck,string tag){

    List<RectTransform> toDeck = new List<RectTransform>();

    for (int i=0;i<6;i++){
        RectTransform card = fromDeck[0]; // Get the card
        card.tag = tag;
        fromDeck.RemoveAt(0); // Remove from source
        toDeck.Add(card); // Add to destination
    }
    return toDeck;
    
}
    public List<List<RectTransform>> CreateBoard(){
        List<List<RectTransform>> obj = new List<List<RectTransform>>();
        for (int i = 0;i<9;i++){
            obj.Add(new List<RectTransform>());
        }
        return obj;
    }
    public void tour(){
        //player pick a card and where to place it
        int pointer = 0;
        int pointer_board = 0;

        if(testing){
            FindAnyObjectByType<Adversary>().GetComponent<Adversary>().PickCard(Board,BlueDeck,"Blue");
            pointer = FindAnyObjectByType<Adversary>().GetComponent<Adversary>().pointer;
            pointer_board = FindAnyObjectByType<Adversary>().GetComponent<Adversary>().pointer_board;}
        else{       
            pointer = FindAnyObjectByType<PlayerMov>().GetComponent<PlayerMov>().pointer;
            pointer_board = FindAnyObjectByType<PlayerMov>().GetComponent<PlayerMov>().pointer_board;}
        

        //the card get added to the board
        
        Board[pointer_board].Add(BlueDeck[pointer]);
        BlueDeck.RemoveAt(pointer);
            
        //player draw a card from Deck SI IL EN RESTE...
        if(Deck.Count>0){
            BlueDeck.Add(Deck[0]);
            BlueDeck[BlueDeck.Count-1].tag = "Blue";
            Deck.RemoveAt(0);
        }
        
        //Update hands and board on game
        DrawingHands.GetComponent<DrawingBoard>().DrawingCardOnBoard(Board[pointer_board][Board[pointer_board].Count-1], pointer_board, Board[pointer_board]);
        DrawingHands.GetComponent<DrawingHands>().DrawBlueHands(BlueDeck);

        //Red pick a card and where to place it
        FindAnyObjectByType<Adversary>().GetComponent<Adversary>().PickCard(Board,RedDeck,"Red");

        int pointer_Adv = FindAnyObjectByType<Adversary>().GetComponent<Adversary>().pointer;
        int pointer_board_Adv = FindAnyObjectByType<Adversary>().GetComponent<Adversary>().pointer_board;

        //the card get add to the board
        Board[pointer_board_Adv].Add(RedDeck[pointer_Adv]);
        RedDeck.RemoveAt(pointer_Adv);
        //Red draw from Deck
        if(Deck.Count>0){
            RedDeck.Add(Deck[0]);
            RedDeck[RedDeck.Count-1].tag = "Red";
            Deck.RemoveAt(0);
        }
        
        //Update hands and board on game
        DrawingHands.GetComponent<DrawingBoard>().DrawingCardOnBoard(Board[pointer_board_Adv][Board[pointer_board_Adv].Count-1], pointer_board_Adv, Board[pointer_board_Adv]);
        DrawingHands.GetComponent<DrawingHands>().DrawRedHands(RedDeck);

        //check if borne color change
        List<RectTransform> ListOfBorne = FindAnyObjectByType<DrawingHands>().GetComponent<DrawingBoard>().ListOfBorne;
        for (int i = 0;i<9;i++){
            if(Board[i].Count==6){
                Color color = CheckWinBorne(Board[i]);
                ListOfBorne[i].GetComponent<Image>().color = color;
            }
        }
        //check if the game is done
        
        for (int i=0;i<7;i++){
            Color col1 = ListOfBorne[i].GetComponent<Image>().color;
            Color col2 = ListOfBorne[i+1].GetComponent<Image>().color;
            Color col3 = ListOfBorne[i+2].GetComponent<Image>().color;
            if(col1 == col2 & col2 == col3 & col1 == Color.red){HasEnded = true; EndGame(Color.red);}
            if(col1 == col2 & col2 == col3 & col1 == Color.blue){HasEnded = true;EndGame(Color.blue);}
    }
        

        if(Deck.Count == 0 & BlueDeck.Count ==0 & RedDeck.Count ==0){
            
            HasEnded = true; 
            int bluecount = 0;
            int redcount = 0;
            for (int i = 0;i<9;i++){
                if(ListOfBorne[i].GetComponent<Image>().color == Color.red){redcount++;}
                if(ListOfBorne[i].GetComponent<Image>().color == Color.blue){bluecount++;}
            }
            if(redcount>bluecount){EndGame(Color.red);}
            else if(redcount<bluecount){EndGame(Color.blue);}
            else{EndGame(Color.gray);}
        }
    }

    public Color CheckWinBorne(List<RectTransform> ListOfCard){
        List<RectTransform> BlueOnBorne = new List<RectTransform>();
        List<RectTransform> RedOnBorne= new List<RectTransform>();
        for (int i = 0;i<6;i++){
            if(ListOfCard[i].tag == "Blue"){BlueOnBorne.Add(ListOfCard[i]);}
            else{RedOnBorne.Add(ListOfCard[i]);}
        }
        int BlueScore = Score(BlueOnBorne);
        int RedScore = Score(RedOnBorne);

        if(BlueScore>RedScore){return Color.blue;}
        if(BlueScore<RedScore){return Color.red;}
        else{return Color.gray;}
    }

    public int Score(List<RectTransform> Onecolor){
        List<int> numbers = new List<int>();
        List<Color> colors = new List<Color>();
        int score = 0;
        for (int i = 0;i<3;i++){
            numbers.Add(Onecolor[i].GetComponent<Properties>().number);
            colors.Add(Onecolor[i].GetComponent<Properties>().color);
        }
        if(AreConsecutive(numbers) & colors[0]==colors[1] & colors[1]==colors[2]){score+=50;}
        else if(numbers[0]==numbers[1] & numbers[1]==numbers[2]){score+=40;}
        else if(!AreConsecutive(numbers) & colors[0]==colors[1] & colors[1]==colors[2]){score+=30;}
        else if(AreConsecutive(numbers) & colors[0]!=colors[1]){score+=20;}
        else if(AreConsecutive(numbers) & colors[0]!=colors[2]){score+=20;}
        else if(AreConsecutive(numbers) & colors[2]!=colors[1]){score+=20;}

        score+=numbers.Max();

        return score;
    }

    public bool AreConsecutive(List<int> numbers)
    {
        if (numbers == null || numbers.Count < 2)
            return false;

        numbers.Sort();
        
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] != numbers[i - 1] + 1)
                return false;
        }

        return true;
    }

    public void EndGame(Color color){
        EndgameCanvas.gameObject.SetActive(true);

        if(testing){
            result.Add(color);
        }

        if(color == Color.gray){
            EndgameCanvas.transform.Find("Draw").gameObject.SetActive(true);
            EndgameCanvas.transform.Find("Wins").gameObject.SetActive(false);
            EndgameCanvas.transform.Find("Loses").gameObject.SetActive(false);
            EndgameCanvas.transform.Find("Who_Win").gameObject.SetActive(false);
            EndgameCanvas.transform.Find("Who_loses").gameObject.SetActive(false);
            }
        if(color == Color.blue){
            EndgameCanvas.transform.Find("Draw").gameObject.SetActive(false);
            EndgameCanvas.transform.Find("Who_Win").GetComponent<TextMeshProUGUI>().text = "BLUE";
            EndgameCanvas.transform.Find("Who_Win").GetComponent<TextMeshProUGUI>().color = color;
            EndgameCanvas.transform.Find("Who_loses").GetComponent<TextMeshProUGUI>().text = "RED";
            EndgameCanvas.transform.Find("Who_loses").GetComponent<TextMeshProUGUI>().color = Color.red;
            
        }
        if(color == Color.red){
            EndgameCanvas.transform.Find("Draw").gameObject.SetActive(false);
            EndgameCanvas.transform.Find("Who_loses").GetComponent<TextMeshProUGUI>().text = "BLUE";
            EndgameCanvas.transform.Find("Who_loses").GetComponent<TextMeshProUGUI>().color = Color.blue;
            EndgameCanvas.transform.Find("Who_Win").GetComponent<TextMeshProUGUI>().text = "RED";
            EndgameCanvas.transform.Find("Who_Win").GetComponent<TextMeshProUGUI>().color = color;
            
        }
        
    }
    public void reset(){
        for(int i = 0;i<9;i++){
            for (int j=0;j<Board[i].Count;j++){
                Destroy(Board[i][j].gameObject);
            }
        }
        Board = new List<List<RectTransform>>();
        
        for (int i = 0;i<BlueDeck.Count;i++){
            Destroy(BlueDeck[i].gameObject);
        }
        BlueDeck = new List<RectTransform>();
        for (int i = 0;i<RedDeck.Count;i++){
            Destroy(RedDeck[i].gameObject);
        }
        RedDeck = new List<RectTransform>();
        for (int i = 0;i<Deck.Count;i++){
            Destroy(Deck[i].gameObject);
        }
        Deck = new List<RectTransform>();

        FindAnyObjectByType<DrawingHands>().GetComponent<DrawingBoard>().resetborne();
        
        EndgameCanvas.gameObject.SetActive(false);
    }



}
