using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Collector : MonoBehaviour
{
    [Header("Настройки")]
    public Transform backpackPoint;
    public float stackHeight = 0.3f;
    public float collectDuration = 0.5f;

    [Header("Размер в рюкзаке")]
    public float itemScaleInBackpack = 0.4f; // <--- НОВАЯ НАСТРОЙКА (поставь 0.4 или 0.5)

    private List<Transform> _collectedTrash = new List<Transform>();
    public bool HasItems => _collectedTrash.Count > 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            CollectItem(collision.transform);
        }
    }

    void CollectItem(Transform trashItem)
    {
        Destroy(trashItem.GetComponent<Rigidbody>());
        Destroy(trashItem.GetComponent<Collider>());
        trashItem.SetParent(transform);

        // ИСПРАВЛЕНИЕ: Используем настройку из инспектора, а не единицу
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