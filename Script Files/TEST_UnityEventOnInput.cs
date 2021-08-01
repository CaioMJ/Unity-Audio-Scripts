using UnityEngine;
using UnityEngine.Events;

public class TEST_UnityEventOnInput : MonoBehaviour
{
    public UnityEvent space;
    public UnityEvent P;
    public UnityEvent S;
    public UnityEvent one, two, three, four, five, six, seven, eight, nine, zero;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            space.Invoke();
        if (Input.GetKeyDown(KeyCode.P))
            P.Invoke();
        if (Input.GetKeyDown(KeyCode.S))
            S.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            one.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            two.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            three.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            four.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha5))
            five.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha6))
            six.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha7))
            seven.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha8))
            eight.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha9))
            nine.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha0))
            zero.Invoke();
    }
}
