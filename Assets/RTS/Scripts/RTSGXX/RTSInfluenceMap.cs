/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autores originales: Opsive (Behavior Designer Samples)
   Revisión: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using UnityEngine;

namespace es.ucm.fdi.iav.rts.gxx
{
    // Mapa de influencia como clase abstracta, ya que existen de muchos tipos
    public abstract class RTSInfluenceMap : MonoBehaviour
    {
        public KeyCode toggleVisible;
        protected int w, h;
        protected float[,] map;
        protected int index;
        protected Renderer[,] renderCubes;
        public GameObject RenderTile;

        protected void Start()
        {
            w = RTSGameManager.Instance.getWidth();
            h = RTSGameManager.Instance.getHeight();
            map = new float[w, h];
            index = RTSGameManager.Instance.GetIndex(gameObject.GetComponent<RTSController>());


            renderCubes = new Renderer[w, h];
            int tileSize = RTSGameManager.Instance.cellSize;
            Vector3 mapOrigin = RTSGameManager.Instance.getTerrainPos();
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Vector3 tilePos = new Vector3(j * tileSize + mapOrigin.x + tileSize / 2, 0, i * tileSize + mapOrigin.z + tileSize / 2);
                    renderCubes[i, j] = Instantiate(RenderTile, tilePos, Quaternion.identity).GetComponent<Renderer>();
                    renderCubes[i, j].GetComponent<Renderer>().enabled = false;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleVisible))
                ShowMap();
            else if (Input.GetKeyUp(toggleVisible))
                HideMap();

            if (Input.GetKey(toggleVisible))
                for (int i = 0; i < w; i += 1)
                    for (int j = 0; j < h; j += 1)
                        DrawCell(i, j);
        }

        public abstract float[,] GetMap();

        protected void resetmap()
        {
            for(int i=0;i<w;i++)
                for(int j=0;j<h;j++)
                    map[i, j] = 0.0f;
        }


        protected void ShowMap()
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    renderCubes[i, j].enabled = true;
                }
            }
        }

        protected void HideMap()
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    renderCubes[i, j].enabled = false;
                }
            }
        }

        protected abstract void DrawCell(int i, int j);
    }
}
