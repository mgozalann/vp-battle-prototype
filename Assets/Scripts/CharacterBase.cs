using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IDamageable
{
    [Header("Stats")] 
    [SerializeField] private CharacterData characterData;
    [SerializeField] private int teamId = 0;
    [SerializeField] private LayerMask enemyLayer;
    
    public bool IsAttacking;
    public bool IsDead;

    [Header("Components")]
    [SerializeField] private Animator animator;
    public Animator Animator => animator;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private Collider _collider;
    
    [Header("Starting Weapon")]
    [SerializeField] private WeaponData startingWeapon; // Inspector’dan atanacak
    public IWeapon currentWeapon;
    
    public Transform weaponHolder;
    
    protected CharacterBase targetEnemy;
    protected float currentHealth;
    protected IAttackStrategy attackStrategy;
    
    private void Awake()
    {
        InitializeWeapon();
    }
    
    protected virtual void Start()
    {
        currentHealth = characterData.MaxHealth;
        
        InitializeAttackStrategy();
    }
    
    protected abstract void InitializeAttackStrategy();
    
    private void InitializeWeapon()
    {
        if (startingWeapon == null)
        {
            Debug.LogWarning("Starting weapon is not assigned!");
            
            return;
        }

        GameObject weaponInstance = Instantiate(startingWeapon.WeaponPrefab,weaponHolder);
        
        weaponInstance.transform.localRotation = Quaternion.identity;
        
        weaponInstance.transform.localPosition = Vector3.zero;

        currentWeapon = weaponInstance.GetComponent<IWeapon>();
        
        EquipWeapon(currentWeapon);

    }
    
    public void EquipWeapon(IWeapon weapon)
    {
        if (weapon == null) return;
        
        currentWeapon?.OnUnequip();
        
        currentWeapon = weapon;
        
        currentWeapon.OnEquip(this);
    }
    
    public void UnequipWeapon()
    {
        currentWeapon?.OnUnequip();
        
        currentWeapon = null;
    }


    protected virtual void Update()
    {
        if(IsDead) return;
        
        if (targetEnemy == null || targetEnemy.IsDead)
            targetEnemy = FindNearestEnemy();

        if (targetEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
            
            _characterMovement.FaceTarget(targetEnemy.transform);

            if (distance > currentWeapon.WeaponData.AttackRange)
            {
                _characterMovement.MoveTowards(targetEnemy.transform.position, currentWeapon.WeaponData.AttackRange);
            }
            else
            {
                _characterMovement.StopMoving();
                
                currentWeapon.SetTarget(targetEnemy);
                
                attackStrategy.TryAttack(targetEnemy);
            }
        }
        else
        {
            _characterMovement.StopMoving();
        }
    }

    private CharacterBase FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, characterData.DetectionRange, enemyLayer);
        
        CharacterBase nearest = null;
        
        var closestDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            CharacterBase character = hit.GetComponent<CharacterBase>();

            if (character != null && character != this && character.currentHealth > 0 && !character.IsSameTeam(this))
            {
                float dist = Vector3.Distance(transform.position, character.transform.position);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    nearest = character;
                }
            }
        }

        return nearest;
    }

    protected void Die()
    {
        IsDead = true;

        _collider.enabled = false;
        
        _characterMovement.OnDead();
        
        animator.SetTrigger("Die");
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    public bool IsSameTeam(CharacterBase other)
    {
        return other != null && teamId == other.teamId;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        Debug.Log(currentHealth);

        if (!IsAttacking)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void OnAttackAnimationStarted()
    {
        IsAttacking = true;
    }
    
    public void OnAttackAnimationEnded()
    {
        IsAttacking = false;
    }
    

}