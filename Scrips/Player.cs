using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity, ITakeDamage, IDoDamage
{
    public InputSystem_Actions inputs;
    public Vector2 moveInput;
    public int MoveSpeed;
    public int AttackDamage = 10;
    public int AttackRange = 10;

    public void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Move.performed += OnMove;
        inputs.Player.Move.started += OnMove;
        inputs.Player.Move.canceled += OnMove;
        inputs.Player.Attack.performed += OnAttack;
    }
    private void OnDisable()
    {
        inputs.Disable();
        inputs.Player.Move.performed -= OnMove;
        inputs.Player.Move.started -= OnMove;
        inputs.Player.Move.canceled -= OnMove;
        inputs.Player.Attack.performed -= OnAttack;
    }
    private void OnMove(InputAction.CallbackContext context)
    {
       moveInput = context.ReadValue<Vector2>();
    }
    private void Awake()
    {
         inputs = new InputSystem_Actions(); 
    }
    void Start()
    {
        damage = 10;
        AttackDamage = damage;
    }

    void Update()
    {
        transform.position += (Vector3)moveInput * MoveSpeed * Time.deltaTime;
       
    }
 
    public void  TakeDamage(int Damage)
    {
        Max_Life -= Damage;
        print($"Entidad {EntityName} recibió {Damage} de daño. Vida actual: {Max_Life}");

        if (Max_Life <= 0)
        {
            print("Has muerto" + EntityName);
            Destroy(gameObject);
        }
    }
    public void DealDamage(int DoDamage) 
    {
        print($"{EntityName} causó {DoDamage} puntos de daño.");
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        Attack();

    }
    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AttackRange);
        foreach (Collider2D hit in hits)
        {
           
            ITakeDamage target = hit.GetComponent<ITakeDamage>();
            if (target != null && hit.gameObject != this.gameObject) 
            {
                target.TakeDamage(AttackDamage); 
                DealDamage(AttackDamage);         
            }
        }

     print($"{EntityName} atacó causando {AttackDamage} de daño.");
    }
    public void PoisonDamage(int damage)
    {
        Max_Life -= damage;
        Debug.Log($"Jugador recibió {damage} de daño por veneno. Vida actual: {Max_Life}");
    }
}
