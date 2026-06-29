using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX")]
    [SerializeField] private Sound[] sounds;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlaySound(string soundName)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.soundName == soundName)
            {
                audioSource.PlayOneShot(sound.clip);
                return;
            }
        }

        Debug.LogWarning($"Sound '{soundName}' not found.");
    }
}