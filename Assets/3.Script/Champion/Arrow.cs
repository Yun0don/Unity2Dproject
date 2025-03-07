using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f; // 화살이 존재할 시간 (초)

    private Vector2 moveDirection;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
        // lifetime 후 자동 삭제
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // 매 프레임마다 지정된 방향으로 이동
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
