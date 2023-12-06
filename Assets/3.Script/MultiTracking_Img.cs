using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ʼ� ��¡
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARTrackedImageManager))]//������Ʈ�� �ִ��� Ȯ��

public class MultiTracking_Img : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [SerializeField] private GameObject[] GameObjects;
    [SerializeField] private Dictionary<string, GameObject> SpawnObject;


    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        SpawnObject = new Dictionary<string, GameObject>();//����

        foreach (GameObject g in GameObjects)
        {
            GameObject newobject = Instantiate(g);
            newobject.name = g.name;

            SpawnObject.Add(newobject.name, newobject);
            newobject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ontrackedImageChaged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ontrackedImageChaged;
    }


    // �̹����� ã���� ��
    private void ontrackedImageChaged(ARTrackedImagesChangedEventArgs e)
    {
        foreach (ARTrackedImage tr in e.added)
        {
            //added -> �̹����� �о��� ��
            Update_SpawnObject(tr);
        }
        foreach (ARTrackedImage tr in e.updated)
        {
            //updated -> �̹��� ��ġ�� ������Ʈ �Ǿ��� ��
            Update_SpawnObject(tr);
        }
        foreach (ARTrackedImage tr in e.removed)
        {
            //removed -> �̹����� �þ߿��� ������� ��
            SpawnObject[tr.referenceImage.name].SetActive(false);
        }
    }

    private void Update_SpawnObject(ARTrackedImage tr)
    {
        string Reimg_name = tr.referenceImage.name;

        Vector3 position = tr.transform.position;

        GameObject prefrebs = SpawnObject[Reimg_name];
        prefrebs.transform.position = position;
        prefrebs.transform.rotation = tr.transform.rotation;

        SpawnObject[Reimg_name].SetActive(true);

/*        foreach (GameObject g in SpawnObject.Values)
        {
            if(g.name != Reimg_name)
            {
                g.SetActive(false);
            }
        }*/

    }

}
