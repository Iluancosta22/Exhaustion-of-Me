using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    [SerializeField] private GameObject InitialCanvas;
    [SerializeField] private GameObject FinalCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name != "MainScene") return;
        StartCoroutine(StartingGame());
    }

    IEnumerator StartingGame()
    {
        InitialCanvas.SetActive(true);

        CanvasGroup canvas = InitialCanvas.GetComponent<CanvasGroup>();
        canvas.alpha = 1f;

        yield return new WaitForSeconds(5f);

        float duration = 2f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        canvas.alpha = 0f;
        InitialCanvas.SetActive(false);
    }

    public void SetWin()
    {
        FinalCanvas.SetActive(true);
        HidenMouse.HandleMouse(true);
        PauseController.PauseGame();
    }

    public void StartGame()
    {
        HidenMouse.HandleMouse(false);
        PauseController.ResumeGame();
        SceneManager.LoadScene("MainScene");
    }
    public void MainMenu()
    {
        HidenMouse.HandleMouse(true);
        PauseController.ResumeGame();
        SceneManager.LoadScene("Menu");
    }
    
    public void RestartScene()
    {
        HidenMouse.HandleMouse(false);
        PauseController.ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }

}