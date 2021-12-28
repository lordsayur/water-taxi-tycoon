using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISeatable
{
    public List<Seat> SeatList { get; set; }
    public bool HasCustomer { get; }
    public bool HasAvailableSeat { get; }
    public List<CustomerController> CustomerList { get; }
    public List<Seat> AvailableSeatList { get; }
    public void RegisterSeat(Transform seat);
    public void SeatCustomer(CustomerController customer, Action<Seat, CustomerState> onCustomerSeat);
    public void UnSeatCustomer(CustomerController customer, Action<CustomerController, Transform, CustomerState> onCustomerUnSeat);
}
