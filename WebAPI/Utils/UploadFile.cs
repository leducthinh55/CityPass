using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Utils
{
    public interface IUploadFile
    {
        Task<String> uploadFile(IFormFile imageUpload, String fileName);
    }
    public class UploadFile : IUploadFile
    {
        public async Task<string> uploadFile(IFormFile imageUpload, String fileName)
        {
            var file = imageUpload;
            Stream ms = file.OpenReadStream();
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
            "citypass131999.appspot.com",
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult("eyJhbGciOiJSUzI1NiIsImtpZCI6IjRlMDBlOGZlNWYyYzg4Y2YwYzcwNDRmMzA3ZjdlNzM5Nzg4ZTRmMWUiLCJ0eXAiOiJKV1QifQ.eyJhZG1pbiI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL2NpdHlwYXNzMTMxOTk5IiwiYXVkIjoiY2l0eXBhc3MxMzE5OTkiLCJhdXRoX3RpbWUiOjE2MTY1NTQzNTksInVzZXJfaWQiOiJscURaSE45T05tZU12MWJQdmk4SjVzS1VQR1YyIiwic3ViIjoibHFEWkhOOU9ObWVNdjFiUHZpOEo1c0tVUEdWMiIsImlhdCI6MTYxNjU1NDM1OSwiZXhwIjoxNjE2NTU3OTU5LCJlbWFpbCI6InRoaW5oQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJ0aGluaEBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.Fo4-suiJN2srzcicYejDGGero5nFmNj-SWfgMA-YcLbfgfHgA1P8gcg1Y9-Kc2bJTffEhpYTiqgUgft9TRMgDSyUsUt4xLpNC77KqjbGEPM5iqrMOepJAm6A6XLizq9s-ija2BIIrUXDkIHkTaVwC_Q5zsVcmiRZkUo8A_RHW-U7-4juCGkKs8o0tjvrSi-64XDcl8OLUbCWeACYfeJw2lcQvH4I4NgZrdXJcgUi9oHQJ8CqyqmUWdIwnLgF6ETRIgolflK0kjNWpO9HA0b2Avu4zggp9DQfPlw3wID3WZG8Pe7c-2Og3pKj8ymVOZnqegvemJkWLtu7Qqin0RynPA"),
                ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    })
            .Child("ticket-type")
            .Child($"{fileName}")
            .Child(file.Name + DateTime.Now.ToString())
            .PutAsync(ms, cancellation.Token);
            String link = await task;
            return link;
        }
    }
}
