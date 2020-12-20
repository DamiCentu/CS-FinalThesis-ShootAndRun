using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IHittable , IPauseable
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
    public IShootable specialGunMineDummy;
    public IShootable specialGunSlowTime;
    public IShootable specialGunBomb;
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
    public int dashTotalCount = 0;
    public int MaxDashCount = 1;
    
    public enum Ults { SlowTime, Berserker,Scatter,Spawn,None}
    public Ults ult;
    public bool _gotShield= false;
    public GameObject shield;
    public float InvulnerableTime=3;
    public float InvulnerableExtraTime = 0.5f;
    internal bool _isInvulnerable;
    private Animator _anim;
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
    public MinionsSpawn minionsSpawn;
    public bool debugMode=false;

    bool _paused;
    private bool portalOut;
    private bool portalIn;
    private bool bossFinished=false;

    public void OnPauseChange(bool v) {
        _paused = v;
        _anim.enabled = !v;
    }

    void Start ()
    {
        _rb = this.GetComponent<Rigidbody>();
        SetEvents();

        SetSpecial();


    
        _isInvulnerable = false;
        timerToUlt = ultTimer;
        _anim = this.GetComponent<Animator>();
        RefreshDashUI();
        object[] container = new object[1];
        container[0] = false;
        EventManager.instance.ExecuteEvent(Constants.SHOW_SKILL_UI, container);
        _meshRends = GetComponentsInChildren<Renderer>();
        ult = Configuration.instance.playerUlt;
        dashTotalCount = 0;
        if (Configuration.instance.mode == Configuration.Mode.RogueLike)
        {
            ult = Player.Ults.None;

        }

    }

    private void SetEvents()
    {
        EventManager.instance.SubscribeEvent("GetShield", powerUpManager.EnableShield);
        EventManager.instance.SubscribeEvent(Constants.START_SECTION, Portal);
        EventManager.instance.SubscribeEvent("PlayerDead", Die);
        EventManager.instance.SubscribeEvent(Constants.START_BOSS_DEAD, BossFinished);
    }

    private void SetSpecial()
    {
        if (Configuration.instance.special == Configuration.PlayerSpecial.DumbMine) {
            specialGun = specialGunMineDummy;
        }
        else if (Configuration.instance.special == Configuration.PlayerSpecial.Bomb)
        {
            specialGun = specialGunBomb;
        }
        else {
            specialGun = specialGunSlowTime;
        }
    }

    private void BossFinished(object[] parameterContainer)
    {
        bossFinished = true;
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
        if (_paused) {
            _rb.velocity = new Vector3(0, 0, 0);
            return;
        }
    
        RefreshTimersUlt();

        if (spawned)
        {
            TryDash();
            TryUlt();
            TryMove();
            TryRotate();
            TryShoot();

        }
        else {
            _anim.SetFloat("Horizontal", 0);
            _anim.SetFloat("Vertical", 0);
            _isDashing = false;
            ChangeShaderValue("_DashF", 0);
            //StartCoroutine("Integrate");
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

        if (CanUlt())
        {
            switch (ult)
            {
                case Ults.SlowTime:
                    EventManager.instance.ExecuteEvent(Constants.SLOW_TIME);
                    break;

                case Ults.Berserker:
                    //print("modo berseker");
                    UpgradeStats();
                    timers.Add(new Timer(bersekerTime, RestoreStats));
                    maxTimeUlting = bersekerTime;
                    timeUlting = 0;
                    _isUlting = true;
                    break;
                case Ults.Scatter:
                    //print("modo Scatter");
                    scatter.StarUlt();
                    break;
                case Ults.Spawn:
                    //print("modo Spawn");
                    minionsSpawn.StarUlt();
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
        MyInputManager.instance.GetPlayerRotation(this.transform, control);
  /*      if (!_isDashing)
            MyInputManager.instance.GetPlayerRotation(this.transform, control);
        else {
            MyInputManager.instance.GetPlayerRotation(this.transform, control);
               this.transform.rotation= Quaternion.FromToRotation(this.transform.position, this.transform.position+ dashDirection);
        }
       */
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

        if(TutorialBehaviour.instance!=null && TutorialBehaviour.instance.IsTutorialNode) {
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
                _anim.SetBool("Dash", false);
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

            if (spawned) {
                _rb.velocity = new Vector3(horizontalVel, 0, verticalVel).normalized * speed;

                _anim.SetFloat("Horizontal", horizontalVel);
                _anim.SetFloat("Vertical", verticalVel);
            }
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
        TimerDash();

        float verticalVel = Input.GetAxis("Vertical");
        float horizontalVel = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalVel, 0, verticalVel).normalized;

        if (!CheckDash((new Vector3(0, 0, 1) * verticalVel).normalized))
        {
            //print("Hola");
            verticalVel = 0;
        }
        if (!CheckDash((new Vector3(1, 0, 0) * horizontalVel).normalized))
        {
            //print("asd");
            horizontalVel = 0;
        }


        Vector3 new_direction = new Vector3(horizontalVel, 0, verticalVel).normalized;

        if (CanDash(new_direction))
        {
            //print("dasheo normal");
            StartDash(new_direction);

        }

    }

    private void TimerDash()
    {
        _dashTimer -= Time.deltaTime;
        if (_dashTimer < 0)
        {
            _dashTimer = defaultDashTimer;
            if (dashCount < MaxDashCount)
            {
                dashCount++;
                RefreshDashUI();
            }
        }
    }

    private void StartDash(Vector3 direction)
    {
        _isDashing = true;  // marco que dasheo
        ChangeShaderValue("_DashF", 1);
        _isInvulnerable = true;
        this.transform.rotation = Quaternion.FromToRotation(this.transform.position, this.transform.position + dashDirection);
        _anim.SetBool("Dash", true);
        //   _dashTimer = defaultDashTimer; // reseteo el timer de dash
        _timeDashing = 0;
        dashDirection = direction;
        dashCount--;
        dashTotalCount++;
        if (dashTotalCount > 20) {
            EventManager.instance.ExecuteEvent(Constants.ACHIVEMENT_DASH_DASH_DASH, new object[] { });
        }

        RefreshDashUI();
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
        if (Physics.Raycast(this.transform.position + Vector3.up * 0.5f, direction, offsetDash * 1.42f, ObstacleLayerMask))
        {

            return false;
        }
        else if (!CheckRun(direction)) {
            return false;
        }
        return true;
    }

    private bool CheckRun(Vector3 direction)
    {
        if (Physics.Raycast(this.transform.position+ Vector3.up * 0.5f, direction, 1f, ObstacleLayerMask))// pasar el 1 a un valor mas chico y arreglar bien el movimiento despues
        
        {
            return false;
        }
        return true;
    }

    private bool CanDash(Vector3 direction) {
        if (MyInputManager.instance.Dash(control)
            && CheckDash(direction) 
            && direction != Vector3.zero 
            && dashCount>0) {
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

        }

        if (c.gameObject.layer == 13)//POWER UP
            EventManager.instance.ExecuteEvent(Constants.POWER_UP_PICKED, new object[] { c.gameObject , "isPlayer"});
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

        switch (ult)
        {
            case Ults.Scatter:
                scatter.Stop();
                break;
            case Ults.Spawn:
                minionsSpawn.Stop();
                break;
        }
    }
  
    public void OnHit(int damage)
    {
        if (!_isInvulnerable&&!isDead&&!_inBerserkerMode&& !debugMode&& !bossFinished) {
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
    IEnumerator Disolve()
    {
        float timer = 1;
        while (timer > -1) {
            timer -= 0.05f;
            ChangeShaderValue("_disolve", timer);
            yield return new WaitForSeconds(.05f);
        }

    }

    public void RogueGetRandomPowerUP() {
        powerUpManager.ExtraDash();
    }

    IEnumerator Integrate()
    {
        float timer = -1;
        while (timer < 1)
        {
            timer += 0.05f;
            ChangeShaderValue("_disolve", timer);
            yield return new WaitForSeconds(.05f);
        }

    }

    private void Portal(object[] parameterContainer)
    {

        //FLOR ESTA FUNCI(portal) SE EJECTUTA 2 VECES Y HACE QUE EL EVENTO DENTRO DE FinishSpawn() SE EJECUTE 2 VECES
        //GENERANDO EL PROBLEMA DE SPAWN ESE DE LA TORRETA QUE SE ESTA MOVIENDO
        //LO QUE PASA ES LO SIGUIENTE: EN SECTIONMANAGER LLAMAS 2 VECES ESTO, LA PRIMERA AUNQUE PAREZCA RARO, 
        //LO ESTAS USANDO PARA QUE APAREZCA EL PORTAL CUANDO TERMINA LA SECCION, (OLVIDATE DE DENTRO DE IF BOSS NODE)
        //ENTONCESY LA OTRA VEZ, ES LA LEGIT, DONDE SE DEBERIA EJECTURAR EL EVENTO PLAYER_CAN_MOVE XQ SE DA POR ENTENDIDO QUE SALIO DEL PORTAL
        //LO IDEAL SERIA SEPARAR EL PORTAL DE CUANDO APARECE DE CUANDO TERMINA ASI SE EJECUTA BIEN(RECORDA QUE EL START_SECTION DEL SECTION MANAGER EN LA LINEA 121 SIRVE PARA
        //CUANDO TERMINA EL NODO "PONELE", TE VAS A DAR CUENTA SI LO TESTEAS LO QUE DIGO (EL BUG ES REPRODUCIBLE SI GANAS LA SECCION, PODES DEJARTE 1 SOLO ENEMIGO Y ALCANZA
        //PARA GANARLA RAPIDO Y PASAS A LA SIGUIENTE)
        if ((string)parameterContainer[0] == "in")
        {
            StartCoroutine("Integrate");
            portalIn = true;
        }
        if ((string)parameterContainer[0] == "out")
        {
            StartCoroutine("Disolve");
            portalOut = true;
        }
        _anim.SetFloat("Hotizontal", 0);
        _anim.SetFloat("Vertical", 0);
       
        Timer portalTimer = new Timer(2.5f, FinishSpawn);
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
        _anim.SetFloat("Hotizontal", 0);
        _anim.SetFloat("Vertical", 0);
        spawnParticle1.gameObject.SetActive(false);
        spawnParticle2.gameObject.SetActive(false);
        spawned = true;
        isDead = false;
        dashCount = MaxDashCount;
        ActiveUI();
        timerToUlt = 0;
        timerCooldownSpecial = 0;
        portalIn = false;
        portalOut = false;
        EventManager.instance.ExecuteEvent(Constants.PLAYER_CAN_MOVE);
    }

    private void ActiveUI()
    {
        object[] container = new object[1];
        container[0] = true;
        EventManager.instance.ExecuteEvent(Constants.SHOW_SKILL_UI, container);
        RefreshDashUI();
    }
}
