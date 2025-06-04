using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TitleMenuController : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject StopPanel;
    public GameObject StopText;
    public GameObject StopButton;
    public GameObject ExitButton;

    public GameObject MenuContainer;
    public VideoPlayer introVideo;

    private bool isMenuOpen = false;
    private bool isPaused = false;

    void Start()
    {
        MenuContainer.SetActive(false);
        MenuPanel.SetActive(false);
        StopPanel.SetActive(false);
        StopText.SetActive(false);
        StopButton.SetActive(false);
        ExitButton.SetActive(false);
    }

    public void ShowMenuControls()
    {
        StopButton.SetActive(true);
        ExitButton.SetActive(true);
    }

    public void ShowMenu()
    {
        MenuContainer.SetActive(true);
    }

    public void Openmenu()
    {
        isMenuOpen = !isMenuOpen;
        MenuPanel.SetActive(isMenuOpen);
        StopButton.SetActive(isMenuOpen);
        ExitButton.SetActive(isMenuOpen);
    }

    public void Stop()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            StopPanel.SetActive(true);
            StopText.SetActive(true);
            introVideo.Pause();
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1f;
            StopPanel.SetActive(false);
            StopText.SetActive(false);
            introVideo.Play();
        }
    }


    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
        GlobalAudioManager.Instance.StopAllMusic();
        GlobalAudioManager.Instance.PlayMainMenuMusic();
    }
}
