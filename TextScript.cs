using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public void Update()
    {
        transform.GetComponent<TextMeshPro>().text = gameObject.GetComponentInParent<PlayerControl>().coins.ToString();
    }
}
