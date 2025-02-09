using TMPro;
using UnityEngine;

public class Properties : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int number = 9;
    public Color color = Color.red;

    public GameObject Card;

    public void Set_properties(){
        for (int i = 0;i<3;i++){
            Card.transform.GetChild(i).GetComponent<TextMeshProUGUI>().color = color;
            Card.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = number.ToString();
        }
        
    }
}
