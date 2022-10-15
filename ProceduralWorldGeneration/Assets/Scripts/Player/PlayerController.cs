using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public LayerMask platformLayerMask;
    public CharacterController2D characterController;

    private bool jump = false;
    private float horizontalMove = 0;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (IsGrounded() && Input.GetButtonDown("Jump"))
            jump = true;

        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 5);
            rb.AddForce(new Vector2(0f, -2f), ForceMode2D.Impulse);
        }

        //BugFix();
    }

    private void FixedUpdate()
    {
        characterController.Move(horizontalMove, jump);
        jump = false;
    }

    private bool IsGrounded()
    {
        float extra = .05f;

        RaycastHit2D raycast = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
             extra, platformLayerMask);

        return raycast.collider != null;
    }
}
