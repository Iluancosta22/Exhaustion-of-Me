using UnityEngine;

public static class PauseController
{
    public static bool GamePause;

    public static void HandlePause()
    {
        if (!GamePause)
        {
            PauseGame();
            HidenMouse.HandleMouse(true);
            GamePause = true;
        }
        else
        {
            ResumeGame();
            HidenMouse.HandleMouse(false);
            GamePause = false;
        }
    }

    //Pause E resume
    public static void PauseGame()
    {
        Time.timeScale = 0f; //tempo parado
        GamePause = true;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f; //tempo volta ao normal
        GamePause = false;
    }
}