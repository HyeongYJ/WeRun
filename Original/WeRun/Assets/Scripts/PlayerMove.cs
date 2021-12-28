using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    #region ĳ����
    CharacterController controller;     //ĳ����
    Animator animator;      //�ִϸ��̼�
    #endregion

    #region PD����
    PlayableDirector pd;    //timeline
    public GameObject targetCam;    //timeline ī�޶� ���
    #endregion

    #region ����� ����
    const int DefaultLife = 3;  //�����
    public static int life = DefaultLife;     //����
    const float StunDuration = 2f;      //2�ʱ���
    float recoverTime = 0.0f;       //���� �ð�
    private float lodingTime = 0;   //�ε��ð�

    private bool loding = false;    //������ ������ ��� ����
    private bool ending = false;    //���� Ŭ����� �ִϸ��̼� �۵� ����
    #endregion

    #region �̵� ��ǥ
    private Vector3 mousePos = Vector3.zero;        //���콺 ��ġ
    private Vector3 moveDirection = Vector3.zero;       //ĳ���� �̵�
    private float levelPosition = 0;        //�ӵ� ���� ���� ��ǥ
    private Camera mainCamera = null;       //ī�޶�
    #endregion

    #region �̵��� ���� ����
    private float speedZ = 4;    //���ۼӵ�
    private float gravity = 20;       //�߷�
    private float speedJump = 8;         //������
    private float accelerationZ = 2;	        //õõ�� ���� ���ӵ� �ø��� �Ķ���� ��
    private int jump;   //���� ����
    #endregion

    #region �浹 ����
    private GameObject attachedObj = null;      // ���� �浹�� �浹ü�� �����ϴ� ���� = �ڽ� �浹���� ���
    public GameObject dropWater = null;         //���� ����Ʈ
    #endregion

    public AudioClip[] clips;       //���� �����

    private Rigidbody playerRigid = null;



    public int Life()   //������ ���� �Լ�
    {
        return life;
    }
    public bool IsStan()    //���� ����
    {
        return recoverTime > 0.0f || life <= 0;
    }

    // ������ ���ۉ��� �� 1ȸ ȣ��Ǵ� �Լ�
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();
        pd = GameObject.Find("pd").GetComponent<PlayableDirector>();

    }

    // �� �����Ӹ��� ȣ��Ǵ� �Լ�
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
            if (IsStan() && lodingTime <= 2f || ending)   //���� ���� �ൿ
            {
                //�������� ���� ���¿��� ���� ī��Ʈ�� �����Ѵ�.
                moveDirection.x = 0.0f;
                moveDirection.z = 0.0f;
                recoverTime -= Time.deltaTime;
            }
            else
            {
                //20�� ī�޶��� 20 �տ��� Ŀ���� ã�´ٴ� ������ 0�̸� ȭ�� ��ü�� �ʹ� �۾Ƽ� ���ݸ� �������� ȮȮ �̵���
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

                //������ �����Ͽ� z�������� ��� ������Ų��
                float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);    //���� �ӵ� ���
                //�ӵ��� acceleratedZ�� 0 ~ speedZ �� �ö󰡸� speedZ�� ���� �ʰ� ��
                moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);


                //���� ���� ��쿡�� �����Ѵ�
                if (controller.isGrounded)  //�����ϰ� �ִ����� ����
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


                //���콺 ���� ����
                if (transform.position.x > 4f && mousePos.x > 0)
                {
                    moveDirection.x = 0;
                }
                else if (transform.position.x < -4f && mousePos.x < 0)
                {
                    moveDirection.x = 0;
                }
            }


            //�߷¸�ŭ�� ���� �� �����ӿ� �߰�
            moveDirection.y -= gravity * Time.deltaTime;    //�߷��� ����

            //�̵� ���� = ĳ���� �̵�
            Vector3 globalDirection = transform.TransformDirection(moveDirection);
            controller.Move(globalDirection * Time.deltaTime);

            //�̵� �� ���� ������ Y ������ �ӵ��� �����Ѵ�
            if (controller.isGrounded) moveDirection.y = 0;

            //�ӵ��� 0 �̻��̸� �޸��� �ִ� �÷��׸� true�� �Ѵ�
            animator.SetBool("run", moveDirection.z > 0.0f);    //idle�� run �ִϸ��̼��� ����
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
                //�� ����
                if (GameController.musicOnOff)
                    GameController.instance.AudioPlay(clips[0], 0.8f);

                animator.SetTrigger("Help");
                loding = true;
                //�������� ���̰� ���� ���·� ��ȭ
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
            //�÷��̾� ��ġ�� ��ƼŬ ����
            Vector3 dropPosition = new Vector3(transform.position.x, -1.5f, transform.position.z-1);
            GameObject waterdrop = Instantiate(dropWater, dropPosition, Quaternion.Euler(-90f, 0,0));
        }

        // �浹�� ������Ʈ�� ���� ���� ��
        if (attachedObj != hit.gameObject)
        {

            attachedObj = hit.gameObject;   // �浹�� ������Ʈ�� ����

            // �浹�� ������Ʈ�� �±װ� Box �̰� �浹ü�� ���˵��� �ʰ� ���� ��
            if (hit.gameObject.CompareTag("Box") || hit.gameObject.CompareTag("backBox"))
            {
                //  ĳ������ ���� ���� 0���� �۰� (�Ʒ��� �������� ���̶� -�� ������)
                // ĳ���� ��ġ�� Box ������Ʈ���� ���� ���� ��
                if (controller.velocity.y < 0 && transform.position.y > 0.04f)
                {
                    // Box������Ʈ�� �ִϸ��̼� ����
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
