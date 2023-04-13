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

namespace es.ucm.fdi.iav.rts.gxx
{
    // Mapa de valor/importancia para el juego de cada casilla
    public class RTSValueMap : RTSInfluenceMap
    {
        List<RTSWorldToMap.Info>[,] vmap;

        // Se activa con la letra V
        private void Awake()
        {
            toggleVisible = KeyCode.V;
        }

        public override float[,] GetMap()
        {
            vmap = RTSGameManager.Instance.getValueMap();
            resetmap();

            // IMPLEMENTAR CÓMO SE CONVIERTEN LOS DATOS BRUTOS EN UN MAPA DE INFLUENCIA (VALOR)
   
            return map;
        }

        // Dibujamos la celda
        protected override void DrawCell(int i, int j)
        {
            float intensity = map[i, j];

            renderCubes[i, j].gameObject.transform.localScale = new Vector3(3, 1 + intensity * 10, 3);
        }
    }
}
