using System;
using System.Collections;
using System.Collections.Generic;
using _ReBoot.Utilities;
using Unity.Netcode;
using UnityEngine;

namespace VSPoliceReBoot.Network
{
    public partial class NetworkObjectBase : NetworkBehaviour , INetworkHostFunction, INetworkNonOwnerFunction, INetworkOwnerFunction, INetworkClientFunction
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
        
        //オーナーではないとき呼ばれる処理
        public virtual void OnNonOwnerStart() { }
        public virtual void OnNonOwnerPreUpdate() { }
        public virtual void OnNonOwnerFixedUpdate() { }
        public virtual void OnNonOwnerLateUpdate() { }

        //クライアントだと呼ばれる処理
        public virtual void OnClientStart() { }
        public virtual void OnClientPreUpdate() { }
        public virtual void OnClientFixedUpdate() { }
        public virtual void OnClientLateUpdate() { }

        protected virtual void Start()
        {
            //NetworkObjectManagerに登録
            if(SingletonPersistent<NetworkObjectManager>.Instance != null)
                SingletonPersistent<NetworkObjectManager>.Instance.AddNetworkObject(this);
        }
        
        protected virtual void OnDisable()
        {
            //NetworkObjectManagerから削除
            if(SingletonPersistent<NetworkObjectManager>.Instance != null)
                SingletonPersistent<NetworkObjectManager>.Instance.RemoveNetworkObject(this);
        }
    }
}
