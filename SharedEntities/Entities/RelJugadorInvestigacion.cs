﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedEntities.Entities
{
    public class RelJugadorInvestigacion
    {
        public int id;
        public Jugador jugador;
        public Investigacion investigacion;

        public RelJugadorInvestigacion(int ID, Jugador jug, Investigacion invest)
        {
            this.id = ID;
            this.jugador = jug;
            this.investigacion = invest;
        }
    }
}
