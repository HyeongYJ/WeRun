using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region UI
    [SerializeField] private GameObject gameStartUI = null;     //���� ���� �� �˾��Ǵ� UI
    [SerializeField] public GameObject gameInUI = null;        //���� �� UI
    [SerializeField] private GameObject gameOverUI = null;      //���� ���� �� �˾��Ǵ� UI
    [SerializeField] public GameObject gameEndingUI = null;      //���� Ŭ���� �� �˾��Ǵ� UI
    #endregion

    #region ��������
    [SerializeField] private GameObject player = null;      //Player
    [SerializeField] private GameObject bgMusic = null;     //���� ��/����

    

    public static bool musicOnOff = true;      //Ŭ�� ����/ �̺�Ʈ ���� ��/����
    public static bool gameOverUIOnOff = false;     //���� �����
    #endregion

    public PlayerMove heart;    //��Ʈ ��
    public LifePanel lifePanel;     //��Ʈ UI

    private AudioSource audioSource = null;

    public static GameController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //Destroy(gameObject);
            Destroy(instance.gameObject);
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        #region ������Ʈ ã��
        gameStartUI = GameObject.Find("Canvas_Start");
        gameInUI = GameObject.Find("Canvas_InGame");
        gameOverUI = GameObject.Find("Canvas_GameOver");
        gameEndingUI = GameObject.Find("Canvas_End");

        player = GameObject.FindGameObjectWithTag("Player");
        bgMusic = GameObject.Find("BgMusic");
        audioSource = GetComponent<AudioSource>();
        #endregion

        gameInUI.SetActive(false);  //���� �� ��Ȱ��ȭ
        gameOverUI.SetActive(false);    //���� ���� UI�� ��Ȱ��ȭ
        gameEndingUI.SetActive(false);  //���� Ŭ���� UI ��Ȱ��ȭ
                                        // check
        GameStart();
    }

    void Update()
    {
        GameOver();
        lifePanel.UpdateLife(heart.Life()); //��Ʈ �ǽð� �� ��������
        if (Input.GetButtonDown("Cancel")) Exit();
    }


    public void GameStart()     //���� ����
    {
        if (player != null)
            player.SetActive(!player.activeSelf);
    }

    public void Exit()    //���� ����
    {
        Application.Quit();
    }

    public void GameOver()    //�װ� �Ǹ� GameOverâ ����
    {
        if (gameOverUIOnOff)    //�װ� �Ǹ� GameOverâ ����
        {
            gameOverUI.SetActive(true);     //GameOver UI ����
            gameInUI.SetActive(false);  //���� �� ��Ȱ��ȭ
            player.SetActive(!player.activeSelf);   //player ������� �ϱ�
            gameOverUIOnOff = false;
        }
    }

    public void ReloadScene()       //�����
    {
        if (gameEndingUI == true)
            gameEndingUI.SetActive(false);
        musicOnOff = true;
        PlayerMove.life = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void Ending()    //���� ���� �ٽ� ����
    {
        Invoke("ReloadScene", 28f);
    }

    public void MusicOnOff()        //���� ����
    {
        musicOnOff = !musicOnOff;       //�̺�Ʈ ���� ����
        if (bgMusic != null)        //����� ���� ����
            bgMusic.SetActive(!bgMusic.activeSelf);
    }

    public void AudioPlay(AudioClip clip, float volume = 1f)
    {
        //���� ������� AudioClip�� ���޹��� AudioCilp�� �������� ��
        //if (audioSource.clip == clip)
            //audioSource.Stop();     //������� Audio�� �ߴ�

        audioSource.clip = clip;                //AudioSource�� AudioClip�� �����ϴ� ���
        audioSource.volume = volume;    //AudioSource�� Volume�� �����ϴ� ���

        audioSource.Play();                  //AudioSource�� ��ϵ� AudioClip�� ���
    }

}
