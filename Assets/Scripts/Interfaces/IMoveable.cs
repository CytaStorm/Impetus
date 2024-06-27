using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
	bool Moveable { get; set; }
	float Speed { get; set; }
	void Knockback(Vector2 force);
}
