using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Reader : MonoBehaviour
{
    int tapeIndex = 0;
    List<PlayerInput> playerInputs;

    public void Setup(List<PlayerInput> playerInputs)
    {
        this.playerInputs = playerInputs;
    }

    public bool HasNext()
    {
        if (playerInputs == null)
            return false;

        return tapeIndex < playerInputs.Count;
    }

    public PlayerInput GetFrame()
    {
        if (playerInputs == null)
            return default;

        return playerInputs[tapeIndex++];
    }

    public void Restart()
    {
        tapeIndex = 0;
    }
}
