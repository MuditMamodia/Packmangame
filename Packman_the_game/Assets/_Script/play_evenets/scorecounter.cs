using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scorecounter : MonoBehaviour
{
    [SerializeField] private int coin_collected;
    [SerializeField] private TextMeshProUGUI text_representatore;
    [SerializeField] private GameObject coincollided;

    public GameObject[] scorecoines;
    public int totalscorecones;

    public start_with_currentstatus swc;

    private void Awake()
    {
        scorecoines = GameObject.FindGameObjectsWithTag("scorecoin");

        totalscorecones = scorecoines.Length;
    }

   

    private void Update()
    {
        if (coin_collected == totalscorecones)
        {
            swc.gamefinished();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("scorecoin"))
        {
            coincollided = collision.gameObject;
            coin_collected++;
            text_representatore.text = coin_collected.ToString();
            Destroy(coincollided);
           
        }
    }
}
