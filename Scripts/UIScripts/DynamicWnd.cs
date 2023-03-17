//动态窗口

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DynamicWnd : WindowRoot
{
     public Animation aniTips;
     public Text txtTip;
     public bool IsShowTips = false; // 标识变量. 

     private Queue<string> tipsQueue =  new Queue<string>();

    protected override void InitWnd()
    {
        base.InitWnd();
        SetActive(txtTip, false);
    }

    //新的tips进栈
    public  void AddTips(string tip)
    {
        lock (tipsQueue)
        {
            tipsQueue.Enqueue(tip);
        }
    }

    private void Update()
    {
        //如果没有IsShowTips，以 Update的频率很快就执行完
        if(tipsQueue.Count > 0 && IsShowTips == false)
        {
            lock (tipsQueue)
            {
                string tip = tipsQueue.Dequeue();
                IsShowTips = true;
                SetTips(tip);
                
            }
        }
    }

    public void SetTips(string tips)
    {
         SetActive(txtTip, true);
         SetText(txtTip, tips);
         //获得动画原件
         AnimationClip clip = aniTips.clip;
         aniTips.Play(); // 播放动画

         //播放完毕后 延时关闭激活状态。 用了协程
         StartCoroutine(AniPlayDone(clip.length, () =>
         {
              SetActive(txtTip, false);
              IsShowTips = false;
         }));

    }

  

    private IEnumerator AniPlayDone(float waitTime, Action cd)
    {
        yield return new WaitForSeconds(waitTime);
        if(cd != null)
        {
            cd();
        }
    }


}
