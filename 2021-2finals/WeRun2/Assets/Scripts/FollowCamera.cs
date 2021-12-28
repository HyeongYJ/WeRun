using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //�����ؾ��� ��� ���� Transform�� �����ϴ� ����
    public Transform target;

    //ī�޶��� �ּ� �ӵ� (ī�޶�� �÷��̾� ������ �Ÿ��� ����� ��)
    public float nearSpeed = 0;

    //ī�޶��� �ְ� �ӵ� (ī�޶�� �÷��̾� ������ �Ÿ��� �֋�)
    public float farSpeed = 0;

    //ī�޶�� ���� �÷��̾ �ִٰ� �Ǵ��ϴ� ������ �Ǵ� ��ġ
    public float minDistance = 0;

    //������ ī�޶��� ����ӵ�
    private float camSpeed = 0;

    //ī�޶��� Transform�� �����ϴ� ����
    private Transform cameraTransform = null;

    //ī�޶�� Player ������ ����� �Ÿ��� �����ϴ� ����
    private Vector3 offset = Vector3.zero;

    //ī�޶�� player ������ �Ÿ��� ��ġ ��
    private float distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        //ī�޶�� Player ������ ����� �Ÿ�
        offset = cameraTransform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        //ī�޶�� Player ������ �Ÿ��� ���ϱ�
        //1 distance = Vector3.Distance(cameraTransform.position, target.position);
        //2 distance = offset.magnitude;
        //3 distance = offset.sqrMagnitude;

        distance = offset.magnitude;
        //print(distance); �Ÿ� �˾ƺ������� ����Ʈ

        //ī�޶�� Player ������ �Ÿ��� �֋�
        if (distance > minDistance)
        {
            //ī�޶��� �̵��ӵ� = �ְ�ӵ�
            camSpeed = farSpeed;
        }
        else
        {
            //ī�޶��� �̵��ӵ� = �ּ� �ӵ�
            camSpeed = nearSpeed;
        }

        Vector3 smoothPosition = Vector3.Lerp(cameraTransform.position, target.position + offset, camSpeed);
        cameraTransform.position = smoothPosition;


        //ī�޶��� ��ġ�� �̵��ϴ� ���
        //ī�޶��� ��ġ = Ÿ�� ��ġ + ī�޶�� Player ������ ����� �Ÿ�
        //cameraTransform.position = target.position + offset;

    }

}
