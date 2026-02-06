using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // За кем следим (Player)
    public float smoothSpeed = 5f; // Насколько плавно камера летит (0.1 - 10)

    private Vector3 _offset;       // Разница позиций (расстояние)

    void Start()
    {
        // В момент старта запоминаем, как далеко камера стоит от игрока
        if (target != null)
        {
            _offset = transform.position - target.position;
        }
    }

    void LateUpdate() // Используем LateUpdate, чтобы камера двигалась ПОСЛЕ движения игрока
    {
        if (target == null) return;

        // Куда камера хочет встать (позиция игрока + тот самый отступ)
        Vector3 desiredPosition = target.position + _offset;

        // Плавное перемещение (Lerp) от текущей точки к желаемой
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}