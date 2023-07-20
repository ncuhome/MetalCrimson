using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed;

    public Rigidbody2D body;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        transform.position += new Vector3(x*speed,0,0) *Time.deltaTime;

        Vector2 a = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawRay(transform.position, a - (Vector2)transform.position);
    }
}
