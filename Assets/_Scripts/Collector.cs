using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Collector : MonoBehaviour
{
    [Header("Настройки")]
    public Transform backpackPoint;
    public float stackHeight = 0.3f;
    public float collectDuration = 0.5f;

    // Сделали список публичным или добавим геттер, чтобы проверять наличие предметов
    private List<Transform> _collectedTrash = new List<Transform>();

    public bool HasItems => _collectedTrash.Count > 0; // Свойство для проверки

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            CollectItem(collision.transform);
        }
    }

    void CollectItem(Transform trashItem)
    {
        // Убираем физику
        Destroy(trashItem.GetComponent<Rigidbody>());
        Destroy(trashItem.GetComponent<Collider>());

        // Привязываем к игроку
        trashItem.SetParent(transform);

        // Рассчитываем позицию
        Vector3 targetPosition = backpackPoint.localPosition + new Vector3(0, _collectedTrash.Count * stackHeight, 0);

        // Анимация полета в рюкзак
        trashItem.DOLocalMove(targetPosition, collectDuration).SetEase(Ease.OutBack);
        trashItem.DOLocalRotate(Vector3.zero, collectDuration);

        _collectedTrash.Add(trashItem);
    }

    // НОВЫЙ МЕТОД: Отдать последний предмет (для продажи)
    public Transform RemoveLastItem()
    {
        if (_collectedTrash.Count == 0) return null;

        // Берем последний предмет из списка (верхушка стека)
        int lastIndex = _collectedTrash.Count - 1;
        Transform itemToRemove = _collectedTrash[lastIndex];

        // Удаляем из списка
        _collectedTrash.RemoveAt(lastIndex);

        return itemToRemove;
    }
}