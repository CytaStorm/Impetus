using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	float Health { get; set; }
	float MaxHealth { get; set; }
	void TakeDamage(float damage, float multiplier);
	void Die();
}
