using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using UnityEngine;
using Unity.Services.Core.Environments;
using UnityEngine.SceneManagement;

public class Relay : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _joinCodeLabel;
    [SerializeField] private TMP_InputField _joinCodeTextField;

    [SerializeField] private GameModeSelector _gameModeSelector;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        var options = new InitializationOptions();

        options.SetEnvironmentName("development");
        await UnityServices.InitializeAsync(options);

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("The player " + AuthenticationService.Instance.PlayerId + " signed in");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        NetworkManager.Singleton.OnClientConnectedCallback += id => {
            Debug.Log("Client " + id.ToString() +  " connected");
            if(NetworkManager.Singleton.IsServer)
            {
                if(NetworkManager.Singleton.ConnectedClients.Count == 2)
                {
                    FinishLoadingMatchClientRpc();
                }
            }
        };
    }

    private void StartLoadingMatch()
    {
        SceneManager.LoadSceneAsync("Match", LoadSceneMode.Additive).completed += scene => {
            if(NetworkManager.Singleton.IsServer)
            {
                _gameModeSelector.Selected.Prepare();
            }
        };
    }

    [ClientRpc]
    private void FinishLoadingMatchClientRpc()
    {
        SceneManager.UnloadSceneAsync("Menu");
    }

    public async void CreateRelay()
    {
        try {
            var allocation = await RelayService.Instance.CreateAllocationAsync(1);
            
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            _joinCodeLabel.text = joinCode;
            
            StartLoadingMatch();

            var relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        } catch(RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }

    public async void JoinRelay()
    {
        try {
            var joinCode = _joinCodeTextField.text;
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            StartLoadingMatch();

            var relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        } catch(RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }
}
