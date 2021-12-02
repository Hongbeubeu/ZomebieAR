using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreen : MonoBehaviour
{
    [SerializeField] Image blood;
    [SerializeField] private Color colorBloodStart;
    [SerializeField] private Color colorBloodFinish;

    public IEnumerator FlickerBlood(float duration)
    {
        var elapsed = duration;
        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;
            blood.color = Color.Lerp(colorBloodStart, colorBloodFinish, 1 - elapsed/duration);
            yield return null;
        }
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
