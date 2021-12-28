using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SeatChannelSO", menuName = "Event Channel/Seat Channel")]
public class SeatChannelSO : ScriptableObject
{
    public event UnityAction<CustomerController> SeatFull = delegate { };

    public void OnSeatFull(CustomerController customer)
    {
        if (SeatFull != null)
        {
            SeatFull.Invoke(customer);
        }
    }
}
