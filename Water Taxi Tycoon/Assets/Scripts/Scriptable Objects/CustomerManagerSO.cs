using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Customer Manager", menuName = "Entity/Customer Manager")]
public class CustomerManagerSO : ScriptableObject
{
    #region Properties
    public int customerCount = 2;
    public int jettyAssignmentInterval = 5;
    public List<CustomerType> customerTypeList;
    public List<CustomerController> customerList = new();
    public bool HasIdleCustomer => customerList.Exists(c => c.data.State == CustomerState.Idle);
    #endregion
    void OnEnable()
    {
        customerList.Clear();
    }
    #region Methods
    public void SpawnAndRegisterCustomer(int id, UnityAction<int, CustomerType, CustomerController> onSpawnCustomer)
    {
        var customerType = GetCustomerType();
        var customer = Instantiate(customerType.prefab, Vector3.zero, Quaternion.identity);
        var customerController = customer.GetComponent<CustomerController>();

        onSpawnCustomer(id, customerType, customerController);

        customerList.Add(customerController);
    }
    public CustomerController GetIdleCustomer()
    {
        return customerList.First(c => c.data.State == CustomerState.Idle);
    }
    private CustomerType GetCustomerType()
    {
        return customerTypeList[Random.Range(0, customerTypeList.Count)];
    }
    #endregion
}
