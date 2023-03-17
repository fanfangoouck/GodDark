/*
 * 加载界面
 */

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot
{
     public Text txtTip;
     public Image imgFg;
     public Image imgPoint;
     public Text txtPrg;

     protected override void InitWnd()
      {
            base.InitWnd();
            SetText(txtTip, "游戏正在加载");
            //txtTip.text = "游戏正在加载";
            SetText(txtPrg, "0%");
            imgFg.fillAmount = 0;
            imgPoint.rectTransform.localPosition = new Vector3(-400, 0, 0);
            txtPrg.rectTransform.localPosition =  new Vector3(-365, 0, 0);
      }

     public void SetProgress(float prg)
      {
          imgFg.fillAmount = prg;
          SetText(txtPrg, prg * 100 + "%");
          //txtPrg.text = prg.ToString();
          
          imgPoint.rectTransform.localPosition = new Vector3(-400 + 790*prg, 0, 0);
          if(prg <= 0.05)
          {
               txtPrg.rectTransform.localPosition = new Vector3(-365 + 790*prg, 0, 0);
          }
         else
          {
               txtPrg.rectTransform.localPosition = new Vector3(-415  + 790*prg, 0, 0);
          }
            
          
      }
     
     
      
    
     
     



}
