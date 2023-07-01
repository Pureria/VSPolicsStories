using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostClientButton : MonoBehaviour
{
    [SerializeField]
    private InputField codeInputField;

    [SerializeField]
    private TextMeshProUGUI codeText;

    private void Start()
    {
        codeInputField.text = "";
    }

    public void StartHost()
    {
        //Unity.Netcode.NetworkManager.Singleton.StartHost();
        UTJ.NetcodeGameObjectSample.RelayServiceUtility.StartUnityRelayHost(SuccessHost, FailedHost);
    }

    public void StartClient()
    {
        //Unity.Netcode.NetworkManager.Singleton.StartClient();        string code = codeInputField.text;
        code.Substring(0, 6);
        UTJ.NetcodeGameObjectSample.RelayServiceUtility.StartClientUnityRelayModeAsync(code);
    }

    public void SuccessHost()
    {
        string code = UTJ.NetcodeGameObjectSample.RelayServiceUtility.HostJoinCode;
        codeText.text = "Code: " + code;
        Debug.Log("ホストを立てました。");
        Debug.Log("コード　：　" + code);
    }

    public void FailedHost() 
    {
        Debug.Log("ホストを立てるのに失敗しました。");
    }
}