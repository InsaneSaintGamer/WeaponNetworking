using Sandbox;

public sealed class UspShooting : Component
{
	[Property] public SkinnedModelRenderer Usp { get; set; }

	protected override void OnUpdate()
	{
		if ( IsProxy ) return;
		if ( Input.Pressed( "attack1" ) )
			ShootAnim();
	}

	[Rpc.Broadcast]
	private void ShootAnim()
	{
		Usp.Set( "b_attack", true );
	}
}
