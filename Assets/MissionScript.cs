using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript : MonoBehaviour
{
    public EnemyMovement[] enemies;

    public Transform player, shogun;
    public GameObject noise;

    private bool task_0_finished = false;
    private bool task_1_finished = false;
    private bool trans_complete = false;
    private bool task_2_finished = false;


    private void Start()
    {
        foreach (EnemyMovement e in enemies)
        {
            e.TakeDamage(10000);
        }


        LevelManager.instance.ShowAmmo(false);
        LevelManager.instance.audioPlayer.Play("rmusic_level_6");
        LevelManager.instance.ShowHint("Unkill the enemies");
        LevelManager.instance.timerHandler.time = 0;
        LevelManager.instance.reverse = true;

        LevelManager.instance.objsHandler.InitTasks(new string[]{
            "Unkill all the enemies",
            "Go Near Shogun",
            "Kill all the enemies"
            }, false);
    }


    private void Update()
    {
        if (!LevelManager.instance.IsReverse() && LevelManager.instance.timerHandler.time<=0)
        {
            LevelManager.instance.GotoMainMenu();
        }


        if (!task_0_finished)
        {
            bool allHealthFull = true;
            foreach (EnemyMovement e in enemies)
            {
                if (!e.IsHealthFull())
                {
                    allHealthFull = false; break;
                }
            }
            if (allHealthFull)
            {
                task_0_finished = true;
                LevelManager.instance.ShowHint("Go near shogun");
                LevelManager.instance.objsHandler.CheckTask(0, true);
            }
        }
        else if (!task_1_finished)
        {
            if (Vector2.Distance(player.position, shogun.position) <= 1)
            {
                task_1_finished = true;
                noise.SetActive(true);
                LevelManager.instance.audioPlayer.Stop("rmusic_level_6");

                //shogun.GetComponent<ShogunEffect>().turnOff();
                Destroy(shogun.gameObject);

                LevelManager.instance.timerHandler.paused = true;
                LevelManager.instance.reverse = false;
                Invoke("DoTransition", 1f);
            }
        }
        else if (!trans_complete) { }
        else if (!task_2_finished)
        {
            bool alldead = true;
            foreach (EnemyMovement e in enemies)
            {
                if (!e.IsDead())
                {
                    alldead = false; break;
                }
            }
            if (alldead) LevelManager.instance.GotoMainMenu();
        }
    }


    public void DoTransition()
    {
        noise.SetActive(false);
        trans_complete = true;
        LevelManager.instance.timerHandler.paused = false;
        LevelManager.instance.audioPlayer.Play("music_level_6");

        LevelManager.instance.ShowHint("Kill all the enemies");
        LevelManager.instance.objsHandler.CheckTask(1, true);

    }
}
