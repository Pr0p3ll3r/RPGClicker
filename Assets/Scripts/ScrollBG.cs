using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBG : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float clamppos;
    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, clamppos);
        transform.position = startPos + Vector2.left * newPos;
    }
}
