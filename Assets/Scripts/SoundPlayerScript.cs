using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundNames
{
    HealthPickup = 0,
    AttackBuffPickup = 1
}
public class SoundPlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource _audioPlayer;
    [SerializeField] private List<AudioClip> _sounds;

    public void PlayHealthPickupSound()
    {
        _audioPlayer.clip = _sounds[(int)SoundNames.HealthPickup];
        _audioPlayer.Play();
    }

    public void PlayAttackBuffPickupSound()
    {
        _audioPlayer.clip = _sounds[(int)SoundNames.AttackBuffPickup];
        _audioPlayer.Play();
    }
}
