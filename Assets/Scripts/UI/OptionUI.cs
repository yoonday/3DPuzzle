using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionUI : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private Slider bgmSlider;

    //[SerializeField] private AudioSource SFXSource;
    //[SerializeField] private Slider sfxSlider;

    private void Start()
    {
        audioSource = AudioManager.Instance.gameObject.GetComponent<AudioSource>();
        CharacterManager.Instance.Player.controller.optionToggle += OptionToggle;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        BGMVolumeControl();
    }

    private void OptionToggle()
    {
        bool isActive = gameObject.activeSelf;
        Time.timeScale = 1f - Time.timeScale;   // if timeScale = 1 (not paused) -> pause,
                                                // if timeScale = 0 (when paused) -> resume game
        gameObject.SetActive(!isActive);
    }

    private void BGMVolumeControl()
    {
        audioSource.volume = bgmSlider.value;
    }
    public void Resume()
    {
        OptionToggle();
        CharacterManager.Instance.Player.controller.ToggleCursor();
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
        if (gameObject.activeSelf)
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
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
