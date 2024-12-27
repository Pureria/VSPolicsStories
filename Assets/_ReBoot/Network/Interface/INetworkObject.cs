using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _ReBoot.Network
{
    public interface INetworkObject
    {
        public abstract void OnNetworkStart();
        public abstract void OnNetworkPreUpdate();
        public abstract void OnNetworkFixedUpdate();
        public abstract void OnNetworkLateUpdate();
    }

    public interface INetworkHostFunction: INetworkObject
    {
        public abstract void OnHostStart();
        public abstract void OnHostPreUpdate();
        public abstract void OnHostFixedUpdate();
        public abstract void OnHostLateUpdate();
    }

    public interface INetworkClientFunction : INetworkObject
    {
        public abstract void OnClientStart();
        public abstract void OnClientPreUpdate();
        public abstract void OnClientFixedUpdate();
        public abstract void OnClientLateUpdate();
    }
    
    public interface INetworkOwnerFunction : INetworkObject
    {
        public abstract void OnOwnerStart();
        public abstract void OnOwnerPreUpdate();
        public abstract void OnOwnerFixedUpdate();
        public abstract void OnOwnerLateUpdate();
    }

}
