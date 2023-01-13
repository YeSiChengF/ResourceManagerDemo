using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ModuleManager:MonoBehaviour
{
    //模块集合
    private List<ModuleBase> modules = new List<ModuleBase>();
    private void Start()
    {
        AddModule<ResourceModule>();
    }
    private void Update()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            var module = modules[i];
            module.Update();
        }
    }
    private void AddModule<T>() where T : ModuleBase,new()
    {
        T module = new T();
        module.Init();
        modules.Add(module);
    }
}