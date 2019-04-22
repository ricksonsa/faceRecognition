using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Newtonsoft.Json;

namespace Training
{
    class Program
    {
        private static string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string FacesPath = exePath + @"\Faces\Data\";

        static CascadeClassifier FaceDetection;
        static CascadeClassifier ProfileFaceDetection;
        static EigenFaceRecognizer Recognizer;

        static Mat Frame;

        //Lista de imagens para treinamento
        static List<Image<Gray, byte>> Faces;

        //Lista de ids para label de reconhecimento
        static List<int> IDs;

        static List<Face> FacesList;

        static int ProcessedImageWidth = 128;
        static int ProcessedImageHeigth = 150;

        int TimerCount = 0;
        int TimeLimit = 15;
        int ScanCounter = 0;
        static int Count = 0;

        static string YMLPath = exePath + @"\trainingData.yml";

        static void Main(string[] args)
        {
            ProfileFaceDetection = new CascadeClassifier(exePath + @"\haarcascade_profileface_default.xml");
            FaceDetection = new CascadeClassifier(exePath + @"\haarcascade_frontalface_default.xml");
            Recognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
            Frame = new Mat();
            Faces = new List<Image<Gray, byte>>();
            IDs = new List<int>();
            FacesList = new List<Face>();

            DirectoryInfo dirInfo = new DirectoryInfo(FacesPath);
            foreach (var dir in dirInfo.GetDirectories())
            {
                Count += 1;
                var dirLastName = dir.FullName.Substring(dir.FullName.LastIndexOf(@"\"));
                if (File.Exists(dir.FullName + dir.FullName.LastIndexOf(@"\")+1 + ".txt"))
                {
                    FileInfo file = new FileInfo(dir.FullName + dirLastName + ".txt");

                    if (file.Name.EndsWith(".txt"))
                    {
                        string json = File.ReadAllText(FacesPath + file.Name);
                        Face face = JsonConvert.DeserializeObject<Face>(json);
                        FacesList.Add(face);
                        Console.Write("Identificação facial de " + face.nome + " encontrado.\n");
                        Console.Write("FacesList Size = " + FacesList.Count + " Count = " + Count + Environment.NewLine);
                        Console.Write("Preparando imagens para treinamento...\n");

                        Prepare(dir.FullName, face.id);
                    }
                }
                else
                {
                    Face face = new Face(Count, dirLastName.Substring(dirLastName.LastIndexOf(@"\")+1));
                    FacesList.Add(face);
                    File.WriteAllText(dir.FullName + @"\"+ dirLastName.Substring(dirLastName.LastIndexOf(@"\") + 1) + ".txt", JsonConvert.SerializeObject(face));
                    Console.Write("Identificação facial de " + face.nome + " encontrado.\n");
                    Console.Write("FacesList Size = " + FacesList.Count + " Count = " + Count + Environment.NewLine);
                    Console.Write("Preparando imagens para treinamento...\n");

                    Prepare(dir.FullName, face.id);
                }

            }

            //Training
            Train();

            Console.Read();
        }

        private static void Enrichment(Bitmap bmp, string path, int label)
        {
            Console.Write("Enriquecendo data set...\n");
            //var processedImage = new Image<Gray, Byte>(bmp);

            //var rotateRight = RotateImage(bmp, 10);
            //rotateRight.Save(path + "_rotated_right.bmp");
            //Faces.Add(new Image<Gray, Byte>(rotateRight));
            //IDs.Add(label);

            //var rotateLeft = RotateImage(bmp, -10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(rotateLeft));
            //rotateLeft.Save(path + "_rotated_left.bmp");

            //var flipped = processedImage.Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
            //IDs.Add(label);
            //Faces.Add(flipped);
            //flipped.Save(path + "_flipped.bmp");

            //var flippedRotatedLeft = RotateImage(flipped.ToBitmap(), -10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(flippedRotatedLeft));
            //flippedRotatedLeft.Save(path + "_flipped_rotated_left.bmp");

            //var flippedRotatedRight = RotateImage(flipped.ToBitmap(), 10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(flippedRotatedRight));
            //flippedRotatedRight.Save(path + "_flipped_rotated_right.bmp");

            //Contrast

            //Light Contrast

            var contrast = ImageUtils.ImageUtil.Contrast(bmp, -20);
            contrast.Save(path + "_light_contrast.bmp");
            Faces.Add(new Image<Gray, Byte>(contrast));
            IDs.Add(label);

            //var rotateRightContrast = RotateImage(contrast, 10);
            //rotateRightContrast.Save(path + "_rotated_right_contrast.bmp");
            //Faces.Add(new Image<Gray, Byte>(rotateRightContrast));
            //IDs.Add(label);

            //var rotateLeftContrast = RotateImage(contrast, -10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(rotateLeftContrast));
            //rotateLeftContrast.Save(path + "_rotated_left_contrast.bmp");

            //var flippedLightContrast = new Image<Gray, Byte>(contrast).Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
            //IDs.Add(label);
            //Faces.Add(flippedLightContrast);
            //flippedLightContrast.Save(path + "_flipped_light_contrast.bmp");

            //var flippedLightContrastRotatedRight = RotateImage(flippedLightContrast.ToBitmap(), 10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(flippedLightContrastRotatedRight));
            //flippedLightContrastRotatedRight.Save(path + "_flipped_light_contrast_rotated_right.bmp");

            //var flippedLightContrastRotatedLeft = RotateImage(flippedLightContrast.ToBitmap(), -10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(flippedLightContrastRotatedLeft));
            //flippedLightContrastRotatedLeft.Save(path + "_flipped_light_contrast_rotated_left.bmp");

            // Dark Contrast

            var darkcontrast = ImageUtils.ImageUtil.Contrast(bmp, 25);
            darkcontrast.Save(path + "_dark_contrast.bmp");
            Faces.Add(new Image<Gray, Byte>(darkcontrast));
            IDs.Add(label);

            //var rotateRightDarkContrast = RotateImage(darkcontrast, 10);
            //rotateRightDarkContrast.Save(path + "_rotated_right_dark_contrast.bmp");
            //Faces.Add(new Image<Gray, Byte>(rotateRightDarkContrast));
            //IDs.Add(label);

            //var rotateLeftDarkContrast = RotateImage(darkcontrast, -10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(rotateLeftDarkContrast));
            //rotateLeftDarkContrast.Save(path + "_rotated_left_dark_contrast.bmp");

            //var flippedDarkContrast = new Image<Gray, Byte>(darkcontrast).Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
            //IDs.Add(label);
            //Faces.Add(flippedDarkContrast);
            //flippedDarkContrast.Save(path + "_flipped_dark_contrast.bmp");

            //var flippedDarkContrastRotatedRight = RotateImage(flippedDarkContrast.ToBitmap(), 10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(flippedDarkContrastRotatedRight));
            //flippedDarkContrastRotatedRight.Save(path + "_flipped_dark_contrast_rotated_right.bmp");

            //var flippedDarkContrastRotatedLeft = RotateImage(flippedDarkContrast.ToBitmap(), -10);
            //IDs.Add(label);
            //Faces.Add(new Image<Gray, Byte>(flippedDarkContrastRotatedLeft));
            //flippedDarkContrastRotatedLeft.Save(path + "_flipped_dark_contrast_rotated_left.bmp");

        }

        static void Train()
        {
            List<Mat> mats = new List<Mat>();
            foreach (var item in Faces)
            {
                mats.Add(item.Mat);
            }
            Console.Write("Treinando com dados recolhidos...\n");
            Recognizer.Train(mats.ToArray(), IDs.ToArray());
            Recognizer.Write(YMLPath);
            Console.Write("Arquivo YML escrito.\nTreinamento finalizado.\n");
        }

        static void Prepare(string dir, int label)
        {
            //Pasta da pessoa dentro da pasta Faces\Data
            var subDirInfo = new DirectoryInfo(dir);

            foreach (var imagem in subDirInfo.GetFiles())
            {
                if (!imagem.Extension.EndsWith(".txt"))
                {
                    //Transforma a imagem em escala de cinza para fazer o treinamento
                    var bmp = new Image<Gray, byte>(imagem.FullName);

                    var faces = FaceDetection.DetectMultiScale(bmp, 1.2, 10);
                    var profileFaces = ProfileFaceDetection.DetectMultiScale(bmp, 1.2, 10);

                    if (profileFaces.Count() > 0)
                    {
                        var frontalFace = bmp.Copy(faces[0]).Resize(ProcessedImageWidth, ProcessedImageHeigth, Emgu.CV.CvEnum.Inter.Cubic);
                        Faces.Add(frontalFace);
                        IDs.Add(label);

                        //Enrich
                        Enrichment(frontalFace.ToBitmap(), dir, label);
                    }
                    if (faces.Count() > 0)
                    {
                        var profileFace = bmp.Copy(faces[0]).Resize(ProcessedImageWidth, ProcessedImageHeigth, Emgu.CV.CvEnum.Inter.Cubic);
                        Faces.Add(profileFace);
                        IDs.Add(label);

                        //Enrich
                        Enrichment(profileFace.ToBitmap(), dir, label);
                    }
                }

            }
        }
    }
}
