using UnityEngine;

public class JettyEntranceArea : MonoBehaviour
{
    [SerializeField] private EventChannelSO eventChannel;
    private JettyController jetty;
    private void Start()
    {
        jetty = GetComponentInParent<JettyController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            var customer = other.GetComponent<CustomerController>();

            if (customer.data.State == CustomerState.ToJetty)
            {
                eventChannel.TriggerChannel.OnCustomerArrived(customer, jetty);
            }
        }
    }
}
