//UI界面基类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class WindowRoot : MonoBehaviour
{
    //拖拽组件
     protected ResSvc resSvc;
     protected AudioSvc audioSvc;



    //设置active模式
    //初始化相关信息.
        //如果 isActive == true, 初始化相关设置
        //如果 isActive == false, 清除窗口
    //设置protected, 只有同类可以访问
        //设置public , loginsystem也能访问了
    public void SetWndState(bool isActive = true)
    {
        if(gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }

    //初始化
    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance; // 获得资源的实例
        audioSvc = AudioSvc.Instance;
    }

    //清理窗口
    protected virtual void ClearWnd()
    {
         resSvc = null;
         audioSvc = null;
    }


    #region  SetActive方法
    protected void SetActive(GameObject gameObject, bool IsActive = true)
    {
        gameObject.SetActive(IsActive);
    }
    protected void SetActive(Transform trans, bool IsActive = true)
    {
        trans.gameObject.SetActive(IsActive);
    }
    protected void SetActive(RectTransform rect, bool IsActive = true)
    {
        rect.gameObject.SetActive(IsActive);
    }
    protected void SetActive(Image img, bool IsActive = true)
    {
        img.gameObject.SetActive(IsActive);
    }
     protected void SetActive(Text txt, bool IsActive = true)
    {
        txt.transform.gameObject.SetActive(IsActive);
    }

    #endregion


    #region SetText方法
    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    protected void SetText(Transform trans, string context = "")
    {
        //找到trans的text组件
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num)
    {
        SetText(txt, num.ToString());
    }
    protected void SetText(Text txt, float num)
    {
        SetText(txt, num.ToString());
    }
    //传入的是Transform和数字
    protected void SetText(Transform trans, int num)
    {
        SetText(trans.GetComponent<Text>(), num.ToString());
    }
     protected void SetText(Transform trans, float num)
    {
        SetText(trans.GetComponent<Text>(), num.ToString());
    }
    #endregion

    //动态更改图片
    protected void SetSprite(Image img, string path)
    {
        Sprite sp = resSvc.LoadSprite(path, true);
        img.sprite = sp;
    }

    #region 获得组件
    protected T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T t = obj.GetComponent<T>();
        if(t == null)
        {
            t = obj.AddComponent<T>();
        }
        return t;
    }

    #endregion


    #region clickEvts
    protected void OnClickDown(GameObject go, Action<PointerEventData> cb) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = cb;
    }

    protected void onClickUp(GameObject go, Action<PointerEventData> cb) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = cb;
    }

    protected void onDrag(GameObject go, Action<PointerEventData> cb) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = cb;
    }

    #endregion














}
