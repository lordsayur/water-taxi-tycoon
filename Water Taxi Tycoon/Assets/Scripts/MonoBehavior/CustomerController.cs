using UnityEngine;

public class CustomerController : MonoBehaviour
{
    [HideInInspector] public CustomerData data;
    [SerializeField] private EventChannelSO eventChannel;

    private bool canMove = false;
    private Transform target;

    void Awake()
    {
        SubscribeEvents();
    }
    void Update()
    {
        if (canMove) MoveCustomer();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    public void GoToCustomerPool()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        data.State = CustomerState.Idle;
    }
    public void SetNewTargetPosition(Transform newTarget)
    {
        target = newTarget;
        transform.SetParent(null);
        canMove = true;
    }
    private void MoveCustomer()
    {
        if (Vector3.Distance(transform.position, target.position) < Vector3.kEpsilon)
        {
            eventChannel.CustomerChannel.OnReachTarget(this, target, data.State);
            canMove = false;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, 5 * Time.deltaTime);
    }
    private void HandleSpawnCustomer(int id, CustomerType type, CustomerController controller)
    {
        if (data != null) return;

        data = new CustomerData()
        {
            Id = id,
            Type = type,
            Controller = controller
        };

        gameObject.name = "Customer " + data.Id;

        GoToCustomerPool();
    }
    private void HandleSeatCustomer(Seat seat, CustomerState customerState)
    {
        if (data.Id != seat.Customer.data.Id) return;

        if (customerState == CustomerState.ToJetty)
        {
            data.State = CustomerState.Waiting;
        }
        if (customerState == CustomerState.Waiting)
        {
            data.State = CustomerState.OnBoard;
        }
        SetNewTargetPosition(seat.Transform);
    }
    private void HandleUnSeatCustomer(CustomerController customer, Transform destination, CustomerState customerState)
    {
        if (data.Id != customer.data.Id) return;

        if (customerState == CustomerState.Waiting)
        {
            data.State = CustomerState.OnBoard;
        }
        if (customerState == CustomerState.OnBoard)
        {
            data.State = CustomerState.Arrived;
            SetNewTargetPosition(destination);
        }
    }
    private void HandleJettyAssigned(int id)
    {
        if (data.Id != id) return;
        transform.position = data.DepartureJetty.data.Group.transform.Find("Ground").transform.position;
        SetNewTargetPosition(data.DepartureJetty.transform);

        gameObject.SetActive(true);
        data.State = CustomerState.ToJetty;
    }
    private void HandleSeatFull(CustomerController customer)
    {
        if (customer.data.Id != data.Id) return;

        GoToCustomerPool();
    }
    private void HandleReachTarget(CustomerController customer, Transform target, CustomerState customerState)
    {
        if (customer.data.Id != data.Id) return;

        if (customerState == CustomerState.OnBoard)
        {
            transform.SetParent(target);
        }
        if (customerState == CustomerState.Arrived)
        {
            GoToCustomerPool();
        }
    }
    private void SubscribeEvents()
    {
        eventChannel.CustomerChannel.SpawnCustomer += HandleSpawnCustomer;
        eventChannel.CustomerChannel.SeatCustomer += HandleSeatCustomer;
        eventChannel.CustomerChannel.UnSeatCustomer += HandleUnSeatCustomer;
        eventChannel.CustomerChannel.JettyAssgined += HandleJettyAssigned;
        eventChannel.SeatChannel.SeatFull += HandleSeatFull;
        eventChannel.CustomerChannel.ReachTarget += HandleReachTarget;
    }
    private void UnsubscribeEvents()
    {
        eventChannel.CustomerChannel.SpawnCustomer -= HandleSpawnCustomer;
        eventChannel.CustomerChannel.SeatCustomer -= HandleSeatCustomer;
        eventChannel.CustomerChannel.UnSeatCustomer -= HandleUnSeatCustomer;
        eventChannel.CustomerChannel.JettyAssgined -= HandleJettyAssigned;
        eventChannel.SeatChannel.SeatFull -= HandleSeatFull;
        eventChannel.CustomerChannel.ReachTarget -= HandleReachTarget;
    }
}
