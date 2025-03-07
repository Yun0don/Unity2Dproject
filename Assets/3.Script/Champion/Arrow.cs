using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f; // ȭ���� ������ �ð� (��)

    private Vector2 moveDirection;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
