using UnityEngine;

public class TargetResponse : MonoBehaviour
{
    [Header("Particle & Sound (optional)")]
    public ParticleSystem hitParticlePrefab;
    public AudioClip hitSound;

    [Header("Color Flash (optional)")]
    public float hitColorDuration = 0.2f;
    public Color hitColor = Color.red;

    private Material _mat;
    private Color _originalColor;
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            _mat = rend.material;
            _originalColor = _mat.color;
        }
        else
        {
            Debug.LogWarning($"[TargetResponse] {name} has no Renderer—color flash will be skipped.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 1) Spawn particles
        if (hitParticlePrefab != null)
        {
            ContactPoint contact = collision.GetContact(0);
            Vector3 spawnPos = contact.point;
            Quaternion spawnRot = Quaternion.LookRotation(contact.normal);

            ParticleSystem ps = Instantiate(
                hitParticlePrefab,
                spawnPos,
                spawnRot
            );
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        // 2) Play sound
        if (hitSound != null)
        {
            _audioSource.PlayOneShot(hitSound);
        }

        // 3) Flash color
        if (_mat != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashHitColor());
        }

        // 4) ***** ADD THIS LINE TO INCREMENT SCORE *****
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoint(1);
        }

        // 5) (Optional) Any other reaction (knockback, health reduction, etc.)
    }

    private System.Collections.IEnumerator FlashHitColor()
    {
        _mat.color = hitColor;
        yield return new WaitForSeconds(hitColorDuration);
        _mat.color = _originalColor;
    }
}
