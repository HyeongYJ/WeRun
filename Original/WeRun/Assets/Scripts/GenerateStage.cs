using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{
    public Transform target;        // 위치를 비교할 대상

    public GameObject[] stages;     // 생성할 스테이지

    public int firstStageCount = 0;     // 처음 생성할 스테이지 개수

    public float generateDistance = 0;      // 스테이지를 생성시키는 거리 간격

    private float moveDistance = 12.5f;          // 맵이 생성 되기 위해 플레이어가 이동해야하는 거리

    private int compareCount = 0;

    public List<GameObject> generatedStageList = new List<GameObject>();    // 생성된 스테이지를 저장하는 변수

    public Vector3 createPos = Vector3.zero;       //생성할 스테이지  위치

    void Start()
    {
        //처음 맵 20개 생성
        createPos.z += 3.5f;
        InitStage();
    }

    void Update()
    {
        //캐릭터의 이동에 따른 맵 생성
        if (target.position.z > moveDistance)
        {
            if (compareCount < 100)
            {
                UpdateStage();
            }

            CompareStage();
        }
    }

    // 처음 게임이 시작되었을 때 맵을 생성하는 기능
    private void InitStage()
    {
        //박스 20개 만들기
        for (int i = 1; i <= firstStageCount; i++)
        {
            // 생성할 Z 축 위치 = i * 4.5f;
            createPos.z += generateDistance;

            //스테이지 생성
            GameObject box = Instantiate(stages[Random.Range(0, stages.Length)], createPos, Quaternion.identity);

            //리스트 추가
            generatedStageList.Add(box);
        }
    }

    private void UpdateStage()
    {
        //오래된 스테이지 삭제
        DestroyOldStage();  //0

        // 현재 스테이지 개수
        compareCount++;

        switch (compareCount)
        {
            case 1:
                generateDistance += 0.5f;
                break;
            case 21:
                generateDistance += 1f;
                break;
            case 41:
                generateDistance += 1f;
                break;
            case 61:
                generateDistance += 1f;
                break;
            case 81:
                generateDistance += 1f;
                break;
        }

        createPos.z += generateDistance;

        //박스 맨 끝 1개 생성
        GameObject box = Instantiate(stages[Random.Range(0, stages.Length)], createPos, Quaternion.identity);

        //리스트 추가
        generatedStageList.Add(box);


    }

    private void CompareStage()
    {
        // 지금 있는 스테이지와 다음 스테이지 간의 간격을 구해오는 기능
        float distance = Vector3.Distance(generatedStageList[0].transform.position, generatedStageList[1].transform.position);

        // 비교할 거리에 누적 시켜준다.
        moveDistance += distance;
    }

    // 리스트에 가장 오래된 오브젝트를 삭제하는 기능
    private void DestroyOldStage()
    {
        GameObject oldStage = generatedStageList[0];    // 리스트에 가장 먼저 저장된 오브젝트를 저장

        generatedStageList.RemoveAt(0);     // 리스트에 가장 먼저 저장된 오브젝트를 리스트에서 삭제

        Destroy(oldStage);  // 리스트에 가장 먼저 저장된 오브젝트를 삭제
    }
}
