﻿using UnityEngine;

[ExecuteInEditMode]
public class TextureCreator : MonoBehaviour {
  
  public NoiseMethodType type = NoiseMethodType.Value;
  [Range(1, 3)] public int dimensions = 3;
  [Range(1, 8)] public int octaves = 1;
  [Range(1f, 4f)] public float lacunarity = 2f;
	[Range(0f, 1f)] public float persistence = 0.5f;
  public float frequency = 1f;
  public int resolution = 256;

  public Material material;
  public Gradient coloring;
  
  private Texture2D texture;

	private void OnEnable () {
		texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);

    texture.name       = "Procedural Texture";
    texture.wrapMode   = TextureWrapMode.Clamp;
    texture.filterMode = FilterMode.Trilinear;
    texture.anisoLevel = 9;

    /* This is the property namer for the base texture in the HDRP/Lit shader */
    material.SetTexture("_BaseColorMap", texture);

    GetComponent<MeshRenderer>().sharedMaterial = material;

    FillTexture();
	}

  private void Update () {
    FillTexture();
	}

  private void FillTexture () {
    NoiseMethod method = Noise.noiseMethods[(int)type][dimensions - 1];
    
    float stepSize = 1f / resolution;

    Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));
		
    for (int y = 0; y < resolution; y++) {
      Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			
      for (int x = 0; x < resolution; x++) {
        Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);

        NoiseSample sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);

        sample = sample * 0.5f + 0.5f;
				
        texture.SetPixel(x, y, Color.white * coloring.Evaluate(sample.value));
      }
    }
		
    texture.Apply();
	}
}
