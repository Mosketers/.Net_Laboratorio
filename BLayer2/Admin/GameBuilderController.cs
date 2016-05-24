﻿using BLayer.Interfaces;
using DALayer.Interfaces;
using SharedEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLayer.Admin
{
    public class GameBuilderController : IGameBuilder
    {
        private IApi builder;

        public GameBuilderController(string tId, IApi gc) {
            builder = gc;
            builder.setTenant(tId);
        }

        //RECURSOS
        public List<Recurso> getAllRecursos()
        {
            return builder.getRecursoHandler().getAllRecursos();
        }

        public Recurso getRecurso(int id)
        {
            return builder.getRecursoHandler().getRecurso(id);
        }

        public void createRecurso(Recurso recurso)
        {
            builder.getRecursoHandler().createRecurso(recurso);
        }

        public void updateRecurso(Recurso recurso)
        {
            builder.getRecursoHandler().updateRecurso(recurso);
        }

        public void deleteRecurso(int id)
        {
            builder.getRecursoHandler().deleteRecurso(id);
        }

        //MAPAS
        public List<MapaNode> getAllMapas()
        {
            return builder.getMapaNodeHandler().getAllMapas();
        }
        public MapaNode getMapa(int id)
        {
            return builder.getMapaNodeHandler().getMapa(id);
        }

        public void createMapa(MapaNode mapa)
        {
            builder.getMapaNodeHandler().CreateMapa(mapa);
        }

        public void deleteMapa(int id)
        {
            builder.getMapaNodeHandler().DeleteMapa(id);
        }

        public void updateMapa(MapaNode mapa)
        {
            builder.getMapaNodeHandler().UpdateMapa(mapa);
        }

        //INVESTIGACION
        public List<Investigacion> getAllInvestigaciones()
        {
            return builder.getInvestigacionHandler().getAllInvestigaciones();
        }

        public Investigacion getInvestigacion(int id)
        {
            return builder.getInvestigacionHandler().getInvestigacion(id);
        }

        public void createInvestigacion(Investigacion investigacion)
        {
            builder.getInvestigacionHandler().createInvestigacion(investigacion);
        }

        public void updateInvestigacion(Investigacion investigacion)
        {
            builder.getInvestigacionHandler().updateInvestigacion(investigacion);
        }

        public void deleteInvestigacion(int id)
        {
            builder.getInvestigacionHandler().deleteInvestigacion(id);
        }

        //DESTACAMENTOS
        public List<Destacamento> getAllDestacamentos()
        {
            return builder.getUnidadHandler().getAllDestacamentos();
        }

        public Destacamento getDestacamento(int id)
        {
            return builder.getUnidadHandler().getDestacamento(id);
        }

        public void createDestacamento(Destacamento destacamento)
        {
            builder.getUnidadHandler().createDestacamento(destacamento);
        }

        public void updateDestacamento(Destacamento destacamento)
        {
            builder.getUnidadHandler().updateDestacamento(destacamento);
        }

        public void deleteDestacamento(int id)
        {
            builder.getUnidadHandler().deleteDestacamento(id);
        }

        //EDIFICIOS
        public List<Edificio> getAllEdificios()
        {
            return builder.getUnidadHandler().getAllEdificios();
        }

        public Edificio getEdificio(int id)
        {
            return builder.getUnidadHandler().getEdificio(id);
        }

        public void createEdificio(Edificio edificio)
        {
            builder.getUnidadHandler().createEdificio(edificio);
        }

        public void updateEdificio(Edificio edificio)
        {
            builder.getUnidadHandler().updateEdificio(edificio);
        }

        public void deleteEdificio(int id)
        {
            builder.getUnidadHandler().deleteEdificio(id);
        }

        //ALIANZA
        public List<Alianza> getAllAlianzas()
        {
            return builder.getAlianzaHandler().getAllAlianzas();
        }

        public Alianza getAlianza(int id)
        {
            return builder.getAlianzaHandler().getAlianza(id);
        }

        public void createAlianza(Alianza alianza)
        {
            builder.getAlianzaHandler().createAlianza(alianza);
        }

        public void updateAlianza(Alianza alianza)
        {
            builder.getAlianzaHandler().updateAlianza(alianza);
        }

        public void deleteAlianza(int id)
        {
//            builder.getAlianzaHandler().deleteAlianza(id);
        }

        //DEPENDENCIA
        public List<Dependencia> getAllDependencias()
        {
            return builder.getDependenciaHandler().getAllDependencias();
        }

        public Dependencia getDependencia(int id)
        {
            return builder.getDependenciaHandler().getDependencia(id);
        }

        public void createDependencia(Dependencia dependencia)
        {
            builder.getDependenciaHandler().createDependencia(dependencia);
        }

        public void updateDependencia(Dependencia dependencia)
        {
            builder.getDependenciaHandler().updateDependencia(dependencia);
        }

        public void deleteDependencia(int id)
        {
            builder.getDependenciaHandler().deleteDependencia(id);
        }
    }
}
