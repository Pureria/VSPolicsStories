using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostClientButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI codeInputField;

    [SerializeField]
    private TextMeshProUGUI codeText;

    public void StartHost()
    {
        //Unity.Netcode.NetworkManager.Singleton.StartHost();
        UTJ.NetcodeGameObjectSample.RelayServiceUtility.StartUnityRelayHost(SuccessHost, FailedHost);
    }

    public void StartClient()
    {
        //Unity.Netcode.NetworkManager.Singleton.StartClient();
        //TODO::コードがうまく取得できていない
        string code = codeInputField.text;
        code.Substring(0, 6);
        UTJ.NetcodeGameObjectSample.RelayServiceUtility.StartClientUnityRelayModeAsync(code);
    }

    public void SuccessHost()
    {
        string code = UTJ.NetcodeGameObjectSample.RelayServiceUtility.HostJoinCode;
        codeText.text += " " + code;
        Debug.Log("ホストを立てました。");
        Debug.Log("コード　：　" + code);
    }

    public void FailedHost() 
    {
        Debug.Log("ホストを立てるのに失敗しました。");
    }
}