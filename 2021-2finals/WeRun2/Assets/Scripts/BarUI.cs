using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    public Image bar2;

    public float amountSpeed = 0;

    private Transform playerPos = null;

    private float upNum = 0;
    private float upFillAmount = 7.5f;      //최대 값 750에 +5
    private float increaseValue = 0;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if(playerPos.position.z - upNum >= upFillAmount)
        {
            upNum += upFillAmount;
            increaseValue += 0.01f;
            //bar2.fillAmount += 0.01f;
        }

        //amountSpeed가 클수록 딱딱딱 올라가고 작을 수록 부드럽게 올라간다
        //bar2.fillAmount가 increaseValue만큼 amountSpeed * Time.deltaTime의 속도록 부드럽게 증가
        bar2.fillAmount = Mathf.MoveTowards(bar2.fillAmount, increaseValue, amountSpeed * Time.deltaTime);

    }
}
