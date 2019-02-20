using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public TattooPositioning tattooPositioning;
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
        tattooPositioning.active = active;
        modelMovement.active = !active;
    }
}
