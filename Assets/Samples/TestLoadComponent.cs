using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.IO;

public class TestLoadComponent : MonoBehaviour {

    public RawImage imageObj;

    public Text textObj;
	private Texture lastLoad;
	private int rawImageNum = 0;
	private int nativeNum = 0;

	private FastLoader.NativeTexture2D nativeTexture;

	// Use this for initialization
	void Start () {
		StartCoroutine(CopyData("test.jpg.bin"));
		StartCoroutine(CopyData("test.bundle"));
		StartCoroutine(CopyData("reuse.jpg.bin"));
	}


    FastLoader.TextureLoader loader = new FastLoader.TextureLoader();

	public void NativeLoadTest(){
		string path = Path.Combine( Application.temporaryCachePath , "test.jpg.bin");

		float stTime = Time.realtimeSinceStartup;

		if (this.nativeTexture != null) {
			this.nativeTexture.Dispose ();
		}
		loader.LoadToBuffer(path);
		float loadTime = Time.realtimeSinceStartup;
		this.nativeTexture = loader.CreateNativeTextureFromBuffer();
		float endTime = Time.realtimeSinceStartup;

		this.imageObj.texture = nativeTexture;

		this.PrintTexture (nativeTexture, nativeNum,stTime , loadTime,endTime);
		++nativeNum;
	}
	public void NativeAutoTest(){
		StartCoroutine (LoadSpan (NativeLoadTest));
	}



    public void TestLoad()
    {
		if (lastLoad != null) {
			Object.Destroy (lastLoad);
		}
        string path = Path.Combine( Application.temporaryCachePath , "test.jpg.bin");

        float stTime = Time.realtimeSinceStartup;

        loader.LoadToBuffer(path);
		float loadTime = Time.realtimeSinceStartup;
        var texture = loader.CreateTexture2DFromBuffer();
        float endTime = Time.realtimeSinceStartup;

		lastLoad = texture;
        this.imageObj.texture = texture;

		this.PrintTexture (texture, rawImageNum,stTime , loadTime,endTime);
		++rawImageNum;
    }

	private void PrintTexture(Texture2D texture,int num,float stTime , float loadTime,float endTime){
		var sb = new System.Text.StringBuilder(64);
		sb.Append ("Num ").Append (num).Append ("\n");
        if (texture != null)
        {
            sb.Append("Texture ").Append(texture.width).Append("*").Append(texture.height).Append("\n")
                .Append(texture.format).Append(":").Append(texture.mipmapCount)
                .Append("\nloadtime:").Append((loadTime - stTime))
                .Append("\nAlltime:").Append((endTime - stTime));
        }
		this.textObj.text = sb.ToString();

	}


	public void AutoTest(){
		StartCoroutine (LoadSpan (TestLoad));
	}
	private IEnumerator LoadSpan(System.Action act){
		for (int i = 0; i < 120; ++i) {
			yield return new WaitForSeconds (0.3f);
			act ();
		}
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
