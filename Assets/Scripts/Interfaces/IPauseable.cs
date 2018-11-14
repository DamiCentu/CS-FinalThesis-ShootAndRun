using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseable {
    void OnPauseChange(bool pauseState);
}
