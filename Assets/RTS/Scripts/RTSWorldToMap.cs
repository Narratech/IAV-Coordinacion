/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autores originales: Opsive (Behavior Designer Samples)
   Revisión: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace es.ucm.fdi.iav.rts
{
    public class RTSWorldToMap : MonoBehaviour
    {
        public struct Info
        {
            int team;
            float total;
            public Info(int t, float tot)
            {
                team = t;
                total = tot;
            }
            public int getTeam() { return team; }
            public float getTotal() { return total; }
        }

        public float baseValue=10.0f, processingValue=5.0f, extractorValue=1.0f, explorationForce=3.0f, destructorForce=3.0f, turretForce = 5.0f;
        int w, h;
        List<Info>[,] forceMap;
        List<Info>[,] valueMap;
        Vector3 terrain;
        int cellSize;

        void Start()
        {
            cellSize = RTSGameManager.Instance.cellSize;
            w = RTSGameManager.Instance.getWidth();
            h = RTSGameManager.Instance.getHeight();
            forceMap = new List<Info>[w, h];
            valueMap = new List<Info>[w, h];
            for(int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    valueMap[i, j] = new List<Info>();
                    forceMap[i, j] = new List<Info>();
                }
            }
            terrain = RTSGameManager.Instance.getTerrainPos();
        }

        private void Update()
        {
            clearMap();
            // Pedimos las unidades al gamemanager y rellenamos los mapas a partir de sus posiciones
            //Convertir de mundo a casilla (cuantificación)
            for (int i = 0; i < RTSGameManager.Instance.ControllerCount; i++)
            {
                List<BaseFacility> bases = RTSGameManager.Instance.GetBaseFacilities(i);
                foreach (BaseFacility b in bases)
                {
                    //value[z,x]
                    Vector2 coord = getCoord(b.gameObject);
                    valueMap[((int)Math.Round(coord.x))/cellSize, ((int)Math.Round(coord.y))/cellSize].Add(new Info(i,baseValue));
                }
                List<ProcessingFacility> processingFacilities=RTSGameManager.Instance.GetProcessingFacilities(i);
                foreach (ProcessingFacility b in processingFacilities)
                {
                    //value[z,x]
                    Vector2 coord = getCoord(b.gameObject);
                    valueMap[((int)Math.Round(coord.x)) / cellSize, ((int)Math.Round(coord.y)) / cellSize].Add(new Info(i, processingValue));
                }
                List<ExtractionUnit> extractors = RTSGameManager.Instance.GetExtractionUnits(i);
                foreach (ExtractionUnit b in extractors)
                {
                    //value[z,x]
                    Vector2 coord = getCoord(b.gameObject);
                    valueMap[((int)Math.Round(coord.x)) / cellSize, ((int)Math.Round(coord.y)) / cellSize].Add(new Info(i, extractorValue));
                }
                List<ExplorationUnit> explorators = RTSGameManager.Instance.GetExplorationUnits(i);
                foreach (ExplorationUnit b in explorators)
                {
                    //value[z,x]
                    Vector2 coord = getCoord(b.gameObject);
                    forceMap[((int)Math.Round(coord.x)) / cellSize, ((int)Math.Round(coord.y)) / cellSize].Add(new Info(i, explorationForce));
                }
                List<DestructionUnit> destructors = RTSGameManager.Instance.GetDestructionUnits(i);
                foreach (DestructionUnit b in destructors)
                {
                    //value[z,x]
                    Vector2 coord = getCoord(b.gameObject);
                    forceMap[((int)Math.Round(coord.x)) / cellSize, ((int)Math.Round(coord.y)) / cellSize].Add(new Info(i, destructorForce));
                }
            }
            List<Tower> neutralTowers = RTSGameManager.Instance.GetTurrets();
            foreach (Tower t in neutralTowers)
            {
                if(t != null)
                {
                    //value[z,x]
                    Vector2 coord = getCoord(t.gameObject);
                    forceMap[((int)Math.Round(coord.x)) / cellSize, ((int)Math.Round(coord.y)) / cellSize].Add(new Info(-1, turretForce));
                }
            }
        }

        private void clearMap()
        {
            foreach(List<Info> lI in valueMap)
            {
                lI.Clear();
            }
            foreach (List<Info> lI in forceMap)
            {
                lI.Clear();
            }
        }

        public List<Info>[,] getForceMap() { return forceMap; }

        public List<Info>[,] getValueMap() { return valueMap; }

        /// <summary>
        /// Obtiene la posicion relativa al mapa del gameObject en el valor del x vendra la cordenada en z y en el valor de y la cordenada en x 
        /// ya que el mapa esta hecho de tal manera que z sea el ancho mientras que x sea el alto (suponiendo mundo 2D)
        /// </summary>
        /// <param name="go"> GameObject que se pasa para obtener su posición relativa</param>
        /// <returns></returns>
        private Vector2 getCoord(GameObject go) {
            return new Vector2((go.transform.position.z - terrain.z), (go.transform.position.x - terrain.x));
        }

    }
}
