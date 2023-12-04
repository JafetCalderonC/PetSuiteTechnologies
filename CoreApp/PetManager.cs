using CoreApp.Utilities;
using DataAccess.CRUD;
using DataAccess.DAOs;
using DataAccess.Mapper;
using DTOs;
using DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class PetManager
    {
        private PetCrudFactory _crud;

        public PetManager()
        {
            _crud = new PetCrudFactory();
        }

        private void EnsureGeneralvalidation(Pet pet, bool isNewService)
        {
            if (pet == null)
            {
                throw new Exception("La mascota no puede ser nula");
            }

            if (string.IsNullOrEmpty(pet.PetName) || string.IsNullOrWhiteSpace(pet.PetName))
            {
                throw new Exception("El nombre de la mascota es requerido");
            }

            if (string.IsNullOrEmpty(pet.Description) || string.IsNullOrWhiteSpace(pet.Description))
            {
                throw new Exception("La descripción de la mascota es requerida");
            }

            if ((pet.Status == 1 || pet.Status == 2) == false)
            {
                throw new Exception("El estado de la mascota es inválido");
            }

            if (_crud.RetrieveAll().Any(x => x.PetName == pet.PetName && x.Id != pet.Id))
            {
                throw new ValidationException("Servicio ya existe con el mismo nombre");
            }
        }

        public void Create(Pet pet)
        {
            pet.NormalizerDTO();
            EnsureGeneralvalidation(pet, true);

            pet.CreatedDate = DateTime.Now;
            pet.ModifiedDate = DateTime.Now;
            _crud.Create(pet);
        }
        public void Update(Pet pet)
        {
            pet.NormalizerDTO();
            EnsureGeneralvalidation(pet, false);

            // get service by id
            var currentService = _crud.RetrieveById(pet.Id);
            if (currentService == null)
            {
                throw new Exception("La mascota no existe");
            }

            currentService.PetName = pet.PetName;
            currentService.Status = pet.Status;
            currentService.Description = pet.Description;
            currentService.PetAge = pet.PetAge;
            currentService.PetBreedType = pet.PetBreedType;
            currentService.PetAggressiveness = pet.PetAggressiveness;
            currentService.ModifiedDate = DateTime.Now;
            currentService.UserId = pet.UserId;

            _crud.Update(currentService);
    }
        public void Delete(int id)
        {
            // get service by id
            var currentService = _crud.RetrieveById(id);
            if (currentService == null)
            {
                throw new Exception("La mascota no existe");
            }
            _crud.Delete(id);
        }

        public Pet RetrieveById(int id)
        {
            // get service by id
            var currentService = _crud.RetrieveById(id);
            if (currentService == null)
            {
                throw new Exception("El servicio no existe");
            }

            // Capitalize first letter
            currentService.PetName = char.ToUpper(currentService.PetName[0]) + currentService.PetName.Substring(1);
            currentService.Description = char.ToUpper(currentService.Description[0]) + currentService.Description.Substring(1);

            return currentService;
        }
        public List<Pet> RetrieveAll()
        {
            var pets = new List<Pet>();

            foreach (var pet in _crud.RetrieveAll())
            {
                // Capitalize first letter
                pet.PetName = char.ToUpper(pet.PetName[0]) + pet.PetName.Substring(1);
                pet.Description = char.ToUpper(pet.Description[0]) + pet.Description.Substring(1);

                pets.Add(pet);
            }
            return pets;
        }

        public List<Pet>? RetrieveByUserId(int id)
        {
            var pets = new List<Pet>();

            foreach (var pet in _crud.RetrieveByUserId(id))
            {
                // Capitalize first letter
                pet.PetName = char.ToUpper(pet.PetName[0]) + pet.PetName.Substring(1);
                pet.Description = char.ToUpper(pet.Description[0]) + pet.Description.Substring(1);

                pets.Add(pet);
            }
            return pets;

        }
        
    }
}