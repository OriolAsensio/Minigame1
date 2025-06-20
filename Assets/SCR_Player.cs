using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SCR_Player : MonoBehaviour
{
    [SerializeField] private float upForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float radius;

    private Animator animator;


    private Rigidbody2D CGrb;

    // Start is called before the first frame update
    void Start()
    {
        CGrb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, ground);
        animator.SetBool("IsGrounded", isGrounded);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {

                CGrb.AddForce(Vector2.up * upForce);

            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstaculo"))
        {

            GameManager.Instance.ShowGameOverScreen();
            animator.SetTrigger("Die");
            Time.timeScale = 0f;
        
        }
    }



}
