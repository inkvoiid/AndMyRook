using Godot;
using System;
using System.Collections.Generic;

public partial class chessboard : Node
{

	// Art related
	[Export] private PackedScene _tileScene;
	[Export] private Material _tileMaterial;
	[Export] private float _tileSize = 1.0f;
	[Export] private float _yOffset = 0.2f;
	[Export] private Vector3 _boardCenter = Vector3.Zero;
	[Export] private float _deadPieceScale = 0.5f;
	[Export] private float _deadPieceSpacing = 0.3f;
	[Export] private float _dragOffset = 1.0f;
	// TODO - _victoryScreen
	// TODO - _pauseScreen
	// TODO - _checkOverlay

	// Logic
	private List<Vector2> _availableMoves = new List<Vector2>();
	[Export] private int _tileCountX = 8;
	[Export] private int _tileCountY = 8;
	private Node3D[,] _tiles;
	[Export] private Node _chessboard;
    private static Camera3D _currentCamera;
    private RayCast3D _raycast = new RayCast3D();
	private Vector3 _bounds;

    [Export] float rayLength = 100f;

    // Private
    private Basis _identityBasis = Basis.Identity;

	// Called when the node enters the scene tree for the first time.
	// ? The equivalent of the start function in Unity
	public override void _Ready()
	{
		// TODO - Verify the music is going, probably should be in a different script anyhow

		// TODO - Clear the moveHistory

		// TODO - Reset the turn to white

		CallDeferred(nameof(GenerateAllTiles), _tileSize, _tileCountX, _tileCountY);

        _currentCamera = GetViewport().GetCamera3D();

		// Configure the _raycast
		AddChild(_raycast);
        _raycast.Enabled = true;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// ? The equivalent of the update function in Unity
	public override void _Process(double delta)
	{
    }

    public override void _PhysicsProcess(double delta)
    {
        // TODO - Raycast to highlight a square when the mouse is over it

        PhysicsDirectSpaceState3D spaceState = GetViewport().World3D.DirectSpaceState;
        Vector2 mousePos = GetViewport().GetMousePosition();

        // Set ray origin and direction
        Vector3 rayOrigin = _currentCamera.ProjectRayOrigin(mousePos);
        Vector3 rayDirection = _currentCamera.ProjectRayNormal(mousePos);
        Vector3 rayEnd = rayOrigin + rayDirection * rayLength;
        PhysicsRayQueryParameters3D rayQuery = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
		
		rayQuery.CollideWithAreas = true;

		var rayResult = spaceState.IntersectRay(rayQuery);

        if (rayResult.Count > 0)
        {
			GD.Print(rayResult);
        }

    }

    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
		_bounds = new Vector3((tileCountX / 2) * (tileSize * 2), 0, (tileCountY / 2) * (tileSize * 2)) + _boardCenter;
		_tiles = new Node3D[tileCountX, tileCountY];

		for (int x = 0; x < tileCountX; x++)
		{
			for (int y = 0; y < tileCountY; y++)
			{
				_tiles[x, y] = GenerateTile(tileSize, y, x);
			}
		}
	}

	private Node3D GenerateTile(float tileSize, int x, int y)
    {
        Node3D tile = _tileScene.Instantiate<Node3D>();
		tile.Name = $"{x}, {y}";
		tile.Scale = new Vector3(tileSize, tileSize, tileSize);

        Node directChild = tile.GetChild(0);
        if (directChild is MeshInstance3D meshChild)
        {
			meshChild.MaterialOverride = _tileMaterial;
        }
        _chessboard.AddChild(tile);

        tile.Translate(new Vector3(x * (tileSize*2), _yOffset, y * (tileSize * 2)) - _bounds);

        return tile;
    }
}
