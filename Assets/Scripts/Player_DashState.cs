using UnityEngine;

public class Player_DashState : EntityState
{

    private float originalGravityScale;
    private int dashDir;
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        dashDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.FacingDir;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();

        player.SetVelocity(dashDir * player.dashSpeed, rb.linearVelocity.y);

        CancelDashWithTimer();

    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
    }

    private void CancelDashWithTimer()
    {
        if (stateTimer <= 0)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
