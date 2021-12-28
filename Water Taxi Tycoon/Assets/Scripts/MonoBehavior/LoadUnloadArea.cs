using UnityEngine;

public class LoadUnloadArea : MonoBehaviour
{
    [SerializeField] private EventChannelSO eventChannel;
    private JettyController jetty;

    void Start()
    {
        jetty = GetComponentInParent<JettyController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            var boat = other.GetComponent<BoatController>();
            eventChannel.TriggerChannel.OnBoatArrived(jetty, boat);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            var boat = other.GetComponent<BoatController>();
            eventChannel.TriggerChannel.OnBoatDeparted(jetty, boat);
        }
    }
}
