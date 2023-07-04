using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostClientButton : MonoBehaviour
{
    [SerializeField]
    private InputField codeInputField;

    [SerializeField]
    private TextMeshProUGUI codeText;

    [SerializeField]
    private GameObject hideUI;

    private bool isConnect;
    private bool isHideUI;

    private string fieldCode;

    private void Start()
    {
        codeInputField.text = "";
        fieldCode = "";
        isConnect = false;
        isHideUI = false;
    }

    private void Update()
    {
        if(isConnect && !isHideUI)
        {
            //UIを隠す処理
            hideUI?.SetActive(false);
        }
        else if(!isConnect && isHideUI)
        {
            //UIを表示する処理
            hideUI?.SetActive(true);
        }
    }

    public void StartHost()
    {
        //Unity.Netcode.NetworkManager.Singleton.StartHost();
        UTJ.NetcodeGameObjectSample.RelayServiceUtility.StartUnityRelayHost(SuccessHost, FailedHost);
    }

    public void StartClient()
    {
        //Unity.Netcode.NetworkManager.Singleton.StartClient();
        string code = codeInputField.text;
        code.Substring(0, 6);
        fieldCode = code;
        UTJ.NetcodeGameObjectSample.RelayServiceUtility.StartClientUnityRelayModeAsync(code, SuccessClient, FailedClient);
    }

    public void SuccessHost()
    {
        string code = UTJ.NetcodeGameObjectSample.RelayServiceUtility.HostJoinCode;
        codeText.text = "Code: " + code;
        Debug.Log("ホストを立てました。");
        Debug.Log("コード　：　" + code);
        isConnect = true;
    }

    public void FailedHost() 
    {
        Debug.Log("ホストを立てるのに失敗しました。");
        isConnect = false;
    }

    public void SuccessClient()
    {
        isConnect = true;
        codeText.text = "Code: " + fieldCode;
        Debug.Log("サーバーに接続しました");
    }

    public void FailedClient()
    {
        isConnect = false;
        Debug.Log("サーバーに接続できませんでした。");
    }
}