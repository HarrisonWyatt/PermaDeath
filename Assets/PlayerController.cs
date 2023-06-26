using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisonOffset = 0.05f;
    public SwordAttack swordAttack;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer SpriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(movementInput != Vector2.zero){
            bool success = TryMove(movementInput);

            if(!success && movementInput.x > 0){
                success = TryMove(new Vector2(movementInput.x, 0));

                if(!success && movementInput.y > 0){
                    success = TryMove(new Vector2(0, movementInput.y)); 
                }
            }

            animator.SetBool("IsMoving", success);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        //set direction
        if(movementInput.x < 0){
            SpriteRenderer.flipX = true;
            
        }
        else if (movementInput.x > 0){
            SpriteRenderer.flipX = false;
            
        }
        

    }
    private bool TryMove(Vector2 direction){
        if(direction != Vector2.zero){
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisonOffset);
            
        if(count == 0)
            {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
            }
        else 
            {
            return false;
            }
        }
        else{
            return false;
        }
    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire(){
        animator.SetTrigger("swordAttack");
    }
    public void SwordAttack(){
        if(SpriteRenderer.flipX == true){
            swordAttack.AttackLeft();
        }else{
            swordAttack.AttackRight();
        }
    }
    public void EndSwordAttack(){
        swordAttack.StopAttack();
    }
}
