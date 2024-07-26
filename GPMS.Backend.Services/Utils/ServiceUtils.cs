using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
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
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils
{
    public static class ServiceUtils
    {
        public static void ValidateInputDTO<I, E>(I inputDTO, IValidator<I> validator,
        EntityListErrorWrapper entityListErrorWrapper)
        where I : class
        where E : class
        {
            List<FormError> errors = new List<FormError>();
            ValidationResult validationResult = validator.Validate(inputDTO);
            if (!validationResult.IsValid)
            {
                foreach (FluentValidation.Results.ValidationFailure validationFailure in validationResult.Errors)
                {
                    errors.Add(new FormError
                    {
                        ErrorMessage = validationFailure.ErrorMessage,
                        Property = validationFailure.PropertyName,
                        EntityOrder = 1
                    });

                }
            }
            if (errors.Count > 0)
            {
                EntityListError entityListError = new EntityListError
                {
                    Entity = typeof(E).Name,
                    Errors = errors
                };

                entityListErrorWrapper.EntityListErrors.Add(entityListError);
            }
        }
        public static void ValidateInputDTOList<I, E>(List<I> inputDTOs, IValidator<I> validator,
        EntityListErrorWrapper entityListErrorWrapper)
        where I : class
        where E : class
        {
            List<FormError> errors = new List<FormError>();
            foreach (I inputDTO in inputDTOs)
            {
                int entityOrder = 1;
                ValidationResult validationResult = validator.Validate(inputDTO);
                if (!validationResult.IsValid)
                {
                    foreach (FluentValidation.Results.ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errors.Add(new FormError
                        {
                            ErrorMessage = validationFailure.ErrorMessage,
                            Property = validationFailure.PropertyName,
                            EntityOrder = entityOrder
                        });
                    }
                }
                entityOrder++;
            }

            if (errors.Count > 0)
            {
                EntityListError entityListError = new EntityListError
                {
                    Entity = typeof(E).Name,
                    Errors = errors
                };
                entityListErrorWrapper.EntityListErrors.Add(entityListError);
            }
        }

        public static async Task CheckFieldDuplicatedWithInputDTOAndDatabase<I, E>(
            I inputDTO, IGenericRepository<E> repository, string inputDTOField, string entityField,
            EntityListErrorWrapper entityListErrorWrapper)
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
                    Property = inputDTOField,
                    ErrorMessage = $"There is {typeof(E).Name} with {inputDTOField} : {inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO)} already existed in system ",
                    EntityOrder = 1
                };
                List<FormError> errors = [error];
                EntityListError entityListError = new EntityListError
                {
                    Entity = typeof(E).Name,
                    Errors = errors
                };
                entityListErrorWrapper.EntityListErrors.Add(entityListError);
            }
        }

        public static async Task CheckFieldDuplicatedWithInputDTOListAndDatabase<I, E>(
            List<I> inputDTOs, IGenericRepository<E> repository, string inputDTOField,
            string entityField, EntityListErrorWrapper entityListErrorWrapper)
        where E : class
        where I : class
        {
            List<FormError> errors = new List<FormError>();
            foreach (I inputDTO in inputDTOs)
            {
                int entityOrder = 1;
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
                        Property = inputDTOField,
                        ErrorMessage = $"There is {typeof(E).Name} with {inputDTOField} : {inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO)} already existed in system ",
                        EntityOrder = entityOrder
                    });
                }
                entityOrder++;
            }
            if (errors.Count > 0)
            {
                EntityListError entityListError = new EntityListError
                {
                    Entity = typeof(E).Name,
                    Errors = errors
                };
                entityListErrorWrapper.EntityListErrors.Add(entityListError);
            }
        }

        public static void CheckForeignEntityCodeInInputDTOListExistedInForeignEntityCodeList<I, E, CU>
        (List<I> inputDTOs, List<CreateUpdateResponseDTO<CU>> foreignEntityCodeList,
        string inputDTOField, EntityListErrorWrapper entityListErrorWrapper)
        where I : class
        where E : class
        where CU : class
        {

            if (inputDTOs.Count > 0)
            {
                List<FormError> errors = new List<FormError>();
                foreach (I inputDTO in inputDTOs)
                {
                    int entityOrder = 1;
                    if (foreignEntityCodeList.FirstOrDefault
                        (foreignEntityCode => foreignEntityCode.Code
                        .Equals(inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO).ToString()))
                        == null)
                    {
                        errors.Add(new FormError
                        {
                            Property = inputDTOField,
                            ErrorMessage = $"{typeof(E).Name} with {inputDTOField} : {inputDTO.GetType().GetProperty(inputDTOField).GetValue(inputDTO)} not existed in {typeof(CU).Name} list",
                            EntityOrder = entityOrder
                        });
                    }
                    entityOrder++;
                }
                if (errors.Count > 0)
                {
                    EntityListError entityListError = new EntityListError
                    {
                        Entity = typeof(E).Name,
                        Errors = errors
                    };
                    entityListErrorWrapper.EntityListErrors.Add(entityListError);
                }
            }
        }
        public static void CheckFieldDuplicatedInInputDTOList<I, E>(List<I> inputDTOs,
        string inputDTOField, EntityListErrorWrapper entityListErrorWrapper)
        where I : class
        where E : class
        {
            List<FormError> errors = new List<FormError>();
            List<string> duplicatedValueField = new List<string>();
            foreach (I inputDTO in inputDTOs)
            {
                int entityOrder = 1;
                string fieldValue = inputDTO.GetType().GetProperty(inputDTOField)
                                            .GetValue(inputDTO).ToString();
                int duplicatedCount = 0;
                foreach (I inputDTOCompare in inputDTOs)
                {
                    string compareValue = inputDTOCompare.GetType()
                                                        .GetProperty(inputDTOField)
                                                        .GetValue(inputDTOCompare)
                                                        .ToString();
                    if (fieldValue.Equals(compareValue))
                    {
                        duplicatedCount++;
                    }
                }
                if (duplicatedCount > 1 && !duplicatedValueField.Contains(fieldValue))
                {
                    duplicatedValueField.Add(fieldValue);
                    errors.Add(new FormError
                    {
                        Property = inputDTOField,
                        ErrorMessage = $"{typeof(E).Name} with {inputDTOField} : {fieldValue} duplicated",
                        EntityOrder = entityOrder
                    });
                }
                entityOrder++;
            }
            if (errors.Count > 0)
            {
                EntityListError entityListError = new EntityListError
                {
                    Entity = typeof(E).Name,
                    Errors = errors
                };
                entityListErrorWrapper.EntityListErrors.Add(entityListError);
            }
        }
        public static void CheckForeignEntityCodeListContainsAllForeignEntityCodeInInputDTOList<I, E, CU>
        (List<I> inputDTOs, List<CreateUpdateResponseDTO<CU>> foreignEntityCodeList,
        string inputDTOField, string foreignEntityCodeField, EntityListErrorWrapper entityListErrorWrapper)
        where I : class
        where E : class
        where CU : class
        {
            List<FormError> errors = new List<FormError>();

            foreach (I inputDTO in inputDTOs)
            {
                int entityOrder = 1;
                string inputDTOCode = inputDTO.GetType().GetProperty(inputDTOField)
                                                .GetValue(inputDTO).ToString();
                CreateUpdateResponseDTO<CU> foreignEntityCode = foreignEntityCodeList.FirstOrDefault
                (foreignEntityCode => foreignEntityCode.GetType().GetProperty(foreignEntityCodeField)
                                                        .GetValue(foreignEntityCode).Equals(inputDTOCode));
                if (foreignEntityCode == null)
                {
                    errors.Add(new FormError
                    {
                        Property = inputDTOField,
                        ErrorMessage = $" {typeof(E).Name} with {inputDTOField} : {inputDTOCode} not existed in {typeof(CU).Name} list",
                        EntityOrder = entityOrder
                    });
                }
                entityOrder++;
            }

            foreach (CreateUpdateResponseDTO<CU> foreignEntityCode in foreignEntityCodeList)
            {
                string entityCode = foreignEntityCode.GetType()
                                                    .GetProperty(foreignEntityCodeField)
                                                    .GetValue(foreignEntityCode)
                                                    .ToString();
                foreach (I inputDTOMissing in inputDTOs)
                {
                    int entityOrder = 1;
                    string inputDTOMissingFieldValue = inputDTOMissing.GetType()
                                                                    .GetProperty(inputDTOField)
                                                                    .GetValue(inputDTOMissing)
                                                                    .ToString();
                    if (!inputDTOMissingFieldValue.Equals(entityCode))
                    {
                        errors.Add(new FormError
                        {
                            Property = inputDTOField,
                            ErrorMessage = $"{typeof(E).Name} list missing {inputDTOField} : {entityCode} in {typeof(CU).Name} list",
                            EntityOrder = entityOrder
                        });
                    }
                    entityOrder++;
                }
            }
            if (errors.Count > 0)
            {
                EntityListError entityListError = new EntityListError
                {
                    Entity = typeof(E).Name,
                    Errors = errors
                };
                entityListErrorWrapper.EntityListErrors.Add(entityListError);
            }
        }

    }
}