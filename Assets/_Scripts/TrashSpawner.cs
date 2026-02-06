using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TrashSpawner : MonoBehaviour
{
    [Header("Настройки")]
    public GameObject trashPrefab;
    public float respawnDelay = 3f;
    public bool spawnOnStart = true;

    private GameObject _currentTrash;
    private bool _isRespawning = false;
    private Vector3 _targetScale; // Тут будем хранить размер из префаба

    void Start()
    {
        // 1. Запоминаем размер, который ты настроил в префабе (например, 0.5)
        if (trashPrefab != null)
            _targetScale = trashPrefab.transform.localScale;
        else
            _targetScale = Vector3.one;

        if (spawnOnStart) SpawnTrash();
    }

    void Update()
    {
        if (_currentTrash == null && !_isRespawning)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    IEnumerator RespawnRoutine()
    {
        _isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);
        SpawnTrash();
        _isRespawning = false;
    }

    void SpawnTrash()
    {
        _currentTrash = Instantiate(trashPrefab, transform.position, Quaternion.identity);
        _currentTrash.transform.SetParent(transform);

        // 2. Ставим размер в 0 (чтобы не было видно)
        _currentTrash.transform.localScale = Vector3.zero;

        // 3. Анимируем не до 1, а до _targetScale (размера префаба)
        _currentTrash.transform.DOScale(_targetScale, 0.5f).SetEase(Ease.OutBack);
    }
}