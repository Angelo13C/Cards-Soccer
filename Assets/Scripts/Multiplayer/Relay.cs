using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using UnityEngine;

public class Relay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _joinCodeLabel;
    [SerializeField] private TMP_InputField _joinCodeTextField;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("The player " + AuthenticationService.Instance.PlayerId + " signed in");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        NetworkManager.Singleton.OnClientConnectedCallback += id => {
            Debug.Log("Client " + id.ToString() +  " connected");
        };
    }

    public async void CreateRelay()
    {
        try {
            var allocation = await RelayService.Instance.CreateAllocationAsync(1);
            
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            _joinCodeLabel.text = joinCode;

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

            var relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        } catch(RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }
}
