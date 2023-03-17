using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSvc : MonoBehaviour
{
     public  static AudioSvc Instance;
     public AudioSource bgAudio;
     public AudioSource uiAudio;

     

    public void InitSvc()
    {
        Instance = this;
        Debug.Log("Init ResSvc...");
    }

    public void PlayBGMusic(string name, bool isLoop = true)
    {
        AudioClip audioClip = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        //如果音频不为空，或者音频的名字不等于传进来的名字
        if(bgAudio !=null || bgAudio.clip.name != audioClip.name)
        {
            bgAudio.clip = audioClip;
            bgAudio.loop = isLoop;
            bgAudio.Play();//播放音乐

        }
    }

    public void PlayUIMusic(string name)
    {
         //UI音乐，一次性的。没有循环
         AudioClip audioClip = ResSvc.Instance.LoadAudio("ResAudio/" + name);
         Debug.Log(" audioClip " + audioClip);
         uiAudio.clip = audioClip;
         uiAudio.Play(); // 播放音乐
    }
}
