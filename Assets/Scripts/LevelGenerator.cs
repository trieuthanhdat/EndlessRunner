using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Vector3 nextPartPosition;

    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    private List<Transform> activePlatforms;
    private Queue<Transform> inactivePlatforms;

    private void Start()
    {
        activePlatforms = new List<Transform>();
        inactivePlatforms = new Queue<Transform>();
    }

    void Update()
    {
        DeletePlatform();
        GeneratePlatform();
    }

    private void GeneratePlatform()
    {
        while (Vector2.Distance(player.transform.position, nextPartPosition) < distanceToSpawn)
        {
            Transform part;
            if (inactivePlatforms.Count > 0)
            {
                part = inactivePlatforms.Dequeue();
                part.gameObject.SetActive(true);
            }
            else
            {
                part = Instantiate(levelPart[Random.Range(0, levelPart.Length)]);
                part.SetParent(transform);
            }

            Vector2 newPosition = new Vector2(nextPartPosition.x - part.Find("StartPoint").position.x, 0);
            part.position = newPosition;

            nextPartPosition = part.Find("EndPoint").position;
            activePlatforms.Add(part);
        }
    }

    private void DeletePlatform()
    {
        if (activePlatforms.Count > 0)
        {
            Transform partToDelete = activePlatforms[0];
            if (Vector2.Distance(player.transform.position, partToDelete.position) > distanceToDelete)
            {
                partToDelete.gameObject.SetActive(false);
                activePlatforms.Remove(partToDelete);
                inactivePlatforms.Enqueue(partToDelete);
            }
        }
    }
}
