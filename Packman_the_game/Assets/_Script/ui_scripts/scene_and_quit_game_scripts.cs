using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using JetBrains.Annotations;

public class scene_and_quit_game_scripts : MonoBehaviour
{
    public void start_Level(int Level_id_number)
    {
        //SceneManager.LoadScene(Level_id_number);

        StartCoroutine(asyncronic_scenecahnger(Level_id_number));
        resumegame();

    }
    public void Quit_the_game()
    {
        Application.Quit();
    }

    IEnumerator asyncronic_scenecahnger(int Level_id_number)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Level_id_number);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    public void paused_game()
    {
        Time.timeScale = 0f;
    }

    public void resumegame()
    {
        Time.timeScale = 1f;
    }

    public void restartlevel(int levelid)
    {
        resumegame();
        SceneManager.LoadScene(levelid);     
    }

}
