using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MyActionEvent
{
    private event Action Evt;

    public void AddListener(Action listener)
    {
        Evt += listener;
    }

    public void RemoveListener(Action listener)
    {
        Evt -= listener;
    }

    public void Trigger()
    {
        Evt?.Invoke();
    }
}