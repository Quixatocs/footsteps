public interface IState
{
    /// <summary>
    /// Returns true when the state is complete
    /// </summary>
    bool IsComplete { get; }
    
    /// <summary>
    /// Next state for the state machine to go to once this one is complete
    /// </summary>
    IState NextState { get; }
    
    /// <summary>
    /// Method to be called when entering the state
    /// </summary>
    void OnEnter(StateMachine controller);
    
    /// <summary>
    /// Method to be called when exiting the state
    /// </summary>
    void OnExit();

    /// <summary>
    /// Method for triggering the default next state transition///../////////
    /// </summary>
    void ProgressState();
}