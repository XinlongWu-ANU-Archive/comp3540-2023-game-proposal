using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCarema : MonoBehaviour
{
    private float caremaHight;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        caremaHight = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float playerPos = player.gameObject.transform.position.y;
        if (playerPos > transform.position.y + caremaHight)
        {
            transform.Translate(new Vector3(0, caremaHight * 2));
        }
        else if (playerPos < transform.position.y - caremaHight)
        {
            transform.Translate(new Vector3(0, -caremaHight * 2));
        }
    }
}
