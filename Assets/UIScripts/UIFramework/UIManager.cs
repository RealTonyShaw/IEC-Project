﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //单例模式
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }
    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    private UIManager()
    {
        ParseJsonInfo();
    }

    //panelStack为实现面板切换的数据结构
    //panelDict存储每个面板
    //panelPathDict存储每个面板的路径
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();
    private Dictionary<PanelType, BasePanel> panelDict = new Dictionary<PanelType, BasePanel>();
    private Dictionary<PanelType, string> panelPathDict = new Dictionary<PanelType, string>();

    /// <summary>
    /// 将面板入栈
    /// </summary>
    /// <param name="panelType"，入栈面板的类型></param>
    /// <returns 返回入栈的面板></returns>
    public BasePanel PushPanel(PanelType panelType)
    {
        if (panelStack.Count != 0)
        {
            panelStack.Peek().OnPause();
        }
        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
        return panel;
    }

    /// <summary>
    /// 将面板从栈中弹出
    /// </summary>
    /// <param name="panelType"，出栈面板的类型></param>
    /// <returns 返回出栈的面板></returns>
    public BasePanel PopPanel(PanelType panelType)
    {
        if (panelStack.Count == 0)
        {
            return null;
        }
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
        if (panelStack.Count == 0)
        {
            return null;
        }
        panelStack.Peek().OnResume();
        return topPanel;
    }

    /// <summary>
    /// 获得一个面板
    /// </summary>
    /// <param name="panelType"，要获得的面板类型></param>
    /// <returns 获得的面板></returns>
    private BasePanel GetPanel(PanelType panelType)
    {
        BasePanel panel;
        if (!panelDict.TryGetValue(panelType, out panel) || panel == null)
        {
            string path;
            if (!panelPathDict.TryGetValue(panelType, out path))
            {
                Debug.Log("error!");
            }
            GameObject insPanel = GameObject.Instantiate(Resources.Load<GameObject>(path));
            insPanel.transform.SetParent(CanvasTransform, false);
            if (panel == null)
            {
                panel = insPanel.GetComponent<BasePanel>();
                panelDict[panelType] = panel;
            }
            else
            {
                panel = insPanel.GetComponent<BasePanel>();
                panelDict.Add(panelType, panel);
            }
        }
        return panel;
    }

    /// <summary>
    /// 将json信息转化为panelPathDict里面的信息
    /// </summary>
    private void ParseJsonInfo()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Json/PanelInfo");
        PanelInfoRoot panelInfoRoot = JsonUtility.FromJson<PanelInfoRoot>(textAsset.text);
        foreach (PanelInfoItem panelInfoItem in panelInfoRoot.panelInfo)
        {
            panelPathDict.Add(panelInfoItem.panelType, panelInfoItem.panelPath);
        }
    }

    /// <summary>
    /// 清空panelStack里面的信息
    /// </summary>
    public void UIStackClean()
    {
        panelStack.Clear();
    }
}
