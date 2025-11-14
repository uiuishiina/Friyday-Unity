using System.Collections;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class player_Script : MonoBehaviour
{
    //-----------    参照   ------------------
    [SerializeField] GameObject Hole;
    [SerializeField] GameObject Front;
    [SerializeField] Front_Script Front_cs;
    [SerializeField] GameObject Body;
    [SerializeField] GameManager GameManager;
    [SerializeField] Animator Anime;
    Rigidbody rb;
    //-----------    移動   -------------------
    Vector2 Inputvec = new Vector2();
    Vector3 Movevec = new Vector3();
    [SerializeField, Header("速度")] float speed = 0.0f;
    [SerializeField, Header("回転速度")] float rote = 0.0f;
    //-----------    掘る   -------------------
    GameObject ChackHole = null;
    bool IsDig = false;
    bool end = false;
    //------------コルーチン-------------------
    private IEnumerator UseCol = null;
    private IEnumerator Value = null;
    //----------- inputAction -----------------
    InputAction Action;
    //-----------------------------------------
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Action = GetComponent<PlayerInput>().currentActionMap.FindAction("Space");
        Action.performed += OnDigp;
        Action.canceled += OnDigc;
    }

    private void OnMove(InputValue inputValue)
    {
        Inputvec = inputValue.Get<Vector2>();
    }

    private void OnDigp(InputAction.CallbackContext context)
    {
        GameObject g = null;
        if (Front_cs.Chack())
        {
            Debug.Log("d");
            if (Front_cs.Wall){
                return;
            }
            ChackHole = Front_cs.Hole;
            if (ChackHole.transform.localScale.x >= 1)
            {
                //埋めるコルーチン
                UseCol = Fill(ChackHole);
                StartCoroutine(UseCol);
                return;
            }
            else
            {
                g = ChackHole;
            }
        }
        //掘るコルーチン
        UseCol = Dig(g);
        StartCoroutine(UseCol);
        return;
    }
    void OnDigc(InputAction.CallbackContext context)
    {
        EndCol();
    }

    private void Update()
    {
        if (!IsDig){
            Movevec = new Vector3(Inputvec.x * speed, rb.linearVelocity.y, Inputvec.y * speed);
            rb.linearVelocity = Movevec;
        }
        else{
            rb.linearVelocity = new Vector3();
        }

        //-----------------------------------------
        if(!IsDig){
            if (Inputvec.magnitude > 0.1f){
                var a = Mathf.Atan2(Inputvec.x, Inputvec.y) * Mathf.Rad2Deg;
                var q = Quaternion.Euler(0, a, 0);
                Body.transform.localRotation = Quaternion.RotateTowards(Body.transform.localRotation, q, rote);
            }
        }
        //-----------------------------------------
        var s = rb.linearVelocity.magnitude;
        Anime.SetFloat("Speed", s);
        Anime.SetBool("IsDigging", IsDig);
    }
    //----------------　掘る　---------------------
    IEnumerator Dig(GameObject h)
    {
        IsDig = true;
        if (h == null) { 
            h = Instantiate(Hole, Front.transform.position, Quaternion.identity);
            h.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        Value = ValueCol(1,h);
        yield return StartCoroutine(Value);
        h.transform.localScale = new Vector3(1, 1, 1);
        h.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
        yield break;
    }
    //---------------　埋める　---------------------
    IEnumerator Fill( GameObject h )
    {
        IsDig = true;
        end = true;
        h.transform.localScale = new Vector3(1, 1, 1);
        Value = ValueCol(-1,h);
        yield return StartCoroutine(Value);
        end = false;
        h.GetComponent<Hole_Script>().D(gameObject);
        yield break;
    }
    //--------------　拡大・縮小　-------------------
    IEnumerator ValueCol(int v, GameObject h )
    {
        for (int i = 0; i < 10; i++)
        {
            h.transform.localScale += new Vector3(0.1f * v, 0.1f * v, 0.1f * v);
            if(h.transform.localScale.x >= 1){
                yield break;
            }
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }
    //---------------　終了関数　--------------------
    void EndCol()
    {
        StopAllCoroutines();
        IsDig = false;
        UseCol = null;
        Value = null;
        if (ChackHole){
            if (end) {
                ChackHole.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GameManager.Check(this.gameObject);
        }
    }
}
