/*
 * 登陆注册业务系统
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;
    public LoginWnd loginWnd;
    public CreateWnd createWnd;
    public override void InitSvc()
    {
        base.InitSvc();
        Instance = this;
        Debug.Log("Init LoginSys Instance is " +  Instance );

        Debug.Log("Init LoginSys...");
    }

    /// <summary>
    /// 进入登陆场景
    /// </summary>
    public void EnterLogin()
    {
        //异步加载登陆场景
            //并显示加载的进度
            //加载完成后再打开注册登陆界面
            //全都放到这一个函数里了
        //直接用resSvc.systemRoot已经声明
        resSvc.AsyncLoadScene(Constants.SceneLogin, OpenLoginWnd);
    }

    /// <summary>
    /// 打开登陆窗口
    /// </summary>
    public void OpenLoginWnd()
     {
        Debug.Log("走到这里 loginWnd.SetWndState(true);");
        loginWnd.SetWndState(true);
         //调用根的方法，包括了以下这两个功能
             //loginWnd.gameObject.SetActive(true);
             //loginWnd.InitWnd();

        //调用音乐
        audioSvc.PlayBGMusic(Constants.BGLogin);
     }

    public void RspLogin() {
        GameRoot.ShowTips("登录成功");

        //打开角色创建界面
        Debug.Log("走到这里createWnd.SetWndState();");
        createWnd.SetWndState(true);
        //关闭登录界面
        loginWnd.SetWndState(false);
    }
}
