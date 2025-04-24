using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Resets : MonoBehaviour
{
    public void EndgameReset()
    {
        FindObjectOfType<PlayerStats>().EndgameReset();
    }
}
