using UnityEngine;
using ChobiAssets.PTM;

public class KillPointCounter : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {
        DamageReceiversManager[] all = GetComponents<DamageReceiversManager>();
        foreach (var a in all)
        {
            a.OnTankDestroyed.AddListener(TankDestroyed);
        }
    }

    private void TankDestroyed(ID_Settings_CS tankId, ID_Settings_CS killerID)
    {
        if (killerID != null)
            print(killerID.transform.name + " kill another one tank");
        else
            print(killerID.transform.name + " self destroyed!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
