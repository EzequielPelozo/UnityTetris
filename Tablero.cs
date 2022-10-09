 	using UnityEngine;
using System.Collections;

public class Tablero : MonoBehaviour 
{	
	Piece Pieza;
	//Piece NextPieza;
//********************************************************************************************************	
	int ColumnaMapa = 10;
	int FilaMapa = 20;	
//***********************************************************************************************************
	float IntervaloCaida = 1000.0f;
	float IntervaloDeMovimiento = 500.0f;	
	float AcumTiempoCaida = 0;
	float acumTiempomovim = 0;
	float MasTiempoPorNivel = 100.0f;
//variables a resetear al perder*********************************************************************************	
	int Nivel = 1;		 
	static int Puntaje = 0;	
	static int TotalLine = 0;
//***************************************************************************************************************__
	public GameObject Prefab;
	private GameObject[,] MapaReal;
	private int[,] MapaLogico =  new int[,] // matriz del mapa que luego voy a copiar
	{ 
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,1},
		{1,1,1,1,1,1,1,1,1,1}			 
	}; 
//*******************************************************************************************************************************
	public void ImprimeTablero() //donde no haya 0 activo los game objects de mi pieza real
	{
		
		for(int i = 0 ; i < MapaLogico.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < MapaLogico.GetLength(1) ; j++)
			{
				if(MapaLogico[i,j] != 0)
				{
					MapaReal[i,j].renderer.enabled = true;
				}
				else
					MapaReal[i,j].renderer.enabled = false;
			}
		}
	}
//****************************************************************************************************************************
	void OnGUI()
	{
		GUI.Box(new Rect(0,0,100,20),"Score " + Puntaje);
		GUI.Box(new Rect(0,30,100,20),"Lineas "+ TotalLine);
		GUI.Box(new Rect(0,60,100,20),"Nivel "+ Nivel);
	}
//****************************************************************************************************************************************
	void SetTime()
	{
		
	}
//******************************************************************************************************************************************
	bool ColisionConBordes()  //chequeo las posibles colisiones
	{
		int auxFila = Pieza.Fila;
		int auxColum = Pieza.Columna;
		for(int i = 0 ; i < Pieza.Alto ; i++)
		{
			for(int j = 0 ; j < Pieza.Ancho ; j++)
			{
				if(auxColum + j >= ColumnaMapa || auxColum + j < 0) //en la ancho
				{					
				 if((Pieza.MatrizLogica[i,j] != 0))
				  {		
						return true;					
				  }					
				}
				else if(auxFila + i >= FilaMapa || auxFila + i < 0) // por arriba y por abajo
				{
					if((Pieza.MatrizLogica[i,j] != 0)) 
				  {		
						return true;					
				  }	
				}
				else if(MapaLogico[auxFila + i , auxColum + j] != 0 && Pieza.MatrizLogica[i,j] != 0) //con pieza
				{
					return true;
				}
			}
		}
		return false;
	}
//*****************************************************************************************************************************************************
	void CopiarPiezaEnTablero()  //estampo la pieza copiando los valores de la pieza al mapa
	{
		int auxFila = Pieza.Fila;
		int auxColum = Pieza.Columna;
		for(int i = 0 ; i < Pieza.Alto ; i++)
		{
			for(int j = 0 ; j < Pieza.Ancho ; j++)
			{
				if((auxColum + j >= 0) && (auxColum + j <= ColumnaMapa) && (auxFila + i >= 0) && (auxFila + i <= FilaMapa) && (Pieza.MatrizLogica[i,j]!= 0))
				{
					MapaLogico[auxFila + i,auxColum + j] = Pieza.MatrizLogica[i,j];
				}
			}
		}
		Pieza.BorrarPieza();
	}
//***********************************************************************************************************************************************
	
	void Start () 
	{ 
		//audio.Play();
		SetTime();
		Pieza = MonoBehaviour.FindObjectOfType(typeof(Piece)) as Piece;
		//NextPieza = MonoBehaviour.FindObjectOfType(typeof(Piece)) as Piece;
		MapaReal = new GameObject [FilaMapa,ColumnaMapa];
		Pieza.RandomPiece(1,3);
		Pieza.CreaMatrizReal();
		//NextPieza.RandomPiece(1,14);
		//NextPieza.CreaMatrizReal();
		InicioTablero();		
	}
//**********************************************************************************************************************************************
	void InicioTablero() //creo mi mapa real
	{
		for(int i = 0 ; i < MapaReal.GetLength(0); i++)
		{
			for(int j = 0 ; j < MapaReal.GetLength(1); j++)
			{
				GameObject go = GameObject.Instantiate(Prefab) as GameObject;
				MapaReal[i,j] = go;
				MapaReal[i,j].transform.parent = this.transform;
				MapaReal[i,j].renderer.enabled = false;				
				MapaReal[i,j].transform.position = new Vector3( j * MapaReal[i,j].transform.localScale.x, - i * MapaReal[i,j].transform.localScale.y,0);
				
			}
		}
		ImprimeTablero(); 
	}
//*******************************************************************************************************************************************************************
	void BorrarLinea(int Fila)  //borra la primera linea y copia lo de arriba hacia abajo en la fila indicada
	{
		for(int i = Fila ; i > 0 ; i--)
		{
			for(int j = 0 ; j < ColumnaMapa ; j++)
			{
				MapaLogico[i,j] = MapaLogico[i-1 , j];
			}
			for(int j = 1 ; j < ColumnaMapa -1 ; j++)
			{
				MapaLogico[0,j] = 0;
			}
		}
	}
//*********************************************************************************************************************
	int ChequeoQueFormaLineas() //chequeo que se formen lineas 
	{
		bool Eslinea;
		int Lineas = 0;	
		
	for(int fila = FilaMapa -2  ; fila >= 0 ; fila--)
	   {		
		Eslinea = true;		
		for(int columna = 1 ; columna < ColumnaMapa ; columna++)
		  {			
 			if(MapaLogico[fila,columna] == 0)
				{
					Eslinea = false;
					break;
				}
		  }
		if(Eslinea)
			{
				TotalLine++;
				Lineas++;
				BorrarLinea(fila);
				fila++;				
			}		
	  }		
		return Lineas;
	}
//*/**********************************************************************************************************
	void CondicionDederrota()
	{
		bool GameOver = false;
		if(ColisionConBordes())
		{
			GameOver = true;
		}
		    if(GameOver)
		    {
			    Application.LoadLevel("Menu");              	 
	            Puntaje = 0;	           	
	            TotalLine = 0;
			    Nivel = 1;
			    MasTiempoPorNivel = 100.0f;
		    }
		
	}
//*******************************************************************************************
	void ChequeoNivel()
	{
		if(TotalLine >= 10 && TotalLine < 20 )			
		        {				
			      Nivel = 2;
			      MasTiempoPorNivel = 400.0f;
		        }
		 else if(TotalLine >= 20 && TotalLine < 30) 
		        {				
			      Nivel = 3;
			      MasTiempoPorNivel = 800.0f;
		        }
		else if(TotalLine >= 30 && TotalLine < 40) 
		        {				
			      Nivel = 4;
			      MasTiempoPorNivel = 1200.0f;
		        }
		else if(TotalLine >= 40 && TotalLine < 50) 
		        {				
			      Nivel = 5;
			      MasTiempoPorNivel = 1600.0f;
		        }
		else if(TotalLine >= 50 && TotalLine < 60) 
		        {				
			      Nivel = 6;
			      MasTiempoPorNivel = 2000.0f;
		        }
		else if(TotalLine >= 60 && TotalLine < 70) 
		        {				
			      Nivel = 7;
			      MasTiempoPorNivel = 2400.0f;
		        }
		else if(TotalLine >= 70 && TotalLine < 80) 
		        {				
			      Nivel = 8;
			      MasTiempoPorNivel = 2800.0f;
		        }
		else if(TotalLine >= 80 && TotalLine < 90) 
		        {				
			      Nivel = 9;
			      MasTiempoPorNivel = 3200.0f;
		        }
		else if(TotalLine >= 90 && TotalLine < 100) 
		        {				
			      Nivel = 10;
			      MasTiempoPorNivel = 3600.0f;
		        }
		else if(TotalLine >= 100 && TotalLine < 110) 
		        {				
			      Nivel = 11;
			      MasTiempoPorNivel = 4000.0f;
		        }
		else if(TotalLine >= 110 && TotalLine < 120) 
		        {				
			      Nivel = 12;
			      MasTiempoPorNivel = 4400.0f;
		        }
		else if(TotalLine >= 120 && TotalLine < 130) 
		        {				
			      Nivel = 13;
			      MasTiempoPorNivel = 4800.0f;
		        }
		else if(TotalLine >= 130 && TotalLine < 140) 
		        {				
			      Nivel = 14;
			      MasTiempoPorNivel = 5200.0f;
		        }
		else if(TotalLine >= 140 && TotalLine < 150) 
		        {				
			      Nivel = 15;
			      MasTiempoPorNivel = 5600.0f;
		        }
		else if(TotalLine >= 150 && TotalLine < 160) 
		        {				
			      Nivel = 16;
			      MasTiempoPorNivel = 6000.0f;
		        }
		else if(TotalLine >= 160 && TotalLine < 170) 
		        {				
			      Nivel = 17;
			      MasTiempoPorNivel = 6400.0f;
		        }
	}
//*********************************************************************************************************
	void Update ()
	{
			
		//Debug.Log(AcumTiempoCaida);	 
		AcumTiempoCaida += (IntervaloDeMovimiento + MasTiempoPorNivel) * Time.deltaTime;
		
		if(AcumTiempoCaida >= IntervaloCaida)
		{			
			Pieza.MoverAbajo();
			if(ColisionConBordes())
			{				
				int lineas = 0;
				Pieza.MoverArriba();
				CopiarPiezaEnTablero();				
			    Pieza.RandomPiece(1,3);
				lineas = ChequeoQueFormaLineas();
				if(lineas == 1)
				{
					Puntaje += 10;
				}
				else if(lineas == 2)
				{
					Puntaje += 30;
				}
				else if(lineas == 3)
				{
					Puntaje += 60;
				}
				else if(lineas == 4)
				{
					Puntaje += 100;
				}				
			}
	
			AcumTiempoCaida = 0;
			
		}
		
			
//**********************************************************************************
		ChequeoNivel();
//**********************************************************************************
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			
			Pieza.MoverAbajo();
			if(ColisionConBordes())
			{
				int lineas = 0;
				Pieza.MoverArriba();
				CopiarPiezaEnTablero();				
			    Pieza.RandomPiece(1,3);
				lineas = ChequeoQueFormaLineas();
				if(lineas == 1)
				{
					Puntaje += 10;
				}
				else if(lineas == 2)
				{
					Puntaje += 30;
				}
				else if(lineas == 3)
				{
					Puntaje += 60;
				}
				else if(lineas == 4)
				{
					Puntaje += 100;
				}
			}
			
			
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Pieza.MoverIzquierda();
			if(ColisionConBordes())
			{
				Pieza.MoverDerecha();
				
			}
		}	
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			Pieza.MoverDerecha();
			if(ColisionConBordes())
			{
				Pieza.MoverIzquierda();
			}
		}		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Pieza.Rotate();
			if(ColisionConBordes())
			{
				Pieza.RotateCCW();
			}
		}		
		if(Input.GetKeyDown(KeyCode.LeftAlt))
		{
			Pieza.BorrarPieza();
			Pieza.RandomPiece(1,3);
		}		    
			Pieza.PosicionReal();
			Pieza.Draw();
			ImprimeTablero();
			CondicionDederrota();
		
    }	
}