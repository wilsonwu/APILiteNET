using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILiteNET.Service
{
    public enum ReturnCodes
    {
        ParameterError = 0,
        GenericException = 1,
        VerifySecretKeySucceeded = 2,
        VerifySecretKeyError = 3,

        DataCreateSucceeded = 100,
        DataCreatePartiallySucceeded = 101,
        DataUpdateSucceeded = 102,
        DataUpdatePartiallySucceeded = 103,
        DataRemoveSucceeded = 104,
        DataRemovePartiallySucceeded = 105,
        DataGetSucceeded = 106,

        DataCreateFailed = 200,
        DataCreateFailedWithDuplicateData = 201,
        DataCreateFailedWithErrorRelationships = 202,
        DataUpdateFailed = 203,
        DataUpdateFailedWithDuplicateData = 204,
        DataUpdateFailedWithErrorRelationships = 205,
        DataRemoveFailed = 206,
        DataGetFailed = 207,
        DataGetFailedWithErrorRelationships = 208,
        DataGetFailedWithNoData = 209,
    }
}