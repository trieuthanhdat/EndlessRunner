using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private float chaneToSpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {

            if (Random.Range(0, 100) <= chaneToSpawn)
            {
               GameObject newEnemy = Instantiate(enemyPrefab, respawnPosition.position, Quaternion.identity);
               Destroy(newEnemy, 30);
            }

        }
    }
}
