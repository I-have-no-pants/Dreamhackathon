using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Kinect;

using System.Xml.Serialization;

public class KinectRecorder : MonoBehaviour {
	
	public DeviceOrEmulator devOrEmu;
	private KinectInterface kinect;
	
	public string outputFile = "Assets/Kinect/Recordings/playback";
	
	
	private bool isRecording = false;
	private ArrayList currentData = new ArrayList();
	
	
	//add by lxjk
	private int fileCount = 0;
	//end lxjk
	
	
	// Use this for initialization
	void Start () {
		kinect = devOrEmu.getKinect();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.F11)){
			SaveFrame();
		}
		
		if(!isRecording){
			if(Input.GetKeyDown(KeyCode.F10)){
				StartRecord();
			}
		} else {
			if(Input.GetKeyDown(KeyCode.F10)){
				StopRecord();
			}
			if (kinect.pollSkeleton()){
				currentData.Add(kinect.getSkeleton());
				//currentData.Add(kinect.getColor());
			}
		}
	}
	
	void StartRecord() {
		isRecording = true;
		Debug.Log("start recording");
	}

	[Serializable]
	public struct ColorEx
	{
		private Color m_color;
		
		public ColorEx(Color color)
		{
			m_color = color;
		}
		
		[XmlIgnore]
		public Color Color
		{ get { return m_color; } set { m_color = value; } }
		
		[XmlAttribute]
		public string ColorHtml { 
			get {
				Debug.Log (""+m_color.r+";"+m_color.g+";"+m_color.b+";"+m_color.a+";");
				return ""+m_color.r+";"+m_color.g+";"+m_color.b+";"+m_color.a+";";
				//return ColorTranslator.ToHtml(this.Color);
			} 
			set {
				//this.Color = ColorTranslator.FromHtml(value);
			}
		}
		
		public static implicit operator Color(ColorEx colorEx)
		{
			return colorEx.Color;
		}
		
		public static implicit operator ColorEx(Color color)
		{
			return new ColorEx(color);
		}
	}
	
	[Serializable]
	public class ColorFrame {
		public Color32[] color;
		public short[] depth;

		public ColorFrame(Color32[] c, short[] d) {
			this.color = c;
			this.depth = d;
		}

	}

	void SaveFrame() {
		isRecording = false;
		//edit by lxjk
		string filePath = outputFile+fileCount.ToString();
		FileStream output = new FileStream(@filePath,FileMode.Create);
		//end lxjk
		BinaryFormatter bf = new BinaryFormatter();

		ColorFrame data = new ColorFrame (kinect.getColor(), kinect.getDepth());



		bf.Serialize(output, data);
		output.Close();
		fileCount++;
		Debug.Log("stop recording");
	}
	
	void StopRecord() {
		isRecording = false;
		//edit by lxjk
		string filePath = outputFile+fileCount.ToString();
		FileStream output = new FileStream(@filePath,FileMode.Create);
		//end lxjk
		BinaryFormatter bf = new BinaryFormatter();
		
		SerialSkeletonFrame[] data = new SerialSkeletonFrame[currentData.Count];
		for(int ii = 0; ii < currentData.Count; ii++){
			data[ii] = new SerialSkeletonFrame((NuiSkeletonFrame)currentData[ii]);
		}
		bf.Serialize(output, data);
		output.Close();
		fileCount++;
		Debug.Log("stop recording");
	}
}
