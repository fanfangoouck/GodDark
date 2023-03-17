/*
 * 根
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameRoot : MonoBehaviour
{
     public static GameRoot Instance = null;

    // 通过根目录调用
    //先创建LoadingWnd 类的脚本，才能创建这个对象
     public  LoadingWnd loadingWnd;
     public DynamicWnd dynamicWnd;
     

    // Start is called before the first frame update
    void Start()
    {
         Instance = this;
        //不能销毁这个root
         DontDestroyOnLoad(this);
         Debug.Log("Game Start...");
         Init();
    }

    void Init()
    {
        //服务模块初始化
        //直接挂载在gameroot上了。这里是直接获取的，没有拿ResSvc 的实例
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();

        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();


        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSvc();

        //主城系统初始化
        MainCitySys mainCity = GetComponent<MainCitySys>();
        mainCity.InitSvc();

        //除了dynamicWnd，把其他wnd都设成不显示
         ClearUIRoot();

        //进入登陆场景并加载相应UI
        Debug.Log("走到这里login.EnterLogin();");
        login.EnterLogin();

    }

    //除了dynamicWnd，把其他wnd都设成不显示
    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for(int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        dynamicWnd.gameObject.SetActive(true);
    }

    //封闭成一个函数，在其他脚本里都可以用
    public static void ShowTips(string tip)
    {
        Instance.dynamicWnd.AddTips(tip);
    }


    private CharacterData characterData = null;
    public CharacterData CharacterData {
        get {
            return characterData;
        }
    }

    /*
    public void SetPlayerData(RspLogin data) {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name) {
        PlayerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data) {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp= data.exp;
        PlayerData.guideid = data.guideid;
    }
    */
}
