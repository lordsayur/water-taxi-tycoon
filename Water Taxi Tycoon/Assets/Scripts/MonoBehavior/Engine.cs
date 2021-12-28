using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Engine : MonoBehaviour
{
    [SerializeField] private float reverseThrustReducer = 0.5f;
    private Rigidbody rb;
    private BoatController boatController;
    private float thrustPower;
    private float turnSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boatController = GetComponent<BoatController>();
        UpdateData();
    }
    void FixedUpdate()
    {
        UpdateData();
        var direction = GetInput();
        Accelerate(direction);
        Turn(direction);
        FixBoatPosition();
    }
    private void UpdateData()
    {
        var boatType = boatController.data.Type;
        thrustPower = boatType.thrust;
        turnSpeed = boatType.turnSpeed;
    }
    private Vector3 GetInput()
    {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        return new Vector3(h, 0f, v);
    }
    private void Accelerate(Vector3 thrust)
    {
        if (thrust.z == 0) return;
        var power = thrust.z >= 0 ? thrustPower : thrustPower * reverseThrustReducer;
        rb.AddForce(transform.forward * thrust.z * power * Time.fixedDeltaTime);
    }
    private void Turn(Vector3 direction)
    {
        var localSpeed = transform.InverseTransformDirection(rb.velocity);

        rb.AddTorque(transform.up * direction.x * turnSpeed * localSpeed.z * Time.fixedDeltaTime);
    }
    private void FixBoatPosition()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        }
    }
}
