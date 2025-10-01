using UnityEngine;
using TMPro;


public class timecounter : MonoBehaviour
{
    public bool start_game;
    [SerializeField] private float Total_time_of_level;
    public TextMeshProUGUI timertextrepresentator;
    public start_with_currentstatus swc;

    public void starting_the_game()
    {
        start_game = true;
    }

    private void Update()
    {
        if (start_game && Total_time_of_level != 0)
        {
            Total_time_of_level -= Time.deltaTime;
        }
        if (Total_time_of_level == 0 && start_game)
        {
            swc.timerrunout();
        }

        // Format time as MM:SS
        int minutes = Mathf.FloorToInt(Total_time_of_level / 60);
        int seconds = Mathf.FloorToInt(Total_time_of_level % 60);

        timertextrepresentator.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
