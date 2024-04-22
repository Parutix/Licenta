using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    public GameObject character;
    public Vector3 spawnPosition;
    public static bool characterSpawned = false;

    void Start()
    {
        if (!characterSpawned)
        {
            Instantiate(character, spawnPosition, Quaternion.identity);
            characterSpawned = true;
        }
        else
        {
            Debug.Log("Character already spawned");
        }
    }
}
