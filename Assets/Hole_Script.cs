using UnityEngine;

public class Hole_Script : MonoBehaviour
{
    GameObject enemy = null;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("e");
            enemy = collision.gameObject;
            GetComponent<CapsuleCollider>().isTrigger = true;
        }
    }

    public void D(GameObject g)
    {
        if(g == enemy)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(enemy);
            Destroy(gameObject);
        }
    }
}
