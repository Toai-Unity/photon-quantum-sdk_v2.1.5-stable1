using Photon.Deterministic;


namespace Quantum
{
    public unsafe class StatusSystem : SystemMainThread, ISignalOnPlayerHit
    {
        public void OnPlayerHit(Frame frame, EntityRef source, EntityRef target, FP damage)
        {
            
        }

        public override void Update(Frame frame)
        {
        }

        private void TakeDamage(Frame frame, EntityRef beAttacked, FP damage)
        {
            Status* playerStatus = frame.Unsafe.GetPointer<Status>(beAttacked);

            playerStatus->CurrentHealth -= damage;
            frame.Events.OnPlayerTakeDamage(beAttacked, damage);

            if (playerStatus->CurrentHealth <= 0)
            {
                // Kill player
            }
        }

    }
}
