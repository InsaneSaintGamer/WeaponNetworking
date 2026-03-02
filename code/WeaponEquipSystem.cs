using Sandbox;

public sealed class WeaponEquipSystem : Component
{
	[Sync, Change( nameof( OnEquipChanged ) )] public bool IsEquipped { get; set; }
	[Property] public GameObject FirstPersonUspPrefab { get; set; }
	[Property] public GameObject ThirdPersonUsp { get; set; }
	[Property] public SkinnedModelRenderer Body { get; set; }
	public GameObject CameraObject { get; set; }
	public GameObject FirstPersonUspObject { get; set; }

	protected override void OnStart()
	{
		CameraObject = Scene.Camera.GetComponent<CameraComponent>().GameObject;
		FirstPersonUspObject = FirstPersonUspPrefab.Clone( new Transform( Vector3.Zero, Rotation.Identity, Vector3.One ), parent: CameraObject );
		FirstPersonUspObject.BreakFromPrefab();
		FirstPersonUspObject.Enabled = false;
	}

	protected override void OnUpdate()
	{
		if ( IsProxy ) return;

		if ( Input.Pressed( "use" ) )
			ToggleWeapon();
		if ( Input.Pressed( "attack1" ) && IsEquipped )
			Shoot();
	}

	private void ToggleWeapon()
	{
		IsEquipped = !IsEquipped;
	}

	private void OnEquipChanged()
	{
		if ( !IsProxy )
		{
			FirstPersonUspObject.Enabled = IsEquipped;
		}
		ThirdPersonUsp.Enabled = IsEquipped;
		Body.Set( "holdtype", IsEquipped ? 1 : 0 );
	}

	private void Shoot()
	{
		ShootAnim();
		ShootRay();
	}

	[Rpc.Broadcast]
	private void ShootAnim()
	{
		Body.Set( "b_attack", true );
	}

	private void ShootRay()
	{
		var camera = Scene.Camera;
		var startPos = camera.WorldPosition;
		var direction = camera.WorldRotation.Forward;

		var trace = Scene.Trace.Ray( startPos, startPos + direction * 5000f )
			.IgnoreGameObject( GameObject.Root )
			.Run();

		if ( trace.Hit )
		{
			Log.Info( $"Hit: {trace.GameObject.Name}" );
		}
	}
}
