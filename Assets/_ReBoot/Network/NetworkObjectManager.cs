using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using _ReBoot.Utilities;

namespace VSPoliceReBoot.Network
{
    public class NetworkObjectManager : SingletonPersistent<NetworkObjectManager>
    {
        public ReadOnlyCollection<NetworkObjectBase> NetworkObjectBases { get; private set; }

        private IList<NetworkObjectBase> _networkObjectBases;

        protected override void Awake()
        {
            base.Awake();
            _networkObjectBases = new List<NetworkObjectBase>();
            NetworkObjectBases = new ReadOnlyCollection<NetworkObjectBase>(_networkObjectBases);
        }

        private void Update()
        {
            foreach(NetworkObjectBase nob in _networkObjectBases)
            {
                nob.OnNetworkPreUpdate();
                if(nob.IsHost) nob.OnHostPreUpdate();
                if(nob.IsOwner) nob.OnOwnerPreUpdate();
                else nob.OnNonOwnerPreUpdate();
                if(nob.IsClient) nob.OnClientPreUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach(NetworkObjectBase nob in _networkObjectBases)
            {
                nob.OnNetworkFixedUpdate();
                if(nob.IsHost) nob.OnHostFixedUpdate();
                if(nob.IsOwner) nob.OnOwnerFixedUpdate();
                else nob.OnNonOwnerFixedUpdate();
                if(nob.IsClient) nob.OnClientFixedUpdate();
            }
        }

        private void LateUpdate()
        {
            foreach(NetworkObjectBase nob in _networkObjectBases)
            {
                nob.OnNetworkLateUpdate();
                if(nob.IsHost) nob.OnHostLateUpdate();
                if(nob.IsOwner) nob.OnOwnerLateUpdate();
                else nob.OnNonOwnerLateUpdate();
                if(nob.IsClient) nob.OnClientLateUpdate();
            }
        }

        public void AddNetworkObject(NetworkObjectBase networkObjectBase)
        {
            //NetworkObjectBaseの初期化処理
            networkObjectBase.OnNetworkStart();
            if(networkObjectBase.IsHost) networkObjectBase.OnHostStart();
            if(networkObjectBase.IsOwner) networkObjectBase.OnOwnerStart();
            else networkObjectBase.OnNonOwnerStart();
            if(networkObjectBase.IsClient) networkObjectBase.OnClientStart();
            
            _networkObjectBases.Add(networkObjectBase);
        }
        
        public void RemoveNetworkObject(NetworkObjectBase networkObjectBase)
        {
            _networkObjectBases.Remove(networkObjectBase);
        }
    }
}