using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoatData : ISeatable
{
    #region Properties
    public int Id { get; set; }
    public BoatType Type { get; set; }
    public BoatController Controller { get; set; }
    public List<Seat> SeatList { get; set; } = new();
    public bool HasCustomer => SeatList.Exists(s => s.Customer != null);
    public bool HasAvailableSeat => SeatList.Exists(s => !s.IsOccupied);
    public List<CustomerController> CustomerList => SeatList.Select(s => s.Customer).Where(c => c != null).ToList();
    public List<Seat> AvailableSeatList => SeatList.Where(s => !s.IsOccupied).ToList();

    #endregion
    #region Public Methods
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
        onCustomerSeat(seat, CustomerState.Waiting);
    }
    public void UnSeatCustomer(CustomerController customer, Action<CustomerController, Transform, CustomerState> onCustomerUnSeat)
    {
        var seat = SeatList.Find(s => s.Customer == customer);
        onCustomerUnSeat(seat.Customer, seat.Customer.data.DestinationJetty.data.Group.transform, CustomerState.OnBoard);
        seat.ClearSeat();
    }
    public List<CustomerController> FindCustomersByDestinationJetty(JettyData jetty)
    {
        return SeatList
                .Where(s => s.Customer != null && s.Customer.data.DestinationJetty.data.Id == jetty.Id)
                .Select(s => s.Customer)
                .ToList();
    }
    #endregion
}
