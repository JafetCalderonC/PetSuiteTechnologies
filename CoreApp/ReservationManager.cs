using CoreApp.Utilities;
using DataAccess.CRUD;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreApp
{
    public class ReservationManager
    {
        private ReservationCrudFactory _crud;

        public ReservationManager()
        {
            _crud = new ReservationCrudFactory();
        }
        private void EnsureGeneralValidation(Reservation reservation, bool isNewReservation)
        {
            if (reservation == null)
            {
                throw new Exception("La reservación es nula");
            }

            if (reservation.StartDate == null)
            {
                throw new Exception("La fecha de inicio es requerida");
            }

            if (reservation.EndDate == null)
            {
                throw new Exception("La fecha de fin es requerida");
            }

            if (reservation.StartDate > reservation.EndDate)
            {
                throw new Exception("La fecha de inicio no puede ser mayor a la fecha de fin");
            }

            if (reservation.UserID <= 0)
            {
                throw new Exception("El usuario es requerido");
            }

            if (reservation.PackageId <= 0)
            {
                throw new Exception("El paquete es requerido");
            }

            if (isNewReservation)
            {
                if (_crud.RetrieveAll().Any(x => x.StartDate == reservation.StartDate && x.EndDate == reservation.EndDate && x.UserID == reservation.UserID && x.PackageId == reservation.PackageId))
                {
                    throw new ValidationException("La reservación ya existe");
                }
            }
        }

        public void Create(Reservation reservation)
        {
            reservation.NormalizerDTO();
            EnsureGeneralValidation(reservation, true);

            reservation.ReservationCreatedDate = DateTime.Now;
            reservation.ReservationModifiedDate = DateTime.Now;
            _crud.Create(reservation);
        }

        public void Update(Reservation reservation)
        {
               reservation.NormalizerDTO();
            EnsureGeneralValidation(reservation, false);

            var currentReservation = _crud.RetrieveById(reservation.ReservationID);
            if (currentReservation == null)
            {
                throw new Exception("La reservación no existe");
            }

            currentReservation.StartDate = reservation.StartDate;
            currentReservation.EndDate = reservation.EndDate;
            currentReservation.UserID = reservation.UserID;
            currentReservation.PackageId = reservation.PackageId;
            currentReservation.ReservationModifiedDate = DateTime.Now;
            _crud.Update(currentReservation);
        }

        public void Delete(int id)
        {
            var currentReservation = _crud.RetrieveById(id);
            if (currentReservation == null)
            {
                throw new Exception("La reservación no existe");
            }

            _crud.Delete(id);
        }

        public Reservation RetrieveById(int id)
        {
            var currentReservation = _crud.RetrieveById(id);
            if (currentReservation == null)
            {
                throw new Exception("La reservación no existe");
            }

            return currentReservation;
        }

        public List<Reservation> RetrieveAll()
        {
            var reservations = new List<Reservation>();

            foreach (var reservation in _crud.RetrieveAll())
            {
                reservations.Add(reservation);
            }
            return reservations;
        }
}}