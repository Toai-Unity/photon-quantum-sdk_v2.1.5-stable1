using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum
{
    partial class RuntimePlayer
    {
        public AssetRefEntityPrototype PlayerPrototypeRef;
        partial void SerializeUserData(BitStream stream)
        {
            // implementation
            stream.Serialize(ref PlayerPrototypeRef);
        }
    }
}
