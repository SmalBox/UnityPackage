using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmalBox.AutoUI;

public class ClickEvent : MonoBehaviour
{
    public void ReturnBase()
    {
        UIManager.instance.ReturnPanel();
    }
    public void ReturnMainMenuOpenSec1()
    {
        UIManager.instance.ReturnPanel(1, "SecondaryMenu1Page");
    }
    public void ReturnMainMenuOpenSec2()
    {
        UIManager.instance.ReturnPanel(1, "SecondaryMenu2Page");
    }
}
