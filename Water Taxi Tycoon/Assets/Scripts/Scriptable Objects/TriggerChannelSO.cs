using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TriggerChannelSO", menuName = "Event Channel/Trigger Channel")]
public class TriggerChannelSO : ScriptableObject
{
    public event UnityAction<CustomerController, JettyController> CustomerArrived = delegate { };
    public event UnityAction<JettyController, BoatController> BoatArrived = delegate { };
    public event UnityAction<JettyController, BoatController> BoatDeparted = delegate { };

    private void OnEnable()
    {
        CustomerArrived = delegate { };
        BoatArrived = delegate { };
    }

    public void OnCustomerArrived(CustomerController customer, JettyController jetty)
    {
        if (CustomerArrived != null)
        {
            CustomerArrived.Invoke(customer, jetty);
        }
    }
    public void OnBoatArrived(JettyController jetty, BoatController boat)
    {
        if (BoatArrived != null)
        {
            BoatArrived.Invoke(jetty, boat);
        }
    }
    public void OnBoatDeparted(JettyController jetty, BoatController boat)
    {
        if (BoatDeparted != null)
        {
            BoatDeparted.Invoke(jetty, boat);
        }
    }
}
