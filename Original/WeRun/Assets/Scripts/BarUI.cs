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
    private float upFillAmount = 7.5f;      //�ִ� �� 750�� +5
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

        //amountSpeed�� Ŭ���� ������ �ö󰡰� ���� ���� �ε巴�� �ö󰣴�
        //bar2.fillAmount�� increaseValue��ŭ amountSpeed * Time.deltaTime�� �ӵ��� �ε巴�� ����
        bar2.fillAmount = Mathf.MoveTowards(bar2.fillAmount, increaseValue, amountSpeed * Time.deltaTime);

    }
}
