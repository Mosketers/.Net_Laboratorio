﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALayer.Entities
{
    public class Recurso {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int cantInicial { get; set; }
        public int capacidadInicial { get; set; }
        public int produccionXTiempo { get; set; }
        public string foto { get; set; }

        public Recurso() { }

        public Recurso(string nombre, string descripcion, int cantInicial, int capacidadInicial, int produccionXTiempo, string foto)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.cantInicial = cantInicial;
            this.capacidadInicial = capacidadInicial;
            this.produccionXTiempo = produccionXTiempo;
            this.foto = foto;
        }

        public SharedEntities.Entities.Recurso getShared()
        {
            return new SharedEntities.Entities.Recurso(id, nombre, descripcion, cantInicial, capacidadInicial, produccionXTiempo, foto);
        }
    }
}
