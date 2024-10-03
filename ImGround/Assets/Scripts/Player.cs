using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Class")]
    public PlayerMove pMove;
    public PlayerAttack pAttack;
    public PlayerBehavior pBehavior;

    [Header("Player Status")]
    public int health = 5;

    public Rigidbody rigid;

    void Awake()
    {
        pMove = GetComponent<PlayerMove>();
        pAttack = GetComponent<PlayerAttack>();
        pBehavior = GetComponent<PlayerBehavior>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        pBehavior.getInput();
        pBehavior.Use();
        pBehavior.Swap();
        pMove.MoveInput();
        pAttack.AttackInput();
        if (!pMove.IsTired)
        {
            pMove.Move();
        }
        pMove.Turn();
        pMove.Jump();
        pMove.Sleep();
        pAttack.Attack();
    }
}