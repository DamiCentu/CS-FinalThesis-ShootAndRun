using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine: MonoBehaviour {
    //   Dictionary<IState, List<IState>> _stateMachine;
    //  Dictionary<string, IState> _allStates;
    IState _currentState;
    public List<IState> states=new List<IState>();
   //S bool ShouldChangeState;
    public static StateMachine instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    internal void AddState(IState state)
    {
        states.Add(state);
    }

    /*  IState NextState() {
          foreach (IState state in _stateMachine[_currentState])
          {
              if (state.MatchesRequirements()) {
                  return state;
              }
          }
          return null;
      }
      */
    void FixedUpdate() {
        if (_currentState == null)
        {
            _currentState = GetState("Fall");
            _currentState.Begin();
        }
        ChangeStateIfNeeded(_currentState.NextState());
        _currentState.Process();
    }

    internal void ChangeStateIfNeeded(string nameNextState , object param=null)
    {
        if (nameNextState != null)
        {
            //Debug.Log(nameNextState);
            IState nextState = GetState(nameNextState);

            if (nextState == null)
            {

                throw new Exception("No existe el nombre del estado al que se quiere acceder");
            }

            _currentState.Finish();

            _currentState = nextState;
            if (param != null)
            {
                _currentState.setParam(param);
            }
            _currentState.Begin();
        }
    }

    IState GetState(string name)
    {
        //Debug.Log(name);
        foreach (IState state in states)
        {
            if (state.GetName() == name)
            {
                return state;
            }
        }
        return null;
    }
}
