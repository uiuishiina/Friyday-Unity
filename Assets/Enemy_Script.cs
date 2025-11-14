using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Script : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    GameManager gameManager;
    GameObject player;
    GameObject[] Point;
    [SerializeField] Animator animator;
    Rigidbody rb;

    GameObject Target = null;
    bool Isfall = false;
    GameObject h;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        player = gameManager.Player;
        Point = gameManager.Points;
        Chack();
    }

    private void Update()
    {
        if (Isfall){
            animator.SetTrigger("Falling");
            return; }
        if (agent.remainingDistance < 1){
            Chack();
        }
        var length = (player.transform.position - transform.position).magnitude;
        if (length < 5)
        {
            Target = player;
        }
        else if (length < 10)
        {
            var vec = player.transform.position - transform.position.normalized;
            var dot = Vector3.Dot(transform.forward, vec);
            if (dot > 0.8f)
            {
                Target = player;
            }
        }

        agent.SetDestination(Target.transform.position);
        var r = rb.linearVelocity.magnitude;
        animator.SetFloat("Speed", r);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Hole")
        {
            h = other.gameObject;
            Isfall = true;
            agent.SetDestination(h.transform.position);
            StartCoroutine(hole());
        }
    }
    IEnumerator hole()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
        }
        animator.SetBool("IsFallen", false);
        Isfall = false;
        if(h != null) { h.GetComponent<Hole_Script>().D(gameObject); }
        yield break;
    }
    void Chack()
    {
        var length = (player.transform.position - transform.position).magnitude;
        if(length < 5)
        {
            Target = player;
            return;
        }
        else if(length < 10)
        {
            var vec = player.transform.position - transform.position.normalized;
            var dot = Vector3.Dot(transform.forward, vec);
            if(dot > 0.8f)
            {
                Target = player;
                return;
            }
        }
        Target = Point[Random.Range(0, Point.Length)];
        return;
    }

    private void OnDestroy()
    {
        gameManager.Check(this.gameObject);
    }
}
