class_name Rotator
extends MeshInstance3D

@export var speed: Vector3

func _process(delta):
    var d = speed * delta
    rotation_degrees += d
