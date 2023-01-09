using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;
    private Queue<GameObject> objects = new Queue<GameObject>();

    public GameObject Get()
    {
        if (objects.Count == 0)
        {
            AddObject();
        }
        GameObject objectToSpawn = objects.Dequeue();
        return objectToSpawn;
    }

    void AddObject()
    {
        GameObject newObject = Instantiate(prefab);
        newObject.SetActive(false);
        objects.Enqueue(newObject);
        newObject.transform.SetParent(parent, false);
        newObject.GetComponent<IPooledObject>().Pool = this;
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }
}
