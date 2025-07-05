using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorGenerator
{
    private static readonly string[] StateNames = { "Normal", "Hovered", "Pressed", "Selected" };

    /// <summary>
    /// Generates an Animator Controller with states and transitions for a button animation effect.
    /// </summary>
    /// <param name="controllerPath">The path where to save the Animator Controller.</param>
    /// <returns>The generated AnimatorController.</returns>
    public static AnimatorController GenerateAnimator(string controllerPath)
    {
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

        AnimationClip[] clips = CreateEmptyClipsAsSubAssets(controllerPath);

        AnimatorState[] states = AddStates(controller, clips);

        AddTriggers(controller);

        AddAnyStateTransitions(controller, states);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return controller;
    }

    /// <summary>
    /// Creates empty AnimationClips as sub-assets of the Animator Controller.
    /// </summary>
    /// <param name="controllerPath">The path where the Animator Controller is saved.</param>
    /// <returns>An array of AnimationClips.</returns>
    static AnimationClip[] CreateEmptyClipsAsSubAssets(string controllerPath)
    {
        AnimationClip[] clips = new AnimationClip[StateNames.Length];

        for (int i = 0; i < StateNames.Length; i++)
        {
            AnimationClip clip = new AnimationClip();
            clip.name = StateNames[i];
            AssetDatabase.AddObjectToAsset(clip, controllerPath);
            clips[i] = clip;
        }

        return clips;
    }

    /// <summary>
    /// Adds states to the Animator Controller based on the provided AnimationClips.
    /// </summary>
    /// <param name="controller">The Animator Controller to add states to.</param>
    /// <param name="clips">An array of AnimationClips to create states from.</param>
    /// <returns>An array of AnimatorStates created from the clips.</returns>
    static AnimatorState[] AddStates(AnimatorController controller, AnimationClip[] clips)
    {
        AnimatorState[] states = new AnimatorState[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            states[i] = controller.AddMotion(clips[i]);
            states[i].name = StateNames[i];
        }

        return states;
    }

    /// <summary>
    /// Adds triggers to the Animator Controller for each state.
    /// </summary>
    /// <param name="controller">The Animator Controller to add triggers to.</param>
    static void AddTriggers(AnimatorController controller)
    {
        foreach (var stateName in StateNames)
        {
            controller.AddParameter(stateName, AnimatorControllerParameterType.Trigger);
        }
    }

    /// <summary>
    /// Adds transitions from Any State to each state in the Animator Controller.
    /// </summary>
    /// <param name="controller">The Animator Controller to add transitions to.</param>
    /// <param name="states">An array of AnimatorStates to transition to.</param>
    static void AddAnyStateTransitions(AnimatorController controller, AnimatorState[] states)
    {
        foreach (var state in states)
        {
            var transition = controller.layers[0].stateMachine.AddAnyStateTransition(state);
            transition.hasExitTime = false;
            transition.hasFixedDuration = true;
            transition.duration = 0.1f;
            transition.AddCondition(AnimatorConditionMode.If, 0, state.name);
        }
    }
}
