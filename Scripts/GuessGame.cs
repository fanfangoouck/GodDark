using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessGame : MonoBehaviour
{
    
 
    //点击人物，出现对话框
    public void Clickchoice ()
      {
        MainCitySys.Instance.OpenGuideWnd();
      }

}
