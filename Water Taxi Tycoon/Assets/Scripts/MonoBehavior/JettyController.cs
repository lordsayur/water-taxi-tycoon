using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class JettyController : MonoBehaviour
{
    [HideInInspector] public JettyData data;
    [SerializeField] JettyType jettyType;
    [SerializeField] JettyManagerSO jettyManager;
    [SerializeField] EventChannelSO eventChannel;
    [SerializeField] int groupId;
    TextMeshProUGUI label;

    void Start()
    {
        InitializeJetty(jettyManager.GetNewId, groupId);
        jettyManager.RegisterJetty(this);
        RegisterAndInitializeSeats();
        SetJettyLabel();
        SubscribeEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void InitializeJetty(int id, int groupId)
    {
        data = new JettyData()
        {
            Id = jettyManager.GetNewId,
            GroupId = groupId,
            Type = jettyType,
            Controller = this,
            Group = transform.parent.gameObject
        };
    }
    private void RegisterAndInitializeSeats()
    {
        var seatTransform = GetComponentsInChildren<Transform>().Where(t => t.CompareTag("Seat")).ToList();

        if (seatTransform == null)
        {
            Debug.LogError("No seat is found.");
            return;
        }

        seatTransform.ForEach(st => data.RegisterSeat(st));
    }
    private void SetJettyLabel()
    {
        label = GetComponentInChildren<TextMeshProUGUI>();
        label.text = "Jetty " + data.Id.ToString();
    }
    private void HandleCustomerArrived(CustomerController customer, JettyController jetty)
    {
        if (jetty.data.Id != data.Id) return;

        if (!data.HasAvailableSeat)
        {
            eventChannel.SeatChannel.OnSeatFull(customer);
            return;
        }

        data.SeatCustomer(customer, eventChannel.CustomerChannel.OnCustomerSeat);
    }
    private void HandleLoadButtonClick(JettyController jetty, BoatController boat, List<CustomerController> customers)
    {
        if (!data.HasCustomer || jetty.data.Id != data.Id) return;

        for (int i = 0; i < customers.Count; i++)
        {
            data.UnSeatCustomer(customers[i], eventChannel.CustomerChannel.OnCustomerUnSeat);
        }
    }
    private void UnsubscribeEvents()
    {
        eventChannel.TriggerChannel.CustomerArrived -= HandleCustomerArrived;
    }
    private void SubscribeEvents()
    {
        eventChannel.TriggerChannel.CustomerArrived += HandleCustomerArrived;
        eventChannel.GUIChannel.LoadButtonClick += HandleLoadButtonClick;
    }
}