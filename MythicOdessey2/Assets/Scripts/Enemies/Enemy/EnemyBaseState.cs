public abstract class EnemyBaseState
{
    private bool _isRootState;
    private EnemyStateMachine _ctx;
    private EnemyBaseState _currentSuperState;
    private EnemyBaseState _currentSubState;

    protected bool IsRootState {
        set => _isRootState = value;
    }

    protected EnemyStateMachine Ctx => _ctx;

    protected EnemyFactoryState Factory { get; }

    protected EnemyBaseState(EnemyFactoryState factory, EnemyStateMachine ctx) {
        Factory = factory;
        _ctx = ctx;
    }

    public bool SetContext(EnemyStateMachine ctx) {
        _ctx = ctx;
        return _ctx != null;
    }

    public abstract void EnterState();

    public virtual void UpdateState() {
        CheckSwitchStates();
    }

    public abstract void FixedUpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public void FixedUpdateStates() {
        FixedUpdateState();
        if (_currentSubState != null) _currentSubState.FixedUpdateStates();
    }

    public void UpdateStates() {
        UpdateState();
        if (_currentSubState != null) _currentSubState.UpdateStates();
    }

    protected void SwitchState(EnemyBaseState newState) {
        ExitState();
        newState.EnterState();
        if (!_isRootState) _ctx.CurrentSubState = newState;
        if (_isRootState) _ctx.CurrentState = newState;
        else if (_currentSuperState != null) _currentSuperState.SetSubState(newState);
    }

    protected void SetSuperState(EnemyBaseState newSuperState) {
        _currentSuperState = newSuperState;
    }

    protected bool SetSubState(EnemyBaseState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        return _currentSubState != null ? true : false;
    }
   
}