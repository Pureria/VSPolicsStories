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
            //UI���B������
            hideUI?.SetActive(false);
        }
        else if(!isConnect && isHideUI)
        {
            //UI��\�����鏈��
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
        Debug.Log("�z�X�g�𗧂Ă܂����B");
        Debug.Log("�R�[�h�@�F�@" + code);
        isConnect = true;
    }

    public void FailedHost() 
    {
        Debug.Log("�z�X�g�𗧂Ă�̂Ɏ��s���܂����B");
        isConnect = false;
    }

    public void SuccessClient()
    {
        isConnect = true;
        codeText.text = "Code: " + fieldCode;
        Debug.Log("�T�[�o�[�ɐڑ����܂���");
    }

    public void FailedClient()
    {
        isConnect = false;
        Debug.Log("�T�[�o�[�ɐڑ��ł��܂���ł����B");
    }
}