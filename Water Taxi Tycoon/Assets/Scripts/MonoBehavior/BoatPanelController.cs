using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoatPanelController : MonoBehaviour
{
    private BoatController boatController;
    [SerializeField] private EventChannelSO eventChannel;
    private List<TextMeshProUGUI> seatLabelList = new();
    private List<GameObject> seatLabelPanelList = new();

    private void Start()
    {
        GetAllSeatLabels();
        ActivateSeatLabelPanels();
        SubscribeEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void HandleUnSeatCustomer(CustomerController customer, Transform seat, CustomerState customerState)
    {
        if (customerState == CustomerState.OnBoard)
        {
            var seats = seatLabelList.Where(s => s.text == customer.data.DestinationJetty.data.Id.ToString()).ToList();

            seats.ForEach(s =>
            {
                s.text = "-";
            });
        }
    }
    private void HandleSeatCustomer(Seat seat, CustomerState customerState)
    {
        if (customerState == CustomerState.Waiting)
        {
            var _seat = seatLabelList.Find(l => l.text == "-");
            _seat.text = seat.Customer.data.DestinationJetty.data.Id.ToString();
        }
    }
    private void ActivateSeatLabelPanels()
    {
        boatController = GameObject.Find("Player").GetComponent<BoatController>();
        var seatCount = boatController.data.SeatList.Count;
        for (int i = 0; i < seatLabelPanelList.Count; i++)
        {
            if (i >= seatCount)
            {
                seatLabelPanelList[i].SetActive(false);
            }
        }
    }
    private void GetAllSeatLabels()
    {
        foreach (Transform child in transform)
        {
            var label = child.GetComponentInChildren<TextMeshProUGUI>();
            label.text = "-";

            seatLabelPanelList.Add(child.gameObject);
            seatLabelList.Add(label);
        }
    }
    private void SubscribeEvents()
    {
        eventChannel.CustomerChannel.SeatCustomer += HandleSeatCustomer;
        eventChannel.CustomerChannel.UnSeatCustomer += HandleUnSeatCustomer;
    }
    private void UnsubscribeEvents()
    {
        eventChannel.CustomerChannel.SeatCustomer -= HandleSeatCustomer;
        eventChannel.CustomerChannel.UnSeatCustomer -= HandleUnSeatCustomer;
    }
}
