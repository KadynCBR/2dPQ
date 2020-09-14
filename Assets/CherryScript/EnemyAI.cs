using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float nextWaypointDistance = 1.5f;
    public Vector2 direction;
    float distance;

    Path path;
    int currentWaypoint = 0;
    // bool reachedEndOfPath = false;
    int path_count;

    Seeker seeker;
    MoveableCharacter moveable_char;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        seeker = GetComponent<Seeker>();
        moveable_char = GetComponent<MoveableCharacter>();

        InvokeRepeating("UpdatePath", 0f, .2f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            // reachedEndOfPath = true;
            direction = new Vector3(0,0,0);
            return;
        } else {
            // reachedEndOfPath = false;
        }

        float character_distance = Vector2.Distance(transform.position, target.position);
        if (character_distance > nextWaypointDistance)
        {
            direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        } else {
            direction = new Vector3(0,0,0);
        }
        
        distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
