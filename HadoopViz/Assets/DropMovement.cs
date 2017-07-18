using UnityEngine;
using System.Collections;

public class DropMovement : MonoBehaviour{

	ParticleSystem drops;
	ParticleSystem.Particle[] particles;
	//ParticleSystem.VelocityOverLifetimeModule velocity;
	//ParticleSystem.ForceOverLifetimeModule force;
	//public Transform receiver;

	void Awake(){
		drops = GetComponent<ParticleSystem> ();
	}

	/*void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			GameObject zero = new GameObject ();
			SendDrop (zero.transform);
			Destroy (zero);
		}
	}*/

	public void SendDrop(Transform receiver){
		float dist = Vector3.Distance (transform.position,receiver.position) / 2f;
		if(dist != 0 && transform != receiver){
			float z = (receiver.position.z - transform.position.z) / dist;
			float y = dist;
			float x = (receiver.position.x - transform.position.x) / dist;

			drops.Emit (1);

			if(particles == null || particles.Length < drops.main.maxParticles){
				particles = new ParticleSystem.Particle[drops.main.maxParticles];
			}

			int numParticles = drops.GetParticles (particles);

			if(numParticles != 0){
				particles [numParticles - 1].velocity = new Vector3(x,y,z);

				drops.SetParticles (particles, numParticles);
			}
		}
	}

}