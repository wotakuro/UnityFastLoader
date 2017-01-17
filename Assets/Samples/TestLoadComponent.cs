using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.IO;

public class TestLoadComponent : MonoBehaviour {

    public RawImage imageObj;

    public Text textObj;

	// Use this for initialization
	void Start () {
        StartCoroutine(CopyData());
	}


    FastLoader.TextureLoader loader = new FastLoader.TextureLoader();

    public void TestLoad()
    {
        string path = Path.Combine( Application.temporaryCachePath , "test.bin");

        float stTime = Time.realtimeSinceStartup;

        loader.LoadToBuffer(path);
        var texture = loader.CreateTexture2DFromBuffer();
        float endTime = Time.realtimeSinceStartup;

        this.imageObj.texture = texture;

        var sb = new System.Text.StringBuilder(64);
        sb.Append("Texture ").Append( texture.width ).Append( "*").Append( texture.height).Append( "\n" ).
            Append(texture.format ).Append( ":").Append( texture.mipmapCount) .Append("\ntime:") .Append( (endTime - stTime) );
        this.textObj.text = sb.ToString();
    }


    IEnumerator CopyData()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "test.jpg.bin");
#else
        string path = "file://" + Path.Combine(Application.streamingAssetsPath, "test.jpg.bin");
#endif
        WWW www = new WWW( path );
        yield return www;
        string outputPath = Path.Combine( Application.temporaryCachePath , "test.bin");
        File.WriteAllBytes( outputPath,www.bytes);
        
    }
}
