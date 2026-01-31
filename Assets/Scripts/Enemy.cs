using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;

    [Header("Configuración")]
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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = enemySpeed;
        agent.acceleration = enemyAcceleration;
        agent.angularSpeed = enemyAngularSpeed; // Velocidad de rotación
        agent.stoppingDistance = attackRange - 0.2f; // Se detiene antes del rango
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    private void Update()
    {
        // Mide la distancia entre el enemigo (transform.position) y la posición del jugador
        float distance = Vector3.Distance(transform.position, player.position);

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
        //player.GetComponent<Health>().TakeDamage(enemyDamage);
        Debug.Log("Golpe realizado por el enemigo");
        
        // Tiempo de espera hasta permitir que el enemigo ataque de nuevo
        yield return new WaitForSeconds(attackCooldown);
        enemyIsAtacking = false;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }    
}