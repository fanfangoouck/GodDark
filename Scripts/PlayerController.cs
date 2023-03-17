//角色控制器

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     private Transform camTrans;
     private Vector3 camOffset;
     public Animator ani;
     public CharacterController ctrl;

    public GuideWndGuess guideWndGuess;

     //是否在移动
     private bool isMove = false;
     private Vector2 dir = Vector2.zero;
     //实时更新dir 和 isMove
     public Vector2 Dir
     {
        get
        {
            return dir;
        }
        set
        {
            if(value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
     }

     private float targetBlend;
     private float currentBlend;



    public void Init()
    {
        //获得相机的transform组件
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }

    private void Update()
    {
        #region Input
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //Vector2 _dir = new Vector2(h, v).normalized;
        //if (_dir != Vector2.zero) {
        //    Dir = _dir;
        //    //Blend= 1
        //    SetBlend(Constants.BlendWalk);
        //}
        //else {
        //    Dir = Vector2.zero;
        //    //Blend = 0;
        //    SetBlend(Constants.BlendIdle);
        //}
        #endregion

        //缓冲，Blend慢慢改变
        if (currentBlend != targetBlend)
        {
            UpdateMixBlend();
        }

        //如果在移动
        if (isMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }


        //检测是否与NPC对话
        if (Input.GetKeyDown(KeyCode.T))
        {
            guideWndGuess = MainCitySys.Instance.OpenGuideWnd2();
            Debug.Log("走到这1");
            guideWndGuess.GuessTalk();
            Debug.Log("走到这2");
        }
    }

       private void SetDir() {
        //当前位置与Vector2(0, 1)（初始位置）之间的夹脚
        //float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        //再把角度 赋给当前物体
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
        }

        private void SetMove() {
        //往前走transform.forward，蓝色轴
            ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
        }

        public void SetCam() {
            if (camTrans != null) {
                camTrans.position = transform.position - camOffset;
            }
        }

       public void SetBlend(float blend) {
           targetBlend = blend;
        }

        private void UpdateMixBlend() {
            //Constants.AccelerSpeed * Time.deltaTime： 一帧的变化
            //currentBlend - targetBlend 小于一帧的变化，直接等于target
            if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime) {
                currentBlend = targetBlend;
            }
            else if (currentBlend > targetBlend) {
                currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
            }
            else {
                currentBlend += Constants.AccelerSpeed * Time.deltaTime;
            }
            //设置blend的值
            ani.SetFloat("Blend", currentBlend);
        }

        public void SetCharacterController(bool Stuation)
        {
              // 关闭CharacterController
          CharacterController cc =  GetComponent(typeof(CharacterController)) as CharacterController;
           cc.enabled = Stuation;
        }

}
