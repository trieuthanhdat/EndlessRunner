using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            Destroy(gameObject);


        if (collision.GetComponent<Player>() != null)
        {
            //AudioManager.instance.PlaySFX(0);
            MonoAudioManager.instance.PlaySound("Coin");
            GameManager.instance.coins++;
            Destroy(gameObject);
        }
    }
}
