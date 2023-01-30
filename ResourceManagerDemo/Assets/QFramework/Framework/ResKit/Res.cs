using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    public abstract class Res : SimpleRC
    {
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