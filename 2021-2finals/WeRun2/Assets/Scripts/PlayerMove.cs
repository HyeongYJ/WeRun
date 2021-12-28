using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    #region 캐릭터
    CharacterController controller;     //캐릭터
    Animator animator;      //애니메이션
    #endregion

    #region PD관련
    PlayableDirector pd;    //timeline
    public GameObject targetCam;    //timeline 카메라 사용
    #endregion

    #region 생명과 기절
    const int DefaultLife = 3;  //생명수
    public static int life = DefaultLife;     //생명
    const float StunDuration = 2f;      //2초기절
    float recoverTime = 0.0f;       //기절 시간
    private float lodingTime = 0;   //로딩시간

    private bool loding = false;    //기절로 움직임 잠시 멈춤
    private bool ending = false;    //게임 클리어로 애니메이션 작동 ㄴㄴ
    #endregion

    #region 이동 좌표
    private Vector3 mousePos = Vector3.zero;        //마우스 위치
    private Vector3 moveDirection = Vector3.zero;       //캐릭터 이동
    private float levelPosition = 0;        //속도 증가 조절 좌표
    private Camera mainCamera = null;       //카메라
    #endregion

    #region 이동을 위한 설정
    private float speedZ = 4;    //시작속도
    private float gravity = 20;       //중력
    private float speedJump = 8;         //점프값
    private float accelerationZ = 2;	        //천천히 전진 가속도 올리는 파라미터 값
    private int jump;   //점프 제안
    #endregion

    #region 충돌 관련
    private GameObject attachedObj = null;      // 현재 충돌한 충돌체를 저장하는 변수 = 박스 충돌에서 사용
    public GameObject dropWater = null;         //물속 이펙트
    #endregion

    public AudioClip[] clips;       //음악 저장소

    private Rigidbody playerRigid = null;



    public int Life()   //라이프 취득용 함수
    {
        return life;
    }
    public bool IsStan()    //기절 판정
    {
        return recoverTime > 0.0f || life <= 0;
    }

    // 게임이 시작됬을 때 1회 호출되는 함수
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();
        pd = GameObject.Find("pd").GetComponent<PlayableDirector>();

    }

    // 매 프레임마다 호출되는 함수
    void Update()
    {
        //IdleTime();
        PlayerRun();
        LodingTime();
        ZeroLife();
        if (Input.GetButtonDown("Fire1") && GameController.musicOnOff)
        {
            GameController.instance.AudioPlay(clips[1], 0.5f);
        }
        

    }

    //private void IdleTime()
    //{
    //    loding = true;
    //    if (lodingTime > 1.4f)
    //    {
    //        loding = false;
    //        lodingTime = 0;

    //        PlayerRun();
    //    }

    //}

    private void PlayerRun()
    {
            if (IsStan() && lodingTime <= 2f || ending)   //기절 시의 행동
            {
                //움직임을 기절 상태에서 복귀 카운트를 진행한다.
                moveDirection.x = 0.0f;
                moveDirection.z = 0.0f;
                recoverTime -= Time.deltaTime;
            }
            else
            {
                //20은 카메라의 20 앞에서 커서를 찾는다는 뜻으로 0이면 화면 전체가 너무 작아서 조금만 움직여서 확확 이동함
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 20f));
                moveDirection.x = mousePos.x * 0.5f;

                if ((transform.position.z - levelPosition >= 90) && speedZ <= 8)
                {
                    levelPosition += 90;
                    speedZ += 1f;
                    print("speed : " + speedZ);
                }

                //if( playerRigid.velocity == speedZ)
                //{
                    
                //}

                //서서히 가속하여 z방향으로 계속 전진시킨다
                float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);    //전진 속도 계산
                //속도가 acceleratedZ이 0 ~ speedZ 로 올라가며 speedZ를 넘지 않게 함
                moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);


                //지상에 있을 경우에만 조작한다
                if (controller.isGrounded)  //접지하고 있는지를 판정
                {
                    jump = 0;
                }
                if (Input.GetButtonDown("Fire1") && (jump < 2))
                {
                    jump++;
                    if (jump == 1)
                        moveDirection.y = speedJump;
                    else if (jump == 2)
                        moveDirection.y = 1.3f * speedJump;
                }


                //마우스 조정 제한
                if (transform.position.x > 4f && mousePos.x > 0)
                {
                    moveDirection.x = 0;
                }
                else if (transform.position.x < -4f && mousePos.x < 0)
                {
                    moveDirection.x = 0;
                }
            }


            //중력만큼의 힘을 매 프레임에 추가
            moveDirection.y -= gravity * Time.deltaTime;    //중력의 가산

            //이동 실행 = 캐릭터 이동
            Vector3 globalDirection = transform.TransformDirection(moveDirection);
            controller.Move(globalDirection * Time.deltaTime);

            //이동 후 땅에 닿으면 Y 방향의 속도는 리셋한다
            if (controller.isGrounded) moveDirection.y = 0;

            //속도가 0 이상이면 달리고 있는 플래그를 true로 한다
            animator.SetBool("run", moveDirection.z > 0.0f);    //idle과 run 애니메이션의 제어
    }

    private void LodingTime()
    {
        if (loding || ending)
        {
            lodingTime += Time.deltaTime;
        }
    }

    private void ZeroLife()
    {
        if (life == 0)
        {
            LodingTime();
            if (lodingTime >= 2.5f)
            {
                GameController.gameOverUIOnOff = true;
                lodingTime = 0;
                enabled = false;
            }

        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.CompareTag("waterCube"))
        {

            if (!loding)
            {
                //물 사운드
                if (GameController.musicOnOff)
                    GameController.instance.AudioPlay(clips[0], 0.8f);

                animator.SetTrigger("Help");
                loding = true;
                //라이프를 줄이고 기절 상태로 전화
                life--;
                recoverTime = StunDuration;
            }
            else if (loding)
            {
                if (hit.gameObject.CompareTag("Box") || lodingTime >= 4.0f)
                {
                    lodingTime = 0;
                    loding = false;
                }
                return;
            }
            //플레이어 위치에 파티클 생성
            Vector3 dropPosition = new Vector3(transform.position.x, -1.5f, transform.position.z-1);
            GameObject waterdrop = Instantiate(dropWater, dropPosition, Quaternion.Euler(-90f, 0,0));
        }

        // 충돌한 오브젝트가 같지 않을 때
        if (attachedObj != hit.gameObject)
        {

            attachedObj = hit.gameObject;   // 충돌한 오브젝트를 저장

            // 충돌한 오브젝트의 태그가 Box 이고 충돌체와 접촉되지 않고 있을 때
            if (hit.gameObject.CompareTag("Box") || hit.gameObject.CompareTag("backBox"))
            {
                //  캐릭터의 낙하 힘이 0보다 작고 (아래로 떨어지는 힘이라 -로 감소함)
                // 캐릭터 위치가 Box 오브젝트보다 위에 있을 때
                if (controller.velocity.y < 0 && transform.position.y > 0.04f)
                {
                    // Box오브젝트의 애니메이션 실행
                    hit.gameObject.GetComponent<Animator>().SetBool("Down", true);
                    if (hit.gameObject.CompareTag("backBox"))
                    {
                        moveDirection.z = 0.0f;
                    }


                }
            }
        }

        if (hit.gameObject.CompareTag("EndCube"))
        {
            ending = true;

            targetCam.GetComponent<FollowCamera>().enabled = false;
            targetCam.GetComponent<CinemachineBrain>().enabled = true;
            pd.enabled = true;
            pd.Play();
            GameController.instance.gameEndingUI.SetActive(true);
            GameController.instance.gameInUI.SetActive(false);
            GameController.instance.Ending();
        }
    }


}
