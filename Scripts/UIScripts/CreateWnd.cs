using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot
{
    public InputField inputField;

     //初始化
    protected override void InitWnd()
    {
        base.InitWnd();
        //随机显示一个名字
        inputField.text = resSvc.GetRDNameData(false);
    }

    //清理窗口
    protected override void ClearWnd()
    {
         base.ClearWnd();
    }

    public void ClickRandBtn()
    {
        //播放点击的音效
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        //获得随机名字
        inputField.text = resSvc.GetRDNameData();
    }


    //点击按钮
    //加载主城场景
    //打开主城页面
    //关闭角色创建页面
    public void ClickEnterGMBtn()
    {
        Debug.Log("test");
        MainCitySys.Instance.RspMainCity();
    }

}
