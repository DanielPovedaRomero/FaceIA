using Azure;
using Azure.AI.Vision.Face;
using static Azure.AI.Vision.Face.FaceAttributeType;


namespace FaceIA
{
    internal class Program
    {
        static async Task Main()
        {

            string endpoint = "END POINT AZURE";
            string apiKey = "KEY AZURE";
            string imagenUrl = "URL IMAGEN";

            var faceClient = new FaceClient(
                 new Uri(endpoint),
                 new AzureKeyCredential(apiKey));


            FaceAttributeType[] features = new FaceAttributeType[]
             {
                Detection01.HeadPose,
                Detection01.Glasses,
                Detection01.Occlusion,
                Detection01.Accessories,
                Detection01.Blur,
                Detection01.Exposure,
                Detection01.Noise
             };

            var response = await faceClient.DetectAsync(
                new Uri(imagenUrl),
                FaceDetectionModel.Detection01, 
                FaceRecognitionModel.Recognition04,
                returnFaceId: false,
                returnFaceAttributes: features);

            IReadOnlyList<FaceDetectionResult> detected_faces = response.Value;

            if (detected_faces.Count > 0)
            {
                Console.WriteLine($"{detected_faces.Count} caras detectadas.\n");

                foreach (var face in detected_faces)
                {
                    var attributes = face.FaceAttributes;

                    var headPose = attributes.HeadPose;
                    Console.WriteLine($"*Posición de la cabeza: Inclinación={headPose.Pitch}, Roll ={headPose.Roll}, Giro ={headPose.Yaw}");
         
                    var glasses = attributes.Glasses;
                    Console.WriteLine($"*Tipo de gafas: {glasses}");

                    var occlusion = attributes.Occlusion;
                    Console.WriteLine($"*Oclusión: Frente={occlusion.ForeheadOccluded}, Ojos={occlusion.EyeOccluded}, Boca={occlusion.MouthOccluded} ");

                    var accessories = attributes.Accessories;
                    if (accessories != null && accessories.Count > 0)
                    {
                        Console.WriteLine("*Accesorios detectados:");
                        foreach (var item in accessories)
                        {
                            Console.WriteLine($"   - Tipo: {item.Type}, Confianza: {item.Confidence:P2}");
                        }
                    }

                    var blur = attributes.Blur;
                    Console.WriteLine($"*Borrosidad: Nivel={blur.BlurLevel}, Valor={blur.Value}");

                    var exposure = attributes.Exposure;
                    Console.WriteLine($"*Exposición: {exposure.ExposureLevel}");


                    var noise = attributes.Noise;
                    Console.WriteLine($"*Ruido: {noise.NoiseLevel}");

                    Console.WriteLine("-------------------------------");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No se detectaron caras.");
            }
        }
    }
}

