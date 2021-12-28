using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GUIChannelSO", menuName = "Event Channel/GUI Channel")]
public class GUIChannelSO : ScriptableObject
{
    public event UnityAction<JettyController, BoatController, List<CustomerController>> LoadButtonClick = delegate { };
    public void OnLoadButtonClick(JettyController jetty, BoatController boat, List<CustomerController> customer)
    {
        if (LoadButtonClick != null)
        {
            LoadButtonClick.Invoke(jetty, boat, customer);
        }
    }
}
