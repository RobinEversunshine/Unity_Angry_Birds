using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private float _damageThreshold = 0.2f;
    [SerializeField] private GameObject _enemyParticle;

    [SerializeField] private AudioClip _deathClip;

    private SpriteRenderer _spriteRenderer;
    private float _currentHealth;


    void Start()
    {
        _currentHealth = _maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    
    void Update()
    {
        
    }


    public void Damage(float amount)
    {
        _currentHealth -= amount;
        _spriteRenderer.color = Color.Lerp(Color.red, Color.white, _currentHealth / _maxHealth);


        if (_currentHealth <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        Destroy(gameObject);
        GameManager.instance.RemoveEnemy(this);

        GameObject particle = Instantiate(_enemyParticle, transform.position, Quaternion.identity);
        Animator animator = particle.GetComponent<Animator>();
        animator.Play("EnemyDeath");
        float animLength = animator.GetCurrentAnimatorStateInfo(0).length;

        AudioSource.PlayClipAtPoint(_deathClip, transform.position);

        Destroy(particle, animLength);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVel = collision.relativeVelocity.magnitude;

        if (impactVel > _damageThreshold)
        {
            Damage(impactVel);
        }
    }


}
