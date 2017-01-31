using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.IO;

public class TestLoadComponent : MonoBehaviour {

    public RawImage imageObj;

    public Text textObj;

	// Use this for initialization
	void Start () {
		StartCoroutine(CopyData("test.jpg.bin"));
		StartCoroutine(CopyData("test.bundle"));
		StartCoroutine(CopyData("reuse.jpg.bin"));
	}


    FastLoader.TextureLoader loader = new FastLoader.TextureLoader();

    public void TestLoad()
    {
        string path = Path.Combine( Application.temporaryCachePath , "test.jpg.bin");

        float stTime = Time.realtimeSinceStartup;

        loader.LoadToBuffer(path);
		float loadTime = Time.realtimeSinceStartup;
        var texture = loader.CreateTexture2DFromBuffer(true);
        float endTime = Time.realtimeSinceStartup;

        this.imageObj.texture = texture;

        var sb = new System.Text.StringBuilder(64);
        sb.Append("Texture ").Append( texture.width ).Append( "*").Append( texture.height).Append( "\n" ).
            Append(texture.format ).Append( ":").Append( texture.mipmapCount)
			.Append("\nloadtime:") .Append( (loadTime - stTime) )
			.Append("\nAlltime:") .Append( (endTime - stTime) );
        this.textObj.text = sb.ToString();
    }

	public void Reuse(){
		if (this.imageObj.texture == null) {
			return;
		}
		Texture2D texture = imageObj.texture as Texture2D;
			
		string path = Path.Combine( Application.temporaryCachePath , "reuse.jpg.bin");

		float stTime = Time.realtimeSinceStartup;

		loader.LoadToBuffer(path);
		float loadTime = Time.realtimeSinceStartup;
		loader.ReUseTexture(texture);
		float endTime = Time.realtimeSinceStartup;

		this.imageObj.texture = texture;

		var sb = new System.Text.StringBuilder(64);
		sb.Append("Reuse ").Append( texture.width ).Append( "*").Append( texture.height).Append( "\n" ).
		Append(texture.format ).Append( ":").Append( texture.mipmapCount)
			.Append("\nloadtime:") .Append( (loadTime - stTime) )
			.Append("\nAlltime:") .Append( (endTime - stTime) );
		this.textObj.text = sb.ToString();
	}

	public void LoadTestAssetBundle(){
		string path = Path.Combine( Application.temporaryCachePath , "test.bundle");
		float stTime = Time.realtimeSinceStartup;
		AssetBundle ab = AssetBundle.LoadFromFile (path);
		float afterAssetBundleTime = Time.realtimeSinceStartup;
		Texture2D texture = ab.LoadAsset<Texture2D> ("test");
		float endTime = Time.realtimeSinceStartup;

		this.imageObj.texture = texture;

		var sb = new System.Text.StringBuilder(64);
		sb.Append("AssetBundle ").Append( texture.width ).Append( "*").Append( texture.height).Append( "\n" ).
		Append(texture.format ).Append( ":").Append( texture.mipmapCount)
			.Append("\nAssetBundle:") .Append( (afterAssetBundleTime - stTime) )
			.Append("\nAlltime:") .Append( (endTime - stTime) );
		this.textObj.text = sb.ToString();

		ab.Unload (false);
	}

	IEnumerator CopyData(string file)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "test.jpg.bin");
#else
		string path = "file://" + Path.Combine(Application.streamingAssetsPath, file );
#endif
        WWW www = new WWW( path );
        yield return www;
        string outputPath = Path.Combine( Application.temporaryCachePath , file);
        File.WriteAllBytes( outputPath,www.bytes);   
    }
}
