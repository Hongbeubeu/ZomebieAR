using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitGamePanel : MonoBehaviour
{
    public Button placeGroundButton;
    public Button placeTombButton;
    public Button startGameButton;

    public void SetActivePlaceGroundButton(bool value)
    {
        placeGroundButton.gameObject.SetActive(value);
    }

    public void SetActivePlaceTombButton(bool value)
    {
        placeTombButton.gameObject.SetActive(value);
    }

    public void SetActiveStartGameButton(bool value)
    {
        startGameButton.gameObject.SetActive(value);
    }

    public void Reset()
    {
        SetActivePlaceGroundButton(false);
        SetActivePlaceTombButton(false);
        SetActiveStartGameButton(false);
    }
}