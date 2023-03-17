//登陆注册界面

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot
{
     public InputField iptAcct;
     public InputField iptPass;
     public Button btnEnter;
     public Button btnNotice;

    //获得账号和密码
    protected override void InitWnd()
      {
          base.InitWnd();

          //是否存在key为"Acct"的数据
          if(PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
          {
              //获得账号和密码
              iptAcct.text = PlayerPrefs.GetString("Acct"); 
              iptPass.text = PlayerPrefs.GetString("Pass");
          }
         else
          {
              iptAcct.text = "";
              Debug.Log("走到这了");
              iptPass.text = "";
              Debug.Log("iptPass.text" + iptPass.text);
          }
      }

    public void ClickEnterBtn()
    {
        //加载点击按钮的音乐
         audioSvc.PlayUIMusic(Constants.UILoginBtn);

       // 获得账号和密码(屏幕上输入框中的)
         string acct = iptAcct.text;
         string pass = iptPass.text;
        //输入框中的账号和密码都不为0
         if(acct != "" && pass != "")
        { //更新本地储存的账号和密码
            PlayerPrefs.SetString("Acct", acct);
            PlayerPrefs.SetString("Pass", pass);

            //TO Remove
            LoginSys.Instance.RspLogin();
        }

        else
        {
            Debug.Log("应该显示 账号或密码为空");
            Debug.Log("2");
            Debug.Log("1");
            Debug.Log("0");
            GameRoot.ShowTips("账号或密码为空");
        }


    }

      //TODO
      //更新本地存储的账号密码

       
     


   
}
