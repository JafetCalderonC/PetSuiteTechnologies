using CoreApp.Utilities;
using DataAccess.CRUD;
using DataAccess.DAOs;
using DataAccess.Mapper;
using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class RoomManager
    {
        private RoomCrudFactory _crud;

        public RoomManager()
        {
            _crud = new RoomCrudFactory();
        }

        private void EnsureGeneralValidations(Room dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                throw new ValidationException("El nombre es requerido");
            }

            if (string.IsNullOrEmpty(dto.Description))
            {
                throw new ValidationException("La descripción es requerida");
            }

            if (dto.Cost <= 0)
            {
                throw new ValidationException("El costo debe ser mayor a 0");
            }

            if (dto.Status != 1 && dto.Status != 2)
            {
                throw new ValidationException("El estado debe ser 1 o 2");
            }

            if (_crud.RetrieveAll().Any(x => x.Name == dto.Name && x.Id != dto.Id))
            {
                throw new ValidationException("Ya existe una habitación con ese nombre");
            }
        }

        public void Create(Room dto)
        {
            dto.NormalizerDTO();
            EnsureGeneralValidations(dto);
                        
            // set default values
            dto.CreatedDate = DateTime.Now;
            dto.ModifiedDate = DateTime.Now;

            _crud.Create(dto);
        }

        public void Delete(int id)
        {
            var dto = _crud.RetrieveById(id) ?? throw new Exception("No se encontró la habitación");
            _crud.Delete(id);
        }

        public List<Room>? RetrieveAll()
        {
            var rooms = new List<Room>();

            foreach (var room in _crud.RetrieveAll())
            {
                // Capitalize first letter
                room.Name = char.ToUpper(room.Name[0]) + room.Name.Substring(1);
                room.Description  = char.ToUpper(room.Description[0]) + room.Description.Substring(1);

                rooms.Add(room);
            }
            return rooms;
        }

        public Room? RetrieveById(int id)
        {
            var dto = _crud.RetrieveById(id) ?? throw new Exception("No se encontró la habitación");

            // Capitalize first letter
            dto.Name = char.ToUpper(dto.Name[0]) + dto.Name.Substring(1);
            dto.Description = char.ToUpper(dto.Description[0]) + dto.Description.Substring(1);

            return dto;
        }

        public void Update(Room dto)
        {
            dto.NormalizerDTO();
            EnsureGeneralValidations(dto);

            var roomFromDB = _crud.RetrieveById(dto.Id) ?? throw new Exception("No se encontró la habitación");

            roomFromDB.Name = dto.Name;
            roomFromDB.Description = dto.Description;
            roomFromDB.Cost = dto.Cost;
            roomFromDB.Status = dto.Status;
            roomFromDB.ModifiedDate = DateTime.Now;
            _crud.Update(roomFromDB);
        }

        public List<Room> RetrieveAvailable()
        {
            var rooms = new List<Room>();

            foreach (var room in _crud.RetrieveAvailable())
            {
                // Capitalize first letter
                room.Name = char.ToUpper(room.Name[0]) + room.Name.Substring(1);
                room.Description = char.ToUpper(room.Description[0]) + room.Description.Substring(1);

                rooms.Add(room);
            }
            return rooms;
        }
    }
}
