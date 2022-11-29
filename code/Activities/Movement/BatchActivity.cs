using System.Collections.Generic;
using System.Linq;
using BonitoBlitz.Activities.CoreBoard;
using BonitoBlitz.Entities.CoreBoard;
using libblitz;
using Sandbox;

namespace BonitoBlitz.Activities.Movement;

/// <summary>
/// Batches as many static tiles as possible into one animation
/// </summary>
public partial class BatchActivity : Activity
{
	/* These constructors are required! */
	public BatchActivity( ActivityDescription d ) : base( d ) { }
	public BatchActivity() { }

	/// <summary>
	/// Networkable details for a part of the animation
	/// </summary>
	protected struct AnimationPartMeta
	{
		public int TileNetIdent;
		public int NextTileNetIdent;
		public float Time;
		public float Length;
	}

	/// <summary>
	/// Wrapper for AnimationPartMeta
	/// </summary>
	protected class AnimationPart
	{
		/// <summary>
		/// Create AnimationPart for provided <see cref="AnimationPartMeta"/>
		/// </summary>
		/// <param name="meta">AnimationPartMeta</param>
		public AnimationPart( AnimationPartMeta meta )
		{
			_meta = meta;

			foreach ( var tile in Entity.All.OfType<BaseTile>() )
			{
				if ( Tile != null && NextTile != null )
				{
					break;
				}

				if ( tile.NetworkIdent == _meta.TileNetIdent )
				{
					Tile = tile;
				}

				if ( tile.NetworkIdent == _meta.NextTileNetIdent )
				{
					NextTile = tile;
				}
			}
		}

		private readonly AnimationPartMeta _meta;

		/// <summary>
		/// Start time of animation
		/// </summary>
		public float Start => _meta.Time;

		/// <summary>
		/// End time of animation
		/// </summary>
		public float End => _meta.Time + _meta.Length;

		/// <summary>
		/// Length of animation
		/// </summary>
		public float Length => _meta.Length;

		/// <summary>
		/// Progress of animation (0-1, float)
		/// </summary>
		public float Progress => 1 - ((End - Time.Now) / Length);

		public bool HasStarted = false;
		public bool HasEnded = false;

		public BaseTile Tile { get; init; }
		public BaseTile NextTile { get; init; }
	}

	private int _movesExpected = 0;

	[Net] private bool PlaybackReady { get; set; }

	[Net, Change( "CreatePlaybackData" )]
	protected IList<AnimationPartMeta> AnimationPartMetas { get; set; } = new List<AnimationPartMeta>();

	private IList<AnimationPart> _parts = new List<AnimationPart>();
	private int _completedParts = 0;
	private bool PlaybackComplete => _completedParts == _parts.Count;
	private bool _completed = false;

	private GameMember _actor;

	/// <summary>
	/// Create playback data from all animation parts
	/// </summary>
	protected void CreatePlaybackData()
	{
		if ( _parts.Count != 0 )
		{
			LogWarn( "CreatePlaybackData called with populated _parts" );
		}

		_parts.Clear();

		foreach ( var meta in AnimationPartMetas )
		{
			_parts.Add( new AnimationPart( meta ) );
		}

		PlaybackReady = true;
	}

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		_actor = Actors.First();

		// Make sure we use BoardPawn
		_actor.UseOrCreatePawn<BoardPawn>();

		// Make sure we got the right ActivityResult
		if ( result is not MoveHostActivity.Result moveHostResult )
		{
			throw new UnexpectedActivityResultException( "BatchActivity requires a MoveHostActivity.Result" );
		}

		_movesExpected = moveHostResult.Moves;

		var tile = _actor.CurrentTile;
		var time = Time.Now;

		for ( var i = 0; i < _movesExpected; i++ )
		{
			if ( tile is not IStaticTile staticTile )
			{
				continue;
			}

			float length = 0;
			if ( tile is IBasicAnimationTile animationTile )
			{
				length = animationTile.AnimationLength;
			}

			AnimationPartMetas.Add(
				new AnimationPartMeta()
				{
					TileNetIdent = tile.NetworkIdent,
					NextTileNetIdent = staticTile.NextTile.NetworkIdent,
					Time = time,
					Length = length,
				}
			);

			time += length;
			tile = staticTile.NextTile;
		}

		CreatePlaybackData();

		LogInfo( $"Batched {_movesExpected} static tile(s)" );
	}

	public override void ActivityClientStart()
	{
		base.ActivityClientStart();

		_actor = Actors.First();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( _completed )
		{
			return;
		}

		if ( PlaybackComplete )
		{
			LogInfo( $"Now: {Time.Now}, PC: {PlaybackComplete}" );
			_completed = true;

			if ( Host.IsServer )
			{
				// Pop activity, we're done here!
				Game.Current.PopActivity( new MoveHostActivity.Expectation()
				{
					MovesExpected = _movesExpected, MovesPerformed = _completedParts
				} );
				return;
			}

			return;
		}

		foreach ( var part in _parts )
		{
			if ( part.HasEnded )
			{
				continue;
			}

			// Check if part animation should start
			if ( !part.HasStarted && Time.Now >= part.Start )
			{
				// Start animation
				part.HasStarted = true;

				if ( part.Tile is IBasicAnimationTile bat )
				{
					bat.StartAnimation( _actor, part.NextTile );
				}

				continue;
			}

			// Check if part animation should end
			if ( Time.Now > part.End )
			{
				// End animation
				part.HasEnded = true;

				if ( part.Tile is IBasicAnimationTile bat )
				{
					bat.EndAnimation( _actor, part.NextTile );
				}

				_actor.CurrentTile = part.NextTile;

				if ( part.NextTile is IActionTile at )
				{
					at.OnPass( _actor );
				}

				_completedParts++;
			}

			// Check if part animation should update
			if ( part.HasStarted )
			{
				// Update animation
				if ( part.Tile is IBasicAnimationTile bat )
				{
					bat.UpdateAnimation( _actor, part.NextTile, part.Progress );
				}
			}
		}
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		if ( _completed )
		{
			return;
		}

		foreach ( var part in _parts )
		{
			if ( part.HasEnded || !part.HasStarted )
			{
				continue;
			}

			// Update animation
			if ( part.Tile is IBasicAnimationTile bat )
			{
				bat.UpdateAnimation( _actor, part.NextTile, part.Progress );
			}
		}
	}
}
