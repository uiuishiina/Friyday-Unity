using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
public class player_Script : MonoBehaviour
{
    //-----------    参照   ------------------
    [SerializeField] GameObject Hole;
    [SerializeField] Front_Script Front;
    [SerializeField] GameObject Body;
    [SerializeField] GameManager GameManager;

    Rigidbody rb;

    //-----------    移動   -------------------
    Vector2 Inputvec = new Vector2();
    Vector3 Movevec = new Vector3();
    [SerializeField, Header("速度")] float speed = 0.0f;
    //-----------    掘る   -------------------
    GameObject ChackHole = null;
    bool IsFrontChack = false;
    bool IsDig = false;
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
        IsFrontChack = Front.Chack();
        if (IsFrontChack)
        {
            if (Front.Wall)
            {
                return;
            }
            ChackHole = Front.Hole;
            if (ChackHole.transform.localScale.x >= 1)
            {
                //埋めるコルーチン
                UseCol = Fill(ChackHole);
                StartCoroutine(UseCol);
                return;
            }
        }
        //掘るコルーチン
        UseCol = Dig(ChackHole);
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
    }

    IEnumerator Dig(GameObject h)
    {
        Value = ValueCol(1);
        Debug.Log("s");
        yield return StartCoroutine(Value);
        yield break;
    }

    IEnumerator Fill(GameObject h)
    {
        Value = ValueCol(-1);
        Debug.Log("e");
        yield return StartCoroutine(Value);
        yield break;
    }
    IEnumerator ValueCol(int v)
    {
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("x");
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }

    void EndCol()
    {
        StopAllCoroutines();
        UseCol = null;
        Value = null;
    }
}
