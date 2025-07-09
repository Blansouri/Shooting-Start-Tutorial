using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;

    [SerializeField] float MIN_PITCH = 0.9f;//最小音高

    [SerializeField] float MAX_PITCH = 1.1f;//最大音高

    public void StopSFX(AudioData audioData)
    {
        sFXPlayer.Stop();
    }

    public void PlaySFX(AudioData audioData)//音频播放器，储存音频剪辑和音量
    {
        sFXPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
    }

    public void PlayRandomSFX(AudioData audioData)//随机音高播放使声音不那么单调
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH,MAX_PITCH);
        PlaySFX(audioData);
    }

    public void PlayRandomSFX(AudioData[] audioData)//多个音频随机播放
    {
        PlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
    }

}
