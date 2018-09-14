using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
//    bool MatchesRequirements();
    void Begin();
    void Finish();
    void Process();
    String NextState();

    String GetName();
    void setParam(object param);
}