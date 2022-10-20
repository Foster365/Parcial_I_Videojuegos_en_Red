using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class EnemyView : MonoBehaviourPun
{
    public int rutina;
    public float cronometro;
    public Animator enemyAnimator;
    public Quaternion angulo;
    public float grado;

    public GameObject target;
    public bool atacando;

    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        enemyAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        target = GameObject.Find("Player");
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Comportamiento_Enemigo();
        }
    }
    public void Comportamiento_Enemigo()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            enemyAnimator.SetBool("run", false);
            cronometro += Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch (rutina)
            {
                case 0:
                    enemyAnimator.SetBool("walk", false);
                    break;
                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;
                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    enemyAnimator.SetBool("walk", true);
                    break;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)
            {
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                enemyAnimator.SetBool("walk", false);

                enemyAnimator.SetBool("run", true);
                transform.Translate(Vector3.forward * 2 * Time.deltaTime);

                enemyAnimator.SetBool("attack", false);
            }
            else
            {
                enemyAnimator.SetBool("walk", false);
                enemyAnimator.SetBool("run", false);

                enemyAnimator.SetBool("attack", true);
                atacando = true;
            }
        }
    }

    public void Final_Ani()
    {
        enemyAnimator.SetBool("attack", false);
        atacando = false;
    }

}
