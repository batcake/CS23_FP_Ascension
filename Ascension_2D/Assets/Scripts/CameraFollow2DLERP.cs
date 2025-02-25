using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2DLERP : MonoBehaviour
{
    public GameObject target;
    public float camSpeed = 4.0f;
    private float distance = 16f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 pos = Vector2.Lerp ((Vector2) transform.position, (Vector2) target.transform.position,
                                    camSpeed * Time.fixedDeltaTime);
        if (pos.x < -distance) {
            transform.position = new Vector3 (-distance, pos.y, transform.position.z);
        } else if (pos.x > distance) {
            transform.position = new Vector3 (distance, pos.y, transform.position.z);
        } else {
            transform.position = new Vector3 (pos.x, pos.y, transform.position.z);
        }
    }
}
