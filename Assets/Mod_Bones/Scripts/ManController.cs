using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ManController : MonoBehaviour
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed;

    public float maxLength;

    public LineRenderer line;

    public Rigidbody2D hand;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        transform.position += new Vector3(x*speed,0,0) *Time.deltaTime;

        Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mp - (Vector2)line.transform.position;
        if(dir.magnitude > maxLength)
        {
            dir = dir.normalized * maxLength;
        }
        
        Vector2 lp = (Vector2)hand.transform.position + dir;

        hand.transform.localPosition = dir;
        line.SetPosition(0, dir);
    }
}
