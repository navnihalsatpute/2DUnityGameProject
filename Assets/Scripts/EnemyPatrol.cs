using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header ("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingleft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }


    private void Update()
    {
        if (movingleft)
            {
                if(enemy.position.x >= leftEdge.position.x)
                    MoveInDirection(-1);
                else
                    DirectionChange();
            }
                
            else
            {
                if(enemy.position.x <= rightEdge.position.x)
                    MoveInDirection(1);
                else
                    DirectionChange();
            }
    }

    public void DirectionChange()
    {
        anim.SetBool("moving", false);

        idleTimer+= Time.deltaTime;

        if(idleTimer > idleDuration)
            if (IsMovingLeft())
            {
                SetMovingLeft(false);
            }
            else
            {
                SetMovingLeft(true);
            }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
         initScale.y, initScale.z);

        //Moce in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
         enemy.position.y, enemy.position.z);

    }

    public bool IsMovingLeft()
    {
        return movingleft;
    }

    public void SetMovingLeft(bool moveLeft)
    {
        movingleft = moveLeft;
    }
}