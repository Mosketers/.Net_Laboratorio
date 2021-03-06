﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedEntities.Entities;

namespace DALayer.Interfaces
{
    public interface ISolicitudJuegoHandler
    {
        void createSolicitudJuego(SolicitudJuego sj);
        void deleteSolicitudJuego(int id);
        void updateSolicitudJuego(SolicitudJuego sj);
        SolicitudJuego getSolicitudJuego(int id);
        List<SolicitudJuego> getAllSolicitudes();
    }
}
