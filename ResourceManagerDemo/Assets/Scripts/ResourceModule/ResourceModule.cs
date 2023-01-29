using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ResourceModule: ModuleBase
{
    #region instance
    private static ResourceModule m_Instance;
    public static ResourceModule Instance
    {
        get { return m_Instance ?? (m_Instance = new ResourceModule()); }
    }
    #endregion
    public readonly int UNLOAD_TIME = 10;
    public readonly int UNLOAD_NUM = 10;
    private float lastUnLoadTime;
    public override void Update()
    {
        
    }
}
