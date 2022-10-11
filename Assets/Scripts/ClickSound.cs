using UnityEngine;
using UnityEngine.Events;

public class ClickSound : MonoBehaviour
{
    public static UnityEvent Click = new UnityEvent();

    [SerializeField] private AudioSource audioSource;

    public void Awake()
    {
        Click.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        audioSource.Play();
    }
}
