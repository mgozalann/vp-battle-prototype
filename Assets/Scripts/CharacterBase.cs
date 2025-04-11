using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class CharacterBase : MonoBehaviour, IDamageable
{
    
    public event Action<int,int> OnDamageTaken;
    
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

    public Transform weaponHolder;

    public IWeapon CurrentWeapon;
    
    private CharacterBase _targetEnemy;
    private int _currentHealth;
    protected IAttackStrategy AttackStrategy;
    

    
    private void Awake()
    {
        InitializeWeapon();
    }
    
    protected virtual void Start()
    {
        _currentHealth = characterData.MaxHealth;
        
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

        CurrentWeapon = weaponInstance.GetComponent<IWeapon>();
        
        EquipWeapon(CurrentWeapon);

    }
    
    public void EquipWeapon(IWeapon weapon)
    {
        if (weapon == null) return;
        
        CurrentWeapon?.OnUnequip();
        
        CurrentWeapon = weapon;
        
        CurrentWeapon.OnEquip(this);
    }
    
    public void UnequipWeapon()
    {
        CurrentWeapon?.OnUnequip();
        
        CurrentWeapon = null;
    }


    protected virtual void Update()
    {
        if(IsDead) return;
        
        if (_targetEnemy == null || _targetEnemy.IsDead)
            _targetEnemy = FindNearestEnemy();

        if (_targetEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
            
            _characterMovement.FaceTarget(_targetEnemy.transform);

            if (distance > CurrentWeapon.WeaponData.AttackRange)
            {
                _characterMovement.MoveTowards(_targetEnemy.transform.position);
            }
            else
            {
                _characterMovement.StopMoving();
                
                CurrentWeapon.SetTarget(_targetEnemy);
                
                AttackStrategy.TryAttack(_targetEnemy);
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

            if (character != null && character != this && character._currentHealth > 0 && !character.IsSameTeam(this))
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
        
        int randomDie = Random.Range(0, 2); 
        
        animator.SetInteger("DieIndex", randomDie);
        
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
    
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        
        OnDamageTaken?.Invoke(_currentHealth,characterData.MaxHealth);
        
        if (!IsAttacking)
        {
            animator.SetTrigger("Hit");
        }

        if (_currentHealth <= 0)
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