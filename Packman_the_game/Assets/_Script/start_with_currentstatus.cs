using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_with_currentstatus : MonoBehaviour
{
    [Header("Script reference")]
    public enemy_ai e_ai;
    public packman_movement_script pms;
    public Player_health ph;
    public scene_and_quit_game_scripts sqgs;

    [Header("ui reference")]
    public GameObject restart_text;
    public GameObject gameover_player_life_nomore;
    public GameObject all_coinescollected;
    public GameObject timer_runout_ui;

    public bool canrestart_checkpooint;

    private void Update()
    {
        start_where_left();  // 👈 now it runs every frame
    }

    public void start_where_left()
    {
        if (ph.Player_life > 0 && canrestart_checkpooint)
        {
            restart_checker();

            if (Input.GetKeyDown(KeyCode.R))
            {
                e_ai.reseatting_position();
                pms.restart();
                ph.life_desearaser();
                restart_checker_reverse();
                canrestart_checkpooint = false;
            }
        }
        else if (ph.Player_life == 0 && canrestart_checkpooint)
        {
            Debug.Log("ye waala");


            gamequit_nolife_left();
            canrestart_checkpooint = false;

        }

    }

    public void gamefinished()
    {
        Debug.Log("game finished function");
        all_coinescollected.SetActive(true);
        sqgs.paused_game();
    }

    public void gamequit_nolife_left()
    {
        gameover_player_life_nomore.SetActive(true);
        Debug.Log("NO LIFE FUNCTION");
        sqgs.paused_game();
    }

    public void restart_checker()
    {
        restart_text.SetActive(true);
        Debug.Log("restart functiuon");
        sqgs.paused_game();
    }

    public void restart_checker_reverse()
    {
        restart_text.SetActive(false);
        sqgs.resumegame();
    }

    public void timerrunout()
    {
        timer_runout_ui.SetActive(true);
        sqgs.paused_game();
    }

}
