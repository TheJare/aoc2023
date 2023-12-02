extends Node3D


# Called when the node enters the scene tree for the first time.
func _ready():
    go_to(%MainMenu)
    %r1.gui_input.connect(_on_press_result.bind(%r1))
    %r2.gui_input.connect(_on_press_result.bind(%r2))

func _on_press_result(event: InputEvent, result: Label):
    if event is InputEventMouseButton:
        if event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
            DisplayServer.clipboard_set(result.text)

func go_to(screen: Control):
    %MainMenu.hide()
    %DayUI.hide()
    screen.show()

func go_to_main():
    %DayUI.EndDay()
    go_to(%MainMenu)
    %MenuDecoration.show();

func go_to_day(day: int):
    go_to(%DayUI)
    %MenuDecoration.hide();
    %DayUI.Result("-- part 1 --", "-- part 2 --")
    %DayUI.RunDay(day)
