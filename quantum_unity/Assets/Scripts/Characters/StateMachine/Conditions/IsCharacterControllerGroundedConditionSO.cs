using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Character Controller Grounded")]
public class IsCharacterControllerGroundedConditionSO : StateConditionSO<IsCharacterControllerGroundedCondition> { }

public class IsCharacterControllerGroundedCondition : Condition
{
	//private CharacterController _characterController;
	private Protagonist _protagonist;

	public override void Awake(StateMachine stateMachine)
	{
        //_characterController = stateMachine.GetComponent<CharacterController>();
        _protagonist = stateMachine.GetComponent<Protagonist>();

    }

	protected override bool Statement()
	{
		//return _protagonist.isGrounded;
		//return _characterController.isGrounded;
		return true;
    }
}
