using DG.Tweening.Core.Easing;
using Photon.Deterministic;
using Quantum;
using System;
using UnityEngine;

/// <summary>
/// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
/// </summary>
public class Protagonist : QuantumCallbacks
{
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private TransformAnchor _gameplayCameraTransform = default;
    private QuantumGame _game = null;

    private Vector2 _inputVector;
	private float _previousSpeed;
	public Transform checkGroundTransform;
	[HideInInspector] public bool isGrounded = false;

	//These fields are read and manipulated by the StateMachine actions
	[NonSerialized] public bool jumpInput;
	[NonSerialized] public bool isMoving;
	[NonSerialized] public bool extraActionInput;
	[NonSerialized] public bool attackInput;
	[NonSerialized] public Vector3 movementInput; //Initial input coming from the Protagonist script
	[NonSerialized] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions
	[NonSerialized] public ControllerColliderHit lastHit;
	[NonSerialized] public bool isRunning; // Used when using the keyboard to run, brings the normalised speed to 1

	public const float GRAVITY_MULTIPLIER = 5f;
	public const float MAX_FALL_SPEED = -50f;
	public const float MAX_RISE_SPEED = 100f;
	public const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
	public const float GRAVITY_DIVIDER = .6f;
	public const float AIR_RESISTANCE = 5f;

    private EntityView entityView;

    private void Start()
    {
        QuantumEvent.Subscribe<EventOnPlayerJump>(this, OnJump);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		lastHit = hit;
	}

    //Adds listeners for events being triggered in the InputReader script
    protected override void OnEnable()
	{
		_inputReader.JumpEvent += OnJumpInitiated;
		_inputReader.JumpCanceledEvent += OnJumpCanceled;
		_inputReader.MoveEvent += OnMove;
		_inputReader.StartedRunning += OnStartedRunning;
		_inputReader.StoppedRunning += OnStoppedRunning;
		_inputReader.AttackEvent += OnStartedAttack;
        //...

        entityView = GetComponent<EntityView>();

    }

	//Removes all listeners to the events coming from the InputReader script
	protected override void OnDisable()
	{
		_inputReader.JumpEvent -= OnJumpInitiated;
		_inputReader.JumpCanceledEvent -= OnJumpCanceled;
		_inputReader.MoveEvent -= OnMove;
		_inputReader.StartedRunning -= OnStartedRunning;
		_inputReader.StoppedRunning -= OnStoppedRunning;
		_inputReader.AttackEvent -= OnStartedAttack;
		//...
	}

	private void Update()
	{
		RecalculateMovement();
		GroundCheck();
		isMoving = IsMoving();
    }
    private bool IsMoving()
	{
        var f = QuantumRunner.Default.Game.Frames.Verified;
        var kkc = f.Get<CharacterController3D>(entityView.EntityRef);
		return kkc.Velocity.Magnitude > FP.FromFloat_UNSAFE(0.01f);
    }

	private bool IsAttacking()
	{
        var f = QuantumRunner.Default.Game.Frames.Verified;
		//var attackComp = f.Get<Attack>(entityView.EntityRef);
        return false;
	}

	private void RecalculateMovement()
	{
		float targetSpeed;
		Vector3 adjustedMovement;

		if (_gameplayCameraTransform.isSet)
		{
			//Get the two axes from the camera and flatten them on the XZ plane
			Vector3 cameraForward = _gameplayCameraTransform.Value.forward;
			cameraForward.y = 0f;
			Vector3 cameraRight = _gameplayCameraTransform.Value.right;
			cameraRight.y = 0f;

			//Use the two axes, modulated by the corresponding inputs, and construct the final vector
			adjustedMovement = cameraRight.normalized * _inputVector.x +
				cameraForward.normalized * _inputVector.y;
		}
		else
		{
			//No CameraManager exists in the scene, so the input is just used absolute in world-space
			Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
			adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);
		}

		//Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
		if (_inputVector.sqrMagnitude == 0f)
			adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

		//Accelerate/decelerate
		targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
		if (targetSpeed > 0f)
		{
			// This is used to set the speed to the maximum if holding the Shift key,
			// to allow keyboard players to "run"
			if (isRunning)
				targetSpeed = 1f;

			if (attackInput)
				targetSpeed = .05f;
		}
		targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4f);

		//movementInput = adjustedMovement.normalized * targetSpeed;
		movementInput = new Vector3(_inputReader.MoveInput.x, 0, _inputReader.MoveInput.y).normalized;

		_previousSpeed = targetSpeed;
	}

	//---- EVENT LISTENERS ----

	private void OnMove(Vector2 movement)
	{

		_inputVector = movement;
	}

	private void OnJumpInitiated()
	{
		jumpInput = true;
	}

	private void OnJumpCanceled()
	{
		jumpInput = false;
	}

	private void OnStoppedRunning() => isRunning = false;

	private void OnStartedRunning() => isRunning = true;


	private void OnStartedAttack() => attackInput = true;

	// Triggered from Animation Event
	public void ConsumeAttackInput() => attackInput = false;

    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(checkGroundTransform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

	private void OnJump(EventOnPlayerJump obj)
	{
        if (entityView.EntityRef.Equals(obj.Player))
		{
			jumpInput = true;

        }

    }
}
