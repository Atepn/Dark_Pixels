using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector2 cursorPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        cursorPos = GameObject.Find("Player").GetComponent<PlayerController>().aimInput;
        cursorPos = Camera.main.ScreenToWorldPoint(cursorPos);
        transform.position = cursorPos;
    }
}
