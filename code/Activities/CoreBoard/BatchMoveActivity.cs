using System.Collections.Generic;
using System.Linq;
using BonitoBlitz.Entities.CoreBoard;
using libblitz;
using Sandbox;
using Game = libblitz.Game;

namespace BonitoBlitz.Activities.CoreBoard;

public partial class BatchMoveActivity : libblitz.Activity
{
	/* These constructors are required! */
	public BatchMoveActivity( ActivityDescription d ) : base( d ) { }
	public BatchMoveActivity() { }

	/// <summary>
	/// This is for data you want to pass to the next activity.
	/// Create an instance of this and provide it to PushActivity / PopActivity
	/// </summary>
	public class Result : MoveControllerActivity.Expectation
	{
	}

	/// <summary>
	/// Part of the walk animation
	/// </summary>
	protected struct AnimationPart
	{
		public int NetIdent;

		/// <summary>
		/// NetIdent of next tile
		/// </summary>
		public int NextNetIdent;

		public float Time;
		public float Length;
	}

	protected class AnimationPlaybackPart
	{
		public AnimationPlaybackPart( AnimationPart part )
		{
			Part = part;

			Tile = All.OfType<BaseTile>()
				.SingleOrDefault( v => v.NetworkIdent == Part.NetIdent );

			NextTile = All.OfType<BaseTile>()
				.SingleOrDefault( v => v.NetworkIdent == Part.NextNetIdent );
		}

		public AnimationPart Part { get; init; }
		public bool HasFinished;
		public bool HasStarted;

		public float EndTime => Part.Time + Part.Length;
		public float Progress => 1 - ((EndTime - Time.Now) / Part.Length); // this seems... wrong??

		public BaseTile Tile { get; init; }
		public BaseTile NextTile { get; init; }
	}

	[Net, Change( "CreatePlaybackData" )]
	protected IList<AnimationPart> AnimationParts { get; set; } = new List<AnimationPart>();

	[Net] protected float StartTime { get; set; }
	[Net] protected float EndTime { get; set; }

	private int _moves;
	private bool _complete; // this sucks

	private readonly List<AnimationPlaybackPart> _playbackData = new();

	/// <summary>
	/// Create playback data from all animation parts
	/// </summary>
	private void CreatePlaybackData()
	{
		if ( _playbackData.Count != 0 )
		{
			Log.Warning( "CreatePlaybackData called with populated _playbackData" );
		}

		_playbackData.Clear();
		foreach ( var part in AnimationParts )
		{
			_playbackData.Add( new AnimationPlaybackPart( part ) );
		}
	}

	private GameMember _actor;

	/// <summary>
	/// Called serverside when the activity is ready to start.
	/// </summary>
	/// <param name="result">Result of previous activity or null</param>
	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		_actor = Actors.First();

		StartTime = Time.Now;

		if ( result is not MoveControllerActivity.Result moveResult )
		{
			Log.Info( $"Unknown argument type {result.GetType().Name}" );
			return;
		}

		_moves = moveResult.Moves;

		// Create animation parts
		var tile = _actor.CurrentTile;
		var time = StartTime;
		for ( var i = 0; i < moveResult.Moves; i++ )
		{
			if ( tile is not IStaticTile staticTile )
			{
				break;
			}

			float length = 0;
			if ( tile is IBasicAnimationTile tileAnimation )
			{
				length = tileAnimation.AnimationLength;
			}

			AnimationParts.Add( new AnimationPart()
			{
				NetIdent = tile.NetworkIdent,
				NextNetIdent = staticTile.NextTile.NetworkIdent,
				Time = time,
				Length = length,
			} );

			time += length;
			tile = staticTile.NextTile;
		}

		EndTime = time;

		Log.Info( $"Batch moving {AnimationParts.Count} static tile(s)" );

		CreatePlaybackData();
	}

	public override void ActivityClientStart()
	{
		base.ActivityClientStart();

		_actor = Actors.First();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( _complete )
		{
			return;
		}

		if ( Time.Now > EndTime )
		{
			if ( !Host.IsServer )
			{
				return;
			}

			// This activity has done what is needed
			Game.Current.PopActivity( new Result() { Moves = _moves - AnimationParts.Count } ); // todo: this sucks
			_complete = true;

			return;
		}

		foreach ( var data in _playbackData.Where( data => !data.HasFinished ) )
		{
			if ( !data.HasStarted && Time.Now > data.Part.Time )
			{
				// if animation hasn't started and it should be...
				data.HasStarted = true;

				if ( data.Tile is IBasicAnimationTile basicAnimationTile )
				{
					basicAnimationTile.StartAnimation( _actor, data.NextTile );
				}

				continue;
			}

			if ( Time.Now > data.Part.Time + data.Part.Length )
			{
				// if animation should be over...
				data.HasFinished = true;

				if ( data.Tile is IBasicAnimationTile basicAnimationTile )
				{
					basicAnimationTile.EndAnimation( _actor, data.NextTile );
				}

				_actor.CurrentTile = data.NextTile;
			}

			if ( !data.HasStarted )
			{
				continue;
			}

			{
				if ( data.Tile is IBasicAnimationTile basicAnimationTile )
				{
					basicAnimationTile.UpdateAnimation( _actor, data.NextTile, data.Progress );
				}
			}
		}
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		foreach ( var data in _playbackData.Where( data => !data.HasFinished && data.HasStarted ) )
		{
			if ( data.Tile is IBasicAnimationTile basicAnimationTile )
			{
				basicAnimationTile.UpdateAnimation( _actor, data.NextTile, data.Progress );
			}
		}
	}
}
