using Unity.VisualScripting;
using UnityEngine;

public class Front_Script : MonoBehaviour
{
    public GameObject Hole { get; private set; } = null;
    public bool Wall { get; private set; } = false;
    public bool Chack()
    {
        if (Hole != null){
            return true;
        }
        else if (Wall) {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "hole")
        {
            Hole = other.gameObject;
        }

        if (other.gameObject.tag == "Wall")
        {
            Wall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(Hole != null)
        {
            if(other.gameObject == Hole)
            {
                Hole = null;
            }
        }

        if (other.gameObject.tag == "Wall")
        {
            Wall = false;
        }
    }
}
