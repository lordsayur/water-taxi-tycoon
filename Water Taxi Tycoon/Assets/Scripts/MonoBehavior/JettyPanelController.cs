using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JettyPanelController : MonoBehaviour
{
    [SerializeField] private EventChannelSO eventChannel;
    private TextMeshProUGUI jettyLabel;
    private Button loadButton;
    private Button cancelButton;
    private TextMeshProUGUI BoatNameLabel;
    private TextMeshProUGUI BoatCapacityLabel;
    private List<GameObject> toggleGameObjectList = new();
    private List<Toggle> toggleComponentList = new();
    private List<Text> toggleLabelList = new();
    private List<CustomerController> selectedCustomerList = new();
    private List<Seat> boatAvailableSeatList = new();
    private int boatSeatCapacity;

    void Awake()
    {
        gameObject.SetActive(true);
        jettyLabel = transform
            .Find("Jetty Label Panel")
            .Find("Jetty Label")
            .GetComponent<TextMeshProUGUI>();
        BoatNameLabel = transform
            .Find("Boat Name Label")
            .GetComponent<TextMeshProUGUI>();
        BoatCapacityLabel = transform
            .Find("Boat Capacity Label")
            .GetComponent<TextMeshProUGUI>();
        var customerPanels = GameObject
            .Find("Customer List Panel").transform;
        foreach (Transform child in customerPanels)
        {
            var toggle = child
                .Find("Toggle")
                .gameObject;
            toggleGameObjectList.Add(toggle);
            toggleComponentList
                .Add(toggle
                .GetComponent<Toggle>());
            toggleLabelList.Add(toggle.GetComponentInChildren<Text>());
        }
        loadButton = transform.Find("Load Button").GetComponent<Button>();
        cancelButton = transform.Find("Cancel Button").GetComponent<Button>();
        SubscribeEvents();
        gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        UnsubsribeEvents();
    }
    private void SubscribeEvents()
    {
        eventChannel.TriggerChannel.BoatArrived += HandleBoatArrived;
        cancelButton.onClick.AddListener(HandleCancelButtonClicked);
    }
    private void UnsubsribeEvents()
    {
        eventChannel.TriggerChannel.BoatArrived -= HandleBoatArrived;
        cancelButton.onClick.RemoveListener(HandleCancelButtonClicked);
    }
    private void HandleBoatArrived(JettyController jetty, BoatController boat)
    {
        boatSeatCapacity = boat.data.SeatList.Count;
        boatAvailableSeatList = boat.data.AvailableSeatList;
        var customerList = jetty.data.CustomerList;
        UpdateLoadButtonState();
        UpdateBoatCapacityLabel();
        if (customerList.Count <= 0)
        {
            return;
        }

        gameObject.SetActive(true);
        jettyLabel.text = "Jetty " + jetty.data.Id;
        BoatNameLabel.text = boat.name;

        for (int i = 0; i < customerList.Count; i++)
        {
            toggleComponentList[i].interactable = true;
            toggleComponentList[i].onValueChanged.AddListener(GenerateToggleEventListener(customerList[i]));
            toggleLabelList[i].text = "Customer " + customerList[i].data.Id + " - To Jetty " + customerList[i].data.DestinationJetty.data.Id;
        }

        loadButton.onClick.AddListener(GenerateLoadEventLestener(jetty, boat));
    }
    private void HandleLoadButtonClicked(JettyController jetty, BoatController boat)
    {
        eventChannel.GUIChannel.OnLoadButtonClick(jetty, boat, selectedCustomerList);
        ResetJettyPanel();
    }
    private void HandleCancelButtonClicked()
    {
        ResetJettyPanel();
    }
    private void ResetJettyPanel()
    {
        selectedCustomerList.Clear();
        for (int i = 0; i < toggleGameObjectList.Count; i++)
        {
            toggleComponentList[i].interactable = false;
            toggleComponentList[i].isOn = false;
            toggleComponentList[i].onValueChanged.RemoveAllListeners();
            toggleLabelList[i].text = "Empty Seat";
        }
        jettyLabel.text = "";
        BoatNameLabel.text = "";
        BoatCapacityLabel.text = "";
        loadButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
    private void HandleValueChanged(bool value, CustomerController customer)
    {
        if (value)
        {
            selectedCustomerList.Add(customer);
        }
        else
        {
            selectedCustomerList.Remove(customer);
        }
        UpdateLoadButtonState();
        UpdateBoatCapacityLabel();
    }
    private void UpdateLoadButtonState()
    {
        loadButton.interactable = selectedCustomerList.Count > 0 && selectedCustomerList.Count <= boatAvailableSeatList.Count;
    }
    private void UpdateBoatCapacityLabel()
    {
        BoatCapacityLabel.text = selectedCustomerList.Count + (boatSeatCapacity - boatAvailableSeatList.Count) + " / " + boatSeatCapacity;
    }
    private UnityAction<bool> GenerateToggleEventListener(CustomerController customer)
    {
        return (value) => HandleValueChanged(value, customer);
    }
    private UnityAction GenerateLoadEventLestener(JettyController jetty, BoatController boat)
    {
        return () => HandleLoadButtonClicked(jetty, boat);
    }
}
