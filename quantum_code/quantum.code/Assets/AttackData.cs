using Photon.Deterministic;


namespace Quantum
{
    public partial class AttackData
    {
        public FP Damage;
        public FP Duration;
        public FP OffsetAttack;

        public unsafe void AttackAction(Frame frame,EntityRef source, EntityRef targetPlayer)
        {
            if(targetPlayer != EntityRef.None)
            {
                frame.Signals.OnPlayerHit(source, targetPlayer, Damage);
            }
        }
    }
}
