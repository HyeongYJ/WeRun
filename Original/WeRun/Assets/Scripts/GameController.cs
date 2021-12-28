using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region UI
    [SerializeField] private GameObject gameStartUI = null;     //게임 실행 시 팝업되는 UI
    [SerializeField] public GameObject gameInUI = null;        //게임 바 UI
    [SerializeField] private GameObject gameOverUI = null;      //게임 오버 시 팝업되는 UI
    [SerializeField] public GameObject gameEndingUI = null;      //게임 클리어 시 팝업되는 UI
    #endregion

    #region 게임정보
    [SerializeField] private GameObject player = null;      //Player
    [SerializeField] private GameObject bgMusic = null;     //음악 온/오프

    

    public static bool musicOnOff = true;      //클릭 사운드/ 이벤트 사운드 온/오프
    public static bool gameOverUIOnOff = false;     //게임 재시작
    #endregion

    public PlayerMove heart;    //하트 수
    public LifePanel lifePanel;     //하트 UI

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
        #region 오브젝트 찾기
        gameStartUI = GameObject.Find("Canvas_Start");
        gameInUI = GameObject.Find("Canvas_InGame");
        gameOverUI = GameObject.Find("Canvas_GameOver");
        gameEndingUI = GameObject.Find("Canvas_End");

        player = GameObject.FindGameObjectWithTag("Player");
        bgMusic = GameObject.Find("BgMusic");
        audioSource = GetComponent<AudioSource>();
        #endregion

        gameInUI.SetActive(false);  //게임 바 비활성화
        gameOverUI.SetActive(false);    //게임 오버 UI를 비활성화
        gameEndingUI.SetActive(false);  //게임 클링어 UI 비활성화
                                        // check
        GameStart();
    }

    void Update()
    {
        GameOver();
        lifePanel.UpdateLife(heart.Life()); //하트 실시간 값 가져오기
        if (Input.GetButtonDown("Cancel")) Exit();
    }


    public void GameStart()     //게임 시작
    {
        if (player != null)
            player.SetActive(!player.activeSelf);
    }

    public void Exit()    //게임 종료
    {
        Application.Quit();
    }

    public void GameOver()    //죽게 되면 GameOver창 띄우기
    {
        if (gameOverUIOnOff)    //죽게 되면 GameOver창 띄우기
        {
            gameOverUI.SetActive(true);     //GameOver UI 띄우기
            gameInUI.SetActive(false);  //게임 바 비활성화
            player.SetActive(!player.activeSelf);   //player 사라지게 하기
            gameOverUIOnOff = false;
        }
    }

    public void ReloadScene()       //재시작
    {
        if (gameEndingUI == true)
            gameEndingUI.SetActive(false);
        musicOnOff = true;
        PlayerMove.life = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void Ending()    //엔딩 보고 다시 시작
    {
        Invoke("ReloadScene", 28f);
    }

    public void MusicOnOff()        //사운드 조절
    {
        musicOnOff = !musicOnOff;       //이벤트 사운드 조절
        if (bgMusic != null)        //배경음 사운드 조절
            bgMusic.SetActive(!bgMusic.activeSelf);
    }

    public void AudioPlay(AudioClip clip, float volume = 1f)
    {
        //현재 재생중인 AudioClip이 전달받은 AudioCilp과 같은지를 비교
        //if (audioSource.clip == clip)
            //audioSource.Stop();     //재생중인 Audio를 중단

        audioSource.clip = clip;                //AudioSource에 AudioClip을 설정하는 기능
        audioSource.volume = volume;    //AudioSource에 Volume을 설정하는 기능

        audioSource.Play();                  //AudioSource에 등록된 AudioClip을 재생
    }

}
