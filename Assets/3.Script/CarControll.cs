using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�߰�
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
        //�ٴ� ���� �� �̹��� ����Լ�
        DetectGround();

        //���� ��ư�� ��ġ�Ѵٸ� ������Ʈ ����
        if(EventSystem.current.currentSelectedGameObject)
        {
            return;
        }
        //���� �ε������Ͱ� Ȱ��ȭ �� ��Ȳ�̰�
        //ȭ���� ��ġ�Ѵٸ�
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
                    //���� ������ ������Ʈ�� �����Ÿ� �̻� ���̳� ���
                    if(Vector3.Distance(PlaceObject.transform.position, indicator.transform.position) > relocateDistance)
                    {
                        PlaceObject.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
                    }
                }
            }
        }


    }


    private void DetectGround() //�ٴ� �ν�
    {
        // ȭ�� ���߾� ��ġ
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        //���̸� ���� ��� ������Ʈ�� ���� �ڷᱸ��
        List<ARRaycastHit> hitinfo = new List<ARRaycastHit>();

        if(rayManager.Raycast(screenSize,hitinfo,TrackableType.Planes))
        {
            indicator.SetActive(true);

            //ǥ�� ������Ʈ�� ��ġ ȸ������ ������ ��ġ (ȭ�����߾�)�� ��ġ��Ų��.
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

