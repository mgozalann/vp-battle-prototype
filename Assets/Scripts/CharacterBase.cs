using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class CharacterBase : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public int teamId = 0;
    [SerializeField] private LayerMask enemyLayer;
    
    [Header("MovementParameters")]
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    public float rotationSpeed = 700f;
    public float closeDistanceMultiplier = 5f;
    public float animationTransitionSpeed = 5f;

    [Header("Components")]
    public Animator animator;
    public Rigidbody rb;
    
    protected CharacterBase targetEnemy;
    protected float currentHealth;
    protected IAttackStrategy attackStrategy;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        InitializeAttackStrategy();
    }

    protected virtual void Update()
    {
        if (targetEnemy == null || targetEnemy.currentHealth <= 0)
            targetEnemy = FindNearestEnemy();

        if (targetEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
            FaceTarget(targetEnemy.transform);

            if (distance > attackStrategy.AttackRange)
            {
                MoveTowards(targetEnemy.transform.position);
            }
            else
            {
                rb.velocity = Vector3.zero;
                
                animator.SetFloat("Speed", 0); 
                
                attackStrategy.TryAttack(targetEnemy);
            }
        }
    }

    private void MoveTowards(Vector3 destination)
    {
        Vector3 dir = (destination - transform.position).normalized;
        
        dir.y = 0;

        float distance = Vector3.Distance(transform.position, destination);

        float speedFactor;

        if (distance < attackStrategy.AttackRange * closeDistanceMultiplier)
        {
            speedFactor = Mathf.Lerp(1, .5f, Time.deltaTime * animationTransitionSpeed); // 0.5x hız ile 1x hız arası geçiş
        }
        else
        {
            speedFactor = 1f;
        }

        float targetSpeed = moveSpeed * speedFactor;
        
        rb.velocity = Vector3.Lerp(rb.velocity, dir * targetSpeed, Time.deltaTime * animationTransitionSpeed);

        Quaternion toRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
    }

    private CharacterBase FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        
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
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f); // Ölüm animasyonuna zaman tanı
    }

    private bool IsSameTeam(CharacterBase other)
    {
        return other != null && teamId == other.teamId;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected abstract void InitializeAttackStrategy();

}