using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleMouse : MonoBehaviour
{
    [SerializeField] Transform cursor;
    Vector2 mouseMove;  // ���콺��ġ �Է�
    Vector3 mousePos;
    private void OnMouse(InputValue value)
    {
        mouseMove = value.Get<Vector2>();
    }
    private void Mouse()
    {
        cursor.position = Camera.main.ScreenToWorldPoint(mouseMove) + mousePos;
    }

    private void Awake()
    {
        mousePos = new Vector3(0, 0, 20);
    }
    private void Update()
    {
        Mouse();
    }
}
