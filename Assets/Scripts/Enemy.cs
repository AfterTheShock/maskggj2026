using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;

    [Header("Configuración")]
    public float enemyHealth = 100f;
    public float enemyDamage = 10f;
    public float attackCooldown = 2.0f;
    public float attackRange = 1.5f;
    [SerializeField] float enemySpeed = 15.0f;

    private Rigidbody rb;

    [Header("Estado")]
    public bool enemyIsAtacking;
    private float lastAttackTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1.8f;
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
        Debug.Log("Golpe realizado");
        
        // Tiempo de espera hasta permitir que el enemigo ataque de nuevo
        yield return new WaitForSeconds(attackCooldown);
        enemyIsAtacking = false;
    }

    void ChasePlayer()
    {
        Vector3 towardsPlayer = (player.position - transform.position).normalized;
        towardsPlayer.y = 0;
        rb.AddForce(towardsPlayer * enemySpeed);
    }
}
