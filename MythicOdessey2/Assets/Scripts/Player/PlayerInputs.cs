using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs
{
    private Player _playerInput;
    private Vector2 _movementInput;
    private PlayerManager _playerManager;
    
    
    //crear constructor
    public PlayerInputs(PlayerManager playerManager)
    {
        _playerManager = playerManager;
    }

    public void ArtificialAwake(){
        _playerInput = new Player();
        _playerInput.PlayerActions.Move.performed += OnMovementInput;
        _playerInput.PlayerActions.Move.canceled += OnMovementInput;
        
        _playerInput.PlayerActions.Attack.started += OnSlashInput;
        _playerInput.PlayerActions.Attack.canceled += OnSlashInput;
        
        // _playerInput.PlayerActions.Dash.started += OnDashInput;
        // _playerInput.PlayerActions.Dash.canceled += OnDashInput;
        
        _playerInput.PlayerActions.OpenCardMenu.started += OnOpenCardMenu;
        _playerInput.PlayerActions.OpenCardMenu.canceled += OnOpenCardMenu;

        _playerInput.PlayerActions.Interact.started += OnInteractionInput;
        _playerInput.PlayerActions.Interact.canceled += OnInteractionInput;

        _playerInput.PlayerActions.ChangeSelected.started += ChangeSelectedCard;
        _playerInput.PlayerActions.ChangeSelected.canceled += ChangeSelectedCard;
    }
    private void OnMovementInput(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
        _playerManager.dir.x = _movementInput.x;
        _playerManager.dir.z = _movementInput.y;
    }
    private void OnSlashInput(InputAction.CallbackContext ctx)
    {
        _playerManager.IsAttackPressed = ctx.ReadValueAsButton();
        _playerManager.RequireNewAttackPress = false;
        // if (ctx.started && CardMenuManager.Instance.menuOpen)
        // {
        //     CardMenuManager.Instance.GetCurrentCardSelected().TriggerInstantiateEvent();
        // }
    }

    private void OnDashInput(InputAction.CallbackContext ctx){
        _playerManager.IsDashPressed = ctx.ReadValueAsButton();
        _playerManager.RequireNewDashPress = false;
    }
    
    private void OnOpenCardMenu(InputAction.CallbackContext ctx){
        //CardMenuManager.Instance.menuOpen = ctx.ReadValueAsButton();
       // CardMenuManager.Instance.OpenMenu(CardMenuManager.Instance.menuOpen);
    }

    private void ChangeSelectedCard(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
            CardMenuManager.Instance.ChangeSelected();
    }
    
    private void OnInteractionInput(InputAction.CallbackContext ctx)
    {    
        if(ctx.started)
            _playerManager.TryInteraction();
    }

    public void OnEnable(){
        _playerInput.Enable();
    }

    public void DisablePI()
    {
        _playerInput.Disable();
    }public void EnablePI()
    {
        _playerInput.Enable();
    }
}
