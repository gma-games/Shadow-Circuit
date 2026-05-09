using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Mozgás attribútumok")]
    public float speed = 2f;
    public float chargeSpeed = 5f;

    [Header("Detection")]
    public float detectionRange = 3f;
    public float loseDistance = 5f;
    public LayerMask playerLayer;

    [Header("Patrol Route")]
    public List<Transform> patrolPoints;
    private int currentPointIndex;


    private FiniteStateMachine fsm;
    private SpriteRenderer spriteRenderer;
    private Transform targetPlayer;

    private float searchTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

       
        fsm = new FiniteStateMachine();
        fsm.ChangeState(EnemyState.Patrol);

        // Melyik állapot melyik metódus
        fsm.stateActions[EnemyState.Patrol] = Patrol;
        fsm.stateActions[EnemyState.Chase] = ChasePlayer;
        fsm.stateActions[EnemyState.Search] = SearchPlayer;
    }

    void Update()
    {
        UpdateState();       // Ellenőrzés ,hohyx kell e állapotpt váltani
        fsm.ExecuteState();  // aktuálios állapot fuittatása
    }

    private void UpdateState()
    {
        bool canSeePlayer = DetectPlayer();

        // Ha őrjáratozik, de meglátja a játékost -> Üldözés!
        if (fsm.currentState == EnemyState.Patrol && canSeePlayer)
        {
            fsm.ChangeState(EnemyState.Chase);
        }
        // Ha üldözi, de túl messze ment a játékos -> Keresés!
        else if (fsm.currentState == EnemyState.Chase && !canSeePlayer)
        {
            searchTimer = 2f; // 2 másodpercig fog keresni
            fsm.ChangeState(EnemyState.Search);
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        if (Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.25f)
        {
            currentPointIndex++;
            if (currentPointIndex >= patrolPoints.Count) currentPointIndex = 0;
        }

        MoveTowards(patrolPoints[currentPointIndex].position, speed);
    }

    private void ChasePlayer()
    {
        if (targetPlayer != null)
        {
            Vector2 targetPos = new Vector2(targetPlayer.position.x, transform.position.y);
            MoveTowards(targetPos, chargeSpeed);
        }
    }

    private void SearchPlayer()
    {
        searchTimer -= Time.deltaTime;

        if (searchTimer <= 0)
        {
            // Ha lejárt az idő, és nem találta meg, visszaáll őrjáratozni
            targetPlayer = null;
            fsm.ChangeState(EnemyState.Patrol);
        }
    }

    //  Segéd metódusok 

    private void MoveTowards(Vector2 targetPosition, float currentSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        
        spriteRenderer.flipX = (transform.position.x - targetPosition.x) < 0f;
    }

    private bool DetectPlayer()
    {
        Vector2 lookDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection, detectionRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            targetPlayer = hit.transform;
            return true;
        }

        if (targetPlayer != null)
        {
            if (Vector2.Distance(transform.position, targetPlayer.position) > loseDistance)
            {
                return false; // Elvesztettük!
            }
            return true; // Még követjük
        }

        return false;
    }
}