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
    public class RelJugadorDestacamentoHandlerEF : IRelJugadorDestacamentoHandler
    {
        TenantContext ctx;
        public RelJugadorDestacamentoHandlerEF(TenantContext tc)
        {
            ctx = tc;
        }

        public void createRelJugadorDestacamento(RelJugadorDestacamento r)
        {
            var j2 = new Entities.Jugador(r.colonia.jugador.nombre, r.colonia.jugador.apellido, r.colonia.jugador.foto,
                 r.colonia.jugador.nickname, r.colonia.jugador.nivel, r.colonia.jugador.experiencia);
            j2.Id = r.colonia.jugador.id;
            var col = new Entities.RelJugadorMapa(r.colonia.nivel1, r.colonia.nivel2, r.colonia.nivel3, r.colonia.nivel4,
                r.colonia.nivel5, j2);
            col.id = r.colonia.id;

            List<Entities.Costo> cos = new List<Entities.Costo>();
            foreach (var item in r.destacamento.costos)
            {
                var c = new Entities.Costo(item.idRecurso, item.valor, item.incrementoNivel);
                cos.Add(c);
            }
            List<Entities.Capacidad> cap = new List<Entities.Capacidad>();
            foreach (var item in r.destacamento.capacidad)
            {
                var c = new Entities.Capacidad(item.idRecurso, item.valor, item.incrementoNivel);
                cap.Add(c);
            }
            var ed = new Entities.Destacamento(r.destacamento.nombre, r.destacamento.descripcion, r.destacamento.foto, cos,
                cap, r.destacamento.ataque, r.destacamento.escudo, r.destacamento.efectividadAtaque, r.destacamento.vida, r.destacamento.velocidad, r.destacamento.enMision);
            ed.id = r.destacamento.id;

            var rje = new Entities.RelJugadorDestacamento(col, ed, r.cantidad);


            try
            {
                ctx.RelJugadorDestacamento.Add(rje);

                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void deleteRelJugadorDestacamento(int id)
        {
            var rjd = (from c in ctx.RelJugadorDestacamento
                       where c.id == id
                       select c).SingleOrDefault();
            try
            {
                ctx.RelJugadorDestacamento.Remove(rjd);
                ctx.SaveChangesAsync().Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RelJugadorDestacamento getRelJugadorDestacamento(int id)
        {
            try
            {
                var rjd = (from c in ctx.RelJugadorDestacamento
                           where c.id == id
                           select c).SingleOrDefault();

                List<Costo> cos = new List<Costo>();
                foreach (var item2 in rjd.destacamento.costos)
                {
                    var c = new Costo(item2.idRecurso, item2.valor, item2.incrementoNivel);
                    cos.Add(c);
                }

                List<Capacidad> capa = new List<Capacidad>();
                foreach (var item3 in rjd.destacamento.capacidad)
                {
                    var c2 = new Capacidad(item3.idRecurso, item3.valor, item3.incrementoNivel);
                    capa.Add(c2);
                }

                Destacamento des = new Destacamento(rjd.destacamento.id, rjd.destacamento.descripcion, rjd.destacamento.foto, rjd.destacamento.ataque,
                                            rjd.destacamento.escudo, rjd.destacamento.efectividadAtaque, rjd.destacamento.vida,rjd.destacamento.velocidad,
                                            rjd.destacamento.enMision, rjd.destacamento.nombre, cos, capa);
                Jugador jug = new Jugador(rjd.colonia.j.Id, rjd.colonia.j.nombre, rjd.colonia.j.apellido,
                                            rjd.colonia.j.Email, rjd.colonia.j.UserName, rjd.colonia.j.PasswordHash,
                                            rjd.colonia.j.foto, rjd.colonia.j.nickname,
                                            rjd.colonia.j.nivel, rjd.colonia.j.experiencia);
                RelJugadorMapa col = new RelJugadorMapa(rjd.colonia.id, rjd.colonia.nivel1, rjd.colonia.nivel2, rjd.colonia.nivel3,
                                                        rjd.colonia.nivel4, rjd.colonia.nivel5, jug);
                RelJugadorDestacamento destacamento = new RelJugadorDestacamento(rjd.id, col, des, rjd.cantidad);
                return destacamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void updateRelJugadorDestacamento(RelJugadorDestacamento rje)
        {
            try
            {
                var r = ctx.RelJugadorDestacamento
                    .Where(w => w.id == rje.id)
                    .SingleOrDefault();

                if (r != null)
                {
                    r.cantidad = rje.cantidad;
                    ctx.SaveChangesAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RelJugadorDestacamento> getDestacamentosByColonia(int id)
        {
            var destacamentos = new List<RelJugadorDestacamento>();
            try
            {
                ctx.Database.Connection.Open();
                List<Entities.RelJugadorDestacamento> destacamentosD = ctx.RelJugadorDestacamento.Where(w => w.colonia.id == id).ToList();
                ctx.Database.Connection.Close();
                foreach (var item in destacamentosD)
                {
                    var des = getRelJugadorDestacamento(item.id);
                    destacamentos.Add(des);
                }

                return destacamentos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
