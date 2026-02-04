using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;

    // ВАЖНО: Мы поменяли тип на просто "Joystick"
    // Теперь сюда можно перетащить и Variable, и Dynamic, и Floating
    public Joystick joystick;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Логика остается та же, она работает для всех джойстиков
        float moveX = Input.GetAxis("Horizontal") + joystick.Horizontal;
        float moveZ = Input.GetAxis("Vertical") + joystick.Vertical;

        Vector3 direction = new Vector3(moveX, 0, moveZ);

        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        _rb.velocity = new Vector3(direction.x * moveSpeed, _rb.velocity.y, direction.z * moveSpeed);

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10f * Time.fixedDeltaTime);
        }
    }
}