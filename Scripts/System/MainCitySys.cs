/*
 * 主城系统
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot
{
    public MainCityWnd mainCityWnd;
    public CreateWnd createWnd;
    public GuideWnd guideWnd;
    public  GuideWndGuess guideWndGuess;

    private PlayerController playerCtrl;
    public InfoWnd infoWnd;
    public Transform charShowCam;
    private AutoGuideCfg curtTaskData;
    private Transform[] npcPosTrans;

    private NavMeshAgent nav;

    public static MainCitySys Instance = null;
    GameObject player = null;

 
    public override void InitSvc()
    {
        base.InitSvc();
        Instance = this;
        Debug.Log("Init MainCitySys...");
    }


    public void RspMainCity()
    {
        GameRoot.ShowTips("进入主城");
        //加载地图数据
         MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);

        resSvc.AsyncLoadScene(Constants.SceneMainCity, ()=> {

            // 打开主城页面
            mainCityWnd.SetWndState(true);
            // 加载游戏主角
            LoadPlayer(mapData);
            //关闭角色创建界面
            createWnd.SetWndState(false);
            //播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);

            //找到MapRoot下的npc position
                //GameObject.FindGameObjectWithTag 这个不对
            GameObject map = GameObject.Find("MapRoot"); 
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            Debug.Log("mcm" + mcm);
           npcPosTrans = mcm.NpcPosTrans;   //找到 npcPosTrans 

            //设置人物展示相机
            if (charShowCam != null)
            {
                charShowCam.gameObject.SetActive(false);
            }
        });
    
    }

    //加载主角
     void LoadPlayer(MapCfg mapData) {
  
       GameObject prefab = Resources.Load<GameObject>(PathDefine.AssissnCityPlayerPrefab);
        player = Instantiate(prefab,new Vector3(28.0F, -1.2F, 45.0F), Quaternion.identity);


        //player = resSvc.LoadPrefab(PathDefine.AssissnCityPlayerPrefab, true);
           // Debug.Log("player   = " + player);

        //这两个根本没用
        //player.transform.position = mapData.playerBornPos;
        //player.transform.position = new Vector3(30.0F, 0F, 45.0F);
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5F, 1.5F, 1.5F);

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        //遥控杆控制 初始化
        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();

        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 dir) {
        StopNavTask(); //设置方向时，就不要执行任务了

        if (dir == Vector2.zero) {
            Debug.Log("1");
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else {
            Debug.Log("2");
            playerCtrl.SetBlend(Constants.BlendWalk);
        }
        playerCtrl.Dir = dir;
        Debug.Log("playerCtrl.Dir" + playerCtrl.Dir);
    }

    //打开人物信息页
    public void OpenInfoWnd()
    {
        StopNavTask(); //打开界面，就不要执行任务了
        //激活并初始化拍摄人物的相机
        if (charShowCam == null)
        {
            Debug.Log("charShowCam == null)");
            charShowCam = GameObject.FindGameObjectWithTag("charShowCam").transform;
        }
        if(charShowCam == null)
        {
            Debug.Log("没找到相机");
        }
        else
        {
             //设置人物展示相机的相对位置
            charShowCam.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
            charShowCam.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
            charShowCam.localScale = Vector3.one;
            charShowCam.gameObject.SetActive(true);
            infoWnd.SetWndState();
        }

        //人物信息页初始化
        infoWnd.SetWndState();
    }

    public void CloseInfoWnd() {
        if (charShowCam != null) {
            charShowCam.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }


    private float startRoate = 0;
    public void SetStartRoate() {
        //记录开始的角度
        startRoate = playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRoate(float roate) {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRoate + roate, 0);
    }


    #region Guide Wnd
    private bool isNavGuide = false; // 标志变量：是否正在导航

    public void RunTask(AutoGuideCfg agc)
    {
         

        if(agc != null)
        {
            curtTaskData = agc; //任务信号
        }

        //解析任务数据
        nav.enabled = true;
          //需要npc
        if (curtTaskData.npcID != -1) {

            //判断主角和npc的距离
            
           float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
            Debug.Log("判断主角和npc的距离:" + dis);
            //距离小于0.5
            if (dis < 0.5f)
            {
                isNavGuide = false;
                playerCtrl.SetCharacterController(true);


                nav.isStopped = true; // 让导航停下来
                playerCtrl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;

                OpenGuideWnd(); 
            }
            else
            {
                isNavGuide = true;
                playerCtrl.SetCharacterController(false);

                nav.enabled = true; //激活导航系统
                nav.speed = Constants.PlayerMoveSpeed;
                //设置目标点，找目标npc
                nav.SetDestination(npcPosTrans[agc.npcID].position);
                //播放人物动画（不然会滑过去，而不是跑过去）
                playerCtrl.SetBlend(Constants.BlendWalk);
            }
        }
        else
        {
            OpenGuideWnd();//不需要找到npc，打开任务页面
        }
    }

    private void Update()
        {
            if (isNavGuide)
            {
                IsArriveNavPos();
                playerCtrl.SetCam(); // 更新摄像机位置
            }
    
        }

    //是否达到目标点
    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curtTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            playerCtrl.SetCharacterController(true);

            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
           
            nav.enabled = false;

            OpenGuideWnd();
        }
    }

    //中断任务 
    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            playerCtrl.SetCharacterController(true);

            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    public void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    }

    public  GuideWndGuess OpenGuideWnd2()
    {
        guideWndGuess.gameObject.SetActive(true);
        return guideWndGuess;
    }

    public AutoGuideCfg GetCurtTaskData()
    {
        return curtTaskData;
    }

    //public void RspGuide(GameMsg msg) {
    //    RspGuide data = msg.rspGuide;

    //    GameRoot.AddTips(Constants.Color("任务奖励 金币+" + curtTaskData.coin + "  经验+" + curtTaskData.exp, TxtColor.Blue));

    //    switch (curtTaskData.actID) {
    //        case 0:
    //            //与智者对话
    //            break;
    //        case 1:
    //            //TODO 进入副本
    //            break;
    //        case 2:
    //            //TODO 进入强化界面
    //            break;
    //        case 3:
    //            //TODO 进入体力购买
    //            break;
    //        case 4:
    //            //TODO 进入金币铸造
    //            break;
    //        case 5:
    //            //TODO 进入世界聊天
    //            break;
    //    }
    //    GameRoot.Instance.SetPlayerDataByGuide(data);
    //    maincityWnd.RefreshUI();
    //}
    #endregion

}

