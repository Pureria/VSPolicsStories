using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.VersionControl.Asset;

public class PlayerController : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine stateMachine { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    #endregion

    #region Component
    public Rigidbody myRB { get; private set; }
    public CapsuleCollider myColl { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler inputController { get; private set; }
    public Core Core { get; private set; }

    public Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
    public Rotation Rotation { get => rotation ?? Core.GetCoreComponent(ref rotation); }

    private Movement movement;
    private Rotation rotation;
    //public Inventory Inventory { get; private set; }
    //public GameObject mainWeapon { get; private set; }
    //public Gun gun { get; private set; }
    //public FunSearch search { get; private set; }
    #endregion

    #region Other Variables
    private Vector3 workspace;

    private Plane plane = new Plane();
    private float distance = 0;

    public bool isHaveMainWeapon;
    public int nowHaveGadget = 0;
    #endregion

    #region Unity Callback Function
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        stateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, stateMachine, playerData, "move");

    }

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myColl = GetComponent<CapsuleCollider>();
        inputController = GetComponent<PlayerInputHandler>();
        Anim = GetComponent<Animator>();

        stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
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
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        //Debug.DrawRay(pos, gameObject.transform.forward.normalized * playerData.meleeDistance, Color.blue, 0.5f);
    }

    private void FixedUpdate()
    {
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
    #endregion
}
