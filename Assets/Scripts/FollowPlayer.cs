using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        // Get the NavMeshAgent component from this GameObject
        agent = GetComponent<NavMeshAgent>();
        
        // Find the player GameObject by its tag and get its transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Set the agent's destination to the player's current position
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}
