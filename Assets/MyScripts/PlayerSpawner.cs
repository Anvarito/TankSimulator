using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField] private List<TankInputInitializer> _listTanks;

    [SerializeField]
    private Transform[] PlayerSpawns;

    private TankInputInitializer playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PlayerConfiguration[] playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            playerPrefab = _listTanks[playerConfigs[i].TankIndex];

            TankInputInitializer player = Instantiate(playerPrefab, PlayerSpawns[i].position, PlayerSpawns[i].rotation);
            player.transform.name += Random.Range(0, 10000);
            //Layout rect
            Vector2 position = new Vector2(0, 0.5f * i);
            Vector2 scale = new Vector2(1, 1.0f / playerConfigs.Length);
            player.GetComponent<CameraViewSetup>().SetupLayoutScreen(position, scale);
            player.GetComponent<CameraViewSetup>().SetScreenAimPointByIndex(playerConfigs[i].PlayerIndex, playerConfigs.Length);

            player.InitializePlayer(playerConfigs[i].Input);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
