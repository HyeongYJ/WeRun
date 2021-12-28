using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{
    public Transform target;        // ��ġ�� ���� ���

    public GameObject[] stages;     // ������ ��������

    public int firstStageCount = 0;     // ó�� ������ �������� ����

    public float generateDistance = 0;      // ���������� ������Ű�� �Ÿ� ����

    private float moveDistance = 12.5f;          // ���� ���� �Ǳ� ���� �÷��̾ �̵��ؾ��ϴ� �Ÿ�

    private int compareCount = 0;

    public List<GameObject> generatedStageList = new List<GameObject>();    // ������ ���������� �����ϴ� ����

    public Vector3 createPos = Vector3.zero;       //������ ��������  ��ġ

    void Start()
    {
        //ó�� �� 20�� ����
        createPos.z += 3.5f;
        InitStage();
    }

    void Update()
    {
        //ĳ������ �̵��� ���� �� ����
        if (target.position.z > moveDistance)
        {
            if (compareCount < 100)
            {
                UpdateStage();
            }

            CompareStage();
        }
    }

    // ó�� ������ ���۵Ǿ��� �� ���� �����ϴ� ���
    private void InitStage()
    {
        //�ڽ� 20�� �����
        for (int i = 1; i <= firstStageCount; i++)
        {
            // ������ Z �� ��ġ = i * 4.5f;
            createPos.z += generateDistance;

            //�������� ����
            GameObject box = Instantiate(stages[Random.Range(0, stages.Length)], createPos, Quaternion.identity);

            //����Ʈ �߰�
            generatedStageList.Add(box);
        }
    }

    private void UpdateStage()
    {
        //������ �������� ����
        DestroyOldStage();  //0

        // ���� �������� ����
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

        //�ڽ� �� �� 1�� ����
        GameObject box = Instantiate(stages[Random.Range(0, stages.Length)], createPos, Quaternion.identity);

        //����Ʈ �߰�
        generatedStageList.Add(box);


    }

    private void CompareStage()
    {
        // ���� �ִ� ���������� ���� �������� ���� ������ ���ؿ��� ���
        float distance = Vector3.Distance(generatedStageList[0].transform.position, generatedStageList[1].transform.position);

        // ���� �Ÿ��� ���� �����ش�.
        moveDistance += distance;
    }

    // ����Ʈ�� ���� ������ ������Ʈ�� �����ϴ� ���
    private void DestroyOldStage()
    {
        GameObject oldStage = generatedStageList[0];    // ����Ʈ�� ���� ���� ����� ������Ʈ�� ����

        generatedStageList.RemoveAt(0);     // ����Ʈ�� ���� ���� ����� ������Ʈ�� ����Ʈ���� ����

        Destroy(oldStage);  // ����Ʈ�� ���� ���� ����� ������Ʈ�� ����
    }
}
