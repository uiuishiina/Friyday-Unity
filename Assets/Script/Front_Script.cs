using Unity.VisualScripting;
using UnityEngine;

public class Front_Script : MonoBehaviour
{
    public GameObject Hole { get; private set; } = null;
    public bool Wall { get; private set; } = false;
    GameObject Enemy = null;
    public bool Chack()
    {
        if (Hole != null) {
            return true;
        }
        else if (Wall) {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hole") {
            Hole = other.gameObject;
            Debug.Log("Front");
        }

        if (other.gameObject.tag == "Wall") {
            Wall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(Hole != null) {
            if(other.gameObject == Hole) {
                Hole = null;
            }
        }

        if (other.gameObject.tag == "Wall") {
            Wall = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            Enemy = collision.gameObject;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (Enemy != null) {
            if (collision.gameObject == Enemy) {
                Enemy = null;
            }
        }
    }

    private void OnDestroy()
    {
        if (Enemy != null) {
            Destroy(Enemy);
        }
    }
}
