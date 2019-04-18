﻿using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CognitiveService
{
    public partial class Form1 : Form
    {
        //paths
        string FacesPath = Application.StartupPath + @"\Faces\";

        VideoCapture Camera;
        CascadeClassifier FaceDetection;
        EigenFaceRecognizer Recognizer;

        Mat Frame;

        List<Image<Gray, byte>> Faces;
        List<int> IDs;

        List<Face> FacesList;

        int ProcessedImageWidth = 128;
        int ProcessedImageHeigth = 150;

        int TimerCount = 0;
        int TimeLimit = 30;
        int ScanCounter = 0;
        int Count = 0;

        string YMLPath = Application.StartupPath + @"\trainingData.yml";

        Timer timer;

        bool isTraining = false;

        //Controls
        public static ListBox rostoListBox;
        
        public Form1()
        {
            InitializeComponent();
            Recognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
            Recognizer.Read(YMLPath);
            FaceDetection = new CascadeClassifier(Application.StartupPath + @"\haarcascade_frontalface_default.xml");
            Frame = new Mat();
            Faces = new List<Image<Gray, byte>>();
            IDs = new List<int>();
            FacesList = new List<Face>();

            rostoListBox = new ListBox();
            rostoListBox.Size = detectedFaceslistBox.Size;
            rostoListBox.Location = detectedFaceslistBox.Location;
            panel1.Controls.Add(rostoListBox);

            try
            {
                DirectoryInfo di = new DirectoryInfo(FacesPath);

                foreach (var file in di.GetFiles())
                {
                    if (file.Name.EndsWith(".txt"))
                    {
                        string json = File.ReadAllText(FacesPath + file.Name);
                        Face face = JsonConvert.DeserializeObject<Face>(json);
                        FacesList.Add(face);
                        foreach (var imagem in face.imagens)
                        {
                            IDs.Add(face.id);
                            Faces.Add(new Image<Gray, byte>(FacesPath + imagem));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\nNada registrado ainda.", "Oh oh!", MessageBoxButtons.OK);
            }
        }

        private void BeginBtn_Click(object sender, EventArgs e)
        {
            if (Camera == null)
            {
                try
                {
                    //Camera = new VideoCapture();
                    Camera = new VideoCapture(FacesPath + "videoFile.mp4");
                }
                catch (Exception)
                {

                    throw;
                }
            }

            Camera.ImageGrabbed += Camera_ImageGrabbed;

            try
            {
                Camera.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK);
            }
        }

        private void Camera_ImageGrabbed(object sender, EventArgs e)
        {
            Camera.Retrieve(Frame);
            var imageFrame = Frame.ToImage<Bgr, byte>();

            if (imageFrame != null)
            {
                var grayFrame = imageFrame.Convert<Gray, byte>();
                var faces = FaceDetection.DetectMultiScale(grayFrame, 1.2, 10);
                Face person = null;

                if (faces.Count() != 0)
                {
                    foreach (var face in faces)
                    {
                        try
                        {
                            var processedImage = imageFrame.Copy(face).Convert<Gray, Byte>().Resize(ProcessedImageWidth, ProcessedImageHeigth, Emgu.CV.CvEnum.Inter.Cubic);

                            //Desenha quadrado em volta
                            imageFrame.Draw(face, new Bgr(Color.BurlyWood), 2);

                            if (!isTraining)
                            {
                                var result = Recognizer.Predict(processedImage);

                                if (result.Distance < 4000)
                                    person = GetPersonById(result.Label);

                                string text;

                                if (person == null)
                                {
                                    text = "Nao conhecido";
                                }
                                else
                                {
                                    text = person.nome;
                                    //if(!rostoListBox.Items.Contains(person.nome))
                                    //{
                                    //    rostoListBox.Items.Add(person.nome);
                                    //}
                                }

                                imageFrame.Draw(text + " - " + result.Distance, new Point(face.Location.X - 2, face.Location.Y - 2), Emgu.CV.CvEnum.FontFace.HersheyTriplex, 0.5, new Bgr(Color.Red));
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erro");
                        }
                    }
                }
                imageBox.Image = imageFrame;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(nameBox.Text))
            {
                timer = new Timer();
                timer.Interval = 500;
                timer.Tick += Timer_Tick;
                timer.Start();

                isTraining = true;
                saveBtn.Enabled = !saveBtn.Enabled;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Count += 1;
            var time = DateTime.Now.ToString("HHmmssss");
            Face face = new Face(Count, nameBox.Text);

            Camera.Retrieve(Frame);
            var imageFrame = Frame.ToImage<Gray, byte>();

            if (TimerCount < TimeLimit)
            {
                TimerCount++;

                if (imageFrame != null)
                {
                    var faces = FaceDetection.DetectMultiScale(imageFrame, 1.2, 10);

                    if (faces.Count() > 0)
                    {
                        var processedImage = imageFrame.Copy(faces[0]).Resize(ProcessedImageWidth, ProcessedImageHeigth, Emgu.CV.CvEnum.Inter.Cubic);
                        processedImage.Save(FacesPath + face.nome + time + ".bmp");

                        if (GetPersonByNome(face.nome) != null)
                        {
                            var person = GetPersonByNome(nameBox.Text);
                            if (person.imagens == null)
                                person.imagens = new List<string>();
                            person.imagens.Add(face.nome + time + ".bmp");
                            File.WriteAllText(FacesPath + @"\" + person.nome + ".txt", JsonConvert.SerializeObject(person));
                        }
                        else
                        {
                            face.imagens = new List<string>();
                            face.imagens.Add(face.nome + time + ".bmp");
                            FacesList.Add(face);
                            File.WriteAllText(FacesPath + @"\" + face.nome + ".txt", JsonConvert.SerializeObject(face));
                        }

                        Faces.Add(processedImage);
                        IDs.Add(face.id);
                        ScanCounter++;
                    }
                }
            }
            else
            {
                timer.Stop();
                List<Mat> mats = new List<Mat>();
                foreach (var item in Faces)
                {
                    mats.Add(item.Mat);
                }
                Recognizer.Train(mats.ToArray(), IDs.ToArray());
                Recognizer.Write(YMLPath);
                TimerCount = 0;
                saveBtn.Enabled = !saveBtn.Enabled;
                MessageBox.Show("Terinamento realizado", "Treinado", MessageBoxButtons.OK);
                isTraining = false;
                Recognizer.Read(YMLPath);

            }

        }

        private Mat GetMatFromSDImage(Image<Gray, byte> image)
        {
            int stride = 0;
            Bitmap bmp = new Bitmap(image.ToBitmap());

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            System.Drawing.Imaging.PixelFormat pf = bmp.PixelFormat;
            if (pf == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                stride = bmp.Width * 4;
            }
            else
            {
                stride = bmp.Width * 3;
            }

            Image<Bgra, byte> cvImage = new Image<Bgra, byte>(bmp.Width, bmp.Height, stride, (IntPtr)bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            return cvImage.Mat;
        }

        Face GetPersonByNome(string nome)
        {
            foreach (var person in FacesList)
            {
                if (person.nome == nome)
                {
                    return person;
                }
            }
            return null;
        }

        Face GetPersonById(int id)
        {
            foreach (var person in FacesList)
            {
                if (person.id == id)
                {
                    return person;
                }
            }
            return null;
        }

        //private void FrameProcedure(object sender, EventArgs e)
        //{
        //    users.Add("");
        //    frame = camera.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
        //    grayFace = frame.Convert<Gray, Byte>();
        //    MCvAvgComp[][] facesDetectedNow = grayFace.DetectHaarCascade(faceDetected, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

        //    foreach (var f in facesDetectedNow[0])
        //    {
        //        result = frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
        //        frame.Draw(f.rect, new Bgr(Color.Green), 2);

        //        if (trainingImages.ToArray().Length != 0)
        //        {

        //            name = recognizer.Recognize(result);
        //            if (!detectedFaceslistBox.Items.Contains(name))
        //            {
        //                detectedFaceslistBox.Items.Add(name);
        //            }
        //            frame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
        //        }
        //        // users[t - 1] = name;
        //        users.Add("");
        //    }

        //    imageBox.Image = frame;
        //    names = "";
        //    users.Clear();
        //}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ImageBox_Click(object sender, EventArgs e)
        {

        }
    }
}
