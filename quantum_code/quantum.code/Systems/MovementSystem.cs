

using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
    public unsafe class MovementSystem : SystemMainThread
    {
        public override void Update(Frame frame)
        {
            var pigChefFilter = frame.Filter<Transform3D, PlayerLink, Status, CharacterController3D>();
            while (pigChefFilter.NextUnsafe(out var pigchef, out var transform, out var player, out var status, out var kkc))
            {
                Input* input = frame.GetPlayerInput(player->Player);

                FPVector3 direction = new FPVector3(input->Direction.X, FP._0, input->Direction.Y);

                // Move
                kkc->Move(frame, pigchef, direction);

                // Rotate
                if(direction != FPVector3.Zero)
                {
                    FPQuaternion lookTo = FPQuaternion.LookRotation(direction, transform->Up);
                    FPVector3 rotation = FPQuaternion.Slerp(transform->Rotation, lookTo, frame.DeltaTime * FP.FromFloat_UNSAFE(1500)).AsEuler;
                    transform->Rotation = FPQuaternion.Euler(rotation);
                }

                if(input->Jump.WasPressed)
                {
                    if (kkc->Grounded)
                    {
                        kkc->Jump(frame as FrameBase);
                        frame.Events.OnPlayerJump(pigchef);
                    }
                }
            }
        }
    }
}
