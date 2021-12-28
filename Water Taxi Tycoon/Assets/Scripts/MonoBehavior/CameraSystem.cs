using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private GameObject ThirdPerson;
    [SerializeField] private GameObject TopDown;
    [SerializeField] private EventChannelSO eventChannel;

    private bool isTopDown = true;
    void Start()
    {
        TopDown.SetActive(isTopDown);
        ThirdPerson.SetActive(!isTopDown);
        eventChannel.TriggerChannel.BoatArrived += HandleBoatArrived;
        eventChannel.TriggerChannel.BoatDeparted += HandleDeparted;
    }
    private void OnDestroy()
    {
        eventChannel.TriggerChannel.BoatArrived -= HandleBoatArrived;
        eventChannel.TriggerChannel.BoatDeparted -= HandleDeparted;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }
    private void SwitchCamera()
    {
        isTopDown = !isTopDown;
        TopDown.SetActive(isTopDown);
        ThirdPerson.SetActive(!isTopDown);
    }
    private void HandleBoatArrived(JettyController jetty, BoatController boat)
    {
        SwitchCamera();
    }
    private void HandleDeparted(JettyController jetty, BoatController boat)
    {
        SwitchCamera();
    }
}
