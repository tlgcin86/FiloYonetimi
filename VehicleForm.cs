using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FiloYonetimi;

public class VehicleForm : Form
{
    readonly Vehicle? existing;
    readonly Action refresh;
    readonly System.Collections.Generic.Dictionary<string,Control> fields = new();

    ComboBox brandBox = new(), modelBox = new(), modelYearBox = new();
    ComboBox typeBox = new(), fuelBox = new(), fleetBox = new(), locationBox = new(), unitBox = new(), tireSizeBox = new(), tireBrandBox = new();

    readonly string[] names = {
        "Plaka","Marka","Model","Model Yılı","Araç Tipi","Yakıt","Yakıt Firması","Şase No","Filo Şirketi",
        "Filo Vergi Dairesi","GPS ID","AdBlue Kit No","Lastik Ebatı","Lastik Markası","Yıkama Kodları",
        "Vize/Muayene Tarihi","Takograf Muayene Tarihi","Çalıştığı Lokasyon","Bağlı Olduğu Birim"
    };

    static readonly string[] VehicleTypes = {
        "Otomobil","SUV","Sedan","Hatchback","Station Wagon","Coupe","Cabrio",
        "Kamyonet","Pickup","Panelvan","Van","Minibüs","Kamyon","Çekici","Tır",
        "Tanker","Dorse","Römork","Otobüs","Midibüs","Servis",
        "İş Makinesi","Özel Amaçlı","Diğer"
    };

    static readonly string[] FuelTypes = {
        "Benzin","Dizel","LPG","Hibrit","Elektrik",
        "Benzin + LPG","Benzin + Elektrik","Dizel + Elektrik",
        "MHEV","HEV","PHEV"
    };

    static readonly string[] FleetCompanies = {
        "KayaTur","Tan Filo","Carpartner","Giray Turizm","Ziraat Filo","Türk Telekom ( Özmal )"
    };

    static readonly string[] Locations = {
        "Manisa - Merkez","Akhisar","Kırkağaç","Soma","Gördes","Demirci","Saruhanlı",
        "Turgutlu","Salihli","Alaşehir","Sarıgöl","Selendi","Kula"
    };

    static readonly string[] Units = {
        "Manisa S.O.Y.",
        "Manisa T.F.Y.",
        "Akhisar S.O.Y.",
        "Akhisar T.F.Y.",
        "Salihli S.O.Y.",
        "Salihli T.F.Y.",
        "Lojistik",
        "Tesis Destek",
        "Temel Şebeke O.Y.",
        "Kişiye Özel"
    };

    public VehicleForm(Vehicle? vehicle, Action refreshAction)
    {
        existing = vehicle;
        refresh = refreshAction;
        Text = vehicle == null ? "Yeni Araç Kaydı" : "Araç Düzenle";
        Width = 1060;
        Height = 780;
        MinimumSize = new Size(960, 700);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = AppTheme.Bg;
        Build();
        if (vehicle != null) Fill(vehicle);
    }

    void Build()
    {
        var title = new Label {
            Text = Text,
            Dock = DockStyle.Top,
            Height = 42,
            Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
            ForeColor = AppTheme.Navy,
            BackColor = AppTheme.Bg,
            Padding = new Padding(20, 10, 0, 0)
        };
        Controls.Add(title);

        var p = new TableLayoutPanel {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = names.Length + 1,
            Padding = new Padding(20, 6, 20, 16),
            BackColor = AppTheme.Bg,
            AutoScroll = true
        };

        p.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        for (int i = 0; i < names.Length; i++)
        {
            p.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));

            p.Controls.Add(new Label {
                Text = names[i],
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(3, 9, 3, 0),
                ForeColor = AppTheme.Text
            }, 0, i);

            Control c;

            if (names[i] == "Plaka")
            {
                c = new TextBox {
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10),
                    CharacterCasing = CharacterCasing.Upper
                };
            }
            else if (names[i] == "Marka")
            {
                brandBox = ChoiceCombo(VehicleCatalog.BrandsAndModels.Keys.OrderBy(x => x).ToArray());
                brandBox.SelectedIndexChanged += (_, _) => LoadModels();
                c = brandBox;
            }
            else if (names[i] == "Model")
            {
                modelBox = ChoiceCombo(Array.Empty<string>());
                c = modelBox;
            }
            else if (names[i] == "Model Yılı")
            {
                var years = Enumerable.Range(DateTime.Now.Year - 40, 41)
                    .OrderByDescending(y => y)
                    .Select(y => y.ToString())
                    .ToArray();
                modelYearBox = ChoiceCombo(years);
                c = modelYearBox;
            }
            else if (names[i] == "Araç Tipi")
            {
                typeBox = ChoiceCombo(VehicleTypes);
                typeBox.SelectedIndexChanged += (_, _) => RefreshTireSizes();
                c = typeBox;
            }
            else if (names[i] == "Yakıt")
            {
                fuelBox = ChoiceCombo(FuelTypes);
                c = fuelBox;
            }
            else if (names[i] == "Filo Şirketi")
            {
                fleetBox = ChoiceCombo(FleetCompanies);
                c = fleetBox;
            }
            else if (names[i] == "Çalıştığı Lokasyon")
            {
                locationBox = ChoiceCombo(Locations);
                c = locationBox;
            }
            else if (names[i] == "Lastik Ebatı")
            {
                tireSizeBox = ChoiceCombo(VehicleCatalog.TireSizesByType.Values.SelectMany(x => x).Distinct().OrderBy(x => x).ToArray());
                c = tireSizeBox;
            }
            else if (names[i] == "Lastik Markası")
            {
                tireBrandBox = ChoiceCombo(VehicleCatalog.TireBrands.OrderBy(x => x).ToArray());
                c = tireBrandBox;
            }
            else if (names[i] == "Bağlı Olduğu Birim")
            {
                unitBox = ChoiceCombo(Units);
                c = unitBox;
            }
            else if (names[i].Contains("Tarihi"))
            {
                c = new DateTimePicker {
                    Format = DateTimePickerFormat.Short,
                    ShowCheckBox = true,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10)
                };
            }
            else
            {
                c = new TextBox {
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10)
                };
            }

            fields[names[i]] = c;
            p.Controls.Add(c, 1, i);
        }

        var save = new Button {
            Text = "✓   Kaydet",
            Width = 140,
            Height = 38,
            BackColor = AppTheme.Blue,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        save.FlatAppearance.BorderSize = 0;

        var cancel = new Button {
            Text = "İptal",
            Width = 110,
            Height = 38,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White
        };

        save.Click += Save;
        cancel.Click += (_, _) => Close();

        var flow = new FlowLayoutPanel {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };
        flow.Controls.Add(cancel);
        flow.Controls.Add(save);

        p.Controls.Add(flow, 1, names.Length);
        Controls.Add(p);
        p.BringToFront();
    }

    static ComboBox ChoiceCombo(string[] items)
    {
        var cb = new ComboBox {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10),
            IntegralHeight = true
        };

        cb.Items.AddRange(items);

        return cb;
    }

    void RefreshTireSizes()
    {
        var selectedType = typeBox.SelectedItem?.ToString() ?? "";
        string category = selectedType switch
        {
            "Kamyonet" or "Panelvan" or "Van" or "Minibüs" => "Kamyonet",
            "Pickup" => "Pickup",
            "Kamyon" or "Çekici" or "Tır" or "Tanker" or "Dorse" or "Römork" => "Kamyon",
            "SUV" => "SUV",
            _ => "Otomobil"
        };

        var current = tireSizeBox.SelectedItem?.ToString();
        tireSizeBox.BeginUpdate();
        tireSizeBox.Items.Clear();
        if (VehicleCatalog.TireSizesByType.TryGetValue(category, out var sizes))
            tireSizeBox.Items.AddRange(sizes.OrderBy(x => x).ToArray());
        tireSizeBox.EndUpdate();

        if (!string.IsNullOrWhiteSpace(current) && tireSizeBox.Items.Contains(current))
            tireSizeBox.SelectedItem = current;
        else if (tireSizeBox.Items.Count > 0)
            tireSizeBox.SelectedIndex = 0;
    }

    void LoadModels()
    {
        modelBox.Items.Clear();

        if (brandBox.SelectedItem is string brand &&
            VehicleCatalog.BrandsAndModels.TryGetValue(brand, out var models))
        {
            modelBox.Items.AddRange(models);
        }

        if (modelBox.Items.Count > 0)
            modelBox.SelectedIndex = 0;
    }

    void Fill(Vehicle v)
    {
        SetText("Plaka", v.Plate);

        if (brandBox.Items.Contains(v.Brand))
        {
            brandBox.SelectedItem = v.Brand;
            LoadModels();
        }

        SetCombo(modelBox, v.Model);
        SetCombo(modelYearBox, v.ModelYear?.ToString() ?? "");
        SetCombo(typeBox, v.Type);
        SetCombo(fuelBox, v.Fuel);
        SetText("Yakıt Firması", v.FuelCompany);
        SetText("Şase No", v.ChassisNo);
        SetCombo(fleetBox, v.FleetCompany);
        SetText("Filo Vergi Dairesi", v.FleetTaxOffice);
        SetText("GPS ID", v.GpsId);
        SetText("AdBlue Kit No", v.AdBlueKitNo);
        SetCombo(tireSizeBox, v.TireSize);
        SetCombo(tireBrandBox, v.TireBrand);
        RefreshTireSizes();
        SetCombo(tireSizeBox, v.TireSize);
        SetText("Yıkama Kodları", v.WashCodes);
        SetDate("Vize/Muayene Tarihi", v.InspectionDate);
        SetDate("Takograf Muayene Tarihi", v.TachographInspectionDate);
        SetCombo(locationBox, v.Location);
        SetCombo(unitBox, v.Unit);
    }

    void SetText(string name, string value)
    {
        if (fields[name] is TextBox t)
            t.Text = value;
    }

    static void SetCombo(ComboBox cb, string value)
    {
        if (!string.IsNullOrWhiteSpace(value) && cb.Items.Contains(value))
            cb.SelectedItem = value;
    }

    void SetDate(string n, DateTime? d)
    {
        var p = (DateTimePicker)fields[n];
        p.Checked = d.HasValue;
        if (d.HasValue)
            p.Value = d.Value;
    }

    string GetText(string n) =>
        fields[n] is TextBox t ? t.Text : "";

    string GetCombo(ComboBox cb) => cb.SelectedItem?.ToString() ?? "";

    DateTime? GetDate(string n)
    {
        var p = (DateTimePicker)fields[n];
        return p.Checked ? p.Value.Date : null;
    }

    void Save(object? s, EventArgs e)
    {
        var plate = GetText("Plaka").Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(plate))
        {
            MessageBox.Show("Plaka zorunludur.");
            return;
        }

        if (Database.PlateExists(plate, existing?.Id ?? 0))
        {
            MessageBox.Show("Bu plakaya ait araç zaten kayıtlı.");
            return;
        }

        var v = existing ?? new Vehicle();

        v.Plate = plate;
        v.Brand = GetCombo(brandBox);
        v.Model = GetCombo(modelBox);
        v.ModelYear = int.TryParse(GetCombo(modelYearBox), out var modelYear) ? modelYear : null;
        v.Type = GetCombo(typeBox);
        v.Fuel = GetCombo(fuelBox);
        v.FuelCompany = GetText("Yakıt Firması");
        v.ChassisNo = GetText("Şase No");
        v.FleetCompany = GetCombo(fleetBox);
        v.FleetTaxOffice = GetText("Filo Vergi Dairesi");
        v.GpsId = GetText("GPS ID");
        v.AdBlueKitNo = GetText("AdBlue Kit No");
        v.TireSize = GetCombo(tireSizeBox);
        v.TireBrand = GetCombo(tireBrandBox);
        v.WashCodes = GetText("Yıkama Kodları");
        v.InspectionDate = GetDate("Vize/Muayene Tarihi");
        v.TachographInspectionDate = GetDate("Takograf Muayene Tarihi");
        v.Location = GetCombo(locationBox);
        v.Unit = GetCombo(unitBox);

        try
        {
            if (existing == null) Database.Save(v);
            else Database.Update(v);

            refresh();
            MessageBox.Show("Kayıt başarıyla kaydedildi.");
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Kayıt sırasında hata oluştu:\n" + ex.Message,
                "Hata",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
