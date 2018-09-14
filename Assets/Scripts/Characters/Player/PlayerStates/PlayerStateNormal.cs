using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateNormal : IState {
    void IState.Begin()
    {
        throw new NotImplementedException();
    }

    void IState.Finish()
    {
        throw new NotImplementedException();
    }

    string IState.GetName()
    {
        return "Normal";
    }

    string IState.NextState()
    {
        throw new NotImplementedException();
    }

    void IState.Process()
    {
        
    }

    void IState.setParam(object param)
    {
        throw new NotImplementedException();
    }

}
