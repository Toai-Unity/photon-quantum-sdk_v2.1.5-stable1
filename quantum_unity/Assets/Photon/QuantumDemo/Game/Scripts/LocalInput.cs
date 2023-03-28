using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

public class LocalInput : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private void OnEnable()
    {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback)
    {
        Quantum.Input input = new Quantum.Input();

        input.Direction = inputReader.MoveInput.ToFPVector2();
        input.Attack = inputReader.AttackInput;
        input.Jump = false;
        callback.SetInput(input, DeterministicInputFlags.Repeatable);
    }
}
