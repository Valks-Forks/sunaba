namespace Sunaba.Core;

public partial class UI : Control
{
	private Node global;

	private Window newRoomDialog;
	private Window settingsDialog;
	private Window mapDialog;
	private Window connectDialog;
	private Window acceptDialog1;
	private Window characterWindow;
	private FileDialog userFileDialog;
	private LineEdit mapPath;
	private OptionButton optionButton;
	private Panel pauseMenu;
	private Settings settings;

	public override void _Ready()
	{
		global = GetNode("/root/Global");
		newRoomDialog = GetNode<Window>("NewRoomDialog");
		settingsDialog = GetNode<Window>("SettingsDialog");
		mapDialog = GetNode<Window>("MapDialog");
		connectDialog = GetNode<Window>("ConnectDialog");
		acceptDialog1 = GetNode<AcceptDialog>("AcceptDialog1");
		characterWindow = GetNode<Window>("CharacterWindow");
		userFileDialog = GetNode<FileDialog>("UserFileDialog");
		mapPath = newRoomDialog.GetNode<NewRoom>("NewRoom").GetNode<LineEdit>("MapPath");
		pauseMenu = GetNode<Panel>("PauseMenu");
		optionButton = mapDialog.GetNode<MapPicker>("StandardMapPicker").GetNode<OptionButton>("OptionButton");
		settings = GetNode<Settings>("/root/Settings");

		GetNode<Control>("MainMenu").Show();
		pauseMenu.Hide();

		GetNode<AcceptDialog>("AcceptDialog2").PopupCentered();
	}

	public void OnCreateButtonPressed() => newRoomDialog.PopupCentered();
	public void OnSettingsButtonPressed() => settingsDialog.PopupCentered();
	public void OnFileButtonPressed() => userFileDialog.PopupCentered();
	//GetNode<Node>("NativeDialogManager").Call("show_native_file_dialog");

	public void OnFileMenuPressed(int id)
	{
		if (id == 0)
			mapDialog.PopupCentered();
		else if (id == 1)
			userFileDialog.PopupCentered();
	}
	public void OnFileSelected(String path)
	{
		var parent = GetParent<Main>();
		parent.path = path;
		mapPath.Text = path;
	}

	public void OnConnectButtonPressed()
	{
		//var build = GetNode<Build>("/root/Build");
		if (settings.multiplayerEnabled == true)
			connectDialog.PopupCentered();
		else
			acceptDialog1.PopupCentered();
	}

	public void OnConnectDialogCloseRequested() => connectDialog.Hide();
	public void OnMapDialogCloseRequested() => mapDialog.Hide();

	public void Unpause()
	{
		pauseMenu.Hide();
		Input.MouseMode = Input.MouseModeEnum.Captured;
		global.Set("gamePaused", false);
	}

	public void OnMapSelected()
	{
		var map = optionButton.Text + ".map";
		var mapPath = "res://maps/" + map;

		mapDialog.Hide();
		OnFileSelected(mapPath);
	}

	public void OnNewRoomDialogCloseRequested() => newRoomDialog.Hide();
	public void OnCustomizeButtonPressed() => characterWindow.PopupCentered();
	public void OnCharacterWindowCloseRequested() => characterWindow.Hide();

	public override void _Process(double delta)
	{
		var gameStarted = global.Get("gameStarted");
		var gamePaused = global.Get("gamePaused");
    ThemeManager themeManager = GetNode<ThemeManager>("/root/ThemeManager");
    Theme = themeManager.theme;

		if (gameStarted.AsBool())
			if (gamePaused.AsBool())
				pauseMenu.Show();
			else
				pauseMenu.Hide();
	}
}
