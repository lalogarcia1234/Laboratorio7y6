using UnityEngine;

public class Enemigo3 : Entity, ITakeDamage, IDoDamage
{
    public float MoveSpeed;            
    public float AttackRange;       
    public float AttackCooldown;
    private Transform player;
    private Rigidbody2D rb;

    private float lastAttackTime;
  
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
            print("No se encontró al jugador. Asegúrate de que tenga Tag 'Player'");
    }

    void FixedUpdate()
    {
        if (player == null) return;

     
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * MoveSpeed;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= AttackRange && Time.time >= lastAttackTime + AttackCooldown)
        {
            EnemyAttack(damage);  
            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        Max_Life -= damage;
        print($"Enemigo {EntityName} recibió {damage} de daño. Vida actual: {Max_Life}");

        if (Max_Life <= 50)
        {
            damage = Mathf.RoundToInt(damage * 1.2f);
            print($"{EntityName}  está fuerte y aumenta su daño a {damage}");
        }

        if (Max_Life <= 0)
        {
            Die();
        }
        
    }

    private void Die()
    {
        print($"{EntityName} ha muerto.");
        Destroy(gameObject);
    }
    public void DealDamage(int DoDamage)
    {
        print($"{EntityName} causó {DoDamage} puntos de daño.");
    }
    public void EnemyAttack(int Damage)
    {
        if (player == null) return;

        ITakeDamage target = player.GetComponent<ITakeDamage>();
        if (target != null)
        {
            target.TakeDamage(Damage);
            DealDamage(Damage);
        }
    }
}


