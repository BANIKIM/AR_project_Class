using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//필수 유징
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARTrackedImageManager))]//컴포넌트가 있는지 확인

public class MultiTracking_Img : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [SerializeField] private GameObject[] GameObjects;
    [SerializeField] private Dictionary<string, GameObject> SpawnObject;


    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        SpawnObject = new Dictionary<string, GameObject>();//생성

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


    // 이미지를 찾았을 때
    private void ontrackedImageChaged(ARTrackedImagesChangedEventArgs e)
    {
        foreach (ARTrackedImage tr in e.added)
        {
            //added -> 이미지를 읽었을 때
            Update_SpawnObject(tr);
        }
        foreach (ARTrackedImage tr in e.updated)
        {
            //updated -> 이미지 위치가 업데이트 되었을 때
            Update_SpawnObject(tr);
        }
        foreach (ARTrackedImage tr in e.removed)
        {
            //removed -> 이미지가 시야에서 사라졌을 때
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
