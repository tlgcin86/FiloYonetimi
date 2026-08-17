using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FiloYonetimi;

public class MainForm : Form
{
    readonly Panel content = new();
    readonly Label pageTitle = new();
    readonly Label clock = new();
    Bitmap? logo;
    Button activeNav = null!;

    public MainForm()
    {
        Text = "Manisa TM Filo Yönetimi";
        Width = 1450; Height = 900; MinimumSize = new Size(1200, 760);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = AppTheme.Bg;
        BuildShell();
        ShowDashboard();
        Shown += (_,_) => StartupAlert();
    }



    void BuildShell()
    {
        var sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 220,
            BackColor = AppTheme.Navy
        };

        var homeTop = new Button
        {
            Text = "⌂   ANASAYFA",
            Left = 10,
            Top = 10,
            Width = 200,
            Height = 54,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = AppTheme.Navy,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(16, 0, 0, 0),
            UseVisualStyleBackColor = false
        };
        homeTop.FlatAppearance.BorderSize = 0;
        homeTop.FlatAppearance.MouseOverBackColor = AppTheme.Navy;
        homeTop.FlatAppearance.MouseDownBackColor = AppTheme.Navy;
        homeTop.Click += (_, _) => ShowDashboard();
        sidebar.Controls.Add(homeTop);

        var nav = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Top = 76,
            Height = 630,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(8, 2, 8, 0),
            AutoScroll = true
        };
        sidebar.Controls.Add(nav);

        AddNav(nav, "▣   ARAÇ KAYIT", () => ShowVehicleForm(null));
        AddNav(nav, "☷   ARAÇ LİSTESİ", ShowVehicleList);
        AddNav(nav, "◷   YAKLAŞAN TARİHLER", ShowUpcoming);
        AddNav(nav, "▥   RAPORLAMA", ShowReports);
        AddNav(nav, "▤   BELGE YÜKLEME", ShowDocumentUpload);
        AddNav(nav, "↻   YEDEKLEME / GERİ YÜKLEME", ShowBackupRestore);
        AddNav(nav, "ⓘ   HAKKINDA", () => MessageBox.Show(
            "Tolga ÇINAR\nTarafından Tüm Hakları Saklıdır.",
            "Hakkında"));

        var exit = new Button
        {
            Text = "⏻   ÇIKIŞ",
            Dock = DockStyle.Bottom,
            Height = 52,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(3, 37, 71),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0)
        };
        exit.FlatAppearance.BorderSize = 0;
        exit.Click += (_, _) => Close();
        sidebar.Controls.Add(exit);

        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 82,
            BackColor = AppTheme.Bg
        };

        pageTitle.Left = 25;
        pageTitle.Top = 18;
        pageTitle.Width = 440;
        pageTitle.Height = 42;
        pageTitle.Font = new Font("Segoe UI Semibold", 18, FontStyle.Bold);
        pageTitle.ForeColor = AppTheme.Navy;
        pageTitle.BackColor = AppTheme.Bg;
        pageTitle.TextAlign = ContentAlignment.MiddleLeft;
        header.Controls.Add(pageTitle);

        clock.Dock = DockStyle.Right;
        clock.Width = 180;
        clock.TextAlign = ContentAlignment.MiddleCenter;
        clock.Font = new Font("Segoe UI", 10);
        clock.ForeColor = AppTheme.Muted;
        clock.BackColor = AppTheme.Bg;
        header.Controls.Add(clock);

        var headerLine = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 1,
            BackColor = Color.FromArgb(220, 226, 234)
        };
        header.Controls.Add(headerLine);

        var timer = new System.Windows.Forms.Timer { Interval = 1000 };
        timer.Tick += (_, _) => clock.Text = DateTime.Now.ToString("dd.MM.yyyy  HH:mm:ss");
        timer.Start();

        logo = AppTheme.LoadLogo();
        if (logo != null)
        {
            var pic = new PictureBox
            {
                Image = logo,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 280,
                Height = 70,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Left = header.Width - 305,
                Top = 6
            };
            header.SizeChanged += (_, _) => pic.Left = header.ClientSize.Width - pic.Width - 12;
            header.Controls.Add(pic);
            pic.BringToFront();
        }

        content.Dock = DockStyle.Fill;
        content.Padding = new Padding(18);
        content.BackColor = AppTheme.Bg;

        Controls.Add(content);
        Controls.Add(header);
        Controls.Add(sidebar);
    }

    void AddNav(FlowLayoutPanel nav, string text, Action action)
    {
        var b = new Button
        {
            Text = text,
            Width = 200,
            Height = 44,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.2f),
            ForeColor = Color.White,
            BackColor = AppTheme.Navy,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0),
            Margin = new Padding(0, 2, 0, 2),
            UseVisualStyleBackColor = false
        };

        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = Color.FromArgb(10, 75, 130);
        b.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 102, 182);
        b.Click += (_, _) => { SetActive(b); action(); };

        nav.Controls.Add(b);

        if (activeNav == null)
        {
            activeNav = b;
            SetActive(b);
        }
    }

    void SetActive(Button b)
    {
        if (activeNav != null)
            activeNav.BackColor = AppTheme.Navy;

        activeNav = b;
        activeNav.BackColor = Color.FromArgb(18, 102, 182);
    }

    Panel Page()
    {
        content.Controls.Clear();
        var p=new Panel{Dock=DockStyle.Fill,AutoScroll=true};
        content.Controls.Add(p); return p;
    }



    void ShowDocumentUpload()
    {
        using var f=new Form
        {
            Text="Belge Yükleme",
            Width=840,
            Height=470,
            StartPosition=FormStartPosition.CenterParent,
            BackColor=AppTheme.Bg,
            FormBorderStyle=FormBorderStyle.FixedDialog,
            MaximizeBox=false,
            MinimizeBox=false
        };

        var header=new Panel{Dock=DockStyle.Top,Height=72,BackColor=AppTheme.Navy};

        var title=new Label
        {
            Text="Belge Yükleme",
            ForeColor=Color.White,
            Font=new Font("Segoe UI Semibold",15,FontStyle.Bold),
            Left=22,Top=18,Width=280,Height=30
        };
        header.Controls.Add(title);

        var save=new Button
        {
            Text="💾  Kaydet",
            Width=126,Height=36,
            BackColor=AppTheme.Green,
            ForeColor=Color.White,
            FlatStyle=FlatStyle.Flat,
            Font=new Font("Segoe UI",9.5f,FontStyle.Bold),
            Anchor=AnchorStyles.Top|AnchorStyles.Right
        };
        save.FlatAppearance.BorderSize=0;
        save.Click+=(_,_)=>MessageBox.Show(
            "Belge işlemleri kaydedildi.",
            "Belge Yükleme",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        header.Controls.Add(save);
        header.Resize+=(_,_)=>save.Left=header.ClientSize.Width-save.Width-18;

        var body=new TableLayoutPanel
        {
            Dock=DockStyle.Fill,
            ColumnCount=3,
            RowCount=5,
            Padding=new Padding(24,20,24,20),
            BackColor=AppTheme.Bg
        };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,60));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,120));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,120));
        body.RowStyles.Add(new RowStyle(SizeType.Absolute,28));
        body.RowStyles.Add(new RowStyle(SizeType.Absolute,40));
        body.RowStyles.Add(new RowStyle(SizeType.Absolute,56));
        body.RowStyles.Add(new RowStyle(SizeType.Absolute,56));
        body.RowStyles.Add(new RowStyle(SizeType.Absolute,56));

        var vehicleLabel=new Label
        {
            Text="Araç Seçimi",
            Dock=DockStyle.Fill,
            Font=new Font("Segoe UI",9.5f,FontStyle.Bold),
            ForeColor=AppTheme.Text,
            TextAlign=ContentAlignment.MiddleLeft
        };
        body.Controls.Add(vehicleLabel,0,0);
        body.SetColumnSpan(vehicleLabel,3);

        var vehicle=new ComboBox
        {
            Dock=DockStyle.Fill,
            DropDownStyle=ComboBoxStyle.DropDownList,
            Font=new Font("Segoe UI",10)
        };

        foreach(var v in Database.GetAll())
            vehicle.Items.Add(new VehicleChoice(v.Id,v.Plate,v.Brand,v.Model));
        if(vehicle.Items.Count>0)
            vehicle.SelectedIndex=0;

        body.Controls.Add(vehicle,0,1);
        body.SetColumnSpan(vehicle,3);

        AddPopupDocumentRow(body,2,"Ruhsat Belgesi","Ruhsat",vehicle);
        AddPopupDocumentRow(body,3,"Ehliyet Ön - Arka Yüz","Ehliyet",vehicle);
        AddPopupDocumentRow(body,4,"Poliçe Belgesi","Police",vehicle);

        f.Controls.Add(body);
        f.Controls.Add(header);
        f.ShowDialog(this);
    }

    void AddPopupDocumentRow(TableLayoutPanel body,int row,string caption,string key,ComboBox vehicle)
    {
        var label=new Label
        {
            Text=caption,
            Dock=DockStyle.Fill,
            Font=new Font("Segoe UI",10,FontStyle.Bold),
            ForeColor=AppTheme.Text,
            TextAlign=ContentAlignment.MiddleLeft
        };
        body.Controls.Add(label,0,row);

        var upload=new Button
        {
            Text="📎  Yükle",
            Dock=DockStyle.Fill,
            BackColor=AppTheme.Blue,
            ForeColor=Color.White,
            FlatStyle=FlatStyle.Flat,
            Font=new Font("Segoe UI",9)
        };
        upload.FlatAppearance.BorderSize=0;
        upload.Click+=(_,_)=>UploadPopupDocument(vehicle,key);

        var view=new Button
        {
            Text="Görüntüle",
            Dock=DockStyle.Fill,
            FlatStyle=FlatStyle.Flat,
            BackColor=Color.White,
            Font=new Font("Segoe UI",9)
        };
        view.Click+=(_,_)=>ViewPopupDocument(vehicle,key);

        body.Controls.Add(upload,1,row);
        body.Controls.Add(view,2,row);
    }

    string DocumentsRoot()
    {
        var root=Path.Combine(AppContext.BaseDirectory,"Documents");
        Directory.CreateDirectory(root);
        return root;
    }

    string VehicleDocumentsFolder(int vehicleId)
    {
        var folder=Path.Combine(DocumentsRoot(),vehicleId.ToString());
        Directory.CreateDirectory(folder);
        return folder;
    }

    void UploadPopupDocument(ComboBox vehicle,string key)
    {
        if(vehicle.SelectedItem is not VehicleChoice c)
        {
            MessageBox.Show("Önce bir araç seçin.","Belge",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            return;
        }

        using var dlg=new OpenFileDialog
        {
            Filter="Belge dosyaları|*.pdf;*.jpg;*.jpeg;*.png",
            Multiselect=false,
            Title="Belge seçin"
        };

        if(dlg.ShowDialog(this)!=DialogResult.OK)
            return;

        var folder=VehicleDocumentsFolder(c.Id);

        foreach(var old in Directory.GetFiles(folder,key+".*"))
        {
            try{File.Delete(old);}catch{}
        }

        File.Copy(
            dlg.FileName,
            Path.Combine(folder,key+Path.GetExtension(dlg.FileName)),
            true);

        MessageBox.Show(
            "Belge başarıyla yüklendi.",
            "Belge",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    void ViewPopupDocument(ComboBox vehicle,string key)
    {
        if(vehicle.SelectedItem is not VehicleChoice c)
        {
            MessageBox.Show("Önce bir araç seçin.","Belge",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            return;
        }

        var file=Directory.GetFiles(
            VehicleDocumentsFolder(c.Id),
            key+".*").FirstOrDefault();

        if(file==null)
        {
            MessageBox.Show(
                "Bu belge henüz yüklenmemiş.",
                "Belge",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        Process.Start(new ProcessStartInfo(file){UseShellExecute=true});
    }

    sealed class VehicleChoice
    {
        public int Id{get;}
        public string Plate{get;}
        public string Brand{get;}
        public string Model{get;}

        public VehicleChoice(int id,string plate,string brand,string model)
        {
            Id=id; Plate=plate; Brand=brand; Model=model;
        }

        public override string ToString()=>$"{Plate} - {Brand} {Model}";
    }


    void ShowDashboard()
    {
        pageTitle.Text="Manisa TM Filo Yönetimi";
        var p=Page();
        var data=Database.GetAll(); var today=DateTime.Today;

        var due=data.Count(v=>v.InspectionDate.HasValue&&v.InspectionDate.Value.Date>=today&&v.InspectionDate.Value.Date<=today.AddDays(25));
        var expired=data.Count(v=>v.InspectionDate.HasValue&&v.InspectionDate.Value.Date<today);
        var tach=data.Count(v=>v.TachographInspectionDate.HasValue&&v.TachographInspectionDate.Value.Date>=today&&v.TachographInspectionDate.Value.Date<=today.AddDays(25));

        var cards=new FlowLayoutPanel{Dock=DockStyle.Top,Height=106,WrapContents=false,Padding=new Padding(2,0,0,0)};
        cards.Controls.Add(Card("Toplam Araç",data.Count,AppTheme.Blue,"▣"));
        cards.Controls.Add(Card("25 Gün İçinde Vize",due,AppTheme.Orange,"⚠"));
        cards.Controls.Add(Card("Vizesi Geçmiş",expired,AppTheme.Red,"◷"));
        cards.Controls.Add(Card("Takograf Yaklaşan",tach,AppTheme.Green,"◴"));
p.Controls.Add(cards);

        // Üst: Son kayıtlar + uyarılar
        var upper=new TableLayoutPanel{Dock=DockStyle.Top,Height=290,ColumnCount=2,Padding=new Padding(0,16,0,0)};
        upper.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,66));
        upper.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,34));

        var recent=PanelCard("Son Kayıtlar");
        recent.Controls.Add(BuildGrid(data.Take(8).ToList()));
        upper.Controls.Add(recent,0,0);

        var alerts=PanelCard("Uyarılar");
        var warnText=new RichTextBox{Dock=DockStyle.Fill,ReadOnly=true,BorderStyle=BorderStyle.None,BackColor=Color.White,Font=new Font("Segoe UI",10)};
        warnText.AppendText($"• {expired} araçta vize tarihi geçmiş.\n\n");
        warnText.AppendText($"• {due} aracın vizesi 25 gün içinde dolacak.\n\n");
        warnText.AppendText($"• {tach} aracın takograf muayenesi 25 gün içinde.\n");
        alerts.Controls.Add(warnText);
        upper.Controls.Add(alerts,1,0);
        p.Controls.Add(upper);

        // Alt: Sabit grafik alanı
        var chartTitle = new Label {
            Text="Filo Dağılımı",
            Dock=DockStyle.Top,
            Height=34,
            AutoSize=false,
            Font=new Font("Segoe UI Semibold",16,FontStyle.Bold),
            ForeColor=AppTheme.Navy,
            TextAlign=ContentAlignment.MiddleLeft,
            Padding=new Padding(6,0,0,0),
            Margin=new Padding(0)
        };
        p.Controls.Add(chartTitle);

        var chartGrid=new TableLayoutPanel{Dock=DockStyle.Top,Height=320,ColumnCount=3,Padding=new Padding(0,6,0,0)};
        chartGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.33f));
        chartGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.33f));
        chartGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.34f));

        var loc=data.GroupBy(v=>string.IsNullOrWhiteSpace(v.Location)?"Tanımsız":v.Location)
                   .OrderByDescending(g=>g.Count()).ToDictionary(g=>g.Key,g=>g.Count());
        var unit=data.GroupBy(v=>string.IsNullOrWhiteSpace(v.Unit)?"Tanımsız":v.Unit)
                    .OrderByDescending(g=>g.Count()).ToDictionary(g=>g.Key,g=>g.Count());
        var fleet=data.GroupBy(v=>string.IsNullOrWhiteSpace(v.FleetCompany)?"Tanımsız":v.FleetCompany)
                     .OrderByDescending(g=>g.Count()).ToDictionary(g=>g.Key,g=>g.Count());

        chartGrid.Controls.Add(ChartCard("Lokasyona Göre Araç Sayısı",loc,AppTheme.Blue),0,0);
        chartGrid.Controls.Add(ChartCard("Birime Göre Araç Sayısı",unit,AppTheme.Cyan),1,0);
        chartGrid.Controls.Add(ChartCard("Filo Şirketine Göre Araç Sayısı",fleet,AppTheme.Green),2,0);

        p.Controls.Add(chartGrid);
    }

    Panel ChartCard(string title, System.Collections.Generic.Dictionary<string,int> values, Color color)
    {
        var panel=PanelCard(title);
        panel.Controls.Add(new BarChart(values,color));
        return panel;
    }

    Panel PanelCard(string title)
    {
        var g=new Panel{Dock=DockStyle.Fill,BackColor=Color.White,Margin=new Padding(6),Padding=new Padding(14),BorderStyle=BorderStyle.FixedSingle};
        g.Controls.Add(new Label{Text=title,Font=new Font("Segoe UI Semibold",11.5f,FontStyle.Bold),ForeColor=AppTheme.Navy,Dock=DockStyle.Top,Height=28,AutoSize=false,TextAlign=ContentAlignment.MiddleLeft});
        return g;
    }

    Panel Card(string title,int value,Color accent,string icon)
    {
        var p=new Panel{Width=245,Height=90,BackColor=Color.White,Margin=new Padding(0,0,12,0),Padding=new Padding(14)};
        var bar=new Panel{Dock=DockStyle.Left,Width=6,BackColor=accent}; p.Controls.Add(bar);
        p.Controls.Add(new Label{Text=icon,ForeColor=accent,Font=new Font("Segoe UI",15,FontStyle.Bold),Left=18,Top=30,AutoSize=true});
        p.Controls.Add(new Label{Text=title,ForeColor=AppTheme.Muted,Font=new Font("Segoe UI",8.5f),Left=55,Top=10,Width=175,Height=20,AutoSize=false});
        p.Controls.Add(new Label{Text=value.ToString(),ForeColor=AppTheme.Text,Font=new Font("Segoe UI",19,FontStyle.Bold),Left=55,Top=34,AutoSize=true});
        return p;
    }

    DataGridView BuildGrid(System.Collections.Generic.List<Vehicle> data)
    {
        var g=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AutoGenerateColumns=false,SelectionMode=DataGridViewSelectionMode.FullRowSelect,RowHeadersVisible=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,BackgroundColor=Color.White,BorderStyle=BorderStyle.None};
        foreach(var h in new[]{"Plaka","Marka","Model","Vize Tarihi","Durum"}) g.Columns.Add(new DataGridViewTextBoxColumn{HeaderText=h,Name=h});
        foreach(var v in data){
            var days=v.InspectionDate.HasValue?(v.InspectionDate.Value.Date-DateTime.Today).Days:int.MaxValue;
            var status=days<0?"Süresi Geçmiş":days<=25?"Yaklaşıyor":"Normal";
            g.Rows.Add(v.Plate,v.Brand,v.Model,v.InspectionDate?.ToString("dd.MM.yyyy")??"-",status);
            var r=g.Rows[^1]; r.Cells["Durum"].Style.ForeColor=status=="Süresi Geçmiş"?AppTheme.Red:status=="Yaklaşıyor"?AppTheme.Orange:AppTheme.Green;
        }
        g.CellDoubleClick += (_,e)=>{if(e.RowIndex>=0){var plate=g.Rows[e.RowIndex].Cells["Plaka"].Value?.ToString();var v=Database.GetAll().FirstOrDefault(x=>x.Plate==plate);if(v!=null)ShowVehicleForm(v);}};
        return g;
    }

    void ShowVehicleForm(Vehicle? v){using var f=new VehicleForm(v,ShowDashboard);f.ShowDialog(this);ShowDashboard();}


    void ShowBackupRestore()
    {
        pageTitle.Text="Yedekle / Geri Yükle";
        var p=Page();

        var card=new Panel{
            Dock=DockStyle.Top,
            Height=220,
            BackColor=Color.White,
            Padding=new Padding(24),
            BorderStyle=BorderStyle.FixedSingle
        };

        card.Controls.Add(new Label{
            Text="Veritabanı İşlemleri",
            Dock=DockStyle.Top,
            Height=34,
            Font=new Font("Segoe UI Semibold",14,FontStyle.Bold),
            ForeColor=AppTheme.Navy
        });

        var info=new Label{
            Text="Araç kayıtlarını tek bir .db yedek dosyası olarak kaydedebilir veya daha önce alınmış bir yedeği geri yükleyebilirsiniz.",
            Dock=DockStyle.Top,
            Height=54,
            Font=new Font("Segoe UI",10),
            ForeColor=AppTheme.Muted
        };
        card.Controls.Add(info);

        var flow=new FlowLayoutPanel{
            Dock=DockStyle.Top,
            Height=60,
            Padding=new Padding(0,8,0,0)
        };

        var backup=new Button{
            Text="💾  Yedek Al",
            Width=150,Height=38,
            BackColor=AppTheme.Green,ForeColor=Color.White,
            FlatStyle=FlatStyle.Flat,Font=new Font("Segoe UI",9.5f,FontStyle.Bold)
        };
        backup.FlatAppearance.BorderSize=0;
        backup.Click += (_,_)=>CreateBackup();

        var restore=new Button{
            Text="↻  Yedeği Geri Yükle",
            Width=175,Height=38,
            BackColor=AppTheme.Blue,ForeColor=Color.White,
            FlatStyle=FlatStyle.Flat,Font=new Font("Segoe UI",9.5f,FontStyle.Bold)
        };
        restore.FlatAppearance.BorderSize=0;
        restore.Click += (_,_)=>RestoreBackup();

        flow.Controls.Add(backup);
        flow.Controls.Add(restore);
        card.Controls.Add(flow);

        p.Controls.Add(card);
    }

    string DatabaseFilePath() => Path.Combine(AppContext.BaseDirectory,"FiloYonetimi.db");

    void CreateBackup()
    {
        var source=DatabaseFilePath();
        if(!File.Exists(source)){
            MessageBox.Show("Henüz veritabanı bulunamadı.","Yedekleme",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            return;
        }

        using var dlg=new SaveFileDialog{
            Filter="Filo Yönetimi Yedek Dosyası (*.db)|*.db",
            FileName="FiloYonetimi_Yedek_"+DateTime.Now.ToString("yyyyMMdd_HHmmss")+".db"
        };

        if(dlg.ShowDialog(this)!=DialogResult.OK) return;

        try{
            File.Copy(source,dlg.FileName,true);
            MessageBox.Show("Yedek başarıyla oluşturuldu.","Yedekleme",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        catch(Exception ex){
            MessageBox.Show("Yedek alınırken hata oluştu:\n"+ex.Message,"Yedekleme Hatası",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }

    void RestoreBackup()
    {
        using var dlg=new OpenFileDialog{
            Filter="Filo Yönetimi Yedek Dosyası (*.db)|*.db",
            Multiselect=false
        };

        if(dlg.ShowDialog(this)!=DialogResult.OK) return;

        var current=DatabaseFilePath();

        var confirm=MessageBox.Show(
            "Mevcut araç kayıtlarının yerine seçilen yedek yüklenecek.\n\nDevam etmek istiyor musunuz?",
            "Yedeği Geri Yükle",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if(confirm!=DialogResult.Yes) return;

        try{
            var backupCopy=current+".before_restore_"+DateTime.Now.ToString("yyyyMMdd_HHmmss")+".bak";
            if(File.Exists(current)) File.Copy(current,backupCopy,true);
            File.Copy(dlg.FileName,current,true);

            MessageBox.Show(
                "Yedek geri yüklendi. Uygulamanın yeni veritabanını kullanması için uygulamayı yeniden başlatın.",
                "Geri Yükleme",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch(Exception ex){
            MessageBox.Show("Yedek geri yüklenirken hata oluştu:\n"+ex.Message,"Geri Yükleme Hatası",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }

    void ShowVehicleList()
    {
        pageTitle.Text="Araç Listesi"; var p=Page();
        var top=new Panel{Dock=DockStyle.Top,Height=52}; var search=new TextBox{Left=0,Top=6,Width=340,Font=new Font("Segoe UI",11)};
        top.Controls.Add(search); top.Controls.Add(new Label{Text="Plaka / marka / model / lokasyon ara",Left=355,Top=11,ForeColor=AppTheme.Muted,AutoSize=true});
        var grid=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AutoGenerateColumns=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,BackgroundColor=Color.White};
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="Id",HeaderText="ID",Visible=false});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="Plaka",HeaderText="Plaka"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="Marka",HeaderText="Marka"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="Model",HeaderText="Model"});
         grid.Columns.Add(new DataGridViewTextBoxColumn{Name="ModelYili",HeaderText="Model Yılı"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="AracTipi",HeaderText="Araç Tipi"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="Yakit",HeaderText="Yakıt"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="VizeTarihi",HeaderText="Vize / Muayene Tarihi"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="Lokasyon",HeaderText="Çalıştığı Lokasyon"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="Birim",HeaderText="Bağlı Olduğu Birim"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{Name="FiloSirketi",HeaderText="Filo Şirketi"});
        void Bind(){
            grid.Rows.Clear();
            foreach(var x in Database.GetAll(search.Text))
                grid.Rows.Add(x.Id,x.Plate,x.Brand,x.Model,x.ModelYear?.ToString() ?? "-",x.Type,x.Fuel,x.InspectionDate?.ToString("dd.MM.yyyy"),x.Location,x.Unit,x.FleetCompany);
        }
        search.TextChanged+=(_,_)=>Bind();grid.CellDoubleClick+=(object? s, DataGridViewCellEventArgs e)=>{if(e.RowIndex>=0&&grid.Rows[e.RowIndex].Cells["Id"].Value is int id){var v=Database.Get(id);if(v!=null)ShowVehicleForm(v);}};
        p.Controls.Add(grid);p.Controls.Add(top);Bind();
    }



    void ShowUpcoming()
    {
        pageTitle.Text="Yaklaşan Tarihler";
        var p=Page();
        var card=PanelCard("Vize / Muayene Takibi");

        var grid=new DataGridView
        {
            Dock=DockStyle.Fill,
            ReadOnly=true,
            AllowUserToAddRows=false,
            AutoGenerateColumns=false,
            SelectionMode=DataGridViewSelectionMode.FullRowSelect,
            RowHeadersVisible=false,
            AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor=Color.White,
            BorderStyle=BorderStyle.None
        };

        grid.Columns.Add(new DataGridViewTextBoxColumn{HeaderText="Plaka",Name="Plaka"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{HeaderText="Vize / Muayene Tarihi",Name="Tarih"});
        grid.Columns.Add(new DataGridViewTextBoxColumn{HeaderText="Durum",Name="Durum"});

        var today=DateTime.Today;

        foreach(var v in Database.GetAll()
            .Where(v=>v.InspectionDate.HasValue &&
                      v.InspectionDate.Value.Date<=today.AddDays(25))
            .OrderBy(v=>v.InspectionDate))
        {
            var days=(v.InspectionDate!.Value.Date-today).Days;
            var status=days<0?"Süresi Geçmiş":"Yaklaşıyor";
            grid.Rows.Add(v.Plate,v.InspectionDate.Value.ToString("dd.MM.yyyy"),status);

            var row=grid.Rows[^1];
            row.Tag=new DueRowInfo
            {
                Days=days,
                Color=status=="Süresi Geçmiş"?AppTheme.Red:AppTheme.Orange
            };
            row.Cells["Durum"].Style.ForeColor=((DueRowInfo)row.Tag).Color;
        }

        var blinkTimer=new System.Windows.Forms.Timer{Interval=500};
        var blink=false;
        blinkTimer.Tick+=(_,_)=>{
            blink=!blink;
            foreach(DataGridViewRow row in grid.Rows)
            {
                if(row.Tag is DueRowInfo info && info.Days>=0 && info.Days<=25)
                {
                    row.Cells["Durum"].Style.ForeColor=blink?Color.White:info.Color;
                    row.Cells["Durum"].Style.BackColor=blink?info.Color:Color.White;
                }
            }
        };
        blinkTimer.Start();

        card.Controls.Add(grid);
        p.Controls.Add(card);
    }

    sealed class DueRowInfo
    {
        public int Days{get;set;}
        public Color Color{get;set;}
    }


    void ShowReports()
    {
        pageTitle.Text="Raporlama Dashboard";
        var p=Page();
        var d=Database.GetAll();

        var toolbar = new FlowLayoutPanel {
            Dock=DockStyle.Top,
            Height=52,
            FlowDirection=FlowDirection.LeftToRight,
            WrapContents=false,
            Padding=new Padding(4,4,0,4)
        };

        var excelBtn = new Button {
            Text="📊  Excel'e Aktar",
            Width=155, Height=36,
            BackColor=AppTheme.Green, ForeColor=Color.White,
            FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",9.5f,FontStyle.Bold)
        };
        excelBtn.FlatAppearance.BorderSize=0;
        excelBtn.Click += (_,_) => ExportExcel();

        var pdfBtn = new Button {
            Text="📄  PDF Rapor",
            Width=145, Height=36,
            BackColor=AppTheme.Red, ForeColor=Color.White,
            FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",9.5f,FontStyle.Bold)
        };
        pdfBtn.FlatAppearance.BorderSize=0;
        pdfBtn.Click += (_,_) => ExportPdf();

        toolbar.Controls.Add(excelBtn);
        toolbar.Controls.Add(pdfBtn);
        p.Controls.Add(toolbar);

        var body=new TableLayoutPanel{
            Dock=DockStyle.Fill,
            ColumnCount=2,
            RowCount=3,
            Padding=new Padding(0,4,0,0)
        };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
        body.RowStyles.Add(new RowStyle(SizeType.Absolute,130));
        body.RowStyles.Add(new RowStyle(SizeType.Percent,50));
        body.RowStyles.Add(new RowStyle(SizeType.Percent,50));

        var today=DateTime.Today;
        var total=d.Count;
        var due=d.Count(v=>v.InspectionDate.HasValue&&v.InspectionDate.Value.Date>=today&&v.InspectionDate.Value.Date<=today.AddDays(25));
        var exp=d.Count(v=>v.InspectionDate.HasValue&&v.InspectionDate.Value.Date<today);

        var s1=PanelCard("Filo Özeti");
        s1.Controls.Add(new Label{
            Text=$"Toplam {total} araç   •   25 gün içinde {due}   •   Süresi geçmiş {exp}",
            Dock=DockStyle.Fill,Font=new Font("Segoe UI",15),ForeColor=AppTheme.Text,TextAlign=ContentAlignment.MiddleCenter
        });
        body.Controls.Add(s1,0,0);

        body.Controls.Add(AddChart("Yakıt Dağılımı",
            d.GroupBy(x=>string.IsNullOrWhiteSpace(x.Fuel)?"Tanımsız":x.Fuel)
             .ToDictionary(g=>g.Key,g=>g.Count()),AppTheme.Blue),1,0);

        body.Controls.Add(AddChart("Araç Tipi Dağılımı",
            d.GroupBy(x=>string.IsNullOrWhiteSpace(x.Type)?"Tanımsız":x.Type)
             .ToDictionary(g=>g.Key,g=>g.Count()),AppTheme.Cyan),0,1);

        body.Controls.Add(AddChart("Lokasyon Dağılımı",
            d.GroupBy(x=>string.IsNullOrWhiteSpace(x.Location)?"Tanımsız":x.Location)
             .ToDictionary(g=>g.Key,g=>g.Count()),AppTheme.Green),1,1);

        body.Controls.Add(AddChart("Vize Durumu",
            new[]{("Normal",d.Count(v=>!v.InspectionDate.HasValue||v.InspectionDate.Value.Date>today.AddDays(25))),
                  ("Yaklaşıyor",due),("Süresi Geçmiş",exp)}
            .ToDictionary(x=>x.Item1,x=>x.Item2),AppTheme.Orange),0,2);

        body.Controls.Add(AddChart("Birim",
            d.GroupBy(x=>string.IsNullOrWhiteSpace(x.Unit)?"Tanımsız":x.Unit)
             .ToDictionary(g=>g.Key,g=>g.Count()),AppTheme.Navy),1,2);

        p.Controls.Add(body);
    }

    string BuildReportText()
    {
        var d=Database.GetAll();
        var sb=new System.Text.StringBuilder();
        sb.AppendLine("MANİSA TM FİLO YÖNETİMİ - FİLO RAPORU");
        sb.AppendLine("Tarih: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
        sb.AppendLine(new string('=', 110));
        sb.AppendLine("Plaka\tMarka\tModel\tAraç Tipi\tYakıt\tFilo Şirketi\tLokasyon\tBirim\tVize/Muayene\tTakograf");
        foreach(var v in d)
        {
            sb.AppendLine(string.Join("\t", new[]{
                v.Plate,v.Brand,v.Model,v.Type,v.Fuel,v.FleetCompany,v.Location,v.Unit,
                v.InspectionDate?.ToString("dd.MM.yyyy")??"-",
                v.TachographInspectionDate?.ToString("dd.MM.yyyy")??"-"
            }));
        }
        return sb.ToString();
    }

    void ExportExcel()
    {
        using var dlg=new SaveFileDialog{
            Filter="Excel uyumlu dosya (*.xls)|*.xls|Excel dosyası (*.xlsx)|*.xlsx",
            FileName="Manisa_TM_Filo_Raporu_"+DateTime.Now.ToString("yyyyMMdd_HHmm")
        };
        if(dlg.ShowDialog(this)!=DialogResult.OK) return;

        // Excel tarafından açılabilen HTML tablo formatı (.xls uzantılı).
        var d=Database.GetAll();
        var sb=new System.Text.StringBuilder();
        sb.Append("<html><head><meta charset='utf-8'><style>");
        sb.Append("body{font-family:Segoe UI,Arial;} table{border-collapse:collapse;width:100%;} ");
        sb.Append("th{background:#042D58;color:#fff;padding:8px;border:1px solid #ccc;} ");
        sb.Append("td{padding:7px;border:1px solid #ddd;} tr:nth-child(even){background:#f6f8fb;}");
        sb.Append("</style></head><body>");
        sb.Append("<h2>Manisa TM Filo Yönetimi - Filo Raporu</h2>");
        sb.Append("<table><tr>");
        foreach(var h in new[]{"Plaka","Marka","Model","Araç Tipi","Yakıt","Filo Şirketi","Lokasyon","Birim","Vize / Muayene","Takograf Muayene"})
            sb.Append("<th>"+System.Net.WebUtility.HtmlEncode(h)+"</th>");
        sb.Append("</tr>");
        foreach(var v in d)
        {
            sb.Append("<tr>");
            foreach(var cell in new[]{
                v.Plate,v.Brand,v.Model,v.Type,v.Fuel,v.FleetCompany,v.Location,v.Unit,
                v.InspectionDate?.ToString("dd.MM.yyyy")??"-",
                v.TachographInspectionDate?.ToString("dd.MM.yyyy")??"-"
            }) sb.Append("<td>"+System.Net.WebUtility.HtmlEncode(cell??"")+"</td>");
            sb.Append("</tr>");
        }
        sb.Append("</table></body></html>");
        File.WriteAllText(dlg.FileName,sb.ToString(),System.Text.Encoding.UTF8);
        MessageBox.Show("Excel uyumlu rapor oluşturuldu.","Rapor",MessageBoxButtons.OK,MessageBoxIcon.Information);
    }


    void ExportPdf()
    {
        var d = Database.GetAll();
        if (d.Count == 0)
        {
            MessageBox.Show("Raporlanacak araç kaydı bulunmuyor.", "PDF Rapor",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var doc = new PrintDocument();
        doc.DefaultPageSettings.Landscape = true;
        doc.DefaultPageSettings.Margins = new Margins(30, 30, 30, 30);

        var pageIndex = 0;
        const int rowsPerPage = 26;

        doc.PrintPage += (object? sender, PrintPageEventArgs e) =>
        {
            using var titleFont = new Font("Segoe UI", 14, FontStyle.Bold);
            using var headFont = new Font("Segoe UI", 8, FontStyle.Bold);
            using var bodyFont = new Font("Segoe UI", 7.5f);
            using var brush = new SolidBrush(Color.Black);

            float y = e.MarginBounds.Top;

            e.Graphics.DrawString(
                "Manisa TM Filo Yönetimi - Filo Raporu",
                titleFont, brush, e.MarginBounds.Left, y);

            y += 28;

            e.Graphics.DrawString(
                "Tarih: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                bodyFont, brush, e.MarginBounds.Left, y);

            y += 22;

            var headers = new[]
            {
                "Plaka","Marka","Model","Araç Tipi","Yakıt",
                "Filo Şirketi","Lokasyon","Birim","Vize","Takograf"
            };

            int cols = headers.Length;
            float totalW = e.MarginBounds.Width;
            float colW = totalW / cols;
            float rowH = 23;

            using var headerBrush = new SolidBrush(AppTheme.Navy);
            using var alternateBrush = new SolidBrush(Color.FromArgb(246, 248, 251));

            for (int i = 0; i < cols; i++)
            {
                e.Graphics.FillRectangle(
                    headerBrush,
                    e.MarginBounds.Left + i * colW,
                    y,
                    colW,
                    rowH);

                using var center = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                e.Graphics.DrawString(
                    headers[i],
                    headFont,
                    Brushes.White,
                    new RectangleF(
                        e.MarginBounds.Left + i * colW + 2,
                        y,
                        colW - 4,
                        rowH),
                    center);
            }

            y += rowH;

            int pageStart = pageIndex * rowsPerPage;
            int pageEnd = Math.Min(d.Count, pageStart + rowsPerPage);

            for (int r = pageStart; r < pageEnd; r++)
            {
                if ((r - pageStart) % 2 == 0)
                {
                    e.Graphics.FillRectangle(
                        alternateBrush,
                        e.MarginBounds.Left,
                        y,
                        totalW,
                        rowH);
                }

                var v = d[r];
                var cells = new[]
                {
                    v.Plate ?? "",
                    v.Brand ?? "",
                    v.Model ?? "",
                    v.Type ?? "",
                    v.Fuel ?? "",
                    v.FleetCompany ?? "",
                    v.Location ?? "",
                    v.Unit ?? "",
                    v.InspectionDate?.ToString("dd.MM.yyyy") ?? "-",
                    v.TachographInspectionDate?.ToString("dd.MM.yyyy") ?? "-"
                };

                for (int i = 0; i < cols; i++)
                {
                    e.Graphics.DrawRectangle(
                        Pens.LightGray,
                        e.MarginBounds.Left + i * colW,
                        y,
                        colW,
                        rowH);

                    using var left = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter,
                        FormatFlags = StringFormatFlags.NoWrap
                    };

                    e.Graphics.DrawString(
                        cells[i],
                        bodyFont,
                        brush,
                        new RectangleF(
                            e.MarginBounds.Left + i * colW + 3,
                            y + 1,
                            colW - 6,
                            rowH - 2),
                        left);
                }

                y += rowH;
            }

            pageIndex++;
            e.HasMorePages = pageEnd < d.Count;
        };

        // Windows PrintDialog is used here because PrintDocument itself
        // does not expose PrintToFile/PrintFileName properties in .NET 8.
        using var printDialog = new PrintDialog
        {
            Document = doc,
            UseEXDialog = true
        };

        try
        {
            if (printDialog.ShowDialog(this) == DialogResult.OK)
            {
                doc.Print();
                MessageBox.Show(
                    "PDF yazdırma işlemi başlatıldı. Yazıcı olarak 'Microsoft Print to PDF' seçtiyseniz Windows dosya adını soracaktır.",
                    "PDF Rapor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "PDF oluşturulurken/yazdırılırken hata oluştu:\n" + ex.Message,
                "PDF Hatası",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    Panel AddChart(string title,System.Collections.Generic.Dictionary<string,int> data,Color c)
    {
        var card=PanelCard(title);card.Controls.Add(new BarChart(data,c));return card;
    }

    class BarChart:Panel
    {
        readonly System.Collections.Generic.Dictionary<string,int> data; readonly Color color;
        public BarChart(System.Collections.Generic.Dictionary<string,int> d,Color c){data=d;color=c;Dock=DockStyle.Fill;Padding=new Padding(6);BackColor=Color.White;Paint+=Draw;}
        void Draw(object? s,PaintEventArgs e)
        {
            e.Graphics.SmoothingMode=SmoothingMode.AntiAlias;
            if(data.Count==0){e.Graphics.DrawString("Henüz veri yok.",Font,new SolidBrush(AppTheme.Muted),10,10);return;}
            var max=Math.Max(1,data.Values.Max());
            int y=8;
            foreach(var kv in data.OrderByDescending(x=>x.Value).Take(8))
            {
                int w=(int)((Width-155)*(kv.Value/(double)max));
                using var brush=new SolidBrush(color);
                e.Graphics.FillRectangle(brush,125,y,Math.Max(4,w),18);
                using var tb=new SolidBrush(AppTheme.Text);
                var label=kv.Key.Length>18?kv.Key[..18]+"…":kv.Key;
                e.Graphics.DrawString(label,Font,tb,0,y-1);
                e.Graphics.DrawString(kv.Value.ToString(),Font,tb,130+w,y-1);
                y+=28;
            }
        }
    }

    void StartupAlert()
    {
        var d=Database.GetAll();var today=DateTime.Today;var due=d.Where(v=>v.InspectionDate.HasValue&&v.InspectionDate.Value.Date>=today&&v.InspectionDate.Value.Date<=today.AddDays(25)).ToList();var exp=d.Where(v=>v.InspectionDate.HasValue&&v.InspectionDate.Value.Date<today).ToList();
        if(due.Count+exp.Count>0) MessageBox.Show($"Süresi geçmiş: {exp.Count}\n25 gün içinde yaklaşan: {due.Count}","Manisa TM Filo Yönetimi - Uyarılar",MessageBoxButtons.OK,MessageBoxIcon.Warning);
    }
}
