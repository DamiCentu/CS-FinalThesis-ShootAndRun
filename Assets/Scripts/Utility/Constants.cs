using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{

    public static string EVENT_BULLET_RETURN_TO_POOL = "returnBulletToPool";
    public static string SLOW_TIME = "slowTime";
    public static string BERSERK = "berserk";
    public static string STOP_BERSERK = "stopBerserk";
    public static string PLAYER_DEAD = "PlayerDead";
    public static string ENEMY_DEAD = "EnemyDead";
    public static string POWER_UP_PICKED = "PowerUpPicked";
    public static string UPDATE_DASH = "updateDash";
    public static string POWER_UP_DROPED = "PowerUpDropped";

    public static string PLAYER_CAN_MOVE = "player_can_move";

    public static string PARTICLE_SET = "SetParticle";
    public static string PARTICLE_RETURN_TO_POOL = "ReturnParticleToPool";
    public static string PARTICLE_ENEMY_EXPLOTION_NAME = "EnemyExplotionParticle";
    public static string PARTICLE_HERO_BULLET_HIT_NAME = "HeroBulletHit";
    public static string PARTICLE_BOSS_EXPLOTION_NAME = "BossExplotionParticle";

    public static string CAMERA_ON_FOLLOW_PLAYER = "CameraFollowPlayer";
    public static string CAMERA_ON_BOSS = "CameraLookAtPlayer";
    public static string CAMERA_STATIONARY = "CameraStationary";

    public static float ENEMIES_NORMAL_MULTIPLICATOR = 1f;

    public static string MISILE_DESTROY = "misileDestroy";
    public static string UPDATE_BOSS_LIFE = "update_boss_life";
    public static string BOSS_DESTROYED = "bossDestroyed";
    public static string BOSS_CAMERA_LOOK_AT = "boss_camera_look_at";

    public static string START_SECTION = "start_section";

    public static string RESTART_SECTION = "restart_section";

    public static string ULTIMATE_TIME = "ultimate_time";
    public static string SPECIAL_TIME = "special_time";

    public const string ULTIMATE_CHANGE = "ultimate_change";
    public const string SPECIAL_CHANGE = "special_change";

    public const string STARTED_SECTION_solo_escucha_camera_iTween_noseporque = "started_section";

    public const string UI_UPDATE_PLAYER_LIFE = "player_life";

    public const string GAME_OVER = "game_over";

    public const string CREATE_SOUL = "set_power_up";

    public const string SOUL_RECOVER = "soul_recover";

    public const string SHOW_SKILL_UI = "show_skill_ui";

    public const string QUANTITY_POWERUPS = "quantity_power_ups";

    public const string CHARGER_CRUSH = "Charger_Crush";

    public const string LEFT_TIME_ULTING = "left_time_ulting";

    public const string SEND_INFO = "send_info";

    public const string BLACK_SCREEN = "black_screen";

    public const string UI_TUTORIAL_RESTART = "ui_tutorial_restart";
    public const string UI_TUTORIAL_DEACTIVATED = "ui_tutorial_deactivated";
    public const string UI_TUTORIAL_CHANGE = "ui_tutorial_change";
    public const string UI_POINTS_UPDATE = "ui_points_Update";
    public const string UI_CLEAR_MULTIPLIER = "ui_clear_multiplier";
    public const string UI_NOTIFICATION_TEXT_UPDATE = "ui_notification_text_update";

    public const string SOUND_FADE_IN = "sound_fade_in";
    public const string SOUND_FADE_OUT = "sound_fade_out";
    public const string SOUND_BULLET_HIT = "bullet_hit";

    public const string START_BOSS_DEAD = "start_boss_dead";

    public const string DESTROY_ENEMIES_BEHIND_WALL = "destroyEnemiesBehindWall";

    public const string CREDIT_LOSED = "credit_losed";
    public const string PAUSE_OR_UNPAUSE = "pauseOrUnpause";

    public const string WIN_LEVEL = "win_level";

    public const string LEVEL_1_SCENE_NAME = "nivelPrueba";
    public const string LEVEL_2_SCENE_NAME = "Scene2";
    public const string LEVEL_ROGUELIKE_SCENE_NAME = "prueba";
    public const string MENU_SCENE_NAME = "Menu";

    public const string GO_TO_LEVEL_COMPLETE_SCENE = "goToLevelCompleteScene";
    public const string GO_TO_GAME_COMPLETE_SCENE = "goToGameCompleteScene";

    public const string MENU_CAMERA_NAVIGATE = "menuCameraNavigate";
    public const string MENU_BUTTON_CLICKED = "menuButtonClicked";


    public const string ACHIVEMENT_NO_DEATH = "UI_ACHIVEMENT_NO_DEATH";
    public const string ACHIVEMENT_MINI_BOSS_DEFEAT = "ACHIVEMENT_MINI_BOSS_DEFEAT";
    public const string ACHIVEMENT_DASH_DASH_DASH = "ACHIVEMENT_DASH_DASH_DASH";
    public const string ACHIVEMENT_LVL2_COMPLETE = "ACHIVEMENT_LVL2_COMPLETE";
    public const string ACHIVEMENT_LVL1_COMPLETE = "ACHIVEMENT_LVL1_COMPLETE";
    public const string ACHIVEMENT_FIRST_STAGE_COMPLETE = "ACHIVEMENT_FIRST_STAGE_COMPLETE";
    public const string ACHIVEMENT_UPGRADE_WEAPON = "ACHIVEMENT_UPGRADE_WEAPON";
    public const string ACHIVEMENT_POWER_UP_RECOVER = "ACHIVEMENT_POWER_UP_RECOVER";
    public const string ACHIVEMENT_MORE_RANGE = "ACHIVEMENT_MORE_RANGE";
    public const string ACHIVEMENT_SHIELD = "ACHIVEMENT_SHIELD";
    public const string ACHIVEMENT_CLOSE_DEATH = "ACHIVEMENT_CLOSE_DEATH";
    public const string ACHIVEMENT_EXTRA_DASH = "ACHIVEMENT_EXTRA_DASH";
    public const string ACHIVEMENT_100_ENEMIES_DEAD = "ACHIVEMENT_100_ENEMIES_DEAD";





}
