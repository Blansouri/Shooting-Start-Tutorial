using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [SerializeField] AudioData audioData;

    [SerializeField] Canvas hUDCanvas;

    [SerializeField] Canvas menuCanvas;

    [SerializeField] Canvas WaveUI;

    [Header("-Player Input-")]

    [SerializeField] Button resumeButton;

    [SerializeField] Button optionsButton;

    [SerializeField] Button mainMenuButton;

    GameObject environmentAudio;

    AudioSource audioSource;

    void OnEnable()
    {
        environmentAudio = GameObject.Find("Music Player");
        audioSource = environmentAudio.GetComponent<AudioSource>();

        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.name,OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.name,OnMainMenuClick);

    }

    void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

    }

    void Pause()
    {     
        WaveUI.enabled = false;
        hUDCanvas.enabled = false;
        menuCanvas.enabled = true;
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(resumeButton);//打开暂停界面自动选择resume按钮
    }

    void Unpause()
    {
        resumeButton.Select();
        resumeButton.animator.SetTrigger("Pressed");
    }

    void OnResumeButtonClick()
    {
        AudioManager.Instance.StopSFX(audioData);
        audioSource.UnPause();
        WaveUI.enabled = true;
        hUDCanvas.enabled = true;
        menuCanvas.enabled = false;
        GameManager.GameState= GameState.Playing;
        TimeController.Instance.Unpause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectUI(optionsButton);

        AudioManager.Instance.PlaySFX(audioData);
        
        audioSource.Pause();
        
        playerInput.EnablePauseMenuInput();
    }

    void OnMainMenuClick()
    {
        menuCanvas.enabled = false;
        
        ScenceLoader.Instance.LoadMainScene();
    }
}
