using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSprite : MonoBehaviour
{
    public float waitTime = 4f;
    public float cubeSpawnTotal = 10;
    public List<GameObject> imagesList;
    bool coroutineStart = false;
    public RectTransform panel;

    public void HealS()
    {
        if(coroutineStart == false) 
        {
            StartCoroutine(SpawnImage()); 
        }
        
    }


    public IEnumerator SpawnImage()
    {
        coroutineStart = true;
        for (int i = 0; i < cubeSpawnTotal; i++)
        {
            GameObject imageToSpawn = imagesList[Random.Range(0, imagesList.Count)];

            Vector3 spawnPosition = GetBottomLeftCorner(panel) - new Vector3(Random.Range(0, panel.rect.x), Random.Range(0, panel.rect.y), 0);

            //print("Spawn image at position: " + spawnPosition);

            GameObject spwanObj = Instantiate(imageToSpawn, spawnPosition, Quaternion.identity, panel);
            Destroy(spwanObj, 2f);
            yield return new WaitForSeconds(waitTime);
        }
        coroutineStart = false;
    }

    Vector3 GetBottomLeftCorner(RectTransform rt)
    {
        Vector3[] v = new Vector3[4];
        rt.GetWorldCorners(v);
        return v[0];
    }
}
