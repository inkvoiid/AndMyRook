using Godot;
using System;

public partial class titlescreen : TextureRect
{
	[Export] private Button _play;

    [Export] private CanvasItem _loadGameScreen;

    private int _audioBusId;

    public override void _Ready()
	{
		_play.Pressed +=
		() => GetNode<sceneloader>("/root/sceneloader").ChangeToScene("gamescreen.tscn");
        _audioBusId = AudioServer.GetBusIndex("Music");
    }

	public void ExitGame()
	{
		GetTree().Quit();
	}

    public void OnMusicVolumeChanged(float value)
    {
		AudioServer.SetBusVolumeDb(_audioBusId, Mathf.LinearToDb(value));
		AudioServer.SetBusMute(_audioBusId, value < 0.05);
    }

    public void OpenLoadScreen()
    {
        _loadGameScreen.Visible = true;
    }

	public void CloseLoadScreen()
    {
        _loadGameScreen.Visible = false;
    }
}
