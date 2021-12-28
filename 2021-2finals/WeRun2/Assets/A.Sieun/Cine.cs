using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class Cine : MonoBehaviour
{
    PlayableDirector pd;
    public GameObject targetCam;

    // Start is called before the first frame update
    void Start()
    {
        pd = gameObject.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {

        // 마우스 오른쪽 버튼을 클릭했을때 컷신을 실행하자.
        if (Input.GetButtonDown("Fire3"))
        {
            pd.Play();
            print("!!");
        }
    }
}
