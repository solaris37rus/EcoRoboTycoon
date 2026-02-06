using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки")]
    public float moveSpeed = 8f;
    public Joystick joystick;

    private Rigidbody _rb;
    private float _currentSpeed;

    // ИЗМЕНЕНИЕ: Используем Awake вместо Start
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _currentSpeed = moveSpeed; // Инициализируем ДО того, как Магазин попробует это изменить
    }

    public void SetSpeed(float newSpeed)
    {
        _currentSpeed = newSpeed;
    }

    void FixedUpdate()
    {
        // ... (тут всё без изменений)
        float moveX = Input.GetAxis("Horizontal") + joystick.Horizontal;
        float moveZ = Input.GetAxis("Vertical") + joystick.Vertical;

        Vector3 direction = new Vector3(moveX, 0, moveZ);

        if (direction.magnitude > 1) direction.Normalize();

        _rb.velocity = new Vector3(direction.x * _currentSpeed, _rb.velocity.y, direction.z * _currentSpeed);

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10f * Time.fixedDeltaTime);
        }
    }
}