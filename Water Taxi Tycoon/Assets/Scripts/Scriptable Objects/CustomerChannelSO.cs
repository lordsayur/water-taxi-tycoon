using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CustomerChannelSO", menuName = "Event Channel/Customer Channel")]
public class CustomerChannelSO : ScriptableObject
{
    public event UnityAction<int, CustomerType, CustomerController> SpawnCustomer = delegate { };
    public event UnityAction<Seat, CustomerState> SeatCustomer = delegate { };
    public event UnityAction<CustomerController, Transform, CustomerState> UnSeatCustomer = delegate { };
    public event UnityAction<int> JettyAssgined = delegate { };
    public event UnityAction<CustomerController, Transform, CustomerState> ReachTarget = delegate { };

    private void OnEnable()
    {
        SeatCustomer = delegate { };
        UnSeatCustomer = delegate { };
        JettyAssgined = delegate { };
    }
    public void OnSpawnCustomer(int id, CustomerType customerType, CustomerController customerController)
    {
        if (SpawnCustomer != null)
        {
            SpawnCustomer.Invoke(id, customerType, customerController);
        }
    }
    public void OnCustomerSeat(Seat seat, CustomerState customerState)
    {
        if (SeatCustomer != null)
        {
            SeatCustomer.Invoke(seat, customerState);
        }
    }
    public void OnCustomerUnSeat(CustomerController customer, Transform jetty, CustomerState customerState)
    {
        if (UnSeatCustomer != null)
        {
            UnSeatCustomer.Invoke(customer, jetty, customerState);
        }
    }
    public void OnJettyAssigned(int id)
    {
        if (JettyAssgined != null)
        {
            JettyAssgined.Invoke(id);
        }
    }
    public void OnReachTarget(CustomerController customer, Transform target, CustomerState customerState)
    {
        if (ReachTarget != null)
        {
            ReachTarget.Invoke(customer, target, customerState);
        }
    }
}
