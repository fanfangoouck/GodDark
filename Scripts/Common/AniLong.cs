//控制龙循环的动画

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniLong : MonoBehaviour
{
     private Animation ani;

    private void Awake()
    {
         ani = GetComponent<Animation>();
    }

    private void Start()
    {
        if(ani != null)
        {
            //间隔0秒，每20秒循环一次
            InvokeRepeating("PlayDragonAni",0,20);
        }
    }

    //循环动画
    private void PlayDragonAni()
    {
          if(ani != null)
        {
            ani.Play();
        }
    }


}
