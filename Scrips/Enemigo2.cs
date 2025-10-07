using UnityEngine;

public class Enemigo2 : Entity, ITakeDamage, IDoDamage
{
    public float MoveSpeed;
    public float AttackRange;
    public float AttackCooldown;

    public int Armor;
    private int currentArmor;

    private Transform player;
    private Rigidbody2D rb;
    private float lastAttackTime;


    void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
      player = GameObject.FindWithTag("Player")?.transform;
        currentArmor=100;
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
        if (currentArmor > 0)
        {
            int absorbed = Mathf.Min(currentArmor, damage);
            currentArmor -= absorbed;
            damage -= absorbed;
            print($"{EntityName} absorbió {absorbed} de daño con armadura. Armadura restante: {currentArmor}");
        }

        if (damage > 0)
        {
            Max_Life -= damage;
            print($"Enemigo {EntityName} recibió {damage} de daño a la vida. Vida actual: {Max_Life}");

            if (Max_Life <= 0)
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



