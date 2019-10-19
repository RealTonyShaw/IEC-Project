using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCanvasController : MonoBehaviour
{
    public Canvas canvas;
    public Text nameText;
    
    private void LateUpdate()
    {
        canvas.transform.forward = Camera.main.transform.forward;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }
}
