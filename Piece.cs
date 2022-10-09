using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour
{
//*************************************************************************************************************************
	public GameObject Prefab;	
	public Vector3 pos;
	public int Fila;
	public int Columna;	
	public int[,]MatrizLogica;	
	public int Ancho;
	public int Alto;
	private GameObject[,] MatrizReal;	
//*********************************************************************************************************************************
	public int[][,] Piezas = new int[][,] 
 {
    	new int[,] 
		{ 
			{0,0,1,0},
			{0,0,2,0},
			{0,0,3,0},
			{0,0,4,0}			 
		},		
   		 new int[,]
		{
			{1,2,0},
			{0,3,0},
			{0,4,0}
			
		},		
      	new int[,]
		{ 
			{0,2,1},
			{0,3,0},
			{0,4,0}			 
		}, 
		 new int[,] 
		{ 
			{1,2,3},
			{0,4,0},
			{0,0,0}					 
		},		
    	new int[,]
		{
			{1,2},
			{3,4}			
		},		
    	new int[,]
		{ 
			{0,2,1},
			{4,3,0},
			{0,0,0}			 
		}, 
		 new int[,]
		{ 
			{1,2,0},
			{0,3,4},
			{0,0,0}			 
		} 
	};	
//*****************************************************************************************************************************************************	
	void Start ()
	{		
		MatrizReal = new GameObject[4,4];
	}
//**********************************************************************************************************************************************************
	public void PosicionReal()
	{
		for(int i = 0; i <  MatrizLogica.GetLength(0); i++)
		{
			for(int j = 0 ; j <  MatrizLogica.GetLength(1) ; j++)
			{
				MatrizReal[i,j].transform.position = new Vector3( (j+Columna) * MatrizReal[i,j].transform.localScale.x, - (i+Fila) * MatrizReal[i,j].transform.localScale.y,0);	
			}	
			
		}
	}
//*****************************************************************************************************************************************************************
	 public void Draw()
	{
		for(int i = 0 ; i <  MatrizLogica.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < MatrizLogica.GetLength(1) ; j++)
			{
				if(MatrizLogica[i,j] == 1)
				{
					MatrizReal[i,j].renderer.enabled = true;
					MatrizReal[i,j].renderer.material.color = Color.green;
				}
				else if(MatrizLogica[i,j] == 2)
				{
					MatrizReal[i,j].renderer.enabled = true;
					MatrizReal[i,j].renderer.material.color = Color.blue;
				}
				else if(MatrizLogica[i,j] == 3)
				{
					MatrizReal[i,j].renderer.enabled = true;
					MatrizReal[i,j].renderer.material.color = Color.red;
				}
				else if(MatrizLogica[i,j] == 4)
				{
					MatrizReal[i,j].renderer.enabled = true;
					MatrizReal[i,j].renderer.material.color = Color.yellow;
				}
				
				else
					MatrizReal[i,j].renderer.enabled = false;
			}
		}
	}
//***************************************************************************************************************************************************
	public void BorrarPieza()
	{
		for(int i = 0 ; i <  MatrizLogica.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < MatrizLogica.GetLength(1) ; j++)
			{				
					MatrizReal[i,j].renderer.enabled = false;
			}
		}
	}
//*****************************************************************************************************************************************************	
	 public void Rotate()
	{		 
		int[,] MatrizAuxiliar = new int[MatrizLogica.GetLength(0),MatrizLogica.GetLength(1)]; // por no hacer esta pavada no me giraba la pieza
		int temp;	 
		for(int i = 0 ; i < MatrizAuxiliar.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < MatrizAuxiliar.GetLength(1) ; j++)
			{
				MatrizAuxiliar[i,j] = MatrizLogica[i,j];
			}
		}
		for(int i = 0 ; i < MatrizAuxiliar.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < MatrizAuxiliar.GetLength(1) ; j++)
			{
				temp = MatrizAuxiliar[i,j];
				MatrizLogica[j,MatrizAuxiliar.GetLength(1) - 1 - i] = temp;	
			}
		}				
	}
//****************************************************************************************************************************************************
	public void RotateCCW()
	{
		int[,] MatrizAuxiliar = new int[MatrizLogica.GetLength(0),MatrizLogica.GetLength(1)]; // por no hacer esta pavada no me giraba la pieza
		int temp;
		for(int i = 0 ; i < MatrizAuxiliar.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < MatrizAuxiliar.GetLength(1) ; j++)
			{
				MatrizAuxiliar[i,j] = MatrizLogica[i,j];
			}
		}
		for(int i = 0 ; i < MatrizAuxiliar.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < MatrizAuxiliar.GetLength(1) ; j++)
			{
				temp = MatrizAuxiliar[i,j];
				MatrizLogica[MatrizAuxiliar.GetLength(1) - 1 - j,i] = temp;	
			}
		}	
	}
//**************************************************************************************************************************
	public void RandomPiece(int fila, int columna )
	{  
		Fila = fila;
		Columna = columna;
		int[,] NuevaMatriz = Piezas[Random.Range(0,6)];
		MatrizLogica = new int[NuevaMatriz.GetLength(0),NuevaMatriz.GetLength(1)];		
		for(int i = 0 ; i < MatrizLogica.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < MatrizLogica.GetLength(1) ; j++)
			{
				MatrizLogica[i,j] = NuevaMatriz[i,j] ;				
			}
		}		
		Alto = NuevaMatriz.GetLength(0);
		Ancho = NuevaMatriz.GetLength(1);
	}
//*******************************************************************************************************************************
	public void CreaMatrizReal()
	{
		for(int i = 0 ; i < MatrizReal.GetLength(0); i++)
		{
			for(int j = 0 ; j < MatrizReal.GetLength(1); j++)
			{
				GameObject go = GameObject.Instantiate(Prefab) as GameObject;
				MatrizReal[i,j] = go;
				MatrizReal[i,j].transform.parent = this.transform;
				MatrizReal[i,j].renderer.enabled = false;
								
			}
		}		
	}	
//************************************************************************************************************************
	 public void MoverArriba()
	{		
			Fila--;		
	}
	public void MoverAbajo()
	{		
			Fila++;		
	}
	public void MoverIzquierda()
	{		
			Columna--;		
	}
	public void MoverDerecha()
	{		
			Columna++;		
	}
	
}
