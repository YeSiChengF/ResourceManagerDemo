using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    public enum ResState
    {
        Waiting,//等待中(还没开始加载)
        Loading,//加载中
        loaded,//已加载
    }
    public abstract class Res : SimpleRC
    {
        public ResState ResState { get; protected set; }
        public Object Asset { get; protected set; }

        public string Name { get; protected set; }

        public abstract bool LoadSync();

        public abstract void LoadAsync(Action<Res> onLoaded);

        protected abstract void OnReleaseRes();

        protected override void OnZeroRef()
        {
            OnReleaseRes();
        }
    }
}