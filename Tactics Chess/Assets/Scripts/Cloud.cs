using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    private float speed;

    public float minX;
    public float maxX;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < minX)
        {
            Vector2 newPos = new Vector2(maxX, transform.position.y);
            transform.position = newPos;
        }
    }
}
