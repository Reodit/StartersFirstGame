using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform misslePortA;
    public Transform misslePortB;

    private Vector3 lockVec;
    private Vector3 tauntVec;
    public bool isLook;
    
    void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _mesh = GetComponentsInChildren<MeshRenderer>();
        _nav = GetComponent<NavMeshAgent>();
        _nav.isStopped = true;
        StartCoroutine(Think());
    }

 
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lockVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(_target.position + lockVec);

        }
        else
        {
            _nav.SetDestination(tauntVec);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case  0:
               
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
               
            case 3:
                StartCoroutine(rockShot());
                break;
            case 4 :
                StartCoroutine(Taunt());
                break;
                
        }
    }

    IEnumerator MissileShot()
    {
        _anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instanMissile = Instantiate(missile, misslePortA.position, misslePortA.rotation);
        BossMissile bossMissileA = instanMissile.GetComponent<BossMissile>();
        bossMissileA.target = _target;
        
        yield return new WaitForSeconds(0.3f);
        GameObject instanMissileB = Instantiate(missile, misslePortB.position, misslePortB.rotation);
        BossMissile bossMissileB = instanMissile.GetComponent<BossMissile>();
        bossMissileA.target = _target;
        
        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }
    IEnumerator rockShot()
    {
        isLook = false;
        
        _anim.SetTrigger("doBigShot");
        Instantiate(_bullet, transform.position, transform.rotation);
        
        yield return new WaitForSeconds(3f);
        isLook = true;
        StartCoroutine(Think());
    }
    
    IEnumerator Taunt()
    {
        tauntVec = _target.position + lockVec;

        isLook = false;
        _nav.isStopped = false;
        _boxCollider.enabled = false;
        
        _anim.SetTrigger("doTaunt");
        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        
        yield return new WaitForSeconds(1f);
        isLook = true;
        _boxCollider.enabled = true;
        _nav.isStopped = true;
        StartCoroutine(Think());
    }
}
