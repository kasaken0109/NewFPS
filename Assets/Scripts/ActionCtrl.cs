using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCtrl
{
    StateBase _currentState = null;
    string currentName;

    public void SetCurrent(StateBase s)
    {
        if (s == null) return;
        if (_currentState != null)
        {
            _currentState.EventCall(StateBase.Event.Leave);
        }

        _currentState = s;
        currentName = s.name;
        _currentState.SetCtrl(this);
        _currentState.EventCall(StateBase.Event.Enter);
    }

    public void SetCurrentName(string s)
    {
        currentName = s;
    }


    public string GetCurrentStateName()
    {
        return currentName;
    }
}