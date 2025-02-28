using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skyloop : MonoBehaviour
{
    public float scrollSpeed = 10f; // 배경 이동 속도
    private float width; // 배경의 가로 길이
    private BoxCollider2D boxcollider2d;

    void Start()
    {
        // BoxCollider2D를 가져와서 width 값을 설정
        TryGetComponent(out boxcollider2d);
        width = boxcollider2d.size.x;
    }

    void Update()
    {
        // 배경을 왼쪽으로 이동
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // 배경이 화면 왼쪽으로 완전히 벗어나면 위치를 오른쪽으로 이동
        if (transform.position.x <= -width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}
