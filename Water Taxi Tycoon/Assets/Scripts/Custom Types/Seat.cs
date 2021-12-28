using UnityEngine;

public class Seat
{
    #region Public Properties
    public int Id { get; set; }
    public Transform Transform { get; set; }
    public bool IsOccupied { get; set; }
    public CustomerController Customer { get; set; }
    #endregion
    #region Public Methods
    public void SeatCustomer(CustomerController customer)
    {
        IsOccupied = true;
        Customer = customer;
    }
    public void ClearSeat()
    {
        IsOccupied = false;
        Customer = null;
    }
    #endregion
}
