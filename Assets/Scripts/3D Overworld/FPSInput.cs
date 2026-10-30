using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Range(1.0f, 20.0f)] private float _speed = 7.5f;
    [SerializeField] private float _gravity = -9.8f;

    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        transform.Translate(
            Input.GetAxis("Horizontal") * _speed * Time.deltaTime, 
            0.0f,
            Input.GetAxis("Vertical") * _speed * Time.deltaTime);
    }
}
