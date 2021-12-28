using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] CustomerManagerSO customerManager;
    [SerializeField] JettyManagerSO jettyManager;
    [SerializeField] EventChannelSO eventChannel;
    void Start()
    {
        for (int i = 0; i < customerManager.customerCount; i++)
        {
            customerManager.SpawnAndRegisterCustomer(id: i, eventChannel.CustomerChannel.OnSpawnCustomer);
        }

        StartCoroutine(AssignJettyForCustomer());
    }

    IEnumerator AssignJettyForCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(customerManager.jettyAssignmentInterval);

            if (customerManager.HasIdleCustomer && jettyManager.HasJettyRegistered)
            {
                AssignJetty();
            }
        }
    }

    private void AssignJetty()
    {
        var customer = customerManager.GetIdleCustomer();
        var (departureJetty, destinationJetty) = jettyManager.GetDepartureAndDestinationJetty();
        customer.data.DepartureJetty = departureJetty;
        customer.data.DestinationJetty = destinationJetty;
        eventChannel.CustomerChannel.OnJettyAssigned(customer.data.Id);
    }
}
