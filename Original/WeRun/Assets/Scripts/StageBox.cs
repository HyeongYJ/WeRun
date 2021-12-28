using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBox : MonoBehaviour
{
    public float StageTipSize = 4f;

    int currentTipIndex;

    public Transform character; //��� ĳ���� ����

    //������ �������� ���� �������� ������ �����Ͽ� �� �迭 �ȿ��� ������ �������� ���� �������� ������
    public GameObject[] stageTips;  //�������� ���� ������ �迭

    public int startTipIndex;   //�ڵ� ���� ���� �ε���

    public int preInstantiate;  //�̸� �����ؼ� �о���� �������� ���� ����

    public List<GameObject> generatedStageList = new List<GameObject>();    //������ �������� ���� ���� ����Ʈ

    void Start()    //�ʱ�ȭ ó��
    {
        currentTipIndex = startTipIndex - 1; //���� ���� �ÿ� � �������� ���� �ڵ� ������ ���������� ����
        UpdateStage(preInstantiate);
    }

    void Update()   //���������� ������Ʈ Ÿ�̹� ����
    {
        //ĳ������ ��ġ���� ���� �������� ���� �ε����� ���
        int charaPositionIndex = (int)(character.position.z / StageTipSize);

        //������ �������� ���� ���� ���������� ������Ʈ ó���� �ǽ�
        if (charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);

            
        }
        
    }

    //���� Index������ �������� ���� �����Ͽ� �����صд�
    void UpdateStage(int toTipIndex)    //���������� ������Ʈ ó��
    {
        if (toTipIndex <= currentTipIndex) return;  //�̷��� �� ���� ���µ�?

        //������ �������� �������� �ۼ�
        for (int i = currentTipIndex + 1; i <= toTipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            //������ ���������� ���� ����Ʈ�� �߰�
            generatedStageList.Add(stageObject);

            //�������� ���� �ѵ��� �ʰ��ߴٸ� ���� ���������� ����
            while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage() ;


            currentTipIndex = toTipIndex;
        }
    }

    //���� �ε��� ��ġ�� Stage ������Ʈ�� ���Ƿ� �ۼ�
    GameObject GenerateStage(int tipIndex)  //���������� ���� ó��
    {
        int nextStageTip = Random.Range(0, stageTips.Length);

        GameObject stageObject = (GameObject)Instantiate(stageTips[nextStageTip], new Vector3(0,0, tipIndex * StageTipSize), Quaternion.identity);

        return stageObject;

    }

    //���� ������ ���������� ����
    void DestroyOldestStage()   //���������� ���� ó��
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
