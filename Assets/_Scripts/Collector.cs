using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Collector : MonoBehaviour
{
    [Header("Настройки сбора")]
    public Transform backpackPoint;
    public float stackHeight = 0.3f;
    public float collectDuration = 0.5f;

    [Header("Настройки Рюкзака")]
    public float itemScaleInBackpack = 0.4f;

    // <--- НОВОЕ: Лимит вместимости
    public int maxCapacity = 10; // Сколько влазит по умолчанию
    private int _currentCapacity; // Реальная вместимость сейчас

    private List<Transform> _collectedTrash = new List<Transform>();
    public bool HasItems => _collectedTrash.Count > 0;

    void Start()
    {
        // При старте устанавливаем лимит
        _currentCapacity = maxCapacity;
    }

    // <--- НОВЫЙ МЕТОД: Вызывается из магазина для расширения рюкзака
    public void SetCapacity(int newCapacity)
    {
        _currentCapacity = newCapacity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            // <--- НОВОЕ: Проверка на переполнение
            // Если мусора уже столько же или больше, чем лимит - не собираем
            if (_collectedTrash.Count >= _currentCapacity)
            {
                return;
            }

            CollectItem(collision.transform);
        }
    }

    void CollectItem(Transform trashItem)
    {
        Destroy(trashItem.GetComponent<Rigidbody>());
        Destroy(trashItem.GetComponent<Collider>());
        trashItem.SetParent(transform);

        trashItem.localScale = Vector3.one * itemScaleInBackpack;

        Vector3 targetPosition = backpackPoint.localPosition + new Vector3(0, _collectedTrash.Count * stackHeight, 0);

        trashItem.DOLocalMove(targetPosition, collectDuration).SetEase(Ease.OutBack);
        trashItem.DOLocalRotate(Vector3.zero, collectDuration);

        _collectedTrash.Add(trashItem);
    }

    public Transform RemoveLastItem()
    {
        if (_collectedTrash.Count == 0) return null;
        int lastIndex = _collectedTrash.Count - 1;
        Transform itemToRemove = _collectedTrash[lastIndex];
        _collectedTrash.RemoveAt(lastIndex);
        return itemToRemove;
    }
}