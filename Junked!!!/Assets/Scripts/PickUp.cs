using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    public string actionKey;
    public abstract void UpdateMe();

    public abstract void Interact();

}
