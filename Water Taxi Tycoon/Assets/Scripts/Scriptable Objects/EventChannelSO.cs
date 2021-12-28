using UnityEngine;

[CreateAssetMenu(fileName = "EventChannelSO", menuName = "Event Channel/Event Channel")]
public class EventChannelSO : ScriptableObject
{
    public CustomerChannelSO CustomerChannel;
    public TriggerChannelSO TriggerChannel;
    public GUIChannelSO GUIChannel;
    public SeatChannelSO SeatChannel;
}
