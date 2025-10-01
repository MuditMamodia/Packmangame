using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_health : MonoBehaviour
{
    public int Player_life;
    public TextMeshProUGUI liferepresentator;

    public void life_desearaser()
    {
        Player_life--;
        liferepresentator.text = Player_life.ToString();
    }

}
