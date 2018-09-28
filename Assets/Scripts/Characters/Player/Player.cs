using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IHittable
{
    public enum ID { Player1,Player2}
    public ID id ;
    public PlayerPowerUpManager powerUpManager;
    public Material mat;

    public MyInputManager.Control control;
    public float speed =2;
    private Rigidbody _rb;
    public IShootable primaryGun;
    public IShootable specialGun;
    public Transform shootPosition;

    internal List<Timer> timers = new List<Timer>();

    public float specialCoolDown;
    private float _dashTimer;
    public float defaultDashTimer;
    private bool _isDashing;
 //   public float dashDistance;
    public float dashMaxAceleration;
    public float dashTime;
    private float _timeDashing;
    public LayerMask layersAttacksPlayer;
    public LayerMask ObstacleLayerMask;
    public float offsetDash=1;
    private float timerCooldownShoot=0;
    public float CooldownShoot=0.5f;
    public AnimationCurve dashMovement;
    private Vector3 dashDirection;
    public int dashCount = 1;
    public int MaxDashCount = 1;
    
    public enum Ults { SlowTime, Berserker,Scatter}
    public Ults ult;
    public bool _gotShield= false;
    public GameObject shield;
    public float InvulnerableTime=3;
    public float InvulnerableExtraTime = 0.5f;
    internal bool _isInvulnerable;
    private Animator an;
    public float bersekerTime=5;
    public ParticleSystem DeadParticle;
    public ParticleSystem spawnParticle1;
    public ParticleSystem spawnParticle2;
    private bool spawned=false;
    public float ultTimer=10;

    private float timerToUlt=10;
    private float timerCooldownSpecial;
    public bool isDead;
    public GameObject soulPrefab;
    private bool _inBerserkerMode;
    private Renderer[] _meshRends;
        public ParticleSystem berserkerParticle;
    private float timeUlting;
    private float maxTimeUlting;
    private bool _isUlting;
    public GameObject wings;
    public ScatterUlt scatter;

    void Start () {
        _rb=this.GetComponent<Rigidbody>();
        EventManager.instance.SubscribeEvent("GetShield", powerUpManager.EnableShield);
        EventManager.instance.SubscribeEvent(Constants.START_SECTION, Portal);
        EventManager.instance.SubscribeEvent("PlayerDead",Die);

        _isInvulnerable = false;
        timerToUlt = ultTimer;
        an = this.GetComponent<Animator>();
        RefreshDashUI();
        object[] container = new object[1];
        container[0] = false;
        EventManager.instance.ExecuteEvent(Constants.SHOW_SKILL_UI, container);
        _meshRends =  GetComponentsInChildren<Renderer>();
        ult = Configuration.instance.playerUlt;
    }
   
    internal void ChangeUlt(Ults newUlt)
    {
        ult = newUlt;
    }

    internal void ChangeSpecial(IShootable special)
    {
        specialGun = special;
    }



    void Update () {
        if (spawned)
        {
            TryDash();
            TryUlt();
            TryMove();
            TryRotate();
            TryShoot();

        }
        else {
            _isDashing = false;
            ChangeShaderValue("_DashF", 0);
            Vector3 directionToLook = transform.position + new Vector3(0, 0, 10);
            transform.LookAt(directionToLook);
            _rb.velocity = Vector3.zero;

        }

        var timersToDelete = new List<Timer>();
        foreach (var timer in timers)
        {
            var shouldDelete = timer.CheckAndRun();
            
            if (shouldDelete)
            {
                timersToDelete.Add(timer);
            }
        }

        foreach (var timer in timersToDelete)
        {
            timers.Remove(timer);
        }
	}



    private void TryUlt()
    {
        RefreshTimersUlt();

        if (CanUlt())
        {
            switch (ult)
            {
                case Ults.SlowTime:
                    EventManager.instance.ExecuteEvent(Constants.SLOW_TIME);
                    break;

                case Ults.Berserker:
                    print("modo berseker");
                    UpgradeStats();
                    timers.Add(new Timer(bersekerTime, RestoreStats));
                    maxTimeUlting = bersekerTime;
                    timeUlting = 0;
                    _isUlting = true;
                    break;
                case Ults.Scatter:
                    print("modo Scatter");
                    scatter.StarUlt();
                    break;
            }

            timerToUlt = ultTimer;
        }
    }

    private void RefreshTimersUlt()
    {
        if (!_isUlting)
        {
            timerToUlt -= Time.deltaTime;
            object[] container = new object[3];
            container[0] = timerToUlt;
            container[1] = ultTimer;
            container[2] = id;
            EventManager.instance.ExecuteEvent(Constants.ULTIMATE_TIME, container);
        }
        else
        {
            timeUlting += Time.deltaTime;
            object[] container = new object[3];
            container[0] = timeUlting;
            container[1] = maxTimeUlting;
            container[2] = id;
            //print(timeUlting);
            //print(maxTimeUlting);
            EventManager.instance.ExecuteEvent(Constants.LEFT_TIME_ULTING, container);
        }
    }
    private void ChangeShaderValue(string name, float value)
    {
        foreach (var item in _meshRends)
        {
            item.material.SetFloat(name, value);
        }
    }

    private void UpgradeStats() {
        dashMaxAceleration*=1.5f;
        dashCount = MaxDashCount;
        dashTime /= 10;
        CooldownShoot /= 2;
        speed *=2f;
        _inBerserkerMode= true;

        ChangeShaderValue("_BerserkF", 1f);
        berserkerParticle.gameObject.SetActive(true);
        berserkerParticle.transform.position= this.transform.position;
        OpenWings(true);
    }


    private void RestoreStats()
    {
        dashMaxAceleration /= 1.5f;
        dashTime *= 10;
        CooldownShoot *= 2;
        speed /= 2;
        _inBerserkerMode = false;

        ChangeShaderValue("_BerserkF", 0f);
        berserkerParticle.gameObject.SetActive(false);
        _isUlting = false;
        OpenWings(false);
    }

    private void OpenWings(bool v)
    {
        wings.SetActive(v);
        wings.GetComponent<Animator>().SetBool("open", v);
    }

    private bool CanUlt()
    {
        if (MyInputManager.instance.Ult(control) && timerToUlt <= 0 )
        {
            return true;
        }
        return false;
    }

    private void TryRotate()
    {
        if (!_isDashing)
            MyInputManager.instance.GetPlayerRotation(this.transform, control);
        else {
            this.transform.rotation= Quaternion.FromToRotation(this.transform.position, this.transform.position+ dashDirection);
        }
       
    }

    private void TryShoot()
    {
        if (_isDashing) {
            return;
        }
        timerCooldownShoot -= Time.deltaTime;
        timerCooldownSpecial -= Time.deltaTime;



        if (MyInputManager.instance.Shoot(control) && timerCooldownShoot < 0)
        {
            timerCooldownShoot = CooldownShoot;
            primaryGun.Shoot(shootPosition, this.transform.forward);
            SoundManager.instance.PlayPlayerShoot();
        }

        if (MyInputManager.instance.ShootSpecial(control) && timerCooldownSpecial < 0) {
            timerCooldownSpecial = specialCoolDown;  // tiempo para disparar de nuevo
           // timerCooldownSpecial = specialCoolDown; // tiempo para poder usar la ulti de nuevo
            specialGun.Shoot(shootPosition, this.transform.forward);
        }


        object[] container = new object[3];
        container[0] = timerCooldownSpecial;
        container[1] = specialCoolDown;
        container[2] = id;

        EventManager.instance.ExecuteEvent(Constants.SPECIAL_TIME, container);
    }

    private void TryMove()
    {

        Vector2 vel = MyInputManager.instance.Move(control);

        if(TutorialBehaviour.instance.IsTutorialNode) {
            if(vel != Vector2.zero) { 
                EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_CHANGE, UIManager.TUTORIAL_DASH);
            }
        }
        float horizontalVel = vel.x;
        float verticalVel = vel.y;

        if (_isDashing)
        {
            if (CheckDash(dashDirection)) {
                //print("lala");
                Dash(dashDirection);
            }
            _timeDashing += Time.deltaTime;
            if (_timeDashing >= dashTime) {
                 _isDashing = false;
                ChangeShaderValue("_DashF", 0);
                // _isInvulnerable = false;
                Timer timer = new Timer(InvulnerableExtraTime, () =>
                {
                    _isInvulnerable = false;
                });
                timers.Add(timer);
                an.SetBool("Dash", false);
            }
        }
        else
        {
            Vector3 direction = (Vector3.forward * verticalVel).normalized;

            List<Vector3> a = new List<Vector3>();
            a.Add(this.transform.position);
            a.Add(this.transform.position + direction * 20);


            if (!CheckRun(direction)) {
                //print("Hola");
                verticalVel = 0;
            }
            if (!CheckRun((new Vector3(1, 0, 0) * horizontalVel).normalized))
            {
                //print("asd");
                horizontalVel = 0;
            }
            //    horizontalVel = CheckRun(Vector3.right * horizontalVel) ? horizontalVel : 0;

            _rb.velocity = new Vector3(horizontalVel, 0, verticalVel).normalized * speed;
            //float animHor = horizontalVel / speed;
            //float animVer = verticalVel / speed;
            an.SetFloat("Horizontal", horizontalVel);
            an.SetFloat("Vertical", verticalVel);
        }
    }

    private void Dash(Vector3 direction)
    {

        float x = _timeDashing / defaultDashTimer;
        float dashAceleration= dashMovement.Evaluate(x);
        float currentSpeed = speed + dashAceleration * dashMaxAceleration;
        _rb.velocity = direction.normalized * currentSpeed;
    }

    private void TryDash()
    {
         _dashTimer -= Time.deltaTime;
        if (_dashTimer < 0) {
            _dashTimer = defaultDashTimer;
            if (dashCount < MaxDashCount) {
                dashCount++;
                RefreshDashUI();
            }
        }

        float verticalVel = Input.GetAxis("Vertical");
        float horizontalVel = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalVel, 0, verticalVel).normalized ;

       // if (MyInputManager.instance.Dash() && CheckDash(direction) &&  _dashTimer <= 0 && direction != Vector3.zero) { // tengo que tener una direccion, haber tocado la tecla y no estar en cooldown
       if (CanDash(direction)) { 
            _isDashing = true;  // marco que dasheo
            ChangeShaderValue("_DashF", 1);
            _isInvulnerable = true;
            this.transform.rotation = Quaternion.FromToRotation(this.transform.position, this.transform.position + dashDirection);
            an.SetBool("Dash", true);
            //   _dashTimer = defaultDashTimer; // reseteo el timer de dash
            _timeDashing = 0;
            dashDirection = direction;
            dashCount--;
            RefreshDashUI();
           
        }
    }

    public void RefreshDashUI()
    {
        object[] container = new object[3];
        container[0] = dashCount ;
        container[1] = MaxDashCount;

        EventManager.instance.ExecuteEvent(Constants.UPDATE_DASH, container);
    }

    private bool CheckDash(Vector3 direction)
    {
        if(Physics.Raycast(this.transform.position, direction,  offsetDash, ObstacleLayerMask))
        {
            return false;
        }
        return true;
    }

    private bool CheckRun(Vector3 direction)
    {
        if (Physics.Raycast(this.transform.position+ Vector3.up * 0.5f, direction, 1, ObstacleLayerMask))
        {
            return false;
        }
        return true;
    }

    private bool CanDash(Vector3 direction) {
        if (MyInputManager.instance.Dash(control) && CheckDash(direction) && direction != Vector3.zero && dashCount>0) {
            return true;
            }
        return false;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (layersAttacksPlayer == (layersAttacksPlayer | (1 << c.gameObject.layer))) {
            if (_inBerserkerMode) {
                IHittable o = c.GetComponent<IHittable>();
                if(o!=null) o.OnHit(10);
            }
            OnHit(1);
            /*        else if (_gotShield)
                    {
                        powerUpManager.DisableShield();
                    }
                    else if (!_isInvulnerable)
                    {
                        OnHit(1); //todo hacer la perdida de vida por eventos
                    }*/

        }

        if (c.gameObject.layer == 13)//POWER UP
            EventManager.instance.ExecuteEvent(Constants.POWER_UP_PICKED, new object[] { c.gameObject });
    }


    public void Die(object[] parameterContainer)
    {
        DeadParticle.transform.position = this.transform.position;
        DeadParticle.gameObject.SetActive(true);
        DeadParticle.Play();
        if (SectionManager.instance.actualNode.id != 0) {

        powerUpManager.CreateSoul();
        }
        gameObject.SetActive(false);
        _isUlting = false;
    }
  
    public void OnHit(int damage)
    {
        if (!_isInvulnerable&&!isDead&&!_inBerserkerMode) {
            if (_gotShield)
            {
                powerUpManager.DisableShield();
                return;
            }
            EventManager.instance.ExecuteEvent(Constants.PLAYER_DEAD);
            isDead = true;
            object[] container = new object[1];
            container[0] = false;
            EventManager.instance.ExecuteEvent(Constants.SHOW_SKILL_UI, container);
        }
    }

    private void Portal(object[] parameterContainer)
    {
        Timer portalTimer = new Timer(2.1f, FinishSpawn);
        timers.Add(portalTimer);
        spawned = false;
        spawnParticle1.transform.position = this.transform.position;
        spawnParticle2.transform.position = transform.position + Vector3.up * 1.5f;

        spawnParticle1.gameObject.SetActive(true);
        spawnParticle2.gameObject.SetActive(true);

        spawnParticle1.Play();
        spawnParticle2.Play();

        timerToUlt = ultTimer;
        timerCooldownSpecial = specialCoolDown;
        object[] container = new object[1];
        container[0] = false;
        EventManager.instance.ExecuteEvent(Constants.SHOW_SKILL_UI, container);
        EventManager.instance.ExecuteEvent(Constants.SOUND_FADE_OUT);
    }

    private void FinishSpawn()
    {
        spawnParticle1.gameObject.SetActive(false);
        spawnParticle2.gameObject.SetActive(false);
        spawned = true;
        isDead = false;
        dashCount = MaxDashCount;
        ActiveUI();
        timerToUlt = 0;
        timerCooldownSpecial = 0;
    }

    private void ActiveUI()
    {
        object[] container = new object[1];
        container[0] = true;
        EventManager.instance.ExecuteEvent(Constants.SHOW_SKILL_UI, container);
        RefreshDashUI();
    }
}
