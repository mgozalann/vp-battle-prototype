using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private Animator animator;
    
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private NavMeshObstacle obstacle;
    
    [Header("MovementParameters")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 700f;
    [SerializeField] private float closeDistanceMultiplier = 5f;
    [SerializeField] private float animationTransitionSpeed = 5f;

    private void OnEnable()
    {
        agent.speed = moveSpeed;
    }

    public void MoveTowards(Vector3 destination)
    {

        obstacle.enabled = false;
        
        agent.enabled = true;
        
        agent.SetDestination(destination);
        
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    public void StopMoving()
    {
        if (agent.enabled)
        {
            agent.ResetPath();
            
            agent.enabled = false;
            
            animator.SetFloat("Speed", 0);

            obstacle.enabled = true;
        }
    }

    public void OnDead()
    {
        agent.enabled = false;
        
        obstacle.enabled = false;
        
        obstacle.carving = false;
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}