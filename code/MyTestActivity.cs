using System;
using System.Collections.Generic;
using System.Text.Json;
using libblitz;

namespace BonitoBlitz;

public class MyTestActivity : libblitz.Activity
{
	public class Result : libblitz.ActivityResult
	{
	}

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		Log.Info( ("hellooooooooooo new york!!!!") );

		var next = CreateDescription().Transform( "MyNextTestActivity" );
		Game.Current.PushActivity<ActivityResult>( next, null );
	}

	public override void ActivityEnd()
	{
		base.ActivityEnd();

		Log.Info( "goodbye!!!!!!!" );
	}
}

public class MyNextTestActivity : libblitz.Activity
{
	public class Result : libblitz.ActivityResult
	{
	}

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		Log.Info( "abc" );
	}

	public MyNextTestActivity( ActivityDescription d ) : base( d ) { }
}
