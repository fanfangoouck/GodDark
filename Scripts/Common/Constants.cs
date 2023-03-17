/*
 * 场景名称
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{

     //AutoGuideNPC
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;


    public const string SceneLogin = "SceneLogin";
     public const string SceneMainCity = "SceneMainCity";
    public const int MainCityMapID = 10000;

      //音效名称
     public const string BGLogin = "bgLogin";
     public const string BGMainCity = "bgMainCity";

     //登录按钮音效
     public const string UILoginBtn = "uiLoginBtn";
     

     //常规UI点击音效
     public const string UIClickBtn = "uiClickBtn";
     public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage  = "uiOpenPage";

   

     //屏幕标准宽高
     public const int ScreenStandardWidth = 1334;
     public const int ScreenStandardHeight = 750;
     //摇杆点标准距离
     public const int ScreenOPDis = 90;

     //混合参数
     public const int BlendIdle = 0;
     public const int BlendWalk = 1;

     //角色移动速度
     public const int PlayerMoveSpeed = 8;
     public const int MonsterMoveSpeed = 4;
     
     //运动平滑加速度
     public const float AccelerSpeed = 5;
}
