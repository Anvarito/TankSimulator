using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
public class PlayerSpawner : MonoBehaviour
{

    [SerializeField] private List<PlayerInputInitializer> _listTanks;

    [SerializeField]
    private Transform[] PlayerSpawns;

    private PlayerInputInitializer playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PlayerConfiguration[] playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            playerPrefab = _listTanks[playerConfigs[i].TankIndex];

            PlayerInputInitializer player = Instantiate(playerPrefab, PlayerSpawns[i].position, PlayerSpawns[i].rotation);
            player.transform.name += Random.Range(0, 10000);

            //Layout rect
            player.GetComponent<CameraViewSetup>().SetupLayoutScreen(playerConfigs[i].PlayerIndex, playerConfigs.Length);
            player.GetComponent<CameraViewSetup>().SetScreenAimPoint(playerConfigs[i].PlayerIndex, playerConfigs.Length);

            player.SetPlayerInput(playerConfigs[i].Input);
            player.Initialize();

            //player.GetComponentInChildren<RecivierUIManager>().Initialize();
        }
    }
}
