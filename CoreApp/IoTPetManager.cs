using CoreApp.Utilities;
using DataAccess.CRUD;
using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CoreApp
{
    public class IoTPetManager
    {
        private IoTPetCrudFactory _crud;

        public IoTPetManager()
        {
            _crud = new IoTPetCrudFactory();
        }

        private void EnsureGeneralValidation(IoTPet ioTPet, bool isNewIoTPet)
        {
            if (ioTPet == null)
            {
                throw new Exception("El objeto IoTPet es nulo");
            }

            if (isNewIoTPet)
            {
                // Verifica si ya existe un IoTPet con el mismo IoTPetID
                if (_crud.RetrieveAll().Any(existingIoTPet => existingIoTPet.PetID == ioTPet.PetID))
                {
                    throw new ValidationException("El objeto IoTPet ya existe");
                }
            }
        }


        public void Create(IoTPet ioTPet)
        {
            ioTPet.NormalizerDTO();
            EnsureGeneralValidation(ioTPet, true);

            ioTPet.Created = DateTime.Now;
            _crud.Create(ioTPet);
        }

        public void Update(IoTPet ioTPet)
        {
            ioTPet.NormalizerDTO();
            EnsureGeneralValidation(ioTPet, false);

            var currentIoTPet = _crud.RetrieveById(ioTPet.PetID);
            if (currentIoTPet == null)
            {
                throw new Exception("El objeto IoTPet no existe");
            }


            currentIoTPet.Temperature = ioTPet.Temperature;
            currentIoTPet.Gas = ioTPet.Gas;
            currentIoTPet.Humidity = ioTPet.Humidity;
            currentIoTPet.Pressure = ioTPet.Pressure;
            currentIoTPet.Altitude = ioTPet.Altitude;
            currentIoTPet.ContadorDePasos = ioTPet.ContadorDePasos;
            currentIoTPet.Created = DateTime.Now;

            _crud.Update(currentIoTPet);
        }

        public void Delete(int id)
        {
            var currentIoTPet = _crud.RetrieveById(id);
            _crud.Delete(id);
        }

        public IoTPet RetrieveById(int id)
        {
            var currentIoTPet = _crud.RetrieveById(id);
            return currentIoTPet;
        }

        public IoTPet RetrieveByPetId(int id)
        {
            var currentIoTPet = _crud.RetrieveByPetId(id);
            return currentIoTPet;
        }

        public List<IoTPet> RetrieveAll()
        {
            var ioTPets = new List<IoTPet>();

            foreach (var ioTPet in _crud.RetrieveAll())
            {
                ioTPets.Add(ioTPet);
            }

            return ioTPets;
        }
    }
}
