using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [HideInInspector] public BoatData data;
    [SerializeField] private BoatType boatType;
    [SerializeField] private EventChannelSO eventChannel;

    void Awake()
    {
        InitializeBoatData();
        RegisterBoatSeats();
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        eventChannel.GUIChannel.LoadButtonClick += HandleLoadButtonClick;
        eventChannel.TriggerChannel.BoatArrived += HandleBoatArrived;
    }

    void OnDestroy()
    {
        eventChannel.GUIChannel.LoadButtonClick -= HandleLoadButtonClick;
        eventChannel.TriggerChannel.BoatArrived -= HandleBoatArrived;
    }
    private void HandleLoadButtonClick(JettyController jetty, BoatController boat, List<CustomerController> customerList)
    {
        customerList.ForEach(customer =>
        {
            data.SeatCustomer(customer, eventChannel.CustomerChannel.OnCustomerSeat);
        });
    }
    private void HandleBoatArrived(JettyController jetty, BoatController boat)
    {
        if (!data.HasCustomer) return;

        var customerToUnloadList = data.FindCustomersByDestinationJetty(jetty.data);

        if (customerToUnloadList.Count <= 0)
        {
            return;
        }

        customerToUnloadList.ForEach(customer =>
        {
            data.UnSeatCustomer(customer, eventChannel.CustomerChannel.OnCustomerUnSeat);
        });
    }
    private void InitializeBoatData()
    {
        data = new BoatData()
        {
            Id = 1,
            Type = boatType,
            Controller = this
        };
    }
    private void RegisterBoatSeats()
    {
        var seatTransform = GetComponentsInChildren<Transform>().Where(t => t.CompareTag("Seat")).ToList();

        if (seatTransform == null)
        {
            Debug.LogError("No seat is found.");
            return;
        }

        seatTransform.ForEach(st => data.RegisterSeat(st));
    }
}
