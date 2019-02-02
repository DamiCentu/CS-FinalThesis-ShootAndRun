using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour, IOffScreen {
    protected SectionManager.WaveNumber _actualWave;
    protected SectionNode _actualSectionNode;
    protected int wallToUnlockID = 0;
    protected bool hasToDestroyThisToUnlockSomething = false;
    protected bool paused = false;

    OnHitWhiteFeedback _onHitWhiteFeedback;

    protected EnemiesIntegrationBehaviour _eIntegration;

    public SectionNode GetCurrentSectionNode { get { return _actualSectionNode; } }

    public Vector3 GetPosition { get { return transform.position; } }

    public AbstractEnemy SetActualWave(SectionManager.WaveNumber wave) {
        _actualWave = wave;
        return this;
    }

    public AbstractEnemy SetActualNode(SectionNode node) {
        _actualSectionNode = node;
        return this;
    }

    public AbstractEnemy SetIffHasToDestroyToOpenSomething(bool value, int wallID) {
        hasToDestroyThisToUnlockSomething = value;
        wallToUnlockID = wallID;
        return this;
    }

    public AbstractEnemy SetIntegration(float timeToReintergrate) {
        if (_eIntegration == null) { 
            _eIntegration = GetComponent<EnemiesIntegrationBehaviour>();
        }

        _eIntegration.SetReintergration(timeToReintergrate); 
        return this; 
    }



    public AbstractEnemy SubscribeToIndicator() {
        OffScreenIndicatorManager.instance.SubscribeIOffScreen(this);
        return this;
    }

    public AbstractEnemy UnSubscribeToIndicator() {
        OffScreenIndicatorManager.instance.UnsubscribeIOffScreen(this);
        return this;
    }

    //settea el ponerse blanco cuando le pegan
    public AbstractEnemy SetTimeAndRenderer() { 
        if(_onHitWhiteFeedback == null) {
            _onHitWhiteFeedback = GetComponent<OnHitWhiteFeedback>();
        }

        _onHitWhiteFeedback.SetFeedback();
        return this;
    } 

    protected void AbstractOnHitWhiteAction() {
        _onHitWhiteFeedback.OnHit();
    }
}
