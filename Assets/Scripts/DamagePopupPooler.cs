using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupPooler : MonoBehaviour
{
    public static DamagePopupPooler Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private Queue<GameObject> popups = new Queue<GameObject>();

    [SerializeField] private GameObject prefab;

    public GameObject Get()
    {
        if(popups.Count == 0)
        {
            AddObject();
        }
        return popups.Dequeue();
    }

    private void AddObject()
    {
        GameObject newObject = Instantiate(prefab);
        newObject.SetActive(false);
        newObject.transform.SetParent(transform);
        popups.Enqueue(newObject);
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        popups.Enqueue(objectToReturn);
    }
}
