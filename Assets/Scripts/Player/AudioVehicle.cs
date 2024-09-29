using UnityEngine;

public class AudioVehicle : MonoBehaviour
{
    [Header("Engine")][SerializeField] private AudioClip enginePurring;
    [SerializeField] private AudioClip engineLow;

    [Space, Header("Tires")][SerializeField] private AudioClip tiresSquealing;

    private AudioSource _engineAudioSource;
    private AudioSource _idleAudioSource;
    private AudioSource _tiresAudioSource;
    private Vehicle _cart;

    private void Start()
    {
        _engineAudioSource = gameObject.AddComponent<AudioSource>();
        _engineAudioSource.clip = engineLow;
        _engineAudioSource.loop = true;
        _engineAudioSource.volume = 0.5f;
        _engineAudioSource.Play();
        _engineAudioSource.Pause();

        _idleAudioSource = gameObject.AddComponent<AudioSource>();
        _idleAudioSource.clip = enginePurring;
        _idleAudioSource.loop = true;
        _idleAudioSource.volume = 0.4f;
        _idleAudioSource.Play();

        _tiresAudioSource = gameObject.AddComponent<AudioSource>();
        _tiresAudioSource.clip = tiresSquealing;
        _tiresAudioSource.loop = true;
        _tiresAudioSource.volume = 0.25f;
        _tiresAudioSource.Play();
        _tiresAudioSource.Pause();

        _cart = GetComponent<Vehicle>();
    }

    private void Update()
    {
        float slip = _cart.GetMaxSlip();
        if (slip > 0.4f)
        {
            _tiresAudioSource.UnPause();
        }
        else
        {
            _tiresAudioSource.Pause();
        }

        _tiresAudioSource.pitch = 1 + slip * 0.05f;

        //motor
        float speed = _cart.GetSpeed();

        if (speed < 0.5f)
        {
            _engineAudioSource.Pause();
            _idleAudioSource.UnPause();
        }
        else
        {
            _idleAudioSource.Pause();
            _engineAudioSource.UnPause();
        }

        _engineAudioSource.pitch = 1 + speed * 0.1f;

        if (Time.timeScale == 0)
        {
            _idleAudioSource.Pause();
            _tiresAudioSource.Pause();
            _engineAudioSource.Pause();
        }
    }
}
