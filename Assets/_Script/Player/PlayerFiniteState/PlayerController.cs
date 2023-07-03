using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour , INetworkSerializable
{
    #region State Variables
    public PlayerStateMachine stateMachine { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerShotState ShotState { get; private set; }
    public PlayerIsGameEndState IsGameEndState { get; private set; }
    #endregion

    #region Component
    public Rigidbody myRB { get; private set; }
    public CapsuleCollider myColl { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler inputController { get; private set; }
    public Core Core { get; private set; }

    public Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
    public Rotation Rotation { get => rotation ?? Core.GetCoreComponent(ref rotation); }
    public Damage Damage { get => damage ?? Core.GetCoreComponent(ref damage); }
    public States States { get => states ?? Core.GetCoreComponent(ref states); }

    private Movement movement;
    private Rotation rotation;
    private Damage damage;
    private States states;
    #endregion

    #region Other Variables
    [SerializeField]
    private string myControllPlayerLayer;
    [SerializeField]
    private string playerBlindLayer;
    [SerializeField]
    private string playerShowLayer;
    [SerializeField]
    private GameObject spriteObject;
    [SerializeField]
    private GameObject playerViewObject;

    public static PlayerController Owner = null;

    private Vector3 workspace;

    private Plane plane = new Plane();
    private float distance = 0;

    public bool isHaveMainWeapon;
    public int nowHaveGadget = 0;

    private bool isStackHP = false;
    private int stackHP = 0;

    public enum Team
    {
        RedTeam,
        BlueTeam,
        None
    }

    public Team nowTeam = Team.None;

    public int nowHP;
    private bool isGameEnd;
    private bool isGameNow;
    #endregion

    #region Network
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        //serializer.SerializeValue(ref workspace);
    }
    #endregion

    #region Unity Callback Function
    private void Awake()
    {
    }

    private void Start()
    {
        //共通処理
        Core = GetComponentInChildren<Core>();
        isGameEnd = false;

        if (!this.IsOwner)
        {
            gameObject.layer = LayerMask.NameToLayer(playerBlindLayer);
            return;
        }

        Owner = this;
        gameObject.layer = LayerMask.NameToLayer(myControllPlayerLayer);
        SetPlayerNameServerRpc(this.IsServer);

        stateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, stateMachine, playerData, "move");
        ShotState = new PlayerShotState(this, stateMachine, playerData, "shot");
        IsGameEndState = new PlayerIsGameEndState(this, stateMachine, playerData, "isGameEnd");

        myRB = GetComponent<Rigidbody>();
        myColl = GetComponent<CapsuleCollider>();
        inputController = GetComponent<PlayerInputHandler>();
        Anim = GetComponent<Animator>();

        //SetPlayerServerRpc();
        SetPlayerPositionServerRpc(GameManagerControll.Singleton.GetLobbyPosition());
    }

    private void Update()
    {
        //共通処理
        switch (nowTeam)
        {
            case Team.RedTeam:
                SetPlayerSprite(GameManagerControll.Singleton?.GetPlayer1Sprite());
                break;

            case Team.BlueTeam:
                SetPlayerSprite(GameManagerControll.Singleton?.GetPlayer2Sprite());
                break;

            default:
                break;
        }

        if (gameObject.layer == LayerMask.NameToLayer(playerBlindLayer) || gameObject.layer == LayerMask.NameToLayer(playerShowLayer))
        {
            spriteObject.layer = gameObject.layer;
            playerViewObject.SetActive(false);
        }

        if (Damage.isDamage)
        {
            Damage?.UseDamageFlg();
            //States?.addDamage(Damage.currentDamage);
            //SetStatesServerRpc(States.nowHP);
            //PlayerAddDamageClientRpc(Damage.currentDamage);
            PlayerAddDamageServerRpc(Damage.currentDamage);
            
            SetAllPlayerInitLocationServerRpc();
        }

        if (!States.setInitFunction)
        {
            States?.InitState(playerData.maxHP);
            //SetStatesServerRpc(States.nowHP);
            SetNowHpClientRpc(States.nowHP);
        }

        if(isStackHP && Core != null)
        {
            isStackHP = false;
            States?.SetNowHp(stackHP);
            this.nowHP = States.nowHP;
        }

        //デバッグ用
        if (States != null)
        {
            Debug.Log(gameObject.name + "のHP : " + States.nowHP);
        }

        if (!this.IsOwner)
        {
            //デバッグ用
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Damage?.addDamage(1);
            }
            return;
        }
        //オーナー処理
        if (stateMachine.CurrentState == null)
            stateMachine.Initialize(IdleState);

        //SetNowHpClientRpc(States.nowHP);

        if (isGameEnd && stateMachine.CurrentState != IsGameEndState)
        {
            stateMachine.ChangeState(IsGameEndState);
        }

        Core.LogicUpdate();
        stateMachine.CurrentState.LogicUpdate();

        //�}�E�X�̈ʒu�����ʂɂȂ�悤�Ƀv���C���[����]�����鏈��
        var ray = Camera.main.ScreenPointToRay(inputController.MousePosition);
        plane.SetNormalAndPosition(Vector3.up, transform.localPosition);
        if (plane.Raycast(ray, out distance))
        {
            Vector3 lookpoint = ray.GetPoint(distance);
            Rotation.SetRotation(lookpoint);
        }

        //プレイヤーが死亡したらサーバー側のゲームマネージャーに伝える
        if(States != null)
        {
            if(States.isDead)
            {
                SetPlayerDeadServerRpc();
            }

            SetNowHpClientRpc(States.nowHP);
        }

        //デバッグ用
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Damage?.addDamage(1);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        //Debug.DrawRay(pos, gameObject.transform.forward.normalized * playerData.meleeDistance, Color.blue, 0.5f);
    }

    private void FixedUpdate()
    {
        if (!this.IsOwner)
            return;

        if(stateMachine.CurrentState != null)
            stateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Other Function
    private void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishTrigger() => stateMachine.CurrentState.AnimationFinishTrigger();

    public bool CheckFrontObject(string tag, out GameObject gameObject, float distance)
    {
        RaycastHit hitObject;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        if (Physics.Raycast(pos, transform.forward, out hitObject, distance))
        {
            if (hitObject.transform.tag == tag)
            {
                gameObject = hitObject.transform.gameObject;
                return true;
            }
        }

        gameObject = null;
        return false;
    }

    public bool CheckFrontObject(string tag, float distance)
    {
        RaycastHit hitObject;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        if (Physics.Raycast(pos, transform.forward, out hitObject, distance))
        {
            if (hitObject.transform.tag == tag)
            {
                return true;
            }
        }
        return false;
    }
    public void SetPlayerSprite(Sprite sprite)
    {
        SpriteRenderer playerSprite = spriteObject.GetComponent<SpriteRenderer>();
        playerSprite.sprite = sprite;
    }

    public Team GetNowTeam()
    {
        return nowTeam;
    }

    #region ServerRpc
    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    public void SetPlayerServerRpc()
    {
        GameManagerControll.Singleton?.PlayerSet(this, transform);
    }

    [Unity.Netcode.ServerRpc]
    private void SetPlayerNameServerRpc(bool isServer)
    {
        if (isServer)
            this.gameObject.name = "Player(Host)";
        else
            this.gameObject.name = "Player(Client)";
    }

    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    private void SetStatesServerRpc(int nowHP)
    {
        GameManagerControll.Singleton?.PlayerSetHP(this, nowHP);
    }

    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    private void SetAllPlayerInitLocationServerRpc()
    {
        GameManagerControll.Singleton?.SetAllPlayerInitLocation();
    }

    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    public void RestartMessageServerRpc()
    {
        GameManagerControll.Singleton?.RestartMessage(this);
    }

    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    public void SetPlayerDeadServerRpc()
    {
        GameManagerControll.Singleton?.SetPlayerDead(this);
    }

    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    public void PlayerAddDamageServerRpc(int damage)
    {
        PlayerAddDamageClientRpc(damage);
    }

    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    public void SetNowHpServerRpc(int nowHp)
    {
        SetNowHpClientRpc(nowHp);
    }

    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    public void AddTeamCountServerRpc(Team nowTeam,int addCount)
    {
        GameManagerControll.Singleton?.AddTeamCount(nowTeam, addCount);
    }

    [Unity.Netcode.ServerRpc(RequireOwnership = false)]
    public void SetPlayerPositionServerRpc(Vector3 pos)
    {
        SetPositionClientRpc(pos);
    }
    #endregion
    #region ClientRpc
    [Unity.Netcode.ClientRpc]
    public void SetPositionClientRpc(Vector3 pos)
    {
        transform.position = pos;
    }

    /*
    [Unity.Netcode.ClientRpc]
    public void SetPlayerSpriteClientRpc(Sprite sprite)
    {
        SetPlayerSprite(sprite);
    }
    */

    [Unity.Netcode.ClientRpc]
    public void SetTeamClientRpc(Team team)
    {
        nowTeam = team;
    }


    [Unity.Netcode.ClientRpc]
    public void SetNowHpClientRpc(int nowHp)
    {
        if (Core != null)
        {
            States?.SetNowHp(nowHp);
            this.nowHP = States.nowHP;
        }
        else
        {
            isStackHP = true;
            stackHP = nowHp;
        }
    }

    [Unity.Netcode.ClientRpc]
    public void PlayerGameEndClientRpc()
    {
        isGameEnd = true;
    }

    [Unity.Netcode.ClientRpc]
    public void RestartClientRpc()
    {
        if (IsOwner)
        {
            stateMachine.ChangeState(IdleState);
            States?.InitState(playerData.maxHP);
            isGameEnd = false;

            SetPlayerServerRpc();
        }
    }    
    
    [Unity.Netcode.ClientRpc]
    public void StartClientRpc()
    {
        if (IsOwner)
        {
            stateMachine.ChangeState(IdleState);
            States?.InitState(playerData.maxHP);
            isGameEnd = false;
        }
    }

    [Unity.Netcode.ClientRpc]
    public void PlayerAddDamageClientRpc(int damage)
    {
        if (IsOwner)
        {
            States?.addDamage(damage);
            //SetNowHpClientRpc(States.nowHP);
            SetNowHpServerRpc(States.nowHP);
        }
    }

    [Unity.Netcode.ClientRpc]
    public void SetIsGameNowClientRpc(bool flg)
    {
        isGameNow = flg;
        Damage?.SetCanDamage(flg);
    }
    #endregion


    public string GetPlayerBlindLayerName() => playerBlindLayer;
    public string GetPlayerShowLayerName() => playerShowLayer;
    #endregion
}
