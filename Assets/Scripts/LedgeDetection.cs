using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;
    public bool ledgeDetected;

    

    public bool canDetect = true;

    BoxCollider2D boxCd => GetComponent<BoxCollider2D>();

    private void Update()
    {
        if(player != null && canDetect)
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);

        if(enemy != null && canDetect)
            enemy.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetect = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCd.bounds.center, boxCd.size, 0);

        foreach (var hit in colliders)
        {
            if (hit.gameObject.GetComponent<PlatformController>() != null)
                return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetect = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
