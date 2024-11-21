using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    private Button ToggleListener;
    [SerializeField]
    private GameObject OnButton;
    [SerializeField]
    private GameObject OffButton;
    [SerializeField]
    private bool _value;
    public bool value { get { return _value; } }

    // Start is called before the first frame update
    void Start()
    {
        setValue(true);
    }

    /// <summary>
    /// 실제 토글 On/Off 버튼의 클릭에 대한 이벤트 처리기입니다.
    /// </summary>
    /// <param name="toggleValue"></param>
    public void onClick(bool toggleValue)
    {
        setValue(toggleValue);
        ToggleListener.onClick.Invoke();
    }

    private void setValue(bool value)
    {
        _value = value;
        OnButton.SetActive(_value);
        OffButton.SetActive(!_value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
