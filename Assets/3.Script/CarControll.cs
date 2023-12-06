using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//추가
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class CarControll : MonoBehaviour
{
    public GameObject indicator;
    public GameObject myCar;

    public float relocateDistance = 1.0f;

    ARRaycastManager rayManager;
    GameObject PlaceObject;

    private void Start()
    {
        rayManager = GetComponent<ARRaycastManager>();
        indicator.SetActive(false);
    }

    private void Update()
    {
        //바닥 감지 및 이미지 출력함수
        DetectGround();

        //만일 버튼이 터치됏다면 업데이트 종료
        if(EventSystem.current.currentSelectedGameObject)
        {
            return;
        }
        //현재 인디케이터가 활성화 된 상황이고
        //화면을 터치한다면
        if (indicator.activeSelf && Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                if (PlaceObject == null)
                {
                    PlaceObject = Instantiate(myCar, indicator.transform.position, indicator.transform.rotation);
                }
                else
                {
                    //만일 생성한 오브젝트와 일정거리 이상 차이날 경우
                    if(Vector3.Distance(PlaceObject.transform.position, indicator.transform.position) > relocateDistance)
                    {
                        PlaceObject.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
                    }
                }
            }
        }


    }


    private void DetectGround() //바닥 인식
    {
        // 화면 정중앙 위치
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        //레이를 쏴서 닿는 오브젝트를 담을 자료구조
        List<ARRaycastHit> hitinfo = new List<ARRaycastHit>();

        if(rayManager.Raycast(screenSize,hitinfo,TrackableType.Planes))
        {
            indicator.SetActive(true);

            //표식 오브젝트의 위치 회전값을 레이의 위치 (화면정중앙)에 위치시킨다.
            indicator.transform.position = hitinfo[0].pose.position;
            indicator.transform.rotation = hitinfo[0].pose.rotation;

            indicator.transform.position += indicator.transform.up * 0.01f;
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}

