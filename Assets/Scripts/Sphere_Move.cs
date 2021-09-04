using UnityEngine;

public class Sphere_Move : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    public float Speed;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.forward * Speed);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * Speed * 10);
        }
    }
}
