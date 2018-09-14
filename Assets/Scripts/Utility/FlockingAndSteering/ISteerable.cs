using UnityEngine;

public interface ISteerable {
	Vector3 position { get; }
	Vector3 velocity { get; }
	float mass { get; }
	float maxVelocity { get; }
}

