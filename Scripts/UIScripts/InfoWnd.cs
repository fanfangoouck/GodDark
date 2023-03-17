using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot
{
    #region UI Define
    public RawImage imgChar;

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;

    public Text txtJob;
    public Text txtFight;
    public Text txtHP;
    public Text txtHurt;
    public Text txtDef;

    public Button btnClose;

    public Button btnDetail;
    public Button btnCloseDetail;
    public Transform transDetail;

    public Text dtxhp;
    public Text dtxad;
    public Text dtxap;
    public Text dtxaddef;
    public Text dtxapdef;
    public Text dtxdodge;
    public Text dtxpierce;
    public Text dtxcritical;
    #endregion

    private Vector2 startPos;

    //初始化
    protected override void InitWnd() {
        base.InitWnd();
        RegTouchEvts();
        SetActive(transDetail, false); // 详细属性面板要关闭
        RefreshUI();
    }

    
    private void RegTouchEvts() {
        OnClickDown(imgChar.gameObject, (PointerEventData evt) => {
            startPos = evt.position; // 按下的位置
            MainCitySys.Instance.SetStartRoate();
        });
        //监听拖拽的事件 
        onDrag(imgChar.gameObject, (PointerEventData evt) => {
            float roate = -(evt.position.x - startPos.x) * 0.4f;
            MainCitySys.Instance.SetPlayerRoate(roate);
        });
    }
    

    private void RefreshUI() {
        CharacterData pd = GameRoot.Instance.CharacterData;
        SetText(txtInfo, pd.name + " LV." + pd.level);
        SetText(txtExp, pd.exp + "/" + pd.level);
        imgExpPrg.fillAmount = pd.exp * 1.0F /   pd.level;
        SetText(txtPower, pd.power + "/" + pd.level);
        imgPowerPrg.fillAmount = pd.power * 1.0F / pd.level;

        SetText(txtJob, " 职业   暗夜刺客");
        SetText(txtFight, " 战力   " + "78");
        SetText(txtHP, " 血量   " + pd.hp);
        SetText(txtHurt, " 伤害   " + (pd.ad + pd.ap));
        SetText(txtDef, " 防御   " + (pd.addef + pd.apdef));

        //detail TODO
        SetText(dtxhp, pd.hp);
        SetText(dtxad, pd.ad);
        SetText(dtxap, pd.ap);
        SetText(dtxaddef, pd.addef);
        SetText(dtxapdef, pd.apdef);
        SetText(dtxdodge, pd.dodge + "%");
        SetText(dtxpierce, pd.pierce + "%");
        SetText(dtxcritical, pd.critical + "%");

    }

    
    public void ClickCloseBtn() {
        SetWndState(false);
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
       // MainCitySys.Instance.CloseInfoWnd();
    }

    //详细属性页
    public void ClickDetailBtn() {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        SetActive(transDetail);
    }

    public void ClickCloseDetailBtn() {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        SetActive(transDetail, false);
    }
    
}
