/*
 * 资源加载服务
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;
    private Action  prgCB = null;

    public void InitSvc()
    {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        Debug.Log("Init ResSvc...");
    }


    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="sceneName"></param>
    
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        //显示加载界面
        //初始化加载界面
        GameRoot.Instance.loadingWnd.SetWndState(true);
        //GameRoot.Instance.loadingWnd.gameObject.SetActive(true);
        //GameRoot.Instance.loadingWnd.Init();

        //only provide the Scene name, Unity loads the first Scene in the list that matches.
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        prgCB = () => {
            float val = sceneAsync.progress;//获得SceneLogin场景的加载进度
            GameRoot.Instance.loadingWnd.SetProgress(val);//更新加载进度
            if(val == 1)
            {
                if(loaded != null)
                {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
        
    }

    void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }

    //建立音频的缓存
    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    //获得加载的音乐
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        Debug.Log(" 进入LoadAudio");
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au))
        {
            au = Resources.Load<AudioClip>(path);
        }
        return au;
    }


    //加载gameObject
      //创造缓存列表
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        //(!goDic.TryGetValue(path, out prefab)： 没有缓存prefab
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            Debug.Log("prefab  是null吗 " + prefab);

            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if (prefab != null)
        {
            //实例化prefab
            go = Instantiate(prefab);
            Debug.Log("实例化prefab, prefab   = " + go);
            Debug.Log(" go  = " + go);
        }
        return go;
    }


      private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
        public Sprite LoadSprite(string path, bool cache = false) {
            Sprite sp = null;
            if (!spDic.TryGetValue(path, out sp)) {
                sp = Resources.Load<Sprite>(path);
                if (cache) {
                    spDic.Add(path, sp);
                }
            }
            return sp;


    }







    //解析
    #region InitCfgs
    private List<string> surnameLst = new List<string>();//姓
    private List<string> manLst = new List<string>();//男名
    private List<string> womanLst = new List<string>();//女名

    //生成xml对应的列表
    private void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(PathDefine.RDNameCfg);
        if (!xml)
        {
            Debug.LogError("xml file:" + path + " not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            //获取根节点下的所有子节点的List
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for(int i = 0; i < nodLst.Count; i++)
            {
                //将某一个节点转化为一个XmlElement
                XmlElement ele = nodLst[i] as XmlElement;
                //从XmlElement里获取名称为"ID"的数据
                string value = ele.GetAttributeNode("ID").InnerText;
                if(value != null)
                {
                    int ID = Convert.ToInt32(value);
                    
                    foreach( XmlElement e in ele.ChildNodes)
                    {
                        switch (e.Name)
                        {
                            case "surname":
                            surnameLst.Add(e.InnerText);
                                break;
                            case "man":
                                manLst.Add(e.InnerText);
                                break;
                            case "woman":
                                womanLst.Add(e.InnerText);
                                break;
                        }
                    }
                }
            }
        }
    }

    //获得列表里的数据
    public string GetRDNameData(bool man = true)
    {
        
        Debug.Log("GetRDNameData");
        string name = surnameLst[PETools.RDInt(0, surnameLst.Count - 1)];
        if (man)
        {
            name += manLst[PETools.RDInt(0, manLst.Count - 1)];
        }
        else
        {
            name += womanLst[PETools.RDInt(0, womanLst.Count - 1)];
        }
        return name;
    }

    #endregion


     

    #region 地图
     private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg(string path) {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml) {
            Debug.Log("xml file:" + path + " not exist");
        }
        else {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++) {
                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapCfg mc = new MapCfg {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes) {
                    switch (e.Name) {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "mainCamPos": {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "mainCamRote": {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornPos": {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornRote": {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                    }
                }
                mapCfgDataDic.Add(ID, mc);
            }
        }
    }

    public MapCfg GetMapCfgData(int id) {
        MapCfg data;
        if (mapCfgDataDic.TryGetValue(id, out data)) {
            return data;
        }
        return null;
    }
    #endregion


    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path) {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml) {
            Debug.Log("xml file:" + path + " not exist");
        }
        else {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++) {
                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg mc = new AutoGuideCfg {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes) {
                    switch (e.Name) {
                        case "npcID":
                            mc.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            mc.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            mc.actID = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            mc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            mc.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                guideTaskDic.Add(ID, mc);
            }
        }
    }
    public AutoGuideCfg GetAutoGuideData(int id) {
        AutoGuideCfg agc = null;
        if (guideTaskDic.TryGetValue(id, out agc)) {
            Debug.Log("这里成功没");
            return agc;
        }
        return null;
    }

    #endregion


}
