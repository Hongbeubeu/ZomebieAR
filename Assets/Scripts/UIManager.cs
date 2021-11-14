using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    public InitGamePanel initGamePanel;
    public InGamePanel inGamePanel;

    #region Unity Event

    private void Awake()
    {
        initGamePanel.Reset();
        initGamePanel.SetActivePlaceGroundButton(true);
        inGamePanel.SetActive(false);
    }

    #endregion


    public void SetActiveInGamePanel(bool value)
    {
        inGamePanel.gameObject.SetActive(value);
    }

    public void SetActiveInitGamePanel(bool value)
    {
        initGamePanel.gameObject.SetActive(value);
    }
}