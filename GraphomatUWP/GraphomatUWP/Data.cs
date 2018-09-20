using GraphomatDrawingLibUwp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace GraphomatUWP
{
    public class Data
    {
        private const float defaultValueWidthAndHeight = 10F, defaultMiddleOfView = 0F;
        private const string filename = "Data.txt";
        private static readonly string absolutePath = ApplicationData.Current.LocalFolder.Path + "\\" + filename;

        public Vector2 ValueSize { get; set; }

        public Vector2 MiddleOfView { get; set; }

        public ObservableCollection<Graph> Graphs { get; set; }

        public Data()
        {
            ValueSize = new Vector2(defaultValueWidthAndHeight, defaultValueWidthAndHeight);
            MiddleOfView = new Vector2(defaultMiddleOfView, defaultMiddleOfView);

            Graphs = new ObservableCollection<Graph>();
        }

        public async static Task Save()
        {
            try
            {
                Data saveData = new Data();

                saveData.ValueSize = ViewModel.Current.ValueSize;
                saveData.MiddleOfView = ViewModel.Current.MiddleOfView;
                saveData.Graphs = ViewModel.Current.Graphs;

                string xmlText = Serialize(saveData);

                await CreateFileIfNotExists();
                await PathIO.WriteTextAsync(absolutePath, xmlText);
            }
            catch { }
        }

        public async static Task CreateFileIfNotExists()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(absolutePath);
            }
            catch
            {
                await ApplicationData.Current.LocalFolder.CreateFileAsync(filename);
            }
        }

        public async static Task<Data> Load()
        {
            try
            {
                string xmlText = await PathIO.ReadTextAsync(absolutePath);

                return Deserialize(xmlText);
            }
            catch { }

            return new Data();
        }

        private static Data Deserialize(string xmlText)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Data));

            TextReader tr = new StringReader(xmlText);
            object deObj = serializer.Deserialize(tr);

            return (Data)deObj;
        }

        private static string Serialize(Data graphs)
        {
            Type type = graphs.GetType();
            XmlSerializer serializer = new XmlSerializer(type);

            TextWriter tw = new StringWriter();
            serializer.Serialize(tw, graphs);
            string xmlText = tw.ToString();

            TextReader tr = new StringReader(xmlText);
            object deObj = serializer.Deserialize(tr);

            return deObj.GetType() == type ? xmlText : "";
        }
    }
}
