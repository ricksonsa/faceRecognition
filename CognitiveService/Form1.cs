using Emgu.CV;
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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Timer = System.Windows.Forms.Timer;

namespace CognitiveService
{
    public partial class Form1 : Form
    {
        //paths
        public static string FacesPath = Application.StartupPath + @"\Faces\";

        VideoCapture Camera;
        CascadeClassifier FaceDetection;
        EigenFaceRecognizer Recognizer;

        Mat Frame;

        List<Image<Gray, byte>> Faces;
        List<int> IDs;
        public static List<ListViewItem> ListItems;

        List<Face> FacesList;

        int ProcessedImageWidth = 128;
        int ProcessedImageHeigth = 150;

        int TimerCount = 0;
        int TimeLimit = 15;
        int ScanCounter = 0;
        int Count = 0;

        string YMLPath = Application.StartupPath + @"\trainingData.yml";
        private int selectedID;
        Timer timer;

        bool isTraining = false;
        bool Empty = false;

        //Controls
        public static ListBox rostoListBox;
        private string selectedName;

        public Form1()
        {
            InitializeComponent();
            Logger.Write("Executando...");
            Recognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
            if (File.Exists(YMLPath))
            {
                Logger.Write("Encontrado arquivo de treinamento em " + YMLPath + " iniciando leitura...");
                Recognizer.Read(YMLPath);
                Logger.Write("Leitura concluida.");
            }
            else
            {
                Logger.Write("Não foi encontrado arquivo de treinamento em " + YMLPath);
                Empty = true;
            }

            if (!Directory.Exists(FacesPath))
            {
                Directory.CreateDirectory(FacesPath);
            }

            FaceDetection = new CascadeClassifier(Application.StartupPath + @"\haarcascade_frontalface_default.xml");
            Frame = new Mat();
            Faces = new List<Image<Gray, byte>>();
            IDs = new List<int>();
            FacesList = new List<Face>();
            ListItems = new List<ListViewItem>();

            //rostoListBox = new ListBox();
            //rostoListBox.Size = detectedFaceslistBox.Size;
            //rostoListBox.Location = detectedFaceslistBox.Location;
            //panel1.Controls.Add(rostoListBox);

            try
            {
                DirectoryInfo di = new DirectoryInfo(FacesPath);

                foreach (var file in di.GetFiles())
                {
                    if (file.Name.EndsWith(".txt"))
                    {
                        Count += 1;

                        string json = File.ReadAllText(FacesPath + file.Name);
                        Face face = JsonConvert.DeserializeObject<Face>(json);
                        FacesList.Add(face);
                        Logger.Write("Identificação facial de " + face.nome + " encontrado.");
                        Logger.Write("Count = " + Count);
                        Logger.Write("FacesList Size = " + FacesList.Count);
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
            Thread cameraThread = new Thread(t => StartCamera());
            cameraThread.Start();
        }

        private void StartCamera()
        {
            if (Camera == null)
            {
                Camera = new VideoCapture();
                //Camera = new VideoCapture(FacesPath + "videoFile.mp4");
            }

            Camera.ImageGrabbed += Camera_ImageGrabbed;
            Camera.FlipHorizontal = true;

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
            //var imageFrame = Frame.ToImage<Bgr, byte>().Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
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
                            var unprocessedImage = imageFrame.Copy(face).Resize(ProcessedImageWidth, ProcessedImageHeigth, Emgu.CV.CvEnum.Inter.Cubic);
                            var processedImage = imageFrame.Copy(face).Convert<Gray, Byte>().Resize(ProcessedImageWidth, ProcessedImageHeigth, Emgu.CV.CvEnum.Inter.Cubic);

                            //Desenha quadrado em volta
                            imageFrame.Draw(face, new Bgr(Color.BurlyWood), 2);

                            if (!isTraining && !Empty)
                            {
                                string text = "Nao conhecido";
                                var result = Recognizer.Predict(processedImage);

                                if (result.Distance < 3000)
                                {
                                    person = GetPersonById(result.Label);

                                    if (person != null)
                                    {
                                        text = person.nome;
                                    }

                                    //if (ListItems.Find(c => c.ImageKey == result.Label.ToString()) == null)
                                    //{
                                    //    imageList1.Images.Add(result.Label.ToString(), unprocessedImage.ToBitmap());
                                    //    var item = new ListViewItem
                                    //    {
                                    //        Text = person.nome,
                                    //        ImageKey = result.Label.ToString()
                                    //    };

                                    //    ListItems.Add(item);
                                    //}
                                }
                                imageFrame.Draw(result.Label + " - " + text + " - " + Math.Round(result.Distance, 2), new Point(face.Location.X - 2, face.Location.Y - 2), Emgu.CV.CvEnum.FontFace.HersheyTriplex, 0.5, new Bgr(Color.Red));
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
                var face = GetPersonByNome(nameBox.Text) ?? new Face(Count++, nameBox.Text);
                selectedName = nameBox.Text;
                selectedID = face.id;

                if (!FacesList.Contains(face))
                    FacesList.Add(face);

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
            var time = DateTime.Now.ToString("HHmmssss");

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
                        var imageFile = FacesPath + selectedName + time;
                        var processedImage = imageFrame.Copy(faces[0]).Resize(ProcessedImageWidth, ProcessedImageHeigth, Emgu.CV.CvEnum.Inter.Cubic);
                        processedImage.Save(imageFile + ".bmp");
                        Faces.Add(processedImage);
                        IDs.Add(selectedID);
                        var bmp = processedImage.ToBitmap();
                        Enrichment(bmp, imageFile, selectedID);
                        ScanCounter++;
                    }
                }
            }
            else
            {
                timer.Stop();
                Camera.Pause();
                List<Mat> mats = new List<Mat>();
                foreach (var item in Faces)
                {
                    mats.Add(item.Mat);
                }
                Logger.Write("Treinando com dados recolhidos...");
                Recognizer.Train(mats.ToArray(), IDs.ToArray());
                Recognizer.Write(YMLPath);
                Logger.Write("Escrevendo em arquivo YML");
                Empty = false;
                TimerCount = 0;
                saveBtn.Enabled = !saveBtn.Enabled;
                MessageBox.Show("Terinamento realizado", "Treinado", MessageBoxButtons.OK);
                isTraining = false;
                Recognizer.Read(YMLPath);
                Logger.Write("Lendo arquivo YML");
                Camera.Start();
            }
        }

        private void Enrichment(Bitmap bmp, string path, int label)
        {
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

        Face GetPersonByNome(string nome)
        {
            return FacesList.Find(x => x.nome == nome);
        }

        Face GetPersonById(int id)
        {
            return FacesList.Find(x => x.id == id);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ImageBox_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Leave(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.Write("Encerrando aplicação...");
        }

        private void FacesReconizedTimer_Tick(object sender, EventArgs e)
        {
            foreach (var item in ListItems)
            {
                if (!listView1.Items.Contains(item))
                {
                    listView1.Items.Add(item);
                }
            }

        }
    }
}
