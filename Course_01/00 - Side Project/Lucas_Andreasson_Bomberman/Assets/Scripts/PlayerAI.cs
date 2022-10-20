using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    private enum AIState { Hiding, Bombing, Running, ReturnToOldPos, Waiting }

    public bool debugActive = false;

    [SerializeField] private AIState state;
    private Vector3[] furthestPositions;
    private AIState oldState;
    private NavMeshAgent agent;
    private GameObject[] boxes;
    private GameObject closestBox;
    private BombLaying bombLaying;
    private int removedBoxes = 0;
    private float bombTimer;
    private int bombAmmo = 1;
    private float bombCooldown = 5.2f;
    private float avoidanceTimer = 2.5f;
    private GameObject[] allBombs;
    private Vector3[] directions = { Vector3.forward, -Vector3.forward, Vector3.right, -Vector3.right };
    private Vector3 lastSeenObjPos;
    private Vector3 oldPos;


    // Start is called before the first frame update
    void Start()
    {
        furthestPositions = new Vector3[4];
        agent = gameObject.GetComponent<NavMeshAgent>();
        bombLaying = gameObject.GetComponent<BombLaying>();
        state = AIState.Bombing;
        UpdateBoxArray();
        ClosestBoxDestination();
        Invoke(nameof(ChangeBombTimer), 60);
    }

    void ChangeBombTimer()
    {
        bombCooldown = 3.5f;
    }

    void UpdateBoxArray()
    {
        boxes = GameObject.FindGameObjectsWithTag("Box");
    }

    void FindClosestBox()
    {
        float closestDistance = 1000000000000;
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i] != null)
            {
                float distance = Vector3.Distance(boxes[i].transform.position, gameObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBox = boxes[i];
                }
            }
            else
            {
                removedBoxes++;
            }
        }
        if (removedBoxes >= 10)
        {
            UpdateBoxArray();
        }
    }

    void NewFurthestDistance(Vector3 pos, float maxDistance = 3, bool moveToClosest = false, bool moveAwayFromLastSeenObj = false)
    {
        state = AIState.Hiding;
        NavMeshPath navMeshPath = new NavMeshPath();
        Vector3 up, down, left, right;
        //used to compare the longest distance
        float dist1, dist2;
        NavMeshHit hit;
        NavMesh.SamplePosition(pos + (Vector3.forward * maxDistance) + (Vector3.right * maxDistance), out hit, 1000, NavMesh.AllAreas);
        right = hit.position;
        NavMesh.SamplePosition(pos + (Vector3.forward * maxDistance) + (-Vector3.right * maxDistance), out hit, 1000, NavMesh.AllAreas);
        left = hit.position;
        dist1 = Vector3.Distance(right, pos);
        dist2 = Vector3.Distance(left, pos);

        furthestPositions[0] = right;
        furthestPositions[1] = left;


        if (dist1 > dist2)
        {
            agent.CalculatePath(right, navMeshPath);

            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                down = right;
            else
                down = left;
        }
        else
        {
            agent.CalculatePath(left, navMeshPath);

            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                down = left;
            else
                down = right;
        }

        NavMesh.SamplePosition(pos + (Vector3.back * maxDistance) + (Vector3.right * maxDistance), out hit, 1000, NavMesh.AllAreas);
        right = hit.position;
        NavMesh.SamplePosition(pos + (Vector3.back * maxDistance) + (-Vector3.right * maxDistance), out hit, 1000, NavMesh.AllAreas);
        left = hit.position;
        dist1 = Vector3.Distance(right, pos);
        dist2 = Vector3.Distance(left, pos);

        furthestPositions[2] = right;
        furthestPositions[3] = left;

        if (dist1 > dist2)
        {
            agent.CalculatePath(right, navMeshPath);

            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                up = right;
            else
                up = left;
        }
        else
        {
            agent.CalculatePath(left, navMeshPath);

            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                up = left;
            else
                up = right;
        }

        dist1 = Vector3.Distance(up, pos);
        dist2 = Vector3.Distance(down, pos);

        if (!moveAwayFromLastSeenObj)
        {

            if (!moveToClosest)
            {

                if (dist1 > dist2)
                {
                    agent.CalculatePath(up, navMeshPath);

                    if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                        agent.SetDestination(up);
                    else
                        agent.SetDestination(down);
                }
                else
                {
                    agent.CalculatePath(down, navMeshPath);

                    if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                        agent.SetDestination(down);
                    else
                        agent.SetDestination(up);
                }
            }
            else
            {
                float dist = 100000000000;
                for (int i = 0; i < furthestPositions.Length; i++)
                {
                    agent.CalculatePath(furthestPositions[i], navMeshPath);
                    if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        float newDist = Vector3.Distance(transform.position, furthestPositions[i]);
                        if (dist > newDist)
                        {
                            dist = newDist;
                            agent.SetDestination(furthestPositions[i]);

                        }
                    }
                }
            }
        }
        else
        {
            float dist = 0;
            for (int i = 0; i < furthestPositions.Length; i++)
            {
                agent.CalculatePath(furthestPositions[i], navMeshPath);
                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {

                    float newDist = Vector3.Distance(lastSeenObjPos, furthestPositions[i]);
                    if (dist < newDist)
                    {
                        dist = newDist;
                        agent.SetDestination(furthestPositions[i]);

                    }
                }
            }
        }

    }

    bool CheckInSight(Vector3 pos)
    {
        bool value = false;

        if (lastSeenObjPos.x + 0.5 > pos.x && lastSeenObjPos.x - 0.5 < pos.x)
        {
            value = true;
        }
        else if (lastSeenObjPos.y + 0.5 > pos.y && lastSeenObjPos.y - 0.5 < pos.y)
        {
            value = true;
        }

        return value;
    }

    void ClosestBoxDestination()
    {
        state = AIState.Bombing;
        FindClosestBox();
        NavMeshHit hit;
        NavMesh.SamplePosition(closestBox.transform.position, out hit, 10, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
    }

    void AvoidBombs()
    {
        avoidanceTimer = 1f;
        for (int i = 0; i < 4; i++)
        {
            int layerMask = 1 << 7;

            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, directions[i], out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.CompareTag("Bomb"))
                {
                    lastSeenObjPos = hit.transform.position;
                    NewFurthestDistance(hit.transform.position, 5, true);
                    state = AIState.Running;
                }
                else if (hit.transform.CompareTag("PowerUp"))
                {
                    if (hit.transform.GetComponent<PowerUps>().GetPowerUpType() == PowerUps.PowerupType.sizeUp || hit.transform.GetComponent<PowerUps>().GetPowerUpType() == PowerUps.PowerupType.bombUp)
                    {
                        oldState = state;
                        oldPos = agent.destination;
                        state = AIState.ReturnToOldPos;
                        NavMeshHit newPos;
                        NavMesh.SamplePosition(hit.transform.position, out newPos, 1000, NavMesh.AllAreas);
                        agent.SetDestination(newPos.position);
                    }
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        bombTimer -= Time.deltaTime;
        avoidanceTimer -= Time.deltaTime;
        bombAmmo = bombLaying.GetBombAmmo();

        if (avoidanceTimer <= 0)
        {
            AvoidBombs();
        }

        if (Vector3.Distance(new Vector3(transform.position.x, agent.destination.y, transform.position.z), agent.destination) < 0.5f)
        {
            //Debug.Log(state + " | " + agent.destination);
            if (state == AIState.Bombing)
            {
                //bombAmmo--;
                bombTimer = bombCooldown;
                NewFurthestDistance(transform.position);
                bombLaying.PutDownBomb();
            }
            else if (state == AIState.Hiding && bombTimer <= 0 && bombAmmo > 0)
            {
                ClosestBoxDestination();
            }
            else if (state == AIState.Running)
            {
                Debug.Log("finnished running to new spot");
                NewFurthestDistance(transform.position, 5, false, true);
            }
            else if (state == AIState.ReturnToOldPos)
            {
                state = oldState;
                agent.SetDestination(oldPos);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (debugActive)
            {
                Vector3[] corners = agent.path.corners;
                Gizmos.color = Color.red;
                for (int i = 0; i < corners.Length; i++)
                {
                    if (i + 1 < corners.Length)
                        Gizmos.DrawLine(corners[i], corners[i + 1]);
                    else
                        Gizmos.DrawLine(corners[i], agent.destination);
                }

                if (state == AIState.Hiding || state == AIState.Running)
                {

                    for (int i = 0; i < furthestPositions.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Gizmos.color = Color.green;
                                break;

                            case 1:
                                Gizmos.color = Color.yellow;
                                break;

                            case 2:
                                Gizmos.color = Color.cyan;
                                break;

                            case 3:
                                Gizmos.color = Color.magenta;
                                break;

                            default:
                                Gizmos.color = Color.white;
                                break;
                        }
                        Gizmos.DrawCube(furthestPositions[i], Vector3.one * 0.25f);
                    }
                }
            }
        }
    }

}
