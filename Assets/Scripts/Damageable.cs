using UnityEngine.Events;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;

    Animator animator;
    private PlayerBlock playerBlock; 

    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);

            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            if (!value) damageableDeath.Invoke();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerBlock = GetComponent<PlayerBlock>(); // Initialize the block reference
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        // Check if the player is blocking
        if (playerBlock != null && playerBlock.IsBlocking())
        {
            Debug.Log("Block absorbed damage!");
            return false;
        }

        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }

        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
            return true;
        }

        return false;
    }

}
