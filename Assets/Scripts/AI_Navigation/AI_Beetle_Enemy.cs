using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Beetle_Enemy : MonoBehaviour
{

    public Transform Player;
    NavMeshAgent agent;
    public float maxDist = 10f;
    RaycastHit hit;
    public LayerMask player,ground;
    bool inRange;
    Vector3 setPoint;
    Vector3 walkPoint,tempPoint;
    bool walkPointSet,isAwayFromTarget,isGrounded;
    public float walkPointRange = 5f;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        walkPoint = transform.position;
        walkPointSet = false;
        
    }

    private void FixedUpdate()
    {
        
        inRange = Physics.CheckSphere(agent.transform.position,maxDist,player);
        Patroling();
 
        setPoint = Player.position;
        bool patrol = !inRange && walkPointSet;
        agent.destination = setPoint * System.Convert.ToInt32(inRange) + walkPoint * System.Convert.ToInt32(patrol) ;
        transform.LookAt(agent.destination);
        
    }
    private void Patroling()
    {
        walkPoint = SearchWalkPoint() * System.Convert.ToInt32(!walkPointSet) + walkPoint * System.Convert.ToInt32(walkPointSet);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        isAwayFromTarget = distanceToWalkPoint.magnitude - agent.stoppingDistance > 1f ;
        walkPointSet = isAwayFromTarget && isGrounded;
    }
    private Vector3 SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float newXpos = transform.position.x -  randomX + Player.position.x;
        float newZpos =  transform.position.z - randomZ+ Player.position.z;

        tempPoint = new Vector3(newXpos, transform.position.y,newZpos);
        isGrounded = Physics.Raycast(walkPoint, -transform.up, 2f, ground);
        return tempPoint;
    }

}
