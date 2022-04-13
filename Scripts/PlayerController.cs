using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    public Vector2 aimInput;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    bool canMove = true;
     

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // FixedUpdate is called once per 0.2 sec
    void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 moveDir)
    {
        if(moveDir != Vector2.zero)
        {
            int count = rb.Cast(moveDir, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnLook(InputValue aimValue)
    {
        print("look");
        // aimInput = Camera.main.ScreenToWorldPoint(aimValue.Get<Vector2>());
        aimInput = aimValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
        
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();  
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }       
}
