using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
	int widthButton = 150;
	int heightButton = 70;
	 void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width/2 - widthButton,Screen.height/2 - heightButton/2,widthButton,heightButton),"Comenzar Juego"))
		{
			Application.LoadLevel("tetris");
			
		}
		if(GUI.Button(new Rect(Screen.width/2 ,Screen.height/2 - heightButton/2,widthButton,heightButton),"Salir del juego"))
		{
			Application.Quit();
		}
		
	}
	
	
}
