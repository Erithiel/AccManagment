
/*
namespace AccManagment.API.Services;


using DlibDotNet;
using DlibDotNet.Dnn;
using System;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;




public class FacialRecognitionService
{
    private readonly IFaceClient _faceClient;

    public FacialRecognitionService(string endpoint, string subscriptionKey)
    {
        _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(subscriptionKey))
        {
            Endpoint = endpoint
        };
    }

    public async Task<bool> AreSamePersonAsync(byte[] image1Data, byte[] image2Data)
    {
        using (var image1Stream = new MemoryStream(image1Data))
        using (var image2Stream = new MemoryStream(image2Data))
        {
            var faceIds = await DetectFacesAsync(image1Stream, image2Stream);

            if (faceIds.Count != 2)
            {
                return false; // Faces not detected in both images
            }

            var verifyResult = await _faceClient.Face.VerifyFaceToFaceAsync(faceIds[0], faceIds[1]);
            return verifyResult.IsIdentical;
        }
    }

    private async Task<List<Guid>> DetectFacesAsync(Stream image1Stream, Stream image2Stream)
    {
        var detectedFaceIds = new List<Guid>();

        var detectedFaces1 = await _faceClient.Face.DetectWithStreamAsync(image1Stream);
        if (detectedFaces1.Count > 0)
        {
            detectedFaceIds.Add(detectedFaces1[0].FaceId.Value);
        }

        var detectedFaces2 = await _faceClient.Face.DetectWithStreamAsync(image2Stream);
        if (detectedFaces2.Count > 0)
        {
            detectedFaceIds.Add(detectedFaces2[0].FaceId.Value);
        }

        return detectedFaceIds;
    }
}*/