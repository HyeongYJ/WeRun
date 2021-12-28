using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePoint : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        //�������� ���� ��ġ�� ����
        GameObject go = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);     //�� ������Ʈ ����

        //�Բ� �����ǵ��� ������ �� ������Ʈ�� �ڽ����� ����
        go.transform.SetParent(transform, false);   //�θ�-�ڽ� ���� ����
    }

    // �������� ���� ���̹Ƿ� ���� ����� ǥ��
    void OnDrawGizmos()
    {
        //������� �Ʒ��κ��� ����� ���� ���̰� �ǵ��� �������� ����
        Vector3 offset = new Vector3(0, 0.5f, 0);

        //���� ǥ��
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + offset, 0.5f);

        //�����ո��� �������� ǥ��
        if (prefab != null) Gizmos.DrawIcon(transform.position + offset, prefab.name, true);
    }
}