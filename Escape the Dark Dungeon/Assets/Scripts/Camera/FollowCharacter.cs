using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform character;
    private Vector3 offset = new Vector3(0, 0, -10);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(character != null)
        {
            transform.position = character.position + offset;
        }
    }
}
