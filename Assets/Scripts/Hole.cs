using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoleState
{
	None,
	Open,
	Idle,
	Close,
	Catch
}


public class Hole : MonoBehaviour {

	public MoleState MS;

	//이미지들의 묶음
	public Texture[] Open_Images;
	public Texture[] Idle_Images;
	public Texture[] Close_Images;
	public Texture[] Catch_Images;

	//어떤 두더지인지 체크하기 위한 값
	public bool GoodMole;
	public int PerGood = 15;

	//이미지들의 묶음
	public Texture[] Open_Images_2;
	public Texture[] Idle_Images_2;
	public Texture[] Close_Images_2;
	public Texture[] Catch_Images_2;

	//애니메이션 속도관리를 위한 변수
	public float Ani_Speed;
	public float _now_ani_time;

	//애니메이션 카운트
	int Ani_count;

	//사운드 플레이용
	public AudioClip Open_Sound;
	public AudioClip Catch_Sound;

	//게임매니져에 접근하기 위한 용도의 변수(GM이라는 이름으로 불러옴)
	public GameManager GM;



	
	// Update is called once per frame
	void Update () {
		if (_now_ani_time >= Ani_Speed) {
			
			if (MS == MoleState.Open) {
				Open_Ing ();
			}
			if (MS == MoleState.Idle) {
				Idle_Ing ();
			}
			if (MS == MoleState.Close) {
				Close_Ing ();
			}
			if (MS == MoleState.Catch) {
				Catch_Ing ();
			}
			_now_ani_time = 0;
		} else {
			_now_ani_time += Time.deltaTime;
		}
		
	}

	public void Open_On()
	{
		MS = MoleState.Open;
		Ani_count = 0;

		AudioSource audio = GetComponent<AudioSource> ();
		audio.clip = Open_Sound;
		audio.Play ();

		int a = Random.Range (0, 100);
		if (a <= PerGood) {
			GoodMole = true;
		} else {
			GoodMole = false;
		}
		if (GM.GS == GameState.Ready) {
			GM.GO();
		}
	}
	public void Open_Ing()
	{
		if (GoodMole == false) {
			GetComponent<Renderer> ().material.mainTexture = Open_Images [Ani_count];
		} else {
			GetComponent<Renderer> ().material.mainTexture = Open_Images_2 [Ani_count];
		}
		Ani_count += 1;
		if (Ani_count >= Open_Images.Length){
			//Open 애니메이션이 끝나는 시점
			Idle_On();
		}
	}

	public void Idle_On()
	{
		MS = MoleState.Idle;
		Ani_count=0;
	}
	public void Idle_Ing()
	{ 
		if (GoodMole == false) {
			GetComponent<Renderer> ().material.mainTexture = Idle_Images [Ani_count];
		} else {
			GetComponent<Renderer> ().material.mainTexture = Idle_Images_2 [Ani_count];
		}
		Ani_count+=1;
		if (Ani_count >= Idle_Images.Length){
			Close_On();
		}
	}

	public void Close_On()
	{
		MS = MoleState.Close;
		Ani_count = 0;
	}
	public void Close_Ing()
	{
		if (GoodMole == false) {
			GetComponent<Renderer> ().material.mainTexture = Close_Images [Ani_count];
		} else {
			GetComponent<Renderer> ().material.mainTexture = Close_Images_2 [Ani_count];
		}
		Ani_count += 1;
		if (Ani_count >= Close_Images.Length) {
			// Close애니메이션이 끝나는 시점
			StartCoroutine("Wait");
		}
	}

	public void Catch_On()
	{ 
		MS = MoleState.Catch;
		Ani_count=0;

		AudioSource audio = GetComponent<AudioSource> ();
		audio.clip = Catch_Sound;
		audio.Play ();

		if (GoodMole == false) {
			GM.Count_Bad += 1;
		} else {
			GM.Count_Good += 1;
		}
	}
	public void Catch_Ing()
	{
		if (GoodMole == false) {
			GetComponent<Renderer> ().material.mainTexture = Catch_Images [Ani_count];
		} else {
			GetComponent<Renderer> ().material.mainTexture = Catch_Images_2 [Ani_count];
		}
		Ani_count+=1;
		if (Ani_count >= Catch_Images.Length){
			StartCoroutine ("Wait");
		}
	}

	public IEnumerator Wait(){
		MS = MoleState.None;
		Ani_count = 0;
		float wait_Time = Random.Range (0.5f, 4.5f);  //랜덤으로 시간을 결정한다.
		yield return new WaitForSeconds(wait_Time);  //그시간만큼 기다린다.
		Open_On();   //Open_On을 실행시킨다.
	}

	public void OnMouseDown(){
		if(MS == MoleState.Idle || MS == MoleState.Open){
			Catch_On ();
		}
}
}
