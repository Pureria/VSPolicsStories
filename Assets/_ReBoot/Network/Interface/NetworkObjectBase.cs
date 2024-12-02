using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace _ReBoot.Network
{
    public partial class NetworkObjectBase : NetworkBehaviour , INetworkHostFunction, INetworkOwnerFunction, INetworkClientFunction
    {
        public virtual void OnNetworkStart() { }
        public virtual void OnNetworkPreUpdate() { }
        public void OnNetworkFixedUpdate() { }
        public void OnNetworkLateUpdate() { }
        
        public void OnHostStart() { }
        public void OnHostPreUpdate() { }
        public void OnHostFixedUpdate() { }
        public void OnHostLateUpdate() { }

        public void OnOwnerStart() { }
        public void OnOwnerPreUpdate() { }
        public void OnOwnerFixedUpdate() { }
        public void OnOwnerLateUpdate() { }

        public void OnClientStart() { }
        public void OnClientPreUpdate() { }
        public void OnClientFixedUpdate() { }
        public void OnClientLateUpdate() { }
    }
}
