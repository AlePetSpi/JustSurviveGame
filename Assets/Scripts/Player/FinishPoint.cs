using System;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public event EventHandler FinishPassed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnFinishPassed();
        }
    }

    protected virtual void OnFinishPassed()
    {
        FinishPassed?.Invoke(this, EventArgs.Empty);
    }
}
