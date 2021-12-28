using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JettyData : ISeatable
{
    #region Properties
    public int Id { get; set; }
    public int GroupId { get; set; }
    public GameObject Group { get; set; }
    public JettyType Type { get; set; }
    public JettyController Controller { get; set; }
    public List<Seat> SeatList { get; set; } = new();
    public bool HasCustomer => SeatList.Any(s => s.Customer != null);
    public bool HasAvailableSeat => SeatList.Exists(s => !s.IsOccupied);
    public List<Seat> AvailableSeatList => SeatList.Where(s => !s.IsOccupied).ToList();
    public List<CustomerController> CustomerList => SeatList.Select(s => s.Customer).Where(c => c != null).ToList();
    #endregion
    #region
    public void RegisterSeat(Transform seat)
    {
        SeatList.Add(new Seat()
        {
            Id = SeatList.Count,
            Transform = seat,
            IsOccupied = false
        });
    }
    public void SeatCustomer(CustomerController customer, Action<Seat, CustomerState> onCustomerSeat)
    {
        if (!HasAvailableSeat) throw new Exception("No available seat.");

        var seat = SeatList.Find(s => !s.IsOccupied);
        seat.SeatCustomer(customer);
        onCustomerSeat(seat, CustomerState.ToJetty);
    }
    public void UnSeatCustomer(CustomerController customer, Action<CustomerController, Transform, CustomerState> onCustomerUnSeat)
    {
        var seat = SeatList.Find(s => s.Customer == customer);
        seat.ClearSeat();
        onCustomerUnSeat(customer, Controller.transform, CustomerState.Waiting);
    }
    #endregion
}
