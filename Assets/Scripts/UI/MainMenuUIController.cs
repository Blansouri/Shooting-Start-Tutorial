using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] AudioData audioData;

    [Header("===CANVAS===")]

    [SerializeField] Canvas mainMenuCanvas;

    [Header("===BUTTONS===")]

    [SerializeField] Button startButton;

    [SerializeField] Button optionsButton;

    [SerializeField] Button quitButton;

    GameObject environmentAudio;

    AudioSource audioSource;


    void OnEnable()
    {
        environmentAudio = GameObject.Find("Music Player");
        audioSource = environmentAudio.GetComponent<AudioSource>();

        ButtonPressedBehavior.buttonFunctionTable.Add(startButton.gameObject.name, OnStartGameButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(quitButton.gameObject.name, OnQuitButtonClick);
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(startButton);
    }

    void OnStartGameButtonClick()
    {
        AudioManager.Instance.StopSFX(audioData);
        audioSource.UnPause();
        mainMenuCanvas.enabled = false;
        ScenceLoader.Instance.LoadGamePlayScene();
    }

    void OnOptionsButtonClick()
    {
        AudioManager.Instance.PlaySFX(audioData);

        audioSource.Pause();

        UIInput.Instance.SelectUI(optionsButton);
    }

    void OnQuitButtonClick()
    {
     #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
     #else
        Application.Quit();
     #endif
    }
}
