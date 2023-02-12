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
        private ResState mResState;

        public ResState ResState
        {
            get { return mResState; }
            protected set
            {
                mResState = value;
                if (mResState == ResState.loaded)
                {
                    mOnLoadedEvent?.Invoke(this);
                }
            }
        }

        public Object Asset { get; protected set; }

        public string Name { get; protected set; }

        public abstract bool LoadSync();

        public abstract void LoadAsync();

        protected abstract void OnReleaseRes();

        protected override void OnZeroRef()
        {
            OnReleaseRes();
        }

        private event Action<Res> mOnLoadedEvent;

        public void RegiesterOnLoadedEvent(Action<Res> loadedEvent)
        {
            mOnLoadedEvent += loadedEvent;
        }
        public void UnRegiesterOnLoadedEvent(Action<Res> loadedEvent)
        {
            mOnLoadedEvent -= loadedEvent;
        }
    }
}