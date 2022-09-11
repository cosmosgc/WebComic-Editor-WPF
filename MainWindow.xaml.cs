using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;
using WebComic_Editor.Class;
using System.ComponentModel;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Media;
using Newtonsoft.Json;
using Microsoft.Win32;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Runtime.Remoting.Messaging;

namespace WebComic_Editor_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string projectName = "Sem Nome";
        public string sourceURL;
        private List<mspa> WebComic = new List<mspa>();
        //private int currentPage = 0;
        public Uri url;
        private int pageSelected = 0;
        public string selectedColor = "#000000";
        public Color selectedColorCOLOR;
        public bool chatOptionActive = false;
        FoldingManager foldingManager;
        XmlFoldingStrategy foldingStrategy;
        public MainWindow()
        {
            InitializeComponent();
            string path = Path.Combine(Environment.CurrentDirectory, @"www\", "template.html");
            webView.Source = new Uri(path);
            foldingManager = FoldingManager.Install(htmlEditor.TextArea);
            foldingStrategy = new XmlFoldingStrategy();
            foldingStrategy.UpdateFoldings(foldingManager, htmlEditor.Document);
        }
        private void updateComicData()
        {
            //WebComicGrid.DataSource = null;
            //var bindingList = new BindingList<mspa>(WebComic);
            WebComicTreeView.Items.Clear();
            for(int i = 0; i < WebComic.Count; i++)
            {
                TreeViewItem item = new TreeViewItem() { Header = WebComic[i].command };
                item.Selected += treeItem_Selected;
                item.Tag = WebComic[i];
                WebComicTreeView.Items.Add(item);
            }
            //var source = new System.Windows.Forms.BindingSource(bindingList, null);
            //WebComicdataGrid.ItemsSource = source;

            //Form1.ActiveForm.Text = projectName;
        }
        void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            mspa tag = (mspa)item.Tag;
            pageSelected = tag.page;
            htmlEditor.Text = WebComic[pageSelected].content;
            PageName.Text = WebComic[pageSelected].command;
        }

        void SelectPage()
        {
            htmlEditor.Text = WebComic[pageSelected].content;
            PageName.Text = WebComic[pageSelected].command;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            updateHtmlPreview();
        }


        private async void updateHtmlPreview(string content = "")
        {
            if(webView == null) { return; }
            await webView.EnsureCoreWebView2Async();
            string js = "document.getElementById(\"content\").innerHTML = `" + htmlEditor.Document.Text + "`;";
            await webView.CoreWebView2.ExecuteScriptAsync(js);
            string UpdateSpoiler = "initSpoiler();";
            await webView.CoreWebView2.ExecuteScriptAsync(UpdateSpoiler);

            //Skorperlog funciona após ser iniciado por essa função
            //await webView.ExecuteScriptAsync("document.body.innerHTML=`" + StringFromRichTextBox(htmlEditor) + "`;");
        }

        private void CreatChatBtn(object sender, RoutedEventArgs e)
        {
            string text = "";
            string chatSystem = ChatSystem.Text;
            bool toRight = chatOptionActive;
            string chatName = charName.Text;
            string selectedText = htmlEditor.SelectedText;
            //Brush selectedColor = ColorPickerBtn.Background;
            if (chatSystem == "SkerperLog")
            {
                text = Wcutils.SkorperLogDiv();
            }
            if (chatSystem == "AmongUs")
            {
                text = Wcutils.AmongUsDiv();
            }
            if (chatSystem == "Earthbound")
            {
                text = Wcutils.Earthbound();
            }
            if (chatSystem == "gimpact")
            {
                text = wcCss.gimpact(chatName);
            }
            if (chatSystem == "dcord2")
            {
                text = wcCss.dcord2();
            }
            if (chatSystem == "aol")
            {
                text = wcCss.aol();
            }
            if (chatSystem == "TF2")
            {
                text = wcCss.TF2();
            }
            if (chatSystem == "PCTERMINAL")
            {
                text = wcCss.PCTERMINAL();
            }
            if (chatSystem == "YBComment")
            {
                text = wcCss.ytcom(chatName);
            }
            if (chatSystem == "falloutvgnorm")
            {
                text = wcCss.falloutvgnorm(selectedText);
            }
            if (chatSystem == "hylics2")
            {
                text = wcCss.hylics2(selectedText);
            }
            if (chatSystem == "jazztronauts")
            {
                text = wcCss.jazztronauts(chatName, selectedText, toRight);
            }
            if (chatSystem == "phone")
            {
                text = wcCss.phone(chatName, selectedColorCOLOR);
            }
            if (chatSystem == "minecraft")
            {
                text = wcCss.mcchat(selectedText);
            }
            if (chatSystem == "Detroit")
            {
                text = wcCss.dthuman(selectedText);
            }
            if (chatSystem == "Undertale Menu")
            {
                text = wcCss.UTmenu3();
            }
            if (chatSystem == "robloxScore")
            {
                text = wcCss.roblox(selectedColorCOLOR);
            }
            if (chatSystem == "deltarune")
            {
                text = wcTextBoxes.deltarune(selectedText);
            }
            if (chatSystem == "Ace Atorney")
            {
                text = wcTextBoxes.acewrap(chatName, selectedText);
            }
            if (chatSystem == "HollowKnight")
            {
                text = wcTextBoxes.HollowKnight(selectedText);
            }
            if (chatSystem == "botw")
            {
                text = wcTextBoxes.botw(chatName, selectedText);
            }
            if (chatSystem == "picto")
            {
                text = wcTextBoxes.picto(selectedColorCOLOR, chatName, selectedText);
            }
            //Add text para o editor
            //testButton.Content = text;
            htmlEditor.SelectedText = text;
            htmlEditor.Select(htmlEditor.CaretOffset, 0);
            foldingStrategy.UpdateFoldings(foldingManager, htmlEditor.Document);
            //htmlEditor.Selection.Text = string.Empty;
            //htmlEditor.CaretPosition.InsertTextInRun(text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string text = "";
            string chatSystem = ChatSystem.Text;
            bool toRight = chatOptionActive;
            string chatName = charName.Text;
            string selectedText = htmlEditor.SelectedText;
            //Brush selectedColor = ColorPickerBtn.Background;
            if (chatSystem == "SkerperLog")
            {
                text = Wcutils.SkorperLogChat(chatName, selectedColorCOLOR);
            }
            if (chatSystem == "AmongUs")
            {
                text = Wcutils.AmongUsChat(chatName, toRight);
            }
            if (chatSystem == "Earthbound")
            {
                text = Wcutils.Earthbound();
            }
            if (chatSystem == "dcord2")
            {
                text = wcCss.dcord2Chat(chatName, selectedColorCOLOR);
            }
            if (chatSystem == "phone")
            {
                text = wcCss.phoneChat(selectedText, toRight);
            }
            if (chatSystem == "Undertale Menu")
            {
                text = wcCss.UTmenu3Item();
            }
            if (chatSystem == "picto")
            {
                text = wcTextBoxes.pictoChat(selectedColorCOLOR, chatName, selectedText);
            }
            htmlEditor.SelectedText = text;
            htmlEditor.Select(htmlEditor.CaretOffset, 0);
            foldingStrategy.UpdateFoldings(foldingManager, htmlEditor.Document);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string text = "";
            string chatSystem = EffectSystem.Text;
            bool toRight = chatOptionActive;
            string chatName = charName.Text;
            string selectedText = htmlEditor.SelectedText;
            //Brush selectedColor = ColorPickerBtn.Background;
            
            if (chatSystem == "glitch")
            {
                text = wcCss.glitch(selectedText);
            }
            if (chatSystem == "COMBOBOX")
            {
                text = wcCss.COMBOBOX(selectedText, selectedColorCOLOR);
            }
            if (chatSystem == "shake")
            {
                text = wcCss.shake(selectedText);
            }
            if (chatSystem == "RAINBOW")
            {
                text = wcCss.RAINBOW(selectedText);
            }
            if (chatSystem == "Diary")
            {
                text = wcCss.Diary(selectedText);
            }
            if (chatSystem == "Minecraft Button")
            {
                text = wcCss.mcBtn(selectedText);
            }
            if (chatSystem == "Minecraft Placa")
            {
                text = wcCss.mcsign(selectedText);
            }
            if (chatSystem == "Animar texto sequencial")
            {
                text = wcCss.TextAppearingSequentially(selectedText);
            }
            if (chatSystem == "Doc Scratch")
            {
                text = wcCss.DocScratch(selectedColorCOLOR, selectedText);
            }
            htmlEditor.SelectedText = text;
            htmlEditor.Select(htmlEditor.CaretOffset, 0);
            foldingStrategy.UpdateFoldings(foldingManager, htmlEditor.Document);
        }

        private void htmlEditorTextChanged(object sender, EventArgs e)
        {
            updateHtmlPreview();
            if (WebComic.Count > 0 )
            {
                WebComic[pageSelected].content = htmlEditor.Text;
            }
        }

        private void addNewPage(object sender, RoutedEventArgs e)
        {
            mspa newPage = new mspa()
            {
                page = WebComic.Count,
                command = "Page " + WebComic.Count,
                content = ""
            };
            WebComic.Add(newPage);
            updateComicData();
        }

        public void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color color = ClrPcker_Background.SelectedColor.Value;
            selectedColor = "#" + color.R.ToString() + color.G.ToString() + color.B.ToString();
            selectedColorCOLOR = Color.FromRgb(color.R, color.G, color.B);
        }

        private void pageNameChanged(object sender, TextChangedEventArgs e)
        {
            if(WebComic.Count > 0)
            {
                if (WebComic[pageSelected].command != PageName.Text)
                {
                    WebComic[pageSelected].command = PageName.Text;
                }
            }
            
        }

        private void saveProject(object sender, RoutedEventArgs e)
        {
            string save = JsonConvert.SerializeObject(WebComic, Formatting.Indented);
            string fileName = projectName + ".json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Save\", fileName);
            if (!Directory.Exists(path))
            {
                // Try to create the directory.
                string FolderPath = Path.Combine(Environment.CurrentDirectory, @"Save\");
                DirectoryInfo di = Directory.CreateDirectory(FolderPath);
            }
            File.WriteAllText(path, save);
        }

        private void abrirProject(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".json"; // Default file extension
            dialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filePath = dialog.FileName;
                var fileStream = dialog.OpenFile();

                projectName = Path.GetFileNameWithoutExtension(filePath);

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string jsonString = reader.ReadToEnd();
                    WebComic = JsonConvert.DeserializeObject<List<mspa>>(jsonString);
                    updateComicData();
                }
            }
        }


        private void recentesMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            recentes.Items.Clear();
            string path = Path.Combine(Environment.CurrentDirectory, @"Save\");
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        // This path is a file
                        MenuItem fileRecent = new MenuItem();
                        fileRecent.Header = file;
                        fileRecent.Click += loadRecent;
                        recentes.Items.Add(fileRecent);
                    }
                }
            }
        }
        private void loadRecent(object sender, RoutedEventArgs e)
        {
            MenuItem _sender = (MenuItem)sender;
            string jsonString = File.ReadAllText(_sender.Header.ToString());
            WebComic = JsonConvert.DeserializeObject<List<mspa>>(jsonString);
            projectName = Path.GetFileNameWithoutExtension(sender.ToString());
            updateComicData();
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            if (WebComic.Count > 0 && pageSelected >= 0 && pageSelected < WebComic.Count-1)
            {
                pageSelected++;
                SelectPage();
            } 
        }

        private void PrevPage(object sender, RoutedEventArgs e)
        {
            if (WebComic.Count > 0 && pageSelected > 0 && pageSelected <= WebComic.Count-1)
            {
                pageSelected--;
                SelectPage();
            }
        }

        private void NovoProjeto(object sender, RoutedEventArgs e)
        {
            string promptValue = Wcutils.ShowDialog("Nome do projeto", "Novo Projeto");
            projectName = promptValue;
            WebComic.Clear();
            updateComicData();
            Title = projectName + " | WEBCOMIC EDITOR";
        }

        private void RenomearProjeto(object sender, RoutedEventArgs e)
        {
            string novoNome = Wcutils.ShowDialog("Renomear projeto", "Novo nome");
            projectName = novoNome;
            Title = projectName + " | WEBCOMIC EDITOR";
        }
    }
}
