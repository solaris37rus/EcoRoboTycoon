using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Чтобы звать его отовсюду

    [Header("Настройки")]
    public AudioSource effectsSource; // Проигрыватель эффектов (звуки)
    public AudioSource musicSource;   // Проигрыватель музыки (фон)

    [Header("Звуковые файлы (Перетащи сюда)")]
    public AudioClip collectSound; // Звук сбора (чпоньк)
    public AudioClip sellSound;    // Звук продажи (дзынь)
    public AudioClip buySound;     // Звук покупки зоны/апгрейда
    public AudioClip stepSound;    // Звук шагов (если захотим)

    private void Awake()
    {
        // Делаем Синглтон (чтобы был один на всю игру)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не удалять при смене сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Метод: Играть звук сбора
    public void PlayCollect()
    {
        PlayClip(collectSound, 0.8f); // 0.8f - громкость чуть тише
    }

    // Метод: Играть звук продажи
    public void PlaySell()
    {
        PlayClip(sellSound, 1f);
    }

    // Метод: Играть звук покупки
    public void PlayBuy()
    {
        PlayClip(buySound, 1f);
    }

    // Универсальный метод проигрывания
    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null && effectsSource != null)
        {
            effectsSource.PlayOneShot(clip, volume);
        }
    }
}