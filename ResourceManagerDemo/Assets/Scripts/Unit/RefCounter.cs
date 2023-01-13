using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IRefCounter
{
    int RefCount { get; }
    void Retain();
    void Release();
}
public delegate void ZeroRefCallBack();
public class SimpleRC : IRefCounter
{
    public SimpleRC(ZeroRefCallBack zeroRefCallBack)
    {
        m_zeroRefCallBack = zeroRefCallBack;
    }
    private ZeroRefCallBack m_zeroRefCallBack;
    public int RefCount { get; private set; }
    public void Retain()
    {
        RefCount++;
    }
    public void Release()
    {
        RefCount--;
        if (RefCount == 0) { m_zeroRefCallBack(); }
    }
}