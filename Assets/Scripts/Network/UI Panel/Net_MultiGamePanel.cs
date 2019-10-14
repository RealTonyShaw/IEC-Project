using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MultiGamePanel
{
    public void OnSignInClick()
    {
        if (userName.text == null || userName.text.Length < 1)
        {
            return;
        }
        if (password.text == null || password.text.Length < 6)
        {
            return;
        }
        DataSync.Login(userName.text, password.text);
    }
}