using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBox : MonoBehaviour
{
    public float StageTipSize = 4f;

    int currentTipIndex;

    public Transform character; //대상 캐릭터 지정

    //생성할 스테이지 팁의 프리팹을 여러개 지정하여 이 배열 안에서 생성할 스테이지 팁을 무작위로 선택함
    public GameObject[] stageTips;  //스테이지 팁의 프리팹 배열

    public int startTipIndex;   //자동 생성 개시 인덱스

    public int preInstantiate;  //미리 생성해서 읽어들일 스테이지 팁의 개수

    public List<GameObject> generatedStageList = new List<GameObject>();    //생성된 스테이지 팁의 보유 리스트

    void Start()    //초기화 처리
    {
        currentTipIndex = startTipIndex - 1; //게임 시작 시에 어떤 스테이지 팁을 자동 생성을 시작할지를 지정
        UpdateStage(preInstantiate);
    }

    void Update()   //스테이지의 업데이트 타이밍 감시
    {
        //캐릭터의 위치에서 현재 스테이지 팁의 인덱스를 계산
        int charaPositionIndex = (int)(character.position.z / StageTipSize);

        //다음의 스테이지 팁에 들어가면 스테이지의 업데이트 처리를 실시
        if (charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);

            
        }
        
    }

    //지정 Index까지의 스테이지 팁을 생성하여 관리해둔다
    void UpdateStage(int toTipIndex)    //스테이지의 업데이트 처리
    {
        if (toTipIndex <= currentTipIndex) return;  //이렇게 될 수가 없는디?

        //지정한 스테이지 팁까지를 작성
        for (int i = currentTipIndex + 1; i <= toTipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            //생성한 스테이지를 관리 리스트에 추가
            generatedStageList.Add(stageObject);

            //스테이지 보유 한도를 초과했다면 예전 스테이지를 삭제
            while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage() ;


            currentTipIndex = toTipIndex;
        }
    }

    //지정 인덱스 위치에 Stage 오브젝트를 임의로 작성
    GameObject GenerateStage(int tipIndex)  //스테이지의 생성 처리
    {
        int nextStageTip = Random.Range(0, stageTips.Length);

        GameObject stageObject = (GameObject)Instantiate(stageTips[nextStageTip], new Vector3(0,0, tipIndex * StageTipSize), Quaternion.identity);

        return stageObject;

    }

    //가장 오래된 스테이지를 삭제
    void DestroyOldestStage()   //스테이지의 삭제 처리
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
