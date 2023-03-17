using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PETools
{
    //在[min,max]随机选择一个数
    public static int RDInt(int min, int max, System.Random rd = null)
    {
        if(rd == null)
        {
            rd = new System.Random();
        }
        //在[min,max+1)随机选择一个数
        Debug.Log("min " + min + "max+1 " + max + 1);
        return rd.Next(min, max+1);
    }
}
