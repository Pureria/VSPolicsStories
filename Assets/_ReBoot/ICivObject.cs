using UnityEngine;

namespace VSPoliceReBoot.Object
{
    /// <summary>
    /// ゲーム内のオブジェクトのインターフェース
    /// 視界に入った時に映るようにする
    /// </summary>
    public interface ICivObject
    {
        public void OnEnterCIV();
        public void OnExitCIV();
    }
}