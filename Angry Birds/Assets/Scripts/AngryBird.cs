using UnityEngine;

public class AngryBird : MonoBehaviour
{
    [SerializeField] private AudioClip _hitClip;


    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;
    public bool _hasLaunched;
    private bool _hasCollided;

    private AudioSource _audioSource;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();

        _rb.bodyType = RigidbodyType2D.Kinematic;
        _circleCollider.enabled = false;
    }

    
    void Update()
    {
        if (_hasLaunched && !_hasCollided)
        {
            transform.right = _rb.linearVelocity.normalized;
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hasCollided = true;
        SoundManager.instance.PlayClip(_hitClip, _audioSource);
        Destroy(this);
    }


    public void launch(Vector2 dir, float force)
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _circleCollider.enabled = true;
        _hasLaunched = true;

        _rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

}
