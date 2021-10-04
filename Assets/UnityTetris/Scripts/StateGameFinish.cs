using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateGameFinish : MonoBehaviour
{
    [SerializeField]
    private float _waitMax = 2.0f;

    [SerializeField]
    private Text _message;

    private GameController _parent;

    public void Setup(GameController parent)
    {
        Debug.Log("StateGameFinish.Setup");
        _parent = parent;
        StartCoroutine(CoRun());
    }

    private IEnumerator CoRun()
    {
        _message.gameObject.SetActive(true); 
        yield return new WaitForSeconds(_waitMax);
        _message.gameObject.SetActive(false);
        _parent.BootGame(); 
        yield return null;
    }
}
