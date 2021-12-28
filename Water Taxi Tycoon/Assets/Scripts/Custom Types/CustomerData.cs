public class CustomerData
{
    #region Properties
    public int Id { get; set; }
    public CustomerType Type { get; set; }
    public CustomerController Controller { get; set; }
    public CustomerState State { get; set; }
    public JettyController DepartureJetty { get; set; }
    public JettyController DestinationJetty { get; set; }
    #endregion
}
