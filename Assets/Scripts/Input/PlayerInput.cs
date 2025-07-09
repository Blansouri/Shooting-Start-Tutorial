using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "PlayerInput")]//通过菜单创建指定脚本文件的资源文件
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions,InputActions.IPauseMenuActions,InputActions.IGameOverScreenActions
{ 
    public event UnityAction<Vector2> onMove = delegate{};//声明事件
    public event UnityAction onStopMove = delegate{};
    public event UnityAction onFire = delegate{};
    public event UnityAction onStopFire = delegate{};
    public event UnityAction onDodge = delegate{};
    public event UnityAction onOverdrive = delegate{};
    public event UnityAction onPause = delegate{};
    public event UnityAction onUnpause = delegate{};
    public event UnityAction onLaunchMissile = delegate{};
    public event UnityAction onConfirmGameOver = delegate{};
    
    InputActions inputActions;

    void OnEnable()
    {
        inputActions = new InputActions();//初始化类的引用

        inputActions.Gameplay.SetCallbacks(this);//登记Gameplay动作表的回调函数
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInput();//调用禁止输入
    }

    void SwitchActionMap(InputActionMap actionMap,bool isUIInput)//切换动作表
    {
        inputActions.Disable();
        actionMap.Enable();
        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SwitchToDynamicUpdateMode()//切换到动态更新模式
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    }

    public void SwitchToFixedUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    }

    public void DisableAllInput()
    {
        inputActions.Disable();
    }

    public void EnableGameplayInput()
    {
        SwitchActionMap(inputActions.Gameplay,false);
    }

    public void EnablePauseMenuInput()
    {
        SwitchActionMap(inputActions.PauseMenu,true);
    }

    public void EnableGameOverScreenInput()
    {
        SwitchActionMap(inputActions.GameOverScreen,true);
    }

   public void OnOnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)//持续按下保持移动
        {
            onMove.Invoke(context.ReadValue<Vector2>());//传入输入的二维向量的值，以调用事件
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)//启用射击时间
    {
        if(context.phase == InputActionPhase.Performed)//持续按下保持射击
        {
            onFire.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)//启用闪避事件
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnpause.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
}
