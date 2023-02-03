using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float velocity;
    public LayerMask layerMask;

    [SerializeField] Rigidbody rb;

    private void Start()
    {
        rb.Sleep();
        //rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("throw!");
            rb.WakeUp();
            ThrowBall();
        }
    }

    private void ThrowBall()
    {
        
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        float angle = Mathf.Asin(-Physics.gravity.y * distance / (velocity * velocity)) / 2;
        float Vy = velocity * Mathf.Sin(angle);
        float Vxz = velocity * Mathf.Cos(angle);

        Vector3 VxzDirection = direction * Vxz;
        
        VxzDirection.y = Vy;
        
        rb.AddForce(VxzDirection, ForceMode.VelocityChange);
    }
}