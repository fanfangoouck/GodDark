//系统基类

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : MonoBehaviour
{
    //注意修饰符，同类才能用
    //拖拽组件过来
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;

    public virtual void InitSvc()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
    }
}
