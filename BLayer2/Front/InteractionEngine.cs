﻿using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteractionSdk.Interfaces;
using DALayer.Api;
using SharedEntities.Entities;
using MongoDB.Bson;  
namespace BLayer.Front
{
    public class InteractionEngine : IJob
    {
        private IInteraction current;
        private IInteractionable requester;
        private IInteractionable receiver;
        private DALayer.Interfaces.IApi api;
        private string tenantId;

        private void init(string _tenantId) {
            api = new EFApi();
            tenantId = _tenantId;
            api.setTenant(tenantId);
        }

        public InteractionEngine(IInteraction current, string tenant)
        {
            this.current = current;
            init(tenant);
        }

        void IJob.Execute(IJobExecutionContext context)
        {
          
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string tenantId = dataMap.GetString("tenantId");
            int interactionId = System.Int32.Parse(dataMap.GetString("interactionId"));
            init(tenantId);//Set Up api tenant 
            current = new Comercio.Clases.Comercio();//we must do this trought refelection searching by interaction.intName

            Interaction interaction = api.getInteractionHandler().GetInteraction(interactionId);
            IntState intState = api.getIntStateHandler().GetIntStateByInteraction(interactionId);
            requester = GetIntFromMeta(intState.requester);
 
            if (intState.receiverId == -1)
            {
                receiver = GetIntFromMeta(intState.receiver);
            }
            else {
                receiver = GetIntFromId(intState.receiverId);
            }

            if (intState.state == SharedEntities.Enum.InteractionState.EXECUTING)//look for interaction state
            {

                List<IInteractionable> list = current.exec(requester, receiver);
                ApplyChanges(list);
                IntState state = GetIntState(interactionId, receiver, requester);
                state.state = SharedEntities.Enum.InteractionState.FINISHING;
                api.getIntStateHandler().SaveIntState(state);
                //Scheduler.Scheduler.ScheduleInteraction(interactionId, time, tenantId);
                testExec(interactionId);

            }
            else {
                List<IInteractionable> list = current.finalize(requester, receiver);
                IntState state = GetIntState(interactionId, receiver, requester);
                state.state = SharedEntities.Enum.InteractionState.FINISH;
                api.getIntStateHandler().SaveIntState(state);
                ApplyChanges(list);
            }
        }
      
        public void testExec(int interactionId) {
            string tenantId = "newT2";
            init(tenantId);//Set Up api tenant 
            current = new Comercio.Clases.Comercio();//we must do this trought refelection searching by interaction.intName

            Interaction interaction = api.getInteractionHandler().GetInteraction(interactionId);
            IntState intState = api.getIntStateHandler().GetIntStateByInteraction(interactionId);
            requester = GetIntFromMeta(intState.requester);
            if (intState.receiverId == -1)
            {
                receiver = GetIntFromMeta(intState.receiver);
            }
            else {
                receiver = GetIntFromId(intState.receiverId);
            }

            if (intState.state == SharedEntities.Enum.InteractionState.EXECUTING)//look for interaction state
            {

                List<IInteractionable> list = current.exec(requester, receiver);
                ApplyChanges(list);
                IntState state = GetIntState(interactionId, receiver, requester);
                state.state = SharedEntities.Enum.InteractionState.FINISHING;
                api.getIntStateHandler().SaveIntState(state);
                //Scheduler.Scheduler.ScheduleInteraction(interactionId, time, tenantId);
                testExec(interactionId);

            }
            else {
                List<IInteractionable> list = current.finalize(requester, receiver);
                IntState state = GetIntState(interactionId, receiver, requester);
                state.state = SharedEntities.Enum.InteractionState.FINISH;
                api.getIntStateHandler().SaveIntState(state);
                ApplyChanges(list);
            }


        }

        
        internal void setRequester(IInteractionable _requester)
        {
            requester = _requester;
        }

        internal void setReceiver(IInteractionable _receiver)
        {
            receiver = _receiver;
        }


        internal void start()
        {
            List<IInteractionable> list = current.initialize(requester, receiver);
            int interactionId = InsertInteraction(requester.GetID(), receiver.GetID());
            ApplyChanges(list);
            IntState state = GetIntState(interactionId, receiver, requester);
            api.getIntStateHandler().SaveIntState(state);
            
            int time = 2;
            Scheduler.Scheduler.ScheduleInteraction(interactionId, time, tenantId);
            testExec(interactionId);
        }

        private IntState GetIntState(int interactionId, IInteractionable receiver, IInteractionable requester)
        {
            IntState state = new IntState();
            state.interactionId = interactionId;
            if (!current.GetConfig().isRecNeedRecursos() && !current.GetConfig().isRecNeedFloat())
            {
                state.receiverId = receiver.GetID();
                state.receiver = null;
            }
            else {
                state.receiver = GetInteractionableMeta(receiver);
                state.receiverId = -1;
            }
            state.requester = GetInteractionableMeta(requester);
            return state;
        }

        private void ApplyChanges(List<IInteractionable> list)
        {
            list.Where(c => c.GetMustUpdate()).ToList().ForEach((Interactionable) =>
            {
                bool isrequester = requester.GetID() == Interactionable.GetID();

                var recursos = api.getRelJugadorRecursoHandler().getRecursosByColonia(Interactionable.GetID());
                var destacamento = api.getRelJugadorDestacamentoHandler().getDestacamentosByColonia(Interactionable.GetID());
                Interactionable.GetDefensas().ForEach((rec) =>
                {

                });
                Interactionable.GetFlota().ForEach((rec) =>
                {

                });
                Interactionable.GetRecursos().ForEach((rec) =>
                {
                    RelJugadorRecurso r = recursos.Where(c => c.recurso.id == rec.GetId()).First();
                    r.cantidadR = (isrequester) ? r.cantidadR + rec.GetAmount(): r.GetAmount();
                    api.getRelJugadorRecursoHandler().updateRelJugadorRecurso(r);
                });
            });
        }

        private int InsertInteraction(int requesterId, int receiverId) {
            SharedEntities.Entities.Interaction interaction = new SharedEntities.Entities.Interaction();
            interaction.Fecha = DateTime.Now;
            interaction.receiverId = receiverId;
            interaction.requesterId = requesterId;
            interaction.intName = current.GetConfig().GetName();
            //if doesn't need confirmation then is confirmed else we must to wait for confirm.
            interaction.confirmed = !current.GetConfig().NeedConfirmation();
            return api.getInteractionHandler().CreateInteraction(interaction);

        }
      
        public InteractuableMetadata GetInteractionableMeta(IInteractionable r)
        {
            InteractuableMetadata meta = new InteractuableMetadata();
            meta.capacidad = receiver.GetCapacidad();
            meta.defensa = GetDestacamentoArrays(receiver.GetDefensas());
            meta.flota = GetDestacamentoArrays(receiver.GetFlota());
            meta.recursos = GetResourceArrays(receiver.GetRecursos());
            meta.interactuableID = r.GetID();
            return meta;
        }
        private Interactuable GetIntFromId(int receiverId)
        {
            Interactuable interactuable = new Interactuable(receiverId);
            var recursos = api.getRelJugadorRecursoHandler().getRecursosByColonia(receiverId);
            var destacamento = api.getRelJugadorDestacamentoHandler().getDestacamentosByColonia(receiverId);
            var flota = destacamento.Where(c => c.GetVelocidad() != 0);
            var defensas = destacamento.Where(c => c.GetVelocidad() == 0);
            interactuable.SetFlota(flota.Cast<IDestacamento>().ToList());
            interactuable.SetDefensas(defensas.Cast<IDestacamento>().ToList());
            interactuable.SetRecursos(recursos.Cast<IResources>().ToList());
            return interactuable;
        }

        public Interactuable GetIntFromMeta(InteractuableMetadata meta) {
            Interactuable interactuable = new Interactuable(meta.interactuableID);
            interactuable.setCapacidad(meta.capacidad);
            if (meta.returnToBase)
            {
                interactuable.Return();
            }
            var recursos = api.getRelJugadorRecursoHandler().getRecursosByColonia(meta.interactuableID);
            var destacamento = api.getRelJugadorDestacamentoHandler().getDestacamentosByColonia(meta.interactuableID);

            var recursoToAssign = new List<RelJugadorRecurso>();
            var flotaToAssign = new List<RelJugadorDestacamento>();
            var defensaToAssign = new List<RelJugadorDestacamento>();
            foreach (var rec in meta.recursos) {
                int id = -1;
                int value = -1;
                foreach (var s in rec.ToBsonDocument().ToArray())
                {
                    if (s.Name.Equals("_id"))
                    {
                        id = s.Value.ToInt32();
                    }
                    else {
                        value = s.Value.ToInt32();
                    }
                }
                var recurso = recursos.Where(c => c.recurso.id == id).ToList().First();
                recurso.cantidadR = value;
                recursoToAssign.Add(recurso);

            }
            foreach (var rec in meta.flota)
            {
                int id = -1;
                int value = -1;
                foreach (var s in rec.ToBsonDocument().ToArray())
                {

                    if (s.Name.Equals("_id"))
                    {
                        id = s.Value.ToInt32();
                    }
                    else {
                        value = s.Value.ToInt32();
                    }
                   
                }
                var flota = destacamento.Where(c => c.destacamento.id == id).ToList().First();
                flota.cantidad = value;
                flotaToAssign.Add(flota);
            }
            foreach (var rec in meta.defensa)
            {
                int id = -1;
                int value = -1;
                foreach (var s in rec.ToBsonDocument().ToArray())
                {
                    if (s.Name.Equals("_id"))
                    {
                        id = s.Value.ToInt32();
                    }
                    else {
                        value = s.Value.ToInt32();

                    }
                }
                var defensa = destacamento.Where(c => c.destacamento.id == id).ToList().First();
                defensa.cantidad = value;
                defensaToAssign.Add(defensa);

            }
            interactuable.SetDefensas(defensaToAssign.Cast<IDestacamento>().ToList());
            interactuable.SetRecursos(recursoToAssign.Cast<IResources>().ToList());
            interactuable.SetFlota(flotaToAssign.Cast<IDestacamento>().ToList());
            return interactuable;
        }
   
        private object[] GetResourceArrays(List<IResources> list)
        {
            object[] l = new object[] { };
            if (list == null) return l;
            list.ForEach((rec) => {
                l = l.Concat(new object[]{
                    new {
                        id = rec.GetId(),
                        cantidad = rec.GetAmount()
                        }
                }).ToArray();
            });
            return l;
        }
        private object[] GetDestacamentoArrays(List<IDestacamento> list)
        {
            object[] l = new object[] { };
            if (list == null) return l;
            list.ForEach((rec) => {
                l = l.Concat(new object[]{
                    new {
                        id = rec.GetId(),
                        cantidad = rec.GetAmount()
                        }
                }).ToArray();
            });
            return l;
        }
    }
}