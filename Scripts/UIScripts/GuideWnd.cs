//引导对话界面

using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot {
    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;

    private CharacterData pd;
    private AutoGuideCfg curtTaskData; //任务数据
    private string[] dialogArr; // 对话 
    private int index;//记录对话标号

    protected override void InitWnd() {
        base.InitWnd();

        pd = CharacterData.sky;
        //以'#'做切割
        curtTaskData = MainCitySys.Instance.GetCurtTaskData();
        dialogArr = curtTaskData.dilogArr.Split('#');
        index = 1;

        SetTalk();
    }

    private void SetTalk() {
        string[] talkArr = dialogArr[index].Split('|');
        if (talkArr[0] == "0") {
            //自己
            SetSprite(imgIcon, PathDefine.SelfIcon);
            SetText(txtName, pd.name);
        }
        else {
            //对话NPC
            switch (curtTaskData.npcID) {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "工匠");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "小芸");
                    break;
            }
        }

        imgIcon.SetNativeSize(); // 防止拉伸
        SetText(txtTalk, talkArr[1].Replace("$name", pd.name)); // 替换成自己的名字
    }


    public void ClickNextBtn()
    {
       audioSvc.PlayUIMusic(Constants.UIClickBtn);

        index += 1;

        //所有对话有显示完成
            if (index == dialogArr.Length) {
                    SetWndState(false);
            }
            else {
                    Debug.Log("SetTalk();");
                    SetTalk();
            }
     }
 }