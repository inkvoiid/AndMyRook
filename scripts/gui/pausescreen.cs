using Godot;
using System;

public partial class pausescreen : ColorRect
{
    [Export] private CanvasItem _pauseScreen;
    [Export] private Button _exitButton;
    private int _audioBusId;
// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _audioBusId = AudioServer.GetBusIndex("Music");
}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }

    public void OnMusicVolumeChanged(float value)
    {
        AudioServer.SetBusVolumeDb(_audioBusId, Mathf.LinearToDb(value));
        AudioServer.SetBusMute(_audioBusId, value < 0.05);
    }

    public void ResumeGame()
    {
        Visible = false;
    }

    public void QuitMatch()
    {
        GetNode<sceneloader>("/root/sceneloader").ChangeToScene("titlescreen.tscn");
    }
}
