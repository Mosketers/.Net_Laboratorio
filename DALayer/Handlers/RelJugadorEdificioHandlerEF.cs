﻿using DALayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedEntities.Entities;
using System.Data.Entity.Validation;

namespace DALayer.Handlers
{
    public class RelJugadorEdificioHandlerEF : IRelJugadorEdificioHandler
    {
        TenantContext ctx;
        public RelJugadorEdificioHandlerEF(TenantContext tc)
        {
            ctx = tc;
        }

        public void createRelJugadorEdificio(RelJugadorEdificio r)
        {
            var col = ctx.RelJugadorMapa.Where(w => w.id == r.colonia.id).SingleOrDefault();
            var ed = ctx.Edificio.Where(w => w.id == r.edificio.id).SingleOrDefault();

            List<Entities.Costo> cos = new List<Entities.Costo>();
            foreach (var item in r.edificio.costos)
            {
                Entities.Recurso rec = new Entities.Recurso(item.recurso.nombre, item.recurso.descripcion, item.recurso.cantInicial, item.recurso.foto);
                var c = new Entities.Costo(item.Id, rec, item.valor, item.incrementoNivel);
                cos.Add(c);
            }
            List<Entities.Capacidad> cap = new List<Entities.Capacidad>();
            foreach (var item in r.edificio.capacidad)
            {
                Entities.Recurso rec = new Entities.Recurso(item.recurso.nombre, item.recurso.descripcion, item.recurso.cantInicial, item.recurso.foto);
                var c = new Entities.Capacidad(item.Id, rec, item.valor, item.incrementoNivel);
                cap.Add(c);
            }

            var rje = new Entities.RelJugadorEdificio(col, ed, r.nivelE);

            try
            {
                ctx.RelJugadorEdificio.Add(rje);

                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void bajarNivel(int id)
        {
            try
            {
                var r = ctx.RelJugadorEdificio
                   .Where(w => w.id == id)
                   .SingleOrDefault();

                if (r != null)
                {
                    r.nivelE -= 1;
                    ctx.SaveChangesAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RelJugadorEdificio getRelJugadorEdificio(int id)
        {
            try
            {
                var rje = (from c in ctx.RelJugadorEdificio
                           where c.id == id
                           select c).SingleOrDefault();

                List<Costo> cos = new List<Costo>();
                foreach (var item2 in rje.edificio.costos)
                {
                    Recurso rec = new Recurso(item2.recurso.id, item2.recurso.nombre, item2.recurso.descripcion, item2.recurso.cantInicial, item2.recurso.foto);
                    var c = new Costo(rec, item2.valor, item2.incrementoNivel);
                    cos.Add(c);
                }
                List<Capacidad> cap = new List<Capacidad>();
                foreach (var item3 in rje.edificio.capacidad)
                {
                    Recurso rec = new Recurso(item3.recurso.id, item3.recurso.nombre, item3.recurso.descripcion, item3.recurso.cantInicial, item3.recurso.foto);
                    var c2 = new Capacidad(rec, item3.valor, item3.incrementoNivel);
                    cap.Add(c2);
                }
                Edificio edi = new Edificio(rje.edificio.id, rje.edificio.descripcion, rje.edificio.foto, rje.edificio.ataque,
                                            rje.edificio.escudo, rje.edificio.efectividadAtaque, rje.edificio.vida,
                                            rje.edificio.nombre, cos, cap);
                Jugador jug = new Jugador(rje.colonia.j.Id, rje.colonia.j.nombre, rje.colonia.j.apellido,
                                            rje.colonia.j.Email, rje.colonia.j.UserName, rje.colonia.j.PasswordHash,
                                            rje.colonia.j.foto, rje.colonia.j.nickname,
                                            rje.colonia.j.nivel, rje.colonia.j.experiencia);
                RelJugadorMapa col = new RelJugadorMapa(rje.colonia.id, rje.colonia.nivel1, rje.colonia.nivel2, rje.colonia.nivel3,
                                                        rje.colonia.nivel4, rje.colonia.nivel5, rje.colonia.coord, jug);
                RelJugadorEdificio edificio = new RelJugadorEdificio(rje.id, col, edi, rje.nivelE);
                return edificio;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void subirNivel(int id)
        {
            try
            {
                var r = ctx.RelJugadorEdificio
                    .Where(w => w.id == id)
                    .SingleOrDefault();

                if (r != null)
                {
                    r.nivelE += 1;
                    ctx.SaveChangesAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RelJugadorEdificio> getEdificiosByColonia(int id)
        {
            var edificios = new List<RelJugadorEdificio>();
            try
            {
                ctx.Database.Connection.Open();
                List<Entities.RelJugadorEdificio> edificiosE = ctx.RelJugadorEdificio.Where(w => w.colonia.id == id).ToList();
                ctx.Database.Connection.Close();
                foreach (var item in edificiosE)
                {
                    var ed = getRelJugadorEdificio(item.id);
                    edificios.Add(ed);
                }

                return edificios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
