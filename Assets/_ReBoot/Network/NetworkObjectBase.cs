using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace _ReBoot.Network
{
    public partial class NetworkObjectBase : NetworkBehaviour , INetworkHostFunction, INetworkOwnerFunction, INetworkClientFunction
    {
        //すべてのクラスで共通する処理
        public virtual void OnNetworkStart() { }
        public virtual void OnNetworkPreUpdate() { }
        public virtual void OnNetworkFixedUpdate() { }
        public virtual void OnNetworkLateUpdate() { }
        
        //ホストだと呼ばれる処理
        public virtual void OnHostStart() { }
        public virtual void OnHostPreUpdate() { }
        public virtual void OnHostFixedUpdate() { }
        public virtual void OnHostLateUpdate() { }

        //オーナーだと呼ばれる処理
        public virtual void OnOwnerStart() { }
        public virtual void OnOwnerPreUpdate() { }
        public virtual void OnOwnerFixedUpdate() { }
        public virtual void OnOwnerLateUpdate() { }

        //クライアントだと呼ばれる処理
        public virtual void OnClientStart() { }
        public virtual void OnClientPreUpdate() { }
        public virtual void OnClientFixedUpdate() { }
        public virtual void OnClientLateUpdate() { }

        private void OnEnable()
        {
            //TODO::ネットワークマネージャーに登録
        }
        
        private void OnDisable()
        {
            //TODO::ネットワークマネージャの登録解除
        }
    }
}
