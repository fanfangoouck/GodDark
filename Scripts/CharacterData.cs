using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public int level { get; set; }
    
    public int fight { get; set; }

    public int id{ get; set; }

	public string name{ get; set; }

	public int exp{ get; set; }

	public int power{ get; set; }

	public int coin{ get; set; }

	public int diamond{ get; set; }

	public int hp{ get; set; }

	public int ad{ get; set; }

	public int ap{ get; set; }

	public int addef{ get; set; }

	public int apdef{ get; set; }

	public int dodge{ get; set; }

	public int pierce{ get; set; }

	public int critical{ get; set; }

	public int guideid{ get; set; }



    public CharacterData(string name, int level, int power, int fight, int hp, int ad, int ap, int addef, int apdef, int dodge, int pierce, int critical, int guideid){
        this.name = name;
        this.level = level;
        this.power = power;
        this.fight = fight;
        this.hp = hp;
        this.ad = ad;
        this.ap = ap;
        this.addef = addef;
        this.apdef = apdef;
        this.dodge = dodge;
        this.pierce = pierce;
        this.critical = critical;
        this.guideid = guideid;

    }

    public static CharacterData sky = new CharacterData("天空妹", 10, 100, 200,50,100,200,400,500,3,4,5,1001);

    void Start()
    {
        Debug.Log(sky.name);
    }


    //public static int GetFightByProps (PlayerData pd)
    //{
	   // return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    //}


}
