using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Engine : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float thrust = 1200f;
    [SerializeField]
    private float turnSpeed = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var direction = GetInput();
        Accelerate(direction.z);
        Turn(direction.x);
    }

    Vector3 GetInput()
    {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        return new Vector3(h, 0f, v);
    }

    void Accelerate(float amount)
    {
        rb.AddForce(transform.forward * amount * thrust * Time.fixedDeltaTime);
    }
    void Turn(float amount)
    {
        rb.AddTorque(transform.up * amount * Vector3.Magnitude(rb.velocity) * turnSpeed * Time.fixedDeltaTime);
    }
}
