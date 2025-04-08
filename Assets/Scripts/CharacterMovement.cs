using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    
    [Header("MovementParameters")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 700f;
    [SerializeField] private float closeDistanceMultiplier = 5f;
    [SerializeField] private float animationTransitionSpeed = 5f;
    
    public void MoveTowards(Vector3 destination,float attackRange)
    {
        Vector3 dir = (destination - transform.position).normalized;
        
        dir.y = 0;

        float distance = Vector3.Distance(transform.position, destination);

        float speedFactor;

        if (distance < attackRange * closeDistanceMultiplier)
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

    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
                
        animator.SetFloat("Speed", 0); 
    }

    public void OnDead()
    {
        rb.useGravity = false;
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
    }
}