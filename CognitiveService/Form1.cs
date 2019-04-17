using Emgu.CV;
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
        string facesPath = Application.StartupPath + @"\Faces\";

        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        HaarCascade faceDetected;
        Image<Bgr, Byte> frame;
        Capture camera;
        Image<Gray, byte> result;
        Image<Gray, byte> trainedFace = null;
        Image<Gray, byte> grayFace = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labelList = new List<string>();
        List<string> users = new List<string>();
        int count, numLabels, t = 0;
        string name, names = null;
        EigenObjectRecognizer recognizer;
        MCvTermCriteria termCreterias;

        private void BeginBtn_Click(object sender, EventArgs e)
        {
            camera = new Capture(0);
            camera.QueryFrame();
            Application.Idle += new EventHandler(FrameProcedure);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            var time = DateTime.Now.ToString("HHmmssss");
            Face face = new Face(count, nameBox.Text);
            grayFace = camera.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            MCvAvgComp[][] detectedFaces = grayFace.DetectHaarCascade(faceDetected, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            foreach (var f in detectedFaces[0])
            {
                trainedFace = frame.Copy(f.rect).Convert<Gray, byte>();
                break;
            }
            trainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            if (labelList.Contains(nameBox.Text))
            {
                try
                {
                    string json = File.ReadAllText(facesPath + nameBox.Text + ".txt");
                    face = JsonConvert.DeserializeObject<Face>(json);
                    face.imagens.Add(face.nome + time + ".bmp");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString() + "\nNada registrado ainda.", "Oh oh!", MessageBoxButtons.OK);
                }
            }
            else
            {
                count = count + 1;
                face.imagens = new List<string>();
                face.imagens.Add(face.nome + time + ".bmp");
            }

            File.WriteAllText(facesPath + @"\" + face.nome + ".txt", JsonConvert.SerializeObject(face));
            trainedFace.Save(facesPath + face.nome + time + ".bmp");
            trainingImages.Add(trainedFace);
            labelList.Add(nameBox.Text);
            recognizer = new EigenObjectRecognizer(trainingImages.ToArray(), labelList.ToArray(), 1500, ref termCreterias);
            MessageBox.Show(nameBox.Text, "Adicionado");
        }

        private void FrameProcedure(object sender, EventArgs e)
        {
            users.Add("");
            frame = camera.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            grayFace = frame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetectedNow = grayFace.DetectHaarCascade(faceDetected, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            
            foreach (var f in facesDetectedNow[0])
            {
                result = frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                frame.Draw(f.rect, new Bgr(Color.Green), 2);

                if (trainingImages.ToArray().Length != 0)
                {
                   
                    name = recognizer.Recognize(result);
                    if (!detectedFaceslistBox.Items.Contains(name))
                    {
                        detectedFaceslistBox.Items.Add(name);
                    }
                    frame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                }
                // users[t - 1] = name;
                users.Add("");
            }

            imageBox.Image = frame;
            names = "";
            users.Clear();
        }

        public Form1()
        {
            InitializeComponent();
            faceDetected = new HaarCascade(Application.StartupPath + @"\haarcascade_frontalface_default.xml");
            try
            {
                DirectoryInfo di = new DirectoryInfo(facesPath);

                foreach (var file in di.GetFiles())
                {
                    if (file.Name.EndsWith(".txt"))
                    {
                        count += 1;
                        string json = File.ReadAllText(facesPath + file.Name);
                        Face face = JsonConvert.DeserializeObject<Face>(json);
                        foreach (var imagem in face.imagens)
                        {
                            labelList.Add(face.nome);
                            trainingImages.Add(new Image<Gray, byte>(facesPath + imagem));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\nNada registrado ainda.", "Oh oh!", MessageBoxButtons.OK);
            }
            termCreterias = new MCvTermCriteria(count, 0.001);
            recognizer = new EigenObjectRecognizer(trainingImages.ToArray(), labelList.ToArray(), 1500, ref termCreterias);


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
