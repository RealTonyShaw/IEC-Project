using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RegisterPanel
{
    public void OnConfirmClick()
    {
        if (userName.text == null || userName.text.Length < 1)
        {
            return;
        }
        if (password.text == null || password.text.Length < 6)
        {
            return;
        }
        if (!password.text.Equals(repeatPw.text))
        {
            Debug.Log("Differance password");
            return;
        }
        Debug.Log("Reg");
        DataSync.Register(userName.text, password.text);
    }
}
