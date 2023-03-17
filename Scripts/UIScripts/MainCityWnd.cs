using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainCityWnd : WindowRoot
{
     #region UIDefine
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    public Animation menuAni;
    public Button btnMenu;

    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Transform expPrgTrans;

    public Button btnGuide;
    #endregion

    private bool menuState = false;
    private float pointDis;
   
    private Vector2 startPos = Vector2.zero;//遥感点初始位置
    private Vector2 defaultPos = Vector2.zero;

    private AutoGuideCfg curtTaskData; // 人物编号


    #region MainFunctions
    protected override void InitWnd() {
        base.InitWnd();
        //遥感自适应
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        SetActive(imgDirPoint, false);

        defaultPos = imgDirBg.transform.position;
        RegisterTouchEvts();
        RefreshUI();
    }

    private void RefreshUI() {
        CharacterData pd = CharacterData.sky;
        //SetText(txtFight, PECommon.GetFightByProps(pd));
        //SetText(txtPower, "体力:" + pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        //imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        //SetText(txtLevel, pd.lv);
        //SetText(txtName, pd.name);


        //expprg


        //int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv) * 100);
        //SetText(txtExpPrg, expPrgVal + "%");
        //int index = expPrgVal / 10;

        //获得组件
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        //获得750/当前高度的 比例
        //float globalRate = 1.0F * Constants.ScreenStandardHeight / Screen.height;
        //获得屏幕应有的宽度
        //float screenWidth = Screen.width * globalRate;
        //（屏幕宽度 - 空隙）/10 = 每一个格子的大小
        //float width = (screenWidth - 180) / 10;
        //float width = (Screen.width - 180) / 10;

        //高度保持不变（7）。格子宽度随屏幕宽度变化
       // grid.cellSize = new Vector2(width, 7);

        //for (int i = 0; i < expPrgTrans.childCount; i++) {
        //    Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
        //    if (i < index) {
        //        img.fillAmount = 1;
        //    }
        //    else if (i == index) {
        //        img.fillAmount = expPrgVal % 10 * 1.0f / 10;
        //    }
        //    else {
        //        img.fillAmount = 0;
        //    }
        //}

        //设置自动任务图标
        Debug.Log("pd.guideid" + pd.guideid);
        curtTaskData = resSvc.GetAutoGuideData(pd.guideid);
        Debug.Log("curtTaskData.npcID" + curtTaskData.npcID);
        if (curtTaskData != null)
        {
            Debug.Log("curtTaskData.npcID" + curtTaskData.npcID);
            SetGuideBtnIcon(curtTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }


    }

    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        Debug.Log("spPath" + spPath);
        SetSprite(img, spPath);
    }
    #endregion


    #region ClickEvts

        public void ClickGuideBtn() {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);

        if (curtTaskData != null) {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else {
            GameRoot.ShowTips("更多引导任务，正在开发中...");
        }
    }


    public void ClickMenuBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIExtenBtn);
        
        menuState = !menuState;
        AnimationClip clip = null;

        if (menuState)
        {
            clip = menuAni.GetClip("aniHeshang");
        }
        else
        {
            clip = menuAni.GetClip("aniZhankai");
        }
        menuAni.Play(clip.name);
    }

    
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }


    //遥控杆
    public void RegisterTouchEvts()
    {

        OnClickDown(imgTouch.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position; // 记录点击位置
            SetActive(imgDirPoint); // 中间点出现
            imgDirBg.transform.position = evt.position; // 更新背景到点击的位置
        });


        onClickUp(imgTouch.gameObject, (PointerEventData evt) =>
        {
            imgDirBg.transform.position = defaultPos; // 设置为初始值
            SetActive(imgDirPoint, false);// 中间点隐藏
            imgDirPoint.transform.localPosition = Vector2.zero;//归正中间点位置
            MainCitySys.Instance.SetMoveDir(Vector2.zero);//角色朝向归零
        });

        onDrag(imgTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            //不能超出ScreenOPDis范围
            if(len > Constants.ScreenOPDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
                Debug.Log("imgDirPoint.transform.position1" + imgDirPoint.transform.position);
            }
            else {
                imgDirPoint.transform.position = evt.position;
                Debug.Log("imgDirPoint.transform.position2" + imgDirPoint.transform.position);
            }
            MainCitySys.Instance.SetMoveDir(dir.normalized);
        });
        
    }
    #endregion



}
