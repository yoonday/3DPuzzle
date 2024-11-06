using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;


    private void Start()
    {
        CharacterManager.Instance.Player.controller.optionToggle += OptionToggle;
        gameObject.SetActive(false);
    }

    private void OptionToggle()
    {
        bool isActive = gameObject.activeSelf;
        Time.timeScale = 1f - Time.timeScale;   // if timeScale = 1 (not paused) -> pause,
                                                // if timeScale = 0 (when paused) -> resume game
        gameObject.SetActive(!isActive);
    }

    public void Resume()
    {
        OptionToggle();
        CharacterManager.Instance.Player.controller.ToggleCursor(); // 
    }
    
    public void Restart()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.LoadCheckPoint();
        }

        else
        {
            Debug.Log("DataManager instance is not exist");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
#endif
    }
}
