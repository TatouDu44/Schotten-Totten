import numpy as np
import random as rd

from colorama import init
from termcolor import colored

class Card():

    def __init__(self, number, color,player):
        self.number = number
        self.color = color
        self.player = player

class Borne():
    def __init__(self,color):
        self.color = color



def one_round():
    
    Deck = []
    BlueDeck = []
    RedDeck = []
    Board = []
    Bornes = []

    HasEnded = False

    for i in range(1,10):
        Board.append([])
        Bornes.append(Borne(None))
        for color in ["red","blue","white","yellow","cyan","green"]:
            Deck.append(Card(i,color,None))
    rd.shuffle(Deck)
    for i in range(0,12):
        if i%2==0:
            card = Deck[0]
            card.player = "Blue"
            BlueDeck.append(card)
            Deck.remove(card)
        else : 
            card = Deck[0]
            card.player = "Red"
            RedDeck.append(card)
            Deck.remove(card)
    """
    print([len(Deck),len(BlueDeck),len(RedDeck)])
    s = " "
    s_red = " "
    for i in range(0,6):
        s += colored(BlueDeck[i].number,BlueDeck[i].color) + " "
        s_red += colored(RedDeck[i].number,RedDeck[i].color) + " "
    print(s)
    print(s_red)
    """

    while(not HasEnded):

        index_card,index_board = AtRandom(BlueDeck,Board,"Blue")
        card = BlueDeck[index_card]
        BlueDeck.remove(card)
        Board[index_board].append(card)

        index_card,index_board = FocusAtColor(RedDeck,Board,"Red")
        card = RedDeck[index_card]
        RedDeck.remove(card)
        Board[index_board].append(card)

        if len(Deck) !=0:
            card1 = Deck[0]
            card2 = Deck[1]
            card1.player = "Blue"
            card2.player = "Red"
            BlueDeck.append(card1)
            RedDeck.append(card2)
            Deck.remove(card1)
            Deck.remove(card2)

        for i in range(0,9):
            
            if Bornes[i].color == None and len(Board[i]) == 6:
                color = change_color(Board,i)
                Bornes[i].color = color


        for i in range(0,6):
            if Bornes[i].color == Bornes[i+1].color and  Bornes[i+2].color == Bornes[i+1].color and Bornes[i].color != None:
                HasEnded = True
                return Bornes[i].color
            
        if (len(BlueDeck) == 0 and len(RedDeck )== 0 ):
            HasEnded = True
            blue = 0
            red = 0
            for i in range(0,9):
                if Bornes[i].color == "Blue":
                    blue+=1
                if Bornes[i].color == "Red":
                    red+=1
            if blue>red:
                return "Blue"
            elif red>blue:
                return "Red"
            else : 
                return "Grey"




def AtRandom(Hand,Board,Player):
    """
    Pick a card at hand at random and selecting a borne at random as well
    """
    index_card = rd.randrange(len(Hand))
    s = []
    for i in range(0,9):
        sub_s = 0
        for j in range(0,len(Board[i])):
            if Board[i][j].player == Player:
                sub_s +=1
        if sub_s <3:
            s.append(i)
    index_board = rd.choice(s)
    return index_card,index_board

def FocusAtColor(Hand,Board,Player):
    """
    Only look into the color of the card
    """
    color = [Hand[i].color for i in range(0,len(Hand))]
    empty = []
    for i in range(0,len(color)):
        for j in range(0,9):
            card_player = 0
            col = []
            for k in range(0,len(Board[j])):
                if Board[j][k].player == Player:
                    card_player +=1
                    col.append( Board[j][k].color)
            if card_player ==0:
                empty.append(j)
            if card_player ==1:
                if col[0]==color[i]:
                    return i,j
            if card_player ==2:
                if col[0]==col[1] and col[0] == color[i]:
                    return i,j
    if len(empty)>0:
        return 0,rd.choice(empty)
    return AtRandom(Hand,Board,Player)


def FocusAtNumber(hand,Board,Player):
    """
    Only look at numbers to make a suite or the same number (preferably a suite)
    """


def change_color(Board,index):

    blue = []
    red = []
    for i in range(0,6):
        if(Board[index][i].player=="Blue"):
            blue.append(Board[index][i])
        else:
            red.append(Board[index][i])

    red_count = count(red)
    blue_count = count(blue)

    if red_count>blue_count : 
        return "Red"
    elif blue_count>red_count:
        return "Blue"
    else : 
        return "Grey"
    
def count(card):
    numbers = [card[0].number,card[1].number,card[2].number]
    colors = [card[0].color,card[1].color,card[2].color]
    s = 0
    nums_sorted = sorted(numbers)

    cons = False
    col = False

    if colors[0]==colors[1] and colors[1] == colors[2]:
        col = True
    if nums_sorted[0] == nums_sorted[1]-1 and nums_sorted[1] == nums_sorted[2]-1:
        cons = True

    if cons & col : 
        s+=200
    elif numbers[0]==numbers[1] and numbers[1]==numbers[2]:
        s+=150
    elif col and  cons ==False : 
        s+=100
    elif cons and col ==False : 
        s+=50
    s+=np.sum(numbers)

    return s
  


def test_model(n):
    result = []
    for i in range(0,n):
        result.append(one_round())
    print("Blue : "+ str(result.count("Blue")) + " --> " + str(result.count("Blue")/n*100) + " %")
    print("Red : "+ str(result.count("Red"))+ " --> " + str(result.count("Red")/n*100) + " %")
    print("Draw : "+ str(result.count("Grey"))+ " --> " + str(result.count("Grey")/n*100) + " %")

    return [result.count("Blue"),result.count("Red"),result.count("Grey")]


init()
test_model(400000)

    

    