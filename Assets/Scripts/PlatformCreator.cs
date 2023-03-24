using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PlatformCreator : MonoBehaviour
{
    [SerializeField] Transform firstPlatform;
    [SerializeField] GameObject platformPrefab;
    [SerializeField] int platformsBefore;
    [SerializeField] int platformsAfter;
    public Vector3 unitDistance;

    private List<Transform?> platforms = new ();
    private int playerIndex;
    private void Start()
    {
        playerIndex = platformsBefore;
        for (int i = 0; i < platformsBefore; i++) platforms.Add(null);
        platforms.Add(firstPlatform);
        for (int i = 0; i < platformsAfter; i++)
        {
            spawnNewPlatform();
        }
    }

    private void spawnNewPlatform()
    {
        int distance = Random.Range(1, 3);

        Vector3 newPlatformPosition = platforms.Last().position + unitDistance * distance;
        GameObject newPlatform = Instantiate(platformPrefab, newPlatformPosition, Quaternion.identity);
        if (distance == 2)
        {
            platforms.Add(null);
        }
        platforms.Add(newPlatform.transform);
    }

    public bool Move(int units)
    {
        playerIndex += units;

        int count = platformsAfter - platforms.Skip(playerIndex + 1).Count(t => t != null);
        for (int i = 0; i < count; i++)
        {
            spawnNewPlatform();   
        }
        count = platforms.SkipLast(platforms.Count - playerIndex).Count(t => t != null) - platformsBefore;
        while (count > 0)
        {
            if (platforms[0] != null)
            {
                Destroy(platforms[0].gameObject);
                count--;
            }
            platforms.RemoveAt(0);
            playerIndex--;
        }

        return platforms[playerIndex] == null;
    }
}