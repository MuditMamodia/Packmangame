using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_ai : MonoBehaviour
{
    public Transform target; // Player reference
    public NavMeshAgent agent;

    [Header("Random Points")]
    public Transform[] randomPoints; // Assign all possible random points in Inspector
    private Vector3[] randomPositions; // Store positions to avoid missing reference issues

    [Header("Base Settings")]
    public float chaseDuration = 4f;
    public float runAwayDuration = 4f;
    public float runAwaySpeed = 2f;
    public float normalSpeed = 5f;
    public float chaseSpeed = 7f;
    public Vector2 enemystartposition;

    [Header("Difficulty Scaling")]
    public float difficultyIncreaseInterval = 20f;
    public float chaseDurationIncrement = 1f;
    public float speedIncrement = 0.5f;

    [Header("Behavior Flags")]
    public bool isChasing = false;
    public bool isRunningAway = false;
    public bool isEvading = false; // NEW → ghost runs away on its own
    public bool caught = false;

    private int randomCount = 0;
    private int maxRandomBeforeNearPlayer = 1;
    private float difficultyTimer;


    [SerializeField] private packman_movement_script pms;

    public void Awake()
    {
        enemystartposition = transform.position;
    }


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Store random point positions so we don’t rely on destroyed transforms
        randomPositions = new Vector3[randomPoints.Length];
        for (int i = 0; i < randomPoints.Length; i++)
        {
            if (randomPoints[i] != null)
                randomPositions[i] = randomPoints[i].position;
        }

        difficultyTimer = 0f;
        GoToRandomPoint();
    }

    private void Update()
    {
        difficultyTimer += Time.deltaTime;

        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            chaseDuration += chaseDurationIncrement;
            normalSpeed += speedIncrement;
            chaseSpeed += speedIncrement;
            difficultyTimer = 0f;
        }

        // Don’t do normal AI when evading
        if (isEvading)
        {
            EvadePlayer();
            return; // Stop other logic
        }

        if (!isChasing && !isRunningAway && agent.remainingDistance < 0.2f && !caught)
        {
            if (randomCount < maxRandomBeforeNearPlayer)
            {
                randomCount++;
                GoToRandomPoint();
            }
            else
            {
                GoNearPlayer();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isEvading)
        {
            caught = true;
            pms.death_of_player();
        }
    }

    void GoToRandomPoint()
    {
        if (randomPositions.Length == 0) return;

        int index = Random.Range(0, randomPositions.Length);
        agent.speed = normalSpeed;
        agent.SetDestination(randomPositions[index]);
    }

    void GoNearPlayer()
    {
        if (isEvading) return; // Prevent chasing while evading

        randomCount = 0;
        agent.speed = normalSpeed;

        Vector3 closest = Vector3.zero;
        float minDist = Mathf.Infinity;

        foreach (Vector3 pos in randomPositions)
        {
            float dist = Vector2.Distance(target.position, pos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = pos;
            }
        }

        if (closest != Vector3.zero)
        {
            agent.SetDestination(closest);
        }

        StartCoroutine(ChasePlayerForTime());
    }

    IEnumerator ChasePlayerForTime()
    {
        if (isEvading) yield break; // Cancel chase if evading

        isChasing = true;
        float elapsed = 0f;

        while (elapsed < chaseDuration && !isEvading)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.position);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isChasing = false;

        if (!isEvading)
        {
            if (Random.value > 0.3f)
                GoNearPlayer();
            else
                GoToRandomPoint();
        }
    }

    public void RunAwayFromPlayer()
    {
        if (!isRunningAway)
            StartCoroutine(RunAwayRoutine());
    }

    IEnumerator RunAwayRoutine()
    {
        isRunningAway = true;
        float elapsed = 0f;

        while (elapsed < runAwayDuration && !isEvading)
        {
            Vector3 dir = (transform.position - target.position).normalized;
            Vector3 runPos = transform.position + dir * 5f;

            agent.speed = runAwaySpeed;
            agent.SetDestination(runPos);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isRunningAway = false;
        if (!isEvading)
            GoToRandomPoint();
    }

    // Evade player whenever isEvading = true
    void EvadePlayer()
    {
        Vector3 dir = (transform.position - target.position).normalized;
        Vector3 evadePos = transform.position + dir * 6f;
        agent.speed = runAwaySpeed;
        agent.SetDestination(evadePos);
    }

    public void reseatting_position()
    {
        transform.position = enemystartposition;
        isChasing = false;
        isRunningAway = false;
        isEvading = false; 
        caught = false;
    }

}
