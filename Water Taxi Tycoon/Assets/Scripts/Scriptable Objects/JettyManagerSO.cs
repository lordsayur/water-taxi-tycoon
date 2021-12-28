using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Jetty Manager", menuName = "Entity/Jetty Manager")]
public class JettyManagerSO : ScriptableObject
{
    public List<JettyController> jettyControllerList = new();

    int idCounter = 1;

    public bool HasJettyRegistered => jettyControllerList.Count > 0;

    private void OnEnable()
    {
        idCounter = 1;
        jettyControllerList.Clear();
    }
    public int GetNewId
    {
        get { return idCounter++; }
    }
    public void RegisterJetty(JettyController jettyController)
    {
        jettyControllerList.Add(jettyController);
    }
    public (JettyController departureJetty, JettyController destinationJetty) GetDepartureAndDestinationJetty()
    {
        var jettyCount = jettyControllerList.Count;

        var departureJetty = jettyControllerList[Random.Range(0, jettyCount)];

        var destinationJetty = jettyControllerList.Where(j => j != departureJetty).ToList()[Random.Range(0, jettyCount - 1)];

        return (departureJetty, destinationJetty);
    }
}