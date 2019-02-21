using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject decalSource;
    public ModelMovement modelMovement;

    public void OnTattooButtonPressed()
    {
        tattooActivate(true);
    }

    public void OnTattooButtonReleased()
    {
        tattooActivate(false);
    }

    protected void tattooActivate(bool active)
    {
        modelMovement.active = !active;
        decalSource.SetActive(active);
    }
}
