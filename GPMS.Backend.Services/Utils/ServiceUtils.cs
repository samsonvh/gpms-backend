using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using AutoMapper.Internal;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Services.Utils
{
    public static class ServiceUtils
    {
        public static void ValidateInputDTO<I, E>(I inputDTO, IValidator<I> validator)
        where I : class
        where E : class
        {
            List<FormError> errors = new List<FormError>();
            ValidationResult validationResult = validator.Validate(inputDTO);
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    errors.Add(new FormError
                    {
                        ErrorMessage = validationFailure.ErrorMessage,
                        Property = validationFailure.PropertyName
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, $"{typeof(E).Name} invalid", errors);
            }
        }
        public static void ValidateInputDTOList<I, E>(List<I> inputDTOs, IValidator<I> validator)
        where I : class
        where E : class
        {
            List<FormError> errors = new List<FormError>();
            foreach (I inputDTO in inputDTOs)
            {
                ValidationResult validationResult = validator.Validate(inputDTO);
                if (!validationResult.IsValid)
                {
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errors.Add(new FormError
                        {
                            ErrorMessage = validationFailure.ErrorMessage,
                            Property = validationFailure.PropertyName
                        });
                    }
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, $"{typeof(E).Name} list invalid", errors);
            }
        }

        public static async Task CheckFieldDuplicatedWithInputDTOAndDatabase<I, E>(
            I inputDTO, IGenericRepository<E> repository, string inputDTOField, string entityField)
        where E : class
        where I : class
        {
            string inputDTOFieldValue = inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO).ToString();
            var parameter = Expression.Parameter(typeof(E), "entity");//tao tham so entity dai dien cho kieu product
            var property = Expression.Property(parameter, entityField); //lay field cua product
            var toStringCall = Expression.Call(property, "ToString", Type.EmptyTypes); //call phuong thuc ToString cho field voi khong tham so dau vao
            var constant = Expression.Constant(inputDTOFieldValue); //tao tham so thu 2 de so sanh voi field
            var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string) });//lay method equal voi 1 tham so string
            var equalCall = Expression.Call(toStringCall, equalsMethod, constant); // Goi phuong thuc Equal de so sanh field voi tham so thu 2
            var lambda = Expression.Lambda<Func<E, bool>>(equalCall, parameter); //tao lambda expression
            E codeDuplicatedEntity = await repository.Search(lambda).FirstOrDefaultAsync();
            if (codeDuplicatedEntity != null)
            {
                FormError error = new FormError
                {
                    Property = inputDTO.GetType().GetProperty(inputDTOField).Name,
                    ErrorMessage = $"{typeof(E).Name} with {inputDTOField} : {inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO)} duplicated"
                };
                throw new APIException((int)HttpStatusCode.BadRequest, $"{inputDTOField} duplicate in {typeof(E).Name} ", error);
            }
        }

        public static async Task CheckFieldDuplicatedWithInputDTOListAndDatabase<I, E>(
            List<I> inputDTOs, IGenericRepository<E> repository, string inputDTOField, string entityField)
        where E : class
        where I : class
        {
            List<FormError> errors = new List<FormError>();
            foreach (I inputDTO in inputDTOs)
            {
                string inputDTOFieldValue = inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO).ToString();
                var parameter = Expression.Parameter(typeof(E), "entity");//tao tham so entity dai dien cho kieu product
                var property = Expression.Property(parameter, entityField); //lay field cua product
                var toStringCall = Expression.Call(property, "ToString", Type.EmptyTypes); //call phuong thuc ToString cho field voi khong tham so dau vao
                var constant = Expression.Constant(inputDTOFieldValue); //tao tham so thu 2 de so sanh voi field
                var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string) });//lay method equal voi 1 tham so string
                var equalCall = Expression.Call(toStringCall, equalsMethod, constant); // Goi phuong thuc Equal de so sanh field voi tham so thu 2
                var lambda = Expression.Lambda<Func<E, bool>>(equalCall, parameter); //tao lambda expression
                E codeDuplicatedEntity =
                await repository
                .Search(lambda).FirstOrDefaultAsync();
                if (codeDuplicatedEntity != null)
                {
                    errors.Add(new FormError
                    {
                        Property = inputDTO.GetType().GetProperty(inputDTOField).Name,
                        ErrorMessage = $"{typeof(E).Name} with {inputDTOField} : {inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO)} duplicated"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, $"{inputDTOField} duplicate in {typeof(E).Name} list", errors);
            }
        }

        public static void CheckForeignEntityCodeInInputDTOLisExistedInForeignEntityCodeList<I, E, CU>
        (List<I> inputDTOs, List<CreateUpdateResponseDTO<CU>> foreignEntityCodeList, string inputDTOField)
        where I : class
        where E : class
        where CU : class
        {
            List<FormError> errors = new List<FormError>();
            foreach (I inputDTO in inputDTOs)
            {
                if (foreignEntityCodeList.FirstOrDefault(
                    foreignEntityCode => foreignEntityCode.Code
                    .Equals(inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO).ToString()))
                    == null)
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(I).GetProperty(inputDTOField).Name,
                        ErrorMessage = $"{typeof(E).Name} with {inputDTOField} : {inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO)} is not in {typeof(CU).Name} list"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, $"{inputDTOField} in {typeof(E).Name} list invalid", errors);
            }
        }
        public static void CheckFieldDuplicatedInInputDTOList<I>(List<I> inputDTOs, string field)
        where I : class
        {
            inputDTOs.OrderBy(inputDTO => inputDTO.GetType().GetProperty(field));
            List<FormError> errors = new List<FormError>();
            foreach (I inputDTO in inputDTOs)
            {
                string fieldValue = inputDTO.GetType().GetProperty(field).GetValue(field).ToString();
                int duplicatedCount = 0;
                foreach (I inputDTOCompare in inputDTOs)
                {
                    string compareValue = inputDTOCompare.GetType().GetProperty(field).GetValue(field).ToString();
                    if (fieldValue.Equals(compareValue))
                    {
                        duplicatedCount ++;
                    }
                    // if (duplicatedCount)        
                }
            }
        }   
    }
}