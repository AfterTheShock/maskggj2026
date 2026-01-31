using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    [Header("Configuraci�n")]
    public float enemyHealth = 100f;
    public float enemyDamage = 10f;
    public float attackCooldown = 2.0f;
    public float attackRange = 1.5f;

    [SerializeField] float enemySpeed = 6;
    [SerializeField] float enemyAcceleration = 25;
    [SerializeField] float enemyAngularSpeed = 50;

    private NavMeshAgent agent;

    [Header("Estado")]
    public bool enemyIsAtacking;
    private float lastAttackTime;
    private Coroutine knockbackCoroutine;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = enemySpeed;
        agent.acceleration = enemyAcceleration;
        agent.angularSpeed = enemyAngularSpeed; // Velocidad de rotaci�n
        agent.stoppingDistance = attackRange - 0.2f; // Se detiene antes del rango
        playerTransform = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    private void Update()
    {
        // Mide la distancia entre el enemigo (transform.position) y la posici�n del jugador
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance <= attackRange && Time.time >= lastAttackTime + attackCooldown && !enemyIsAtacking)
        {
            Attack();
        }

    }

    void Attack()
    {
        enemyIsAtacking = true;
        // Guarda el momento de ataque
        lastAttackTime = Time.time;
        Debug.Log("El enemigo comenzo a atacar");

        // Se ejecuta en una corrutina para evitar el bloqueo del motor (ejecucion en paralelo)
        StartCoroutine(HitLogic());
    }

    System.Collections.IEnumerator HitLogic()
    {
        playerTransform.gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyDamage);
        Debug.Log("Golpe realizado por el enemigo");
        
        // Tiempo de espera hasta permitir que el enemigo ataque de nuevo
        yield return new WaitForSeconds(attackCooldown);
        enemyIsAtacking = false;
    }

    void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
    }

    public void TakeDamage(float damage)
    {
        float modifiedDamage = damage * UpgradeableStatsSingleton.Instance.damage;
        if (enemyHealth > modifiedDamage)
        {
            enemyHealth = enemyHealth - modifiedDamage;
            Debug.Log("Vida del enemigo: " + enemyHealth);
        }
        else
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector3 impulse, float duration = 0.35f)
    {
        if (TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic)
        {
            rb.AddForce(impulse, ForceMode.Impulse);
            return;
        }

        if (knockbackCoroutine != null) StopCoroutine(knockbackCoroutine);
        knockbackCoroutine = StartCoroutine(HandleKnockback(impulse, duration));
    }
    IEnumerator HandleKnockback(Vector3 impulse, float duration)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float dt = Time.deltaTime;
            float t = 1f - (elapsed / duration);
            Vector3 move = impulse * t * dt;
            transform.position += move;
            elapsed += dt;
            yield return null;
        }

        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
        }

        knockbackCoroutine = null;
    }

    void Die()
    {
        Destroy(gameObject);
        Debug.Log("El enemigo ha muerto");
    }
}