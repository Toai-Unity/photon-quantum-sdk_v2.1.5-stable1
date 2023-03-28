using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe class PlayerSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
    {
        public void OnPlayerDataSet(Frame frame, PlayerRef player)
        {
            RuntimePlayer data = frame.GetPlayerData(player);
            EntityRef pigchefEntity = frame.Create(data.PlayerPrototypeRef);

            if(frame.Unsafe.TryGetPointer<Transform3D>(pigchefEntity, out var transform))
            {
                transform->Position = new FPVector3(player * 2, 3, 0);
            }
            if (frame.Unsafe.TryGetPointer<PlayerLink>(pigchefEntity, out var playerlink)){
                playerlink->Player = player;
            }

        }
    }
}
