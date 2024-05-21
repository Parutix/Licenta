using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private NavMeshAgent agent;
    [SerializeField]
    private float distanceThreshold = 10f;
    private bool isFollowing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Make sure it has the correct tag.");
            return;
        }

        agent = GetComponentInChildren<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the child objects.");
            return;
        }

        agent.updatePosition = false;
        agent.updateUpAxis = false;

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not on a NavMesh. Ensure the agent is placed on a valid NavMesh.");
            return;
        }
    }

    void Update()
    {
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if(distanceToPlayer <= distanceThreshold || isFollowing == true)
            {
                agent.SetDestination(player.transform.position);
                isFollowing = true;
            }
        }
    }

    // Updates for smoother movement
    private void FixedUpdate()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            transform.position = agent.nextPosition;
        }
    }

    private void LateUpdate()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            transform.position = agent.nextPosition;
        }
    }
}
