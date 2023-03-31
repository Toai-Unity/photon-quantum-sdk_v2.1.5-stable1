using Photon.Deterministic;

namespace Quantum
{
    public unsafe class AttackSystem : SystemMainThread
    {
        public override void Update(Frame frame)
        {
            var pigChefFilter = frame.Filter<Transform3D, PlayerLink, Attack>();
            while(pigChefFilter.NextUnsafe(out var pigchef, out var playerTransform, out var player, out var attack))
            {
                Input* input = frame.GetPlayerInput(player->Player);

                if(input->Attack.WasPressed)
                {
                    CheckRaycastCollision(frame, pigchef);
                }
            }

        }

        private bool CheckRaycastCollision(Frame frame, EntityRef pigchef)
        {
            var attackComponent = frame.Get<Attack>(pigchef);
            AttackData data = frame.FindAsset<AttackData>(attackComponent.AttackData.Id);
            FP offsetAttack = data.OffsetAttack;


            Transform3D* bulletTransform = frame.Unsafe.GetPointer<Transform3D>(pigchef);
            Physics3D.HitCollection3D hits = frame.Physics3D.LinecastAll(bulletTransform->Position, bulletTransform->Forward * offsetAttack);
            for(int i=0;i<hits.Count;i++)
            {
                EntityRef entity = hits[i].Entity;
                if(entity != EntityRef.None && frame.Has<Status>(entity))
                {
                    if (frame.Get<Status>(entity).IsDead)
                    {
                        continue;
                    }
                    data.AttackAction(frame, pigchef, entity);
                }
            }

            return false;
        }
    }
}
