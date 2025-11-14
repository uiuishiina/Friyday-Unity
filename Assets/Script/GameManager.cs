using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas Maincanvas;
    public GameObject Player;
    public GameObject[] Points;
    [SerializeField] GameObject[] Enemy;
    int count = 0;

    int pcount = 5;

    private void Start()
    {
        count = Enemy.Length;
    }

    public void Check(GameObject g)
    {
        if (g == Player){
            D();
        }
        for (int i = 0; i < Enemy.Length; i++) {
            if (Enemy[i] == g) 
            { 
                Enemy[i] = null; 
                count--;
                if (count == 0) {
                    Result(true);
                }
                return; 
            }
        }
    }
    private void D()
    {
        if(pcount == 0)
        {
            Result(false);
        }
        pcount--;
    }
    void Result(bool win)
    {
        Debug.Log(win);
    }
}