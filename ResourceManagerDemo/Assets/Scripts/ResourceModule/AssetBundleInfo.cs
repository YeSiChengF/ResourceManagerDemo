using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleInfo
{
    private AssetBundle m_AssetBundle;   //AB包引用
    public AssetBundle AssetBundle { get { return m_AssetBundle; } }
    public int ReferencedCount { 
        get { return m_ReferencedCount; } 
        set {
            m_ReferencedCount = value;
            if (m_ReferencedCount<= 0)
            {
                IsUnLoadFlag = true;
            }
            else
            {
                IsUnLoadFlag = false;
            }
        }
    }
    public bool IsUnLoadFlag { get; private set; }
    private int m_ReferencedCount;           //引用计数
    public AssetBundleInfo(AssetBundle assetBundle)
    {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = 1;
    }
}
