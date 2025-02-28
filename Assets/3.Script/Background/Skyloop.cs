using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skyloop : MonoBehaviour
{
    public float scrollSpeed = 10f; // ��� �̵� �ӵ�
    private float width; // ����� ���� ����
    private BoxCollider2D boxcollider2d;

    void Start()
    {
        // BoxCollider2D�� �����ͼ� width ���� ����
        TryGetComponent(out boxcollider2d);
        width = boxcollider2d.size.x;
    }

    void Update()
    {
        // ����� �������� �̵�
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // ����� ȭ�� �������� ������ ����� ��ġ�� ���������� �̵�
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
