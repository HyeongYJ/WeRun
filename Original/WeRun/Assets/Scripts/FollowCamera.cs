using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //추적해야할 대상에 대한 Transform을 저장하는 변수
    public Transform target;

    //카메라의 최소 속도 (카메라와 플레이어 사이의 거리가 가까울 떄)
    public float nearSpeed = 0;

    //카메라의 최고 속도 (카메라와 플레이어 사이의 거리가 멀떄)
    public float farSpeed = 0;

    //카메라로 부터 플레이어가 멀다고 판단하는 기준이 되는 수치
    public float minDistance = 0;

    //추적할 카메라의 현재속도
    private float camSpeed = 0;

    //카메라의 Transform을 저장하는 변수
    private Transform cameraTransform = null;

    //카메라와 Player 사이의 방향과 거리를 저장하는 변수
    private Vector3 offset = Vector3.zero;

    //카메라와 player 사이의 거리의 수치 값
    private float distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        //카메라와 Player 사이의 방향과 거리
        offset = cameraTransform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        //카메라와 Player 사이의 거리를 구하기
        //1 distance = Vector3.Distance(cameraTransform.position, target.position);
        //2 distance = offset.magnitude;
        //3 distance = offset.sqrMagnitude;

        distance = offset.magnitude;
        //print(distance); 거리 알아보기위해 프린트

        //카메라와 Player 사이의 거리가 멀떄
        if (distance > minDistance)
        {
            //카메라의 이동속도 = 최고속도
            camSpeed = farSpeed;
        }
        else
        {
            //카메라의 이동속도 = 최소 속도
            camSpeed = nearSpeed;
        }

        Vector3 smoothPosition = Vector3.Lerp(cameraTransform.position, target.position + offset, camSpeed);
        cameraTransform.position = smoothPosition;


        //카메라의 위치를 이동하는 기능
        //카메라의 위치 = 타겟 위치 + 카메라와 Player 사이의 방향과 거리
        //cameraTransform.position = target.position + offset;

    }

}
