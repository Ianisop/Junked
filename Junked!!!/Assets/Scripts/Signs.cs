using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signs : MonoBehaviour
{
    public List<GameObject> signs;
    // Start is called before the first frame update
    public void RemoveSigns()
    {
        foreach (GameObject sign in signs)
        {
            if (sign == null)
            {
                return;
            }
            Destroy(sign);
        }
    }

}
