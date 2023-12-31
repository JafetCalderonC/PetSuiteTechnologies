﻿using DataAccess.DAOs;
using DataAccess.Mapper;
using DTOs.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class UserCrudFactory : CrudFactory<User>
    {
        private readonly UserMapper _mapper;
        protected SqlDao _dao;

        public UserCrudFactory()
        {
            _mapper = new UserMapper();
            _dao = SqlDao.GetInstance();
        }

        public override void Create(User dto)
        {
            var sqlOperation = new SqlOperation("CREATE_USER_PR");
            sqlOperation.AddParameter("@P_IS_PASSWORD_REQUIRED_CHANGE", dto.IsPasswordRequiredChange);
            sqlOperation.AddParameter("@P_PASSWORD_HASH", dto.PasswordHash);
            sqlOperation.AddParameter("@P_PASSWORD_SALT", dto.PasswordSalt);
            sqlOperation.AddParameter("@P_ROLE", dto.Role);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_FIRST_NAME", dto.FirstName);
            sqlOperation.AddParameter("@P_LAST_NAME", dto.LastName);
            sqlOperation.AddParameter("@P_IDENTIFICATION_TYPE", dto.IdentificationType);
            sqlOperation.AddParameter("@P_IDENTIFIER_VALUE", dto.IdentificationValue);
            sqlOperation.AddParameter("@P_EMAIL", dto.Email);
            sqlOperation.AddParameter("@P_CLOUNDINARY_PUBLIC_ID", dto.CloudinaryPublicId);
            sqlOperation.AddParameter("@P_THEME_PREFERENCE", dto.ThemePreference);
            sqlOperation.AddParameter("@P_CREATED_DATE", dto.CreatedDate);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ModifiedDate);
            sqlOperation.AddParameter("@P_ADDRESS_LATITUDE", dto.AddressLatitude);
            sqlOperation.AddParameter("@P_ADDRESS_LONGITUDE", dto.AddressLongitude);

            // Create the user y get the id
            _dao.ExecuteProcedure(sqlOperation, out int id);
            dto.Id = id;

            // Add phone numbers to user
            foreach (var phoneNumber in dto.PhoneNumbers)
            {
                sqlOperation = new SqlOperation("ADD_PHONE_NUMBER_TO_USER_PR");
                sqlOperation.AddParameter("@P_USER_ID", dto.Id);
                sqlOperation.AddParameter("@P_PHONE_NUMBER", phoneNumber);
                _dao.ExecuteProcedure(sqlOperation, out int phoneNumberId);
            }
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation("DELETE_USER_PR");
            sqlOperation.AddParameter("@P_USER_ID", id);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", DateTime.UtcNow);

            _dao.ExecuteProcedure(sqlOperation);
        }

        public override List<User> RetrieveAll()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ALL_USERS_PR");
            var result = _dao.ExecuteQueryProcedure(sqlOperation);

            return _mapper.BuildObjects(result);
        }

        public override User? RetrieveById(int id)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_USER_BY_ID_PR");
            sqlOperation.AddParameter("@P_USER_ID", id);

            var result = _dao.ExecuteQueryProcedure(sqlOperation);
            if (result.Count == 0)
                return null;

            return _mapper.BuildObject(result[0]);
        }

        public override void Update(User dto)
        {
            var sqlOperation = new SqlOperation("UPDATE_USER_PR");
            sqlOperation.AddParameter("@P_USER_ID", dto.Id);
            sqlOperation.AddParameter("@P_PASSWORD_HASH", dto.PasswordHash);
            sqlOperation.AddParameter("@P_PASSWORD_SALT", dto.PasswordSalt);
            sqlOperation.AddParameter("@P_ROLE", dto.Role);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_FIRST_NAME", dto.FirstName);
            sqlOperation.AddParameter("@P_LAST_NAME", dto.LastName);
            sqlOperation.AddParameter("@P_IDENTIFICATION_TYPE", dto.IdentificationType);
            sqlOperation.AddParameter("@P_IDENTIFIER_VALUE", dto.IdentificationValue);
            sqlOperation.AddParameter("@P_EMAIL", dto.Email);
            sqlOperation.AddParameter("@P_CLOUNDINARY_PUBLIC_ID", dto.CloudinaryPublicId);
            sqlOperation.AddParameter("@P_THEME_PREFERENCE", dto.ThemePreference);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ModifiedDate);
            sqlOperation.AddParameter("@P_ADDRESS_LATITUDE", dto.AddressLatitude);
            sqlOperation.AddParameter("@P_ADDRESS_LONGITUDE", dto.AddressLongitude);
            sqlOperation.AddParameter("@P_IS_PASSWORD_REQUIRED_CHANGE", dto.IsPasswordRequiredChange);

            _dao.ExecuteProcedure(sqlOperation);

            // Get phone numbers of the user
            sqlOperation = new SqlOperation("RETRIEVE_PHONE_NUMBERS_BY_USER_ID_PR");
            sqlOperation.AddParameter("@P_USER_ID", dto.Id);
            var resultNumbers = _dao.ExecuteQueryProcedure(sqlOperation);

            if (dto.PhoneNumbers == null)
            {
                return;
            }

            // If the phone number is not in the database, add it
            foreach (var phoneNumber in dto.PhoneNumbers)
            {
                if (!resultNumbers.Any(p => (string)p["phone_number"] == phoneNumber))
                {
                    sqlOperation = new SqlOperation("ADD_PHONE_NUMBER_TO_USER_PR");
                    sqlOperation.AddParameter("@P_USER_ID", dto.Id);
                    sqlOperation.AddParameter("@P_PHONE_NUMBER", phoneNumber);
                    _dao.ExecuteProcedure(sqlOperation);
                }
            }

            // If the phone number is not in the dto, delete it
            foreach (var phoneNumberRow in resultNumbers)
            {
                var currentPhoneNumber = (string)phoneNumberRow["phone_number"];
                if (!dto.PhoneNumbers.Any(p => p == currentPhoneNumber))
                {
                    sqlOperation = new SqlOperation("REMOVE_PHONE_NUMBER_TO_USER_PR");
                    sqlOperation.AddParameter("@P_USER_ID", dto.Id);
                    sqlOperation.AddParameter("@P_PHONE_NUMBER", currentPhoneNumber);
                    _dao.ExecuteProcedure(sqlOperation);
                }
            }
        }

        public void AddPhoneNumberToUser(int userId, string phoneNumber)
        {
            var sqlOperation = new SqlOperation("ADD_PHONE_NUMBER_TO_USER_PR");
            sqlOperation.AddParameter("@P_USER_ID", userId);
            sqlOperation.AddParameter("@P_PHONE_NUMBER", phoneNumber);
            _dao.ExecuteProcedure(sqlOperation);
        }

        public void RemovePhoneNumberToUser(int userId, string phoneNumber)
        {
            var sqlOperation = new SqlOperation("REMOVE_PHONE_NUMBER_TO_USER_PR");
            sqlOperation.AddParameter("@P_USER_ID", userId);
            sqlOperation.AddParameter("@P_PHONE_NUMBER", phoneNumber);
            _dao.ExecuteProcedure(sqlOperation);
        }

        public List<string> RetrieveAllPhoneNumbersByUserId(int userId)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_PHONE_NUMBERS_BY_USER_ID_PR");
            sqlOperation.AddParameter("@P_USER_ID", userId);
            var result = _dao.ExecuteQueryProcedure(sqlOperation);

            return result.Select(p => (string)p["phone_number"]).ToList();
        }

        public User? RetrieveByEmail(string email)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_USER_BY_EMAIL_PR");
            sqlOperation.AddParameter("@P_EMAIL", email);

            var result = _dao.ExecuteQueryProcedure(sqlOperation);
            if (result.Count == 0)
                return null;

            return _mapper.BuildObject(result[0]);
        }

        public User? RetrieveByPhoneNumber(string phoneNumber)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_USER_BY_PHONE_NUMBER_PR");
            sqlOperation.AddParameter("@P_PHONE_NUMBER", phoneNumber);

            var result = _dao.ExecuteQueryProcedure(sqlOperation);
            if (result.Count == 0)
                return null;

            return _mapper.BuildObject(result[0]);
        }

        public List<User> RetrieveAllClients()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ALL_CLIENTS_PR");
            var result = _dao.ExecuteQueryProcedure(sqlOperation);

            return _mapper.BuildObjects(result);
        }
    }
}