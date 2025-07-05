using UnityEngine;

public class ButtonAnimatorTester : MonoBehaviour
{
    [Header("Assign Your Animator Here")]
    public Animator animator;

    [ContextMenu("Trigger Normal")]
    public void TriggerNormal()
    {
        ResetAllTriggers();
        animator.SetTrigger("Normal");
        Debug.Log("▶️ Triggered: Normal");
    }

    [ContextMenu("Trigger Hovered")]
    public void TriggerHovered()
    {
        ResetAllTriggers();
        animator.SetTrigger("Hovered");
        Debug.Log("▶️ Triggered: Hovered");
    }

    [ContextMenu("Trigger Pressed")]
    public void TriggerPressed()
    {
        ResetAllTriggers();
        animator.SetTrigger("Pressed");
        Debug.Log("▶️ Triggered: Pressed");
    }

    [ContextMenu("Trigger Selected")]
    public void TriggerSelected()
    {
        ResetAllTriggers();
        animator.SetTrigger("Selected");
        Debug.Log("▶️ Triggered: Selected");
    }

    private void ResetAllTriggers()
    {
        if (animator == null) return;

        foreach (var parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(parameter.name);
        }
    }
}