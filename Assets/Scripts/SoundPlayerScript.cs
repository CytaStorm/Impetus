using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource _healthPickupSound;
    public void PlayHealthPickupSound()
    {
        _healthPickupSound.Play();
    }
}
