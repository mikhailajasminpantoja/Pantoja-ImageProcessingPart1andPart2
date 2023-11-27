using System.Drawing.Imaging;
using WebCamLib;

namespace Pantoja_ImageProcessingPart1andPart2
{
    public partial class Form1 : Form
    {
        Bitmap loadImage, resultImage, imageB, imageA;
        Device[] allDevices;
        Device firstDevice;
        public Form1()
        {
            InitializeComponent();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Bitmap newFrame = CaptureAndDisplayImage();
            if (newFrame != null)
            {
                subtract(newFrame);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (firstDevice != null)
            {
                imageA = CaptureAndDisplayImage();
                if (imageA != null)
                {
                    if (pictureBox2 != null && pictureBox2.Image != null)
                    {
                        pictureBox2.Image.Dispose();
                    }
                    pictureBox2.Image = imageA;
                }
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //resultImage.Save(saveFileDialog1.FileName);


            if (resultImage != null)
            {

                if (!string.IsNullOrEmpty(saveFileDialog1.FileName))
                {

                    string fileExtension = System.IO.Path.GetExtension(saveFileDialog1.FileName);
                    resultImage.Save(saveFileDialog1.FileName, GetImageFormatFromExtension(fileExtension));
                }
            }
        }

        private void openFileDialog3_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = imageA;
        }

        private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = imageB;
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //loadimage
            loadImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadImage;
        }

        private ImageFormat GetImageFormatFromExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".png":
                    return ImageFormat.Png;
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".bmp":
                    return ImageFormat.Bmp;
                default:
                    return ImageFormat.Png;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //save
            saveFileDialog1.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg, *.jpeg)|*.jpg;*.jpeg|Bitmap Image (*.bmp)|*.bmp|All Files (*.*)|*.*";
            saveFileDialog1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //basic copy.
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < resultImage.Width; x++)
                for (int y = 0; y < resultImage.Height; y++)
                {
                    Color pixel = loadImage.GetPixel(x, y);
                    resultImage.SetPixel(x, y, pixel);
                }
            pictureBox2.Image = resultImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //greyscale
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < resultImage.Width; x++)
                for (int y = 0; y < resultImage.Height; y++)
                {
                    Color pixel = loadImage.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    resultImage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            pictureBox2.Image = resultImage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //invert image
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < resultImage.Width; x++)
                for (int y = 0; y < resultImage.Height; y++)
                {
                    Color pixel = loadImage.GetPixel(x, y);
                    resultImage.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            pictureBox2.Image = resultImage;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < resultImage.Width; x++)
                for (int y = 0; y < resultImage.Height; y++)
                {
                    Color pixel = loadImage.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    resultImage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            Color sample;
            int[] histdata = new int[256];
            for (int x = 0; x < resultImage.Width; x++)
                for (int y = 0; y < resultImage.Height; y++)
                {
                    sample = resultImage.GetPixel(x, y);
                    histdata[sample.R]++;
                }

            Bitmap mydata = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 800; y++)
                {
                    mydata.SetPixel(x, y, Color.White);
                }

            //plot histdata
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < Math.Min(histdata[x] / 5, 800); y++) //we get the data from each intensity we have
                {
                    mydata.SetPixel(x, 799 - y, Color.Black);
                }
            pictureBox2.Image = mydata;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //sepia
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int y = 0; y < resultImage.Height; y++)
                for (int x = 0; x < resultImage.Width; x++)
                {
                    Color pixelColor = loadImage.GetPixel(x, y);

                    int outputRed = (int)(pixelColor.R * 0.393 + pixelColor.G * 0.769 + pixelColor.B * 0.189);
                    int outputGreen = (int)(pixelColor.R * 0.349 + pixelColor.G * 0.686 + pixelColor.B * 0.168);
                    int outputBlue = (int)(pixelColor.R * 0.272 + pixelColor.G * 0.534 + pixelColor.B * 0.131);


                    outputRed = Math.Min(255, Math.Max(0, outputRed));
                    outputGreen = Math.Min(255, Math.Max(0, outputGreen));
                    outputBlue = Math.Min(255, Math.Max(0, outputBlue));

                    Color outputColor = Color.FromArgb(outputRed, outputGreen, outputBlue);
                    resultImage.SetPixel(x, y, outputColor);
                }

            pictureBox2.Image = resultImage;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //resultImage = new Bitmap(imageB.Width, imageB.Height);
            //Color mygreen = Color.FromArgb(0,0,255);
            //int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;  
            //int threshold = 5;


            //for (int x = 0; x < imageB.Width; x++)
            // for (int y = 0; y < imageB.Height; y++)
            //{
            // Color pixel = imageB.GetPixel(x, y);
            // Color backpixel = imageA.GetPixel(x, y);
            // int grey = (pixel.R + pixel.G + pixel.B) / 3;
            // int subtractvalue = Math.Abs(grey - greygreen); 


            //if (subtractvalue > threshold)
            // resultImage.SetPixel(x, y, pixel);
            // else      
            // resultImage.SetPixel(x, y, imageA.GetPixel(x, y));
            // }
            //pictureBox3.Image = resultImage;

            if (imageA == null || imageB == null)
            {
                MessageBox.Show("Please load both images before subtracting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (timer1.Enabled && pictureBox2 != null && pictureBox2.Image != null)
            {
                timer2.Start();
            }
            else
            {
                subtract(imageA);
            }
        }

        public void subtract(Bitmap frame)
        {
            resultImage = new Bitmap(imageB.Width, imageB.Height);
            Color mygreen = Color.FromArgb(0, 255, 0);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 10;
            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color fg = imageB.GetPixel(x, y);
                    Color bg = frame.GetPixel(x, y);
                    int grey = (fg.R + fg.G + fg.B) / 3;
                    bool s = Math.Abs(grey - greygreen) < threshold;
                    if (s)
                        resultImage.SetPixel(x, y, bg);
                    else
                        resultImage.SetPixel(x, y, fg);
                }
            }
            pictureBox3.Image = resultImage;
        }

        private Bitmap CaptureAndDisplayImage()
        {
            if (firstDevice == null)
            {
                MessageBox.Show("No device selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            firstDevice.Sendmessage();
            IDataObject data = Clipboard.GetDataObject();
            if (data != null && data.GetData("System.Drawing.Bitmap", true) != null)
            {
                Image bmap = (Image)data.GetData("System.Drawing.Bitmap", true);
                if (bmap != null)
                {
                    return new Bitmap(bmap);
                }
            }
            return null;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (firstDevice == null)
            {
                try
                {
                    allDevices = DeviceManager.GetAllDevices();
                    firstDevice = allDevices[0];
                    if (allDevices.Length > 0)
                    {
                        string deviceInfo = "Available Devices:\n\n";

                        for (int i = 0; i < allDevices.Length; i++)
                        {
                            deviceInfo += $"Device {i + 1}: {allDevices[i].Name} - Version: {allDevices[i].Version}\n";
                        }
                        MessageBox.Show(deviceInfo, "Available Devices");

                        firstDevice.ShowWindow(pictureBox4); // if mo opt silag use sa real time background as background 
                    }
                    else
                    {
                        MessageBox.Show("No webcam devices found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error initializing webcam: {ex.Message}");
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null && timer1.Enabled)
            {
                timer1.Stop();
                pictureBox2.Image.Dispose();
                pictureBox2.Image = null;
                imageA = null;
            }
            if (pictureBox3.Image != null && timer2.Enabled)
            {
                timer2.Stop();
                pictureBox3.Image.Dispose();
                pictureBox3.Image = null;
            }
            if (firstDevice != null)
            {
                firstDevice.Stop();
                firstDevice = null;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (firstDevice != null)
            {
                if (pictureBox3 != null && pictureBox3.Image != null)
                {
                    pictureBox3.Image.Dispose();
                    pictureBox3.Image = null;
                }
                timer1.Start();
            }
            else
            {
                MessageBox.Show("No device selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (firstDevice == null)
            {
                MessageBox.Show("No device selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
            if (timer2.Enabled)
            {
                timer2.Stop();
            }
            if (pictureBox2 != null && pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose();
                imageA = null;
            }
            if (pictureBox3 != null && pictureBox3.Image != null)
            {
                pictureBox3.Image.Dispose();
                pictureBox3.Image = null;
            }
            try
            {
                imageA = CaptureAndDisplayImage();
                pictureBox2.Image = imageA;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nPlease open webcam first");
            }
        }
    }
}