using ClassDataLand;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Web.WebView2.Core;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly RazorLightEngine engine;
        private readonly string basePath;
        private string cnn = "Server=45.120.229.42\\VILISHauGiang;Database=LIS;User Id=sa;Password=HauGiang@1236$;MultipleActiveResultSets=True;";

        public Form1()
        {
            InitializeComponent();

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Views");
            basePath = Path.GetFullPath(basePath);

            engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(basePath)
                .UseMemoryCachingProvider()
                .Build();
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await web_1.EnsureCoreWebView2Async(null);

            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Views", "index.html");

            if (File.Exists(htmlPath))
            {
                web_1.CoreWebView2.NavigateToString(File.ReadAllText(htmlPath));

                web_1.NavigationCompleted += WebView2_NavigationCompleted;
            }
            else
            {
                MessageBox.Show(htmlPath);
            }
        }
        private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                string id = txt_areaID.Text;
                get_data(id);
            }
        }

        //Gui data qua web view
        private void SendDataToWebView()
        {
            var dataLand = new DataLand
            {
                Id = "AA 0987654",
                Muc1 = new Dictionary<int, Person>
                        {
                            { 1, new Person { PersonName = "Nguyễn Văn A", PersonCCCD = "231414141" } },
                            { 2, new Person { PersonName = "Nguyễn Văn B", PersonCCCD = "231414142" } },
                            { 3, new Person { PersonName = "Nguyễn Văn C", PersonCCCD = "231414143" } }
                        },
                Muc2 = new Dictionary<int, Land>
                        {
                            { 1, new Land { LandNumber = "01", LandMapNumber = "100m2", LandArea = "100", LandClass = "abc", LandUseDate = "abc", LandUse = "abc", LandAddress = "abc", LandPurpose = "abc" } },
                            { 2, new Land { LandNumber = "02", LandMapNumber = "200m2", LandArea = "100", LandClass = "abc", LandUseDate = "abc", LandUse = "abc", LandAddress = "abc", LandPurpose = "abc" } }
                        },
                Muc3 = new Dictionary<int, Asset>
                        {
                            { 1, new Asset { AssetName = "01", AssetArea = "100m2", AssetAreaUse = "01", AssetNumberFloor = "100m2", AssetStructure = "01", AssetLevel = "100m2", AssetUse = "01", AssetUseTime = "100m2", AssetAddress = "abc" } },
                            { 2, new Asset { AssetName = "02", AssetArea = "200m2", AssetAreaUse = "02", AssetNumberFloor = "200m2", AssetStructure = "02", AssetLevel = "200m2", AssetUse = "02", AssetUseTime = "200m2", AssetAddress = "abc" } }
                        },
                Muc4 = new Dictionary<int, string>
                        {
                            { 0, "https://d1hjkbq40fs2x4.cloudfront.net/2017-08-21/files/landscape-photography_1645-t.jpg" }
                        },
                IsCheckMuc2 = false,
                IsCheckMuc3 = false
            };

            //convert data to json
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dataLand);

            web_1.CoreWebView2.PostWebMessageAsJson(json);
        }

        private void get_data(string id)
        {
            using (SqlConnection conn = new SqlConnection(cnn))
            {
                conn.Open();
                string query = "SELECT giayChungNhanId,soHieuGiayChungNhan FROM dbo.GiayChungNhan WHERE giayChungNhanId = @Id";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    bool checkMuc2 = false;
                    bool checkMuc3 = false;
                    bool checkMuc1 = false;
                    var dataLand = new DataLand
                    {
                        Id = "",
                        Muc1 = new Dictionary<int, Person>(),
                        Muc2 = new Dictionary<int, Land>(),
                        Muc3 = new Dictionary<int, Asset>(),
                        Muc4 = new Dictionary<int, string>(),
                        IsCheckMuc2 = checkMuc2,
                        IsCheckMuc3 = checkMuc3,
                        IsCheckMuc1 = checkMuc1
                    };

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string Id = reader["giayChungNhanId"].ToString();

                            dataLand.Id = reader["soHieuGiayChungNhan"].ToString();

                            //Lay thong tin nguoi
                            string query_per = "SELECT nguoiId FROM dbo.NguoiPhapLy WHERE giayChungNhanId = @Id";
                            using (SqlCommand command_per = new SqlCommand(query_per, conn))
                            {
                                command_per.Parameters.AddWithValue("@Id", id);
                                using (SqlDataReader reader_1 = command_per.ExecuteReader())
                                {
                                    int personCount = 1;
                                    while (reader_1.Read())
                                    {
                                        string personName = "";
                                        string personCCCD = "";
                                        string personQT = "VIET NAM";
                                        string currentNguoiId = reader_1["nguoiId"].ToString();
                                        string hoGD = "";
                                        string loaiDT = "";
                                        string hoten2 = "";

                                        using (SqlCommand command_name = new SqlCommand("SELECT hoGiaDinh,loaiDoiTuongId,hoTen2 FROM dbo.Nguoi WHERE nguoiId = @Id", conn))
                                        {
                                            command_name.Parameters.AddWithValue("@Id", currentNguoiId);
                                            using (SqlDataReader reader_name = command_name.ExecuteReader())
                                            {
                                                if (reader_name.Read())
                                                {
                                                    hoGD = reader_name["hoGiaDinh"].ToString();
                                                    loaiDT = reader_name["loaiDoiTuongId"].ToString();
                                                    hoten2 = reader_name["hoTen2"].ToString();
                                                }
                                            }
                                        }

                                        
                                        List<string> listDT = new List<string> { "1", "8", "15", "18", "19", "21", "22", "23"};
                                        bool CList = listDT.Contains(loaiDT);
                                        lb_check1.Text = loaiDT;

                                        //ca nhan
                                        if (CList)
                                        {
                                            checkMuc1 = true;
                                            if (hoten2 == "")
                                            {
                                                //lay ra ho ten
                                                using (SqlCommand command_name = new SqlCommand("SELECT hoTen,gioiTinh,quocTichID11,quocTichID12 FROM dbo.Nguoi WHERE nguoiId = @Id", conn))
                                                {
                                                    command_name.Parameters.AddWithValue("@Id", currentNguoiId);
                                                    using (SqlDataReader reader_name = command_name.ExecuteReader())
                                                    {
                                                        if (reader_name.Read())
                                                        {
                                                            string c = reader_name["gioiTinh"].ToString();
                                                            if (c == "0")
                                                            {
                                                                personName = "Bà " + reader_name["hoTen"].ToString();
                                                            }
                                                            else
                                                            {
                                                                personName = "Ông " + reader_name["hoTen"].ToString();
                                                            }

                                                        }
                                                        string qt1 = reader_name["quocTichID11"].ToString();
                                                        string qt2 = reader_name["quocTichID12"].ToString();

                                                        if (qt1 != "" && qt2 != "")
                                                        {
                                                            if (qt1 != "231" && qt2 == "231")
                                                            {
                                                                using (SqlCommand command_qt = new SqlCommand("SELECT tenQuocGia FROM dbo.QuocTich WHERE quocGiaId = @Id", conn))
                                                                {
                                                                    command_qt.Parameters.AddWithValue("@Id", qt1);
                                                                    using (SqlDataReader reader_qt = command_qt.ExecuteReader())
                                                                    {
                                                                        if (reader_qt.Read())
                                                                        {
                                                                            personQT = reader_qt["tenQuocGia"].ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            if (qt1 == "231" && qt2 != "231")
                                                            {
                                                                using (SqlCommand command_qt = new SqlCommand("SELECT tenQuocGia FROM dbo.QuocTich WHERE quocGiaId = @Id", conn))
                                                                {
                                                                    command_qt.Parameters.AddWithValue("@Id", qt2);
                                                                    using (SqlDataReader reader_qt = command_qt.ExecuteReader())
                                                                    {
                                                                        if (reader_qt.Read())
                                                                        {
                                                                            personQT = reader_qt["tenQuocGia"].ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }



                                                    }
                                                }

                                                //lay ra cccd,cc,cmnd
                                                using (SqlCommand command_cccd = new SqlCommand("SELECT soGiayTo,FORMAT(ngayCap, 'dd/MM/yyyy') AS ngayCap_f,noiCap,loaiGiayTo FROM dbo.ThongTinBoSung WHERE nguoiId = @Id and laThongTinChinh = 1", conn))
                                                {
                                                    command_cccd.Parameters.AddWithValue("@Id", currentNguoiId);
                                                    using (SqlDataReader reader_cccd = command_cccd.ExecuteReader())
                                                    {
                                                        if (reader_cccd.Read())
                                                        {
                                                            if (reader_cccd["loaiGiayTo"].ToString() == "CCCD")
                                                            {
                                                                string ngaycap = reader_cccd["ngayCap_f"].ToString();
                                                                string noicap = reader_cccd["noiCap"].ToString();
                                                                personCCCD = "CCCD: " + reader_cccd["soGiayTo"].ToString() + ",cấp ngày: " + ngaycap + ",nơi cấp: " + noicap;
                                                            }
                                                            if (reader_cccd["loaiGiayTo"].ToString() == "CC")
                                                            {
                                                                string ngaycap = reader_cccd["ngayCap_f"].ToString();
                                                                string noicap = reader_cccd["noiCap"].ToString();
                                                                personCCCD = "CC: " + reader_cccd["soGiayTo"].ToString() + ",cấp ngày: " + ngaycap + ",nơi cấp: " + noicap;
                                                            }
                                                            if (reader_cccd["loaiGiayTo"].ToString() == "CMND")
                                                            {
                                                                string ngaycap = reader_cccd["ngayCap_f"].ToString();
                                                                string noicap = reader_cccd["noiCap"].ToString();
                                                                personCCCD = "CMND: " + reader_cccd["soGiayTo"].ToString() + ",cấp ngày: " + ngaycap + ",nơi cấp: " + noicap;
                                                            }
                                                        }
                                                    }
                                                }

                                                dataLand.Muc1.Add(personCount, new Person
                                                {
                                                    PersonName = personName,
                                                    PersonCCCD = personCCCD,
                                                    PersonQT = personQT
                                                });
                                            }
                                            else
                                            {
                                                //lay ho ten vo chong
                                                using (SqlCommand command_name = new SqlCommand("SELECT hoTen,gioiTinh,hoTen2,quocTichID11,quocTichID12,quocTichID21,quocTichID22 FROM dbo.Nguoi WHERE nguoiId = @Id", conn))
                                                {
                                                    command_name.Parameters.AddWithValue("@Id", currentNguoiId);
                                                    using (SqlDataReader reader_name = command_name.ExecuteReader())
                                                    {
                                                        if (reader_name.Read())
                                                        {
                                                            string c = reader_name["gioiTinh"].ToString();
                                                            if (c == "0")
                                                            {
                                                                personName = "Bà " + reader_name["hoTen"].ToString();
                                                                dataLand.Muc1.Add(1, new Person
                                                                {
                                                                    PersonName = personName,
                                                                });

                                                                personName = "Chồng " + reader_name["hoTen2"].ToString();
                                                                dataLand.Muc1.Add(2, new Person
                                                                {
                                                                    PersonName = personName,
                                                                });

                                                            }
                                                            else
                                                            {
                                                                personName = "Ông " + reader_name["hoTen"].ToString();
                                                                dataLand.Muc1.Add(1, new Person
                                                                {
                                                                    PersonName = personName,
                                                                });

                                                                personName = "Vợ " + reader_name["hoTen2"].ToString();
                                                                dataLand.Muc1.Add(2, new Person
                                                                {
                                                                    PersonName = personName,
                                                                });
                                                            }
                                                            //quoc tich cua nguoi 1
                                                            string qt11 = reader_name["quocTichID11"].ToString();
                                                            string qt12 = reader_name["quocTichID12"].ToString();

                                                            if (qt11 != "231" && qt12 == "231")
                                                            {
                                                                using (SqlCommand command_qt = new SqlCommand("SELECT tenQuocGia FROM dbo.QuocTich WHERE quocTichId = @Id", conn))
                                                                {
                                                                    command_qt.Parameters.AddWithValue("@Id", qt11);
                                                                    using (SqlDataReader reader_qt = command_qt.ExecuteReader())
                                                                    {
                                                                        if (reader_qt.Read())
                                                                        {
                                                                            personQT = reader_qt["tenQuocGia"].ToString();
                                                                            dataLand.Muc1[1].PersonQT = personQT;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            if (qt11 == "231" && qt12 != "231")
                                                            {
                                                                using (SqlCommand command_qt = new SqlCommand("SELECT tenQuocGia FROM dbo.QuocTich WHERE quocTichId = @Id", conn))
                                                                {
                                                                    command_qt.Parameters.AddWithValue("@Id", qt12);
                                                                    using (SqlDataReader reader_qt = command_qt.ExecuteReader())
                                                                    {
                                                                        if (reader_qt.Read())
                                                                        {
                                                                            personQT = reader_qt["tenQuocGia"].ToString();
                                                                            dataLand.Muc1[1].PersonQT = personQT;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            //quoc tich cua nguoi 2
                                                            string qt21 = reader_name["quocTichID21"].ToString();
                                                            string qt22 = reader_name["quocTichID22"].ToString();

                                                            if (qt21 != "231" && qt22 == "231")
                                                            {
                                                                using (SqlCommand command_qt = new SqlCommand("SELECT tenQuocGia FROM dbo.QuocTich WHERE quocTichId = @Id", conn))
                                                                {
                                                                    command_qt.Parameters.AddWithValue("@Id", qt21);
                                                                    using (SqlDataReader reader_qt = command_qt.ExecuteReader())
                                                                    {
                                                                        if (reader_qt.Read())
                                                                        {
                                                                            personQT = reader_qt["tenQuocGia"].ToString();
                                                                            dataLand.Muc1[2].PersonQT = personQT;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            if (qt21 == "231" && qt22 != "231")
                                                            {
                                                                using (SqlCommand command_qt = new SqlCommand("SELECT tenQuocGia FROM dbo.QuocTich WHERE quocTichId = @Id", conn))
                                                                {
                                                                    command_qt.Parameters.AddWithValue("@Id", qt22);
                                                                    using (SqlDataReader reader_qt = command_qt.ExecuteReader())
                                                                    {
                                                                        if (reader_qt.Read())
                                                                        {
                                                                            personQT = reader_qt["tenQuocGia"].ToString();
                                                                            dataLand.Muc1[2].PersonQT = personQT;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //lay ra cccd,cc,cmnd
                                                using (SqlCommand command_cccd = new SqlCommand("SELECT soGiayTo,FORMAT(ngayCap, 'dd/MM/yyyy') AS ngayCap_f,noiCap,loaiGiayTo FROM dbo.ThongTinBoSung WHERE nguoiId = @Id and laThongTinChinh = 1", conn))
                                                {
                                                    command_cccd.Parameters.AddWithValue("@Id", currentNguoiId);
                                                    using (SqlDataReader reader_cccd = command_cccd.ExecuteReader())
                                                    {
                                                        int p = 1;
                                                        while (reader_cccd.Read())
                                                        {
                                                            if (reader_cccd["loaiGiayTo"].ToString() == "CCCD")
                                                            {
                                                                string ngaycap = reader_cccd["ngayCap_f"].ToString();
                                                                string noicap = reader_cccd["noiCap"].ToString();
                                                                personCCCD = "CCCD: " + reader_cccd["soGiayTo"].ToString() + ",cấp ngày: " + ngaycap + ",nơi cấp: " + noicap;
                                                            }
                                                            if (reader_cccd["loaiGiayTo"].ToString() == "CC")
                                                            {
                                                                string ngaycap = reader_cccd["ngayCap_f"].ToString();
                                                                string noicap = reader_cccd["noiCap"].ToString();
                                                                personCCCD = "CC: " + reader_cccd["soGiayTo"].ToString() + ",cấp ngày: " + ngaycap + ",nơi cấp: " + noicap;
                                                            }
                                                            if (reader_cccd["loaiGiayTo"].ToString() == "CMND")
                                                            {
                                                                string ngaycap = reader_cccd["ngayCap_f"].ToString();
                                                                string noicap = reader_cccd["noiCap"].ToString();
                                                                personCCCD = "CMND: " + reader_cccd["soGiayTo"].ToString() + ",cấp ngày: " + ngaycap + ",nơi cấp: " + noicap;
                                                            }
                                                            dataLand.Muc1[p].PersonCCCD = personCCCD;

                                                            p++;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        //to chuc
                                        else
                                        {
                                            checkMuc1 = false;

                                            using (SqlCommand command_1 = new SqlCommand("SELECT hoTen FROM dbo.Nguoi WHERE nguoiId = @Id", conn))
                                            {
                                                command_1.Parameters.AddWithValue("@Id", currentNguoiId);
                                                using (SqlDataReader reader_11 = command_1.ExecuteReader())
                                                {
                                                    if (reader_11.Read())
                                                    {
                                                        personName = reader_11["hoTen"].ToString();
                                                    }
                                                }
                                            }

                                            using (SqlCommand command_2 = new SqlCommand("SELECT soGiayTo,FORMAT(ngayCap, 'dd/MM/yyyy') AS ngayCap_f,noiCap,loaiGiayTo FROM dbo.ThongTinBoSung WHERE nguoiId = @Id and laThongTinCuaChuThe = 1 and laThongTinChinh = 1", conn))
                                            {
                                                command_2.Parameters.AddWithValue("@Id", currentNguoiId);
                                                using (SqlDataReader reader_12 = command_2.ExecuteReader())
                                                {
                                                    if (reader_12.Read())
                                                    {
                                                        string ngaycap = reader_12["ngayCap_f"].ToString();
                                                        string noicap = reader_12["noiCap"].ToString();
                                                        personCCCD = "Số giấy tờ: " + reader_12["soGiayTo"].ToString() + ",cấp ngày: " + ngaycap + ",nơi cấp: " + noicap;
                                                    }
                                                }
                                            }

                                            dataLand.Muc1.Add(personCount, new Person
                                            {
                                                PersonName = personName,
                                                PersonCCCD = personCCCD,
                                                PersonQT = personQT
                                            });
                                        }
                                    }

                                }
                            }

                            //Lay thong tin dat
                            string query_land = "SELECT thuaDatId,dongSuDung FROM dbo.DangKyThua WHERE giayChungNhanId = @Id";
                            using (SqlCommand command_land = new SqlCommand(query_land, conn))
                            {
                                command_land.Parameters.AddWithValue("@Id", id);
                                using (SqlDataReader reader_21 = command_land.ExecuteReader())
                                {
                                    int landCount = 1;
                                    if (reader_21.HasRows)
                                    {
                                        List<int> results = new List<int>();
                                        while (reader_21.Read())
                                        {
                                            string landid = reader_21["thuaDatId"].ToString();
                                            int check = 0;
                                            int.TryParse(reader_21["dongSuDung"].ToString(), out check);
                                            results.Add(check);
                                        }
                                        int countLand = results.Count;
                                        string thuaDatId = reader_21["thuaDatId"].ToString();
                                        string landNumber = "";
                                        string landMapNumber = "";
                                        string landArea = "";
                                        string landClass = "";
                                        string landUseDate = "";
                                        string landUse = "";
                                        string landAddress = "";
                                        string landPurpose = "";



                                        //truong hop nhieu thua dat
                                        if (countLand > 1)
                                        {
                                            checkMuc2 = true;
                                            foreach (var items in results)
                                            {
                                                //lay ra nguon goc su dung
                                                using (SqlCommand command_land_info_3 = new SqlCommand("SELECT loaiNguonGocSuDungDatId FROM dbo.NguonGocGiaoDat WHERE thuaDatId = @Id", conn))
                                                {
                                                    command_land_info_3.Parameters.AddWithValue("@Id", thuaDatId);
                                                    using (SqlDataReader reader_land_info_3 = command_land_info_3.ExecuteReader())
                                                    {
                                                        if (reader_land_info_3.Read())
                                                        {
                                                            string md_id = reader_land_info_3["loaiNguonGocSuDungDatId"].ToString();
                                                            using (SqlCommand command_land_info_4 = new SqlCommand("SELECT tenNguonGocSuDungDat FROM dbo.LoaiNguonGocSuDungDat WHERE loaiNguonGocSuDungDatId = @Id", conn))
                                                            {
                                                                command_land_info_4.Parameters.AddWithValue("@Id", md_id);
                                                                using (SqlDataReader reader_land_info_4 = command_land_info_4.ExecuteReader())
                                                                {
                                                                    if (reader_land_info_4.Read())
                                                                    {
                                                                        landPurpose = reader_land_info_4["tenNguonGocSuDungDat"].ToString();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //lay ra hinh thuc su dung
                                                string check_land = reader_21["dongSuDung"].ToString();
                                                if (check_land == "1")
                                                {
                                                    landUse = "Sử dụng chung";
                                                }
                                                else
                                                {
                                                    landUse = "Sử dụng riêng";
                                                }

                                                //lay ra so thu tu thua, so hieu ban do, dien tich, thoi han su dung
                                                using (SqlCommand command_land_info = new SqlCommand("SELECT soThuTuThua,soHieuToBanDo,dienTich,thoiHanSuDung FROM dbo.DaMucDichSuDung WHERE thuaDatId = @Id", conn))
                                                {
                                                    command_land_info.Parameters.AddWithValue("@Id", thuaDatId);
                                                    using (SqlDataReader reader_land_info = command_land_info.ExecuteReader())
                                                    {
                                                        if (reader_land_info.Read())
                                                        {
                                                            landNumber = reader_land_info["soThuTuThua"].ToString();
                                                            landMapNumber = reader_land_info["soHieuToBanDo"].ToString();
                                                            landArea = reader_land_info["dienTich"].ToString();
                                                            landUseDate = reader_land_info["thoiHanSuDung"].ToString();
                                                        }
                                                    }
                                                }

                                                //lay ra dia chi, loai dat
                                                using (SqlCommand command_land_info_2 = new SqlCommand("SELECT diaChi,khuDanCu,datDoThi FROM dbo.ThuaDat WHERE thuaDatId = @Id", conn))
                                                {
                                                    command_land_info_2.Parameters.AddWithValue("@Id", thuaDatId);
                                                    using (SqlDataReader reader_land_info_2 = command_land_info_2.ExecuteReader())
                                                    {
                                                        if (reader_land_info_2.Read())
                                                        {
                                                            landAddress = reader_land_info_2["diaChi"].ToString();
                                                            string check_dancu = reader_land_info_2["khuDanCu"].ToString();
                                                            string check_dothi = reader_land_info_2["datDoThi"].ToString();
                                                            if (check_dancu == "1" && check_dothi == "0")
                                                            {
                                                                landClass = "Khu dân cư";
                                                            }
                                                            else if (check_dancu == "0" && check_dothi == "1")
                                                            {
                                                                landClass = "Đất đô thị";
                                                            }
                                                            else
                                                            {
                                                                landClass = "Khu dân cư và Đất đô thị";
                                                            }
                                                        }
                                                    }
                                                }

                                                dataLand.Muc2.Add(landCount, new Land
                                                {
                                                    LandNumber = landNumber,
                                                    LandMapNumber = landMapNumber,
                                                    LandArea = landArea,
                                                    LandClass = landClass,
                                                    LandUseDate = landUseDate,
                                                    LandUse = landUse,
                                                    LandAddress = landAddress,
                                                    LandPurpose = landPurpose
                                                });
                                                dataLand.IsCheckMuc2 = checkMuc2;
                                                landCount++;
                                            }
                                        }
                                        //truong hop 1 thua dat
                                        else
                                        {
                                            landCount = 1;
                                            checkMuc2 = false;

                                            //1 thua dat nhieu muc dich su dung
                                            if (landCount > 1)
                                            {
                                                string query_loai = "WITH NumberRanges AS (SELECT @Id AS StartNumber,(SELECT MIN(thuaDatId) FROM dbo.NguonGocGiaoDat WHERE thuaDatId > @Id) AS NextNumber) SELECT daMucDichSuDungId,loaiNguonGocSuDungDatId FROM dbo.NguonGocGiaoDat, NumberRanges WHERE thuaDatId IS NULL AND thuaDatId BETWEEN StartNumber AND NextNumber";
                                                using (SqlCommand command_loai = new SqlCommand(query_loai, conn))
                                                {
                                                    command_loai.Parameters.AddWithValue("@Id", thuaDatId);
                                                    using (SqlDataReader reader_loai = command_loai.ExecuteReader())
                                                    {
                                                        List<string> results_1 = new List<string>();
                                                        string daMd = "";
                                                        if (reader_loai.Read())
                                                        {
                                                            string check_1 = reader_loai["loaiNguonGocSuDungDatId"].ToString();
                                                            daMd = reader_loai["daMucDichSuDungId"].ToString();

                                                            results_1.Add(check_1);
                                                        }

                                                        List<string> sameValues = new List<string>();
                                                        List<string> uniqueValues = new List<string>();

                                                        var groupedValues = results_1.GroupBy(x => x)
                                                                                  .Select(g => new { Value = g.Key, Count = g.Count() });

                                                        foreach (var item in groupedValues)
                                                        {
                                                            if (item.Count > 1)
                                                            {
                                                                sameValues.Add(item.Value);
                                                            }
                                                            else
                                                            {
                                                                uniqueValues.Add(item.Value);
                                                            }
                                                        }

                                                        bool allSame = results.All(x => x == results[0]);

                                                        //tat ca loai nguon goc trung nhau
                                                        if (allSame)
                                                        {
                                                            foreach (var item in results_1)
                                                            {
                                                                string loaiNg = reader_loai["loaiNguonGocSuDungDatId"].ToString();

                                                                using (SqlCommand command_ten = new SqlCommand("SELECT tenNguonGocSuDungDat FROM dbo.LoaiNguonGocSuDungDat WHERE loaiNguonGocSuDungDatId = @Id", conn))
                                                                {
                                                                    command_loai.Parameters.AddWithValue("@Id", loaiNg);
                                                                    using (SqlDataReader reader_ten = command_loai.ExecuteReader())
                                                                    {
                                                                        if (reader_ten.Read())
                                                                        {
                                                                            if (item == loaiNg)
                                                                            {
                                                                                landPurpose = reader_ten["tenNguonGocSuDungDat"].ToString() + ": ";
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                using (SqlCommand command_area = new SqlCommand("SELECT dienTich FROM dbo.DaMucDichSuDung WHERE daMucDichSuDungId = @Id", conn))
                                                                {
                                                                    command_area.Parameters.AddWithValue("@Id", daMd);
                                                                    using (SqlDataReader reader_area = command_loai.ExecuteReader())
                                                                    {
                                                                        if (reader_area.Read())
                                                                        {
                                                                            int area = 0;
                                                                            area = area + int.Parse(reader_area["dienTich"].ToString());
                                                                            landPurpose += area + " m2";
                                                                        }
                                                                    }
                                                                }

                                                                dataLand.Muc2.Add(0, new Land
                                                                {
                                                                    LandPurpose = landPurpose,
                                                                });
                                                                landPurpose = "";
                                                            }
                                                        }
                                                        //loai nguoc goc dat khong trung nhau
                                                        else
                                                        {
                                                            foreach (var value in sameValues)
                                                            {
                                                                string loaiNg = value;
                                                                using (SqlCommand command_ten = new SqlCommand("SELECT tenNguonGocSuDungDat FROM dbo.LoaiNguonGocSuDungDat WHERE loaiNguonGocSuDungDatId = @Id", conn))
                                                                {
                                                                    command_loai.Parameters.AddWithValue("@Id", loaiNg);
                                                                    using (SqlDataReader reader_ten = command_loai.ExecuteReader())
                                                                    {
                                                                        if (reader_ten.Read())
                                                                        {
                                                                            landPurpose = reader_ten["tenNguonGocSuDungDat"].ToString() + ": ";
                                                                        }
                                                                    }
                                                                }
                                                                using (SqlCommand command_area = new SqlCommand("SELECT dienTich FROM dbo.DaMucDichSuDung WHERE daMucDichSuDungId = @Id", conn))
                                                                {
                                                                    command_area.Parameters.AddWithValue("@Id", daMd);
                                                                    using (SqlDataReader reader_area = command_loai.ExecuteReader())
                                                                    {
                                                                        if (reader_area.Read())
                                                                        {
                                                                            int area = int.Parse(reader_area["dienTich"].ToString());
                                                                            area = area + int.Parse(reader_area["dienTich"].ToString());
                                                                            landPurpose += area + " m2,";
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            foreach (var value in uniqueValues)
                                                            {
                                                                string loaiNg = value;
                                                                using (SqlCommand command_ten = new SqlCommand("SELECT tenNguonGocSuDungDat FROM dbo.LoaiNguonGocSuDungDat WHERE loaiNguonGocSuDungDatId = @Id", conn))
                                                                {
                                                                    command_loai.Parameters.AddWithValue("@Id", loaiNg);
                                                                    using (SqlDataReader reader_ten = command_loai.ExecuteReader())
                                                                    {
                                                                        if (reader_ten.Read())
                                                                        {
                                                                            landPurpose += reader_ten["tenNguonGocSuDungDat"].ToString() + ": ";
                                                                        }
                                                                    }
                                                                }
                                                                using (SqlCommand command_area = new SqlCommand("SELECT dienTich FROM dbo.DaMucDichSuDung WHERE daMucDichSuDungId = @Id", conn))
                                                                {
                                                                    command_area.Parameters.AddWithValue("@Id", daMd);
                                                                    using (SqlDataReader reader_area = command_loai.ExecuteReader())
                                                                    {
                                                                        if (reader_area.Read())
                                                                        {
                                                                            int area = int.Parse(reader_area["dienTich"].ToString());
                                                                            landPurpose += area + " m2";
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            dataLand.Muc2.Add(0, new Land
                                                            {
                                                                LandPurpose = landPurpose,
                                                            });
                                                            landPurpose = "";
                                                        }

                                                        using (SqlCommand command_same = new SqlCommand("SELECT dienTich,suDungChung FROM dbo.DaMucDichSuDung WHERE daMucDichSuDungId = @Id", conn))
                                                        {
                                                            command_same.Parameters.AddWithValue("@Id", daMd);
                                                            using (SqlDataReader reader_same = command_same.ExecuteReader())
                                                            {
                                                                while (reader_same.Read())
                                                                {
                                                                    string check_same = reader_same["suDungChung"].ToString();
                                                                    if (check_same == "1")
                                                                    {
                                                                        int area = int.Parse(reader_same["dienTich"].ToString());
                                                                        area = area + int.Parse(reader_same["dienTich"].ToString());
                                                                        landUse = "Sử dụng chung : " + area + " m2";
                                                                    }
                                                                    else
                                                                    {
                                                                        int area = int.Parse(reader_same["dienTich"].ToString());
                                                                        area = area + int.Parse(reader_same["dienTich"].ToString());
                                                                        landUse = "Sử dụng riêng : " + area + " m2";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            //1 thua dat 1 muc dich su dung
                                            else
                                            {
                                                //lay ra nguon goc su dung
                                                using (SqlCommand command_land_info_3 = new SqlCommand("SELECT loaiNguonGocSuDungDatId FROM dbo.NguonGocGiaoDat WHERE thuaDatId = @Id", conn))
                                                {
                                                    command_land_info_3.Parameters.AddWithValue("@Id", thuaDatId);
                                                    using (SqlDataReader reader_land_info_3 = command_land_info_3.ExecuteReader())
                                                    {
                                                        if (reader_land_info_3.Read())
                                                        {
                                                            string md_id = reader_land_info_3["loaiNguonGocSuDungDatId"].ToString();
                                                            using (SqlCommand command_land_info_4 = new SqlCommand("SELECT tenNguonGocSuDungDat FROM dbo.LoaiNguonGocSuDungDat WHERE loaiNguonGocSuDungDatId = @Id", conn))
                                                            {
                                                                command_land_info_4.Parameters.AddWithValue("@Id", md_id);
                                                                using (SqlDataReader reader_land_info_4 = command_land_info_4.ExecuteReader())
                                                                {
                                                                    if (reader_land_info_4.Read())
                                                                    {
                                                                        landPurpose = reader_land_info_4["tenNguonGocSuDungDat"].ToString();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //lay ra hinh thuc su dung
                                                string check_land = reader_21["dongSuDung"].ToString();
                                                if (check_land == "1")
                                                {
                                                    landUse = "Sử dụng chung";
                                                }
                                                else
                                                {
                                                    landUse = "Sử dụng riêng";
                                                }
                                            }


                                        }
                                        dataLand.Muc2.Add(landCount, new Land
                                        {
                                            LandNumber = landNumber,
                                            LandMapNumber = landMapNumber,
                                            LandArea = landArea,
                                            LandClass = landClass,
                                            LandUseDate = landUseDate,
                                            LandUse = landUse,
                                            LandAddress = landAddress,
                                            LandPurpose = landPurpose
                                        });
                                        dataLand.IsCheckMuc2 = checkMuc2;
                                        landCount++;

                                    }
                                }
                            }

                            //Lay thong tin tai san
                            string query_asset = "SELECT congTrinhXayDungId,dongSuDung,hangMucCongTrinhXayDungId FROM dbo.DangKyHangMucCongTrinh  WHERE giayChungNhanId = @Id";
                            using (SqlCommand command_asset = new SqlCommand(query_asset, conn))
                            {
                                command_asset.Parameters.AddWithValue("@Id", id);
                                using (SqlDataReader reader_3 = command_asset.ExecuteReader())
                                {
                                    //tai san la cong trinh
                                    if (reader_3.HasRows)
                                    {
                                        dataLand.IsCheckMuc3 = true;
                                        int assetCount = 1;
                                        while (reader_3.Read())
                                        {
                                            string assetId = reader_3["congTrinhXayDungId"].ToString();
                                            string check_su_dung = reader_3["dongSuDung"].ToString();
                                            string hangMucId = reader_3["hangMucCongTrinhXayDungId"].ToString();
                                            string assetName = "";
                                            string assetArea = "";
                                            string assetAreaUse = "";
                                            string assetNumberFloor = "";
                                            string assetStructure = "";
                                            string assetLevel = "";
                                            string assetUse = "";
                                            string assetUseTime = "";
                                            string assetAddress = "";

                                            //neu co nhieu hang muc liet ke tu 2 hang muc tro len
                                            if (assetCount > 1)
                                            {
                                                //lay ten hang muc, dien tich san, dien tich xay dung, thoi han so huu
                                                string query_2 = "SELECT tenHangMuc,dienTichXayDung,dienTichSan,thoiHanSoHuu,soTang FROM dbo.HangMucCongTrinhXayDung WHERE hangMucCongTrinhXayDungId = @Id";
                                                using (SqlCommand command_2 = new SqlCommand(query_2, conn))
                                                {
                                                    command_2.Parameters.AddWithValue("@Id", hangMucId);
                                                    using (SqlDataReader reader_5 = command_2.ExecuteReader())
                                                    {
                                                        if (reader_5.Read())
                                                        {
                                                            assetName = reader_5["tenHangMuc"].ToString();
                                                            assetArea = reader_5["dienTichSan"].ToString();
                                                            assetAreaUse = reader_5["dienTichXayDung"].ToString();
                                                            assetUseTime = reader_5["thoiHanSoHuu"].ToString();
                                                            assetNumberFloor = reader_5["soTang"].ToString();
                                                        }
                                                    }
                                                }

                                                //lay cap cong trinh
                                                string query_3 = "SELECT loaiCapCongTrinhId FROM dbo.HangMucCongTrinhXayDung WHERE congTrinhXayDungId = @Id";
                                                using (SqlCommand command_3 = new SqlCommand(query_3, conn))
                                                {
                                                    command_3.Parameters.AddWithValue("@Id", assetId);
                                                    using (SqlDataReader reader_6 = command_3.ExecuteReader())
                                                    {
                                                        if (reader_6.Read())
                                                        {
                                                            string ccc_id = reader_6["loaiCapCongTrinhId"].ToString();
                                                            using (SqlCommand command_4 = new SqlCommand("SELECT maCapCongTrinh FROM dbo.LoaiCapCongTrinh WHERE loaiCapCongTrinhId = @Id", conn))
                                                            {
                                                                command_4.Parameters.AddWithValue("@Id", ccc_id);
                                                                using (SqlDataReader reader_7 = command_4.ExecuteReader())
                                                                {
                                                                    if (reader_7.Read())
                                                                    {
                                                                        assetLevel = reader_7["maCapCongTrinh"].ToString();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //lay ra hinh thuc su dung
                                                string query_4 = "SELECT dongSuDung FROM dbo.DangKyCongTrinh WHERE giayChungNhanId = @Id";
                                                using (SqlCommand command_4 = new SqlCommand(query_4, conn))
                                                {
                                                    command_4.Parameters.AddWithValue("@Id", id);
                                                    using (SqlDataReader reader_8 = command_4.ExecuteReader())
                                                    {
                                                        if (reader_8.Read())
                                                        {
                                                            assetUse = reader_8["dongSuDung"].ToString();
                                                            if (assetUse == "1")
                                                            {
                                                                assetUse = "Sử dụng chung";
                                                            }
                                                            else
                                                            {
                                                                assetUse = "Sử dụng riêng";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //thong tin chung 
                                            else
                                            {
                                                //lay ten cong trinh, dia chi
                                                string query_1 = "SELECT tenCongTrinh,diaChi FROM dbo.CongTrinhXayDung  WHERE congTrinhXayDungId = @Id";
                                                using (SqlCommand command_1 = new SqlCommand(query_1, conn))
                                                {
                                                    command_1.Parameters.AddWithValue("@Id", assetId);
                                                    using (SqlDataReader reader_4 = command_1.ExecuteReader())
                                                    {
                                                        if (reader_4.Read())
                                                        {
                                                            assetName = reader_4["tenCongTrinh"].ToString();
                                                            assetAddress = reader_4["diaChi"].ToString();
                                                            if (check_su_dung == "1")
                                                            {
                                                                assetUse = "Sử dụng riêng";
                                                            }
                                                            else
                                                            {
                                                                assetUse = "Sử dụng chung";
                                                            }
                                                        }
                                                    }
                                                }

                                                //lay ten hang muc, dien tich san, dien tich xay dung, thoi han so huu
                                                string query_2 = "SELECT tenHangMuc,dienTichXayDung,dienTichSan,thoiHanSoHuu,soTang FROM dbo.HangMucCongTrinhXayDung WHERE hangMucCongTrinhXayDungId = @Id";
                                                using (SqlCommand command_2 = new SqlCommand(query_2, conn))
                                                {
                                                    command_2.Parameters.AddWithValue("@Id", hangMucId);
                                                    using (SqlDataReader reader_5 = command_2.ExecuteReader())
                                                    {
                                                        if (reader_5.Read())
                                                        {
                                                            assetName = reader_5["tenHangMuc"].ToString();
                                                            assetArea = reader_5["dienTichSan"].ToString();
                                                            assetAreaUse = reader_5["dienTichXayDung"].ToString();
                                                            assetUseTime = reader_5["thoiHanSoHuu"].ToString();
                                                            assetNumberFloor = reader_5["soTang"].ToString();
                                                        }
                                                    }
                                                }

                                                //lay cap cong trinh
                                                string query_3 = "SELECT loaiCapCongTrinhId FROM dbo.HangMucCongTrinhXayDung WHERE congTrinhXayDungId = @Id";
                                                using (SqlCommand command_3 = new SqlCommand(query_3, conn))
                                                {
                                                    command_3.Parameters.AddWithValue("@Id", assetId);
                                                    using (SqlDataReader reader_6 = command_3.ExecuteReader())
                                                    {
                                                        if (reader_6.Read())
                                                        {
                                                            string ccc_id = reader_6["loaiCapCongTrinhId"].ToString();
                                                            using (SqlCommand command_4 = new SqlCommand("SELECT maCapCongTrinh FROM dbo.LoaiCapCongTrinh WHERE loaiCapCongTrinhId = @Id", conn))
                                                            {
                                                                command_4.Parameters.AddWithValue("@Id", ccc_id);
                                                                using (SqlDataReader reader_7 = command_4.ExecuteReader())
                                                                {
                                                                    if (reader_7.Read())
                                                                    {
                                                                        assetLevel = reader_7["maCapCongTrinh"].ToString();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //lay ra hinh thuc su dung
                                                string query_4 = "SELECT dongSuDung FROM dbo.DangKyCongTrinh WHERE giayChungNhanId = @Id";
                                                using (SqlCommand command_4 = new SqlCommand(query_4, conn))
                                                {
                                                    command_4.Parameters.AddWithValue("@Id", id);
                                                    using (SqlDataReader reader_8 = command_4.ExecuteReader())
                                                    {
                                                        if (reader_8.Read())
                                                        {
                                                            assetUse = reader_8["dongSuDung"].ToString();
                                                            if (assetUse == "1")
                                                            {
                                                                assetUse = "Sử dụng chung";
                                                            }
                                                            else
                                                            {
                                                                assetUse = "Sử dụng riêng";
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            if (assetNumberFloor == "") assetNumberFloor = "1";
                                            if (assetUseTime == "") assetUseTime = "Chưa có thông tin";
                                            if (assetName == "") assetName = "Chưa có thông tin";
                                            dataLand.Muc3.Add(assetCount, new Asset
                                            {
                                                AssetName = assetName,
                                                AssetArea = assetArea,
                                                AssetAreaUse = assetAreaUse,
                                                AssetNumberFloor = assetNumberFloor,
                                                AssetStructure = assetStructure,
                                                AssetLevel = assetLevel,
                                                AssetUse = assetUse,
                                                AssetUseTime = assetUseTime,
                                                AssetAddress = assetAddress
                                            });
                                            assetCount++;
                                        }
                                    }
                                    //tai san la nha hoac can ho
                                    else
                                    {
                                        dataLand.IsCheckMuc3 = false;

                                        string query_home = "SELECT nhaId FROM dbo.DangKyHangMucNha  WHERE giayChungNhanId = @Id";
                                        using (SqlCommand command_home = new SqlCommand(query_home, conn))
                                        {
                                            command_home.Parameters.AddWithValue("@Id", id);
                                            using (SqlDataReader reader_9 = command_home.ExecuteReader())
                                            {
                                                //neu co hang muc nha, bang hang muc nha thieu dien tich xay dung
                                                if (reader_9.HasRows)
                                                {
                                                    int assetCount = 1;
                                                    while (reader_9.Read())
                                                    {
                                                        string assetName = "";
                                                        string assetArea = "";
                                                        string assetAreaUse = "";
                                                        string assetNumberFloor = "";
                                                        string assetStructure = "";
                                                        string assetLevel = "";
                                                        string assetUse = "";
                                                        string assetUseTime = "";
                                                        string assetAddress = "";
                                                        if (assetCount > 1)
                                                        {
                                                            string query_1 = "SELECT tenHangMucNha,dienTichSan,thoiHanSoHuu,soHuuChung FROM dbo.HangMucNha WHERE nhaId = @Id";
                                                            using (SqlCommand command1 = new SqlCommand(query_1, conn))
                                                            {
                                                                command1.Parameters.AddWithValue("@Id", reader_9["nhaId"].ToString());
                                                                using (SqlDataReader reader_10 = command1.ExecuteReader())
                                                                {
                                                                    while (reader_10.Read())
                                                                    {
                                                                        assetName = reader_10["tenHangMucNha"].ToString();
                                                                        assetArea = reader_10["dienTichSan"].ToString();
                                                                        assetUseTime = reader_10["thoiHanSoHuu"].ToString();
                                                                        if (reader_9["soHuuChung"] != "1")
                                                                        {
                                                                            assetUse = "Sử dụng chung";
                                                                        }
                                                                        else
                                                                        {
                                                                            assetUse = "Sử dụng riêng";
                                                                        }
                                                                        if (assetName == "") assetName = "Chưa có thông tin";
                                                                        dataLand.Muc3.Add(assetCount, new Asset
                                                                        {
                                                                            AssetName = assetName,
                                                                            AssetArea = assetArea,
                                                                            AssetAreaUse = assetAreaUse,
                                                                            AssetNumberFloor = assetNumberFloor,
                                                                            AssetStructure = assetStructure,
                                                                            AssetLevel = assetLevel,
                                                                            AssetUse = assetUse,
                                                                            AssetUseTime = assetUseTime,
                                                                            AssetAddress = assetAddress
                                                                        });
                                                                        assetCount++;
                                                                    }
                                                                }
                                                            }


                                                        }
                                                        else
                                                        {
                                                            string query_home_2 = "SELECT * FROM dbo.Nha  WHERE nhaId = @Id";
                                                            using (SqlCommand command_home_2 = new SqlCommand(query_home_2, conn))
                                                            {
                                                                string assetId = reader_9["nhaId"].ToString();
                                                                string chungcuId = "";
                                                                string loaiNhaId = "";

                                                                string query_chungcu = "SELECT chungCuId,loaiCapNhaId FROM dbo.Nha  WHERE nhaId = @Id";
                                                                using (SqlCommand command_chungcu = new SqlCommand(query_chungcu, conn))
                                                                {
                                                                    command_chungcu.Parameters.AddWithValue("@Id", assetId);
                                                                    using (SqlDataReader reader_11 = command_chungcu.ExecuteReader())
                                                                    {
                                                                        if (reader_11.Read())
                                                                        {
                                                                            chungcuId = reader_11["chungCuId"].ToString();
                                                                            loaiNhaId = reader_11["loaiCapNhaId"].ToString();
                                                                        }
                                                                    }
                                                                }

                                                                while (reader_9.Read())
                                                                {
                                                                    int.TryParse(reader_9["dongSuDung"].ToString(), out int check);

                                                                    //tai san la chung cu
                                                                    if (chungcuId != null)
                                                                    {
                                                                        assetCount = 1;
                                                                        string query_1 = "SELECT dienTichXayDung,dienTichSan,ketCauChiTiet,diaChi,soTang FROM dbo.ChungCu  WHERE chungCuId = @Id";
                                                                        using (SqlCommand command_1 = new SqlCommand(query_1, conn))
                                                                        {
                                                                            command_1.Parameters.AddWithValue("@Id", chungcuId);
                                                                            using (SqlDataReader reader_4 = command_1.ExecuteReader())
                                                                            {
                                                                                if (reader_4.Read())
                                                                                {
                                                                                    assetAddress = reader_4["diaChi"].ToString();
                                                                                    assetArea = reader_4["dienTichSan"].ToString();
                                                                                    assetAreaUse = reader_4["dienTichSuDung"].ToString();
                                                                                    assetStructure = reader_4["ketCauChiTiet"].ToString();
                                                                                    assetNumberFloor = reader_4["soTang"].ToString();
                                                                                }
                                                                            }
                                                                        }

                                                                        string query_2 = "SELECT kyHieuLoaiCapNha FROM dbo.LoaiCapNha WHERE loaiCapNhaID = @Id";
                                                                        using (SqlCommand command_2 = new SqlCommand(query_2, conn))
                                                                        {
                                                                            command_2.Parameters.AddWithValue("@Id", loaiNhaId);
                                                                            using (SqlDataReader reader_5 = command_2.ExecuteReader())
                                                                            {
                                                                                if (reader_5.Read())
                                                                                {
                                                                                    assetLevel = reader_5["kyHieuLoaiCapNha"].ToString();
                                                                                }
                                                                            }
                                                                        }

                                                                        if (check == 0)
                                                                        {
                                                                            assetUse = "Sử dụng chung";
                                                                        }
                                                                        else
                                                                        {
                                                                            assetUse = "Sử dụng riêng";
                                                                        }
                                                                    }

                                                                    //tai san la nha
                                                                    else
                                                                    {
                                                                        string query_1 = "SELECT dienTichXayDung,dienTichSan,ketCauChiTiet,diaChi,soTang FROM dbo.Nha  WHERE nhaId = @Id";
                                                                        using (SqlCommand command_1 = new SqlCommand(query_1, conn))
                                                                        {
                                                                            command_1.Parameters.AddWithValue("@Id", assetId);
                                                                            using (SqlDataReader reader_4 = command_1.ExecuteReader())
                                                                            {
                                                                                if (reader_4.Read())
                                                                                {
                                                                                    assetAddress = reader_4["diaChi"].ToString();
                                                                                    assetArea = reader_4["dienTichSan"].ToString();
                                                                                    assetAreaUse = reader_4["dienTichSuDung"].ToString();
                                                                                    assetStructure = reader_4["ketCauChiTiet"].ToString();
                                                                                    assetNumberFloor = reader_4["soTang"].ToString();
                                                                                }
                                                                            }
                                                                        }

                                                                        string query_2 = "SELECT kyHieuLoaiCapNha FROM dbo.LoaiCapNha WHERE loaiCapNhaID = @Id";
                                                                        using (SqlCommand command_2 = new SqlCommand(query_2, conn))
                                                                        {
                                                                            command_2.Parameters.AddWithValue("@Id", loaiNhaId);
                                                                            using (SqlDataReader reader_5 = command_2.ExecuteReader())
                                                                            {
                                                                                if (reader_5.Read())
                                                                                {
                                                                                    assetLevel = reader_5["kyHieuLoaiCapNha"].ToString();
                                                                                }
                                                                            }
                                                                        }

                                                                        if (check == 0)
                                                                        {
                                                                            assetUse = "Sử dụng chung";
                                                                        }
                                                                        else
                                                                        {
                                                                            assetUse = "Sử dụng riêng";
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (assetName == "") assetName = "Chưa có thông tin";

                                                        dataLand.Muc3.Add(assetCount, new Asset
                                                        {
                                                            AssetName = assetName,
                                                            AssetArea = assetArea,
                                                            AssetAreaUse = assetAreaUse,
                                                            AssetNumberFloor = assetNumberFloor,
                                                            AssetStructure = assetStructure,
                                                            AssetLevel = assetLevel,
                                                            AssetUse = assetUse,
                                                            AssetUseTime = assetUseTime,
                                                            AssetAddress = assetAddress
                                                        });
                                                        assetCount++;
                                                    }
                                                }
                                                //neu khong co hang muc nha
                                                else
                                                {
                                                    string query_home_1 = "SELECT nhaId,dongSuDung FROM dbo.DangKyNha  WHERE giayChungNhanId = @Id";
                                                    using (SqlCommand command_home_1 = new SqlCommand(query_home_1, conn))
                                                    {
                                                        command_home_1.Parameters.AddWithValue("@Id", id);
                                                        using (SqlDataReader reader_10 = command_home_1.ExecuteReader())
                                                        {
                                                            //co thong tin
                                                            if (reader_10.HasRows)
                                                            {
                                                                string assetId = "";
                                                                List<int> results = new List<int>();
                                                                while (reader_10.Read())
                                                                {
                                                                    assetId = reader_10["nhaId"].ToString();
                                                                    int check = 0;
                                                                    int.TryParse(reader_10["dongSuDung"].ToString(), out check);
                                                                    results.Add(check);
                                                                }
                                                                int assetCount = 1;
                                                                string chungcuId = "";
                                                                string loaiNhaId = "";

                                                                if (assetId != "")
                                                                {
                                                                    string query_chungcu = "SELECT chungCuId,loaiCapNhaId,loaiTaiSanId FROM dbo.Nha  WHERE nhaId = @Id";
                                                                    using (SqlCommand command_chungcu = new SqlCommand(query_chungcu, conn))
                                                                    {
                                                                        command_chungcu.Parameters.AddWithValue("@Id", assetId);
                                                                        using (SqlDataReader reader_11 = command_chungcu.ExecuteReader())
                                                                        {
                                                                            if (reader_11.Read())
                                                                            {
                                                                                chungcuId = reader_11["chungCuId"].ToString();
                                                                                loaiNhaId = reader_11["loaiCapNhaId"].ToString();
                                                                            }
                                                                        }
                                                                    }

                                                                    foreach (var item in results)
                                                                    {
                                                                        string assetName = "Chưa có thông tin";
                                                                        string assetArea = "0";
                                                                        string assetAreaUse = "0";
                                                                        string assetNumberFloor = "1";
                                                                        string assetStructure = "Chưa có thông tin";
                                                                        string assetLevel = "Chưa có thông tin";
                                                                        string assetUse = "Chưa có thông tin";
                                                                        string assetUseTime = "Chưa có thông tin";
                                                                        string assetAddress = "Chưa có thông tin";
                                                                        string taisanID = reader_10["loaiTaiSanId"].ToString();

                                                                        //tai san la chung cu
                                                                        if (chungcuId != "")
                                                                        {
                                                                            assetCount = 1;
                                                                            string query_1 = "SELECT dienTichXayDung,dienTichSan,ketCauChiTiet,diaChi,soTang FROM dbo.ChungCu  WHERE chungCuId = @Id";
                                                                            using (SqlCommand command_1 = new SqlCommand(query_1, conn))
                                                                            {
                                                                                command_1.Parameters.AddWithValue("@Id", chungcuId);
                                                                                using (SqlDataReader reader_4 = command_1.ExecuteReader())
                                                                                {
                                                                                    if (reader_4.Read())
                                                                                    {
                                                                                        assetAddress = reader_4["diaChi"].ToString();
                                                                                        assetArea = reader_4["dienTichSan"].ToString();
                                                                                        assetAreaUse = reader_4["dienTichSuDung"].ToString();
                                                                                        assetStructure = reader_4["ketCauChiTiet"].ToString();
                                                                                        assetNumberFloor = reader_4["soTang"].ToString();
                                                                                    }
                                                                                }
                                                                            }

                                                                            string query_2 = "SELECT kyHieuLoaiCapNha FROM dbo.LoaiCapNha WHERE loaiCapNhaID = @Id";
                                                                            using (SqlCommand command_2 = new SqlCommand(query_2, conn))
                                                                            {
                                                                                command_2.Parameters.AddWithValue("@Id", loaiNhaId);
                                                                                using (SqlDataReader reader_5 = command_2.ExecuteReader())
                                                                                {
                                                                                    if (reader_5.Read())
                                                                                    {
                                                                                        assetLevel = reader_5["kyHieuLoaiCapNha"].ToString();
                                                                                    }
                                                                                }
                                                                            }

                                                                            string query_3 = "SELECT tenLoaiTaiSan FROM dbo.LoaiTaiSan WHERE loaiTaiSanId = @Id";
                                                                            using (SqlCommand command_3 = new SqlCommand(query_3, conn))
                                                                            {
                                                                                command_3.Parameters.AddWithValue("@Id", taisanID);
                                                                                using (SqlDataReader reader_6 = command_3.ExecuteReader())
                                                                                {
                                                                                    if (reader_6.Read())
                                                                                    {
                                                                                        assetName = reader_6["tenLoaiTaiSan"].ToString();
                                                                                    }
                                                                                }
                                                                            }

                                                                            if (item == 0)
                                                                            {
                                                                                assetUse = "Sử dụng chung";
                                                                            }
                                                                            else
                                                                            {
                                                                                assetUse = "Sử dụng riêng";
                                                                            }

                                                                            //thieu ten tai san
                                                                            dataLand.Muc3.Add(assetCount, new Asset
                                                                            {
                                                                                AssetName = assetName,
                                                                                AssetArea = assetArea,
                                                                                AssetAreaUse = assetAreaUse,
                                                                                AssetNumberFloor = assetNumberFloor,
                                                                                AssetStructure = assetStructure,
                                                                                AssetLevel = assetLevel,
                                                                                AssetUse = assetUse,
                                                                                AssetUseTime = assetUseTime,
                                                                                AssetAddress = assetAddress
                                                                            });
                                                                            assetCount++;
                                                                        }
                                                                        //tai san la nha
                                                                        else
                                                                        {
                                                                            string query_1 = "SELECT dienTichXayDung,dienTichSan,ketCauChiTiet,diaChi,soTang FROM dbo.Nha  WHERE nhaId = @Id";
                                                                            using (SqlCommand command_1 = new SqlCommand(query_1, conn))
                                                                            {
                                                                                command_1.Parameters.AddWithValue("@Id", assetId);
                                                                                using (SqlDataReader reader_4 = command_1.ExecuteReader())
                                                                                {
                                                                                    if (reader_4.Read())
                                                                                    {
                                                                                        assetAddress = reader_4["diaChi"].ToString();
                                                                                        assetArea = reader_4["dienTichSan"].ToString();
                                                                                        assetAreaUse = reader_4["dienTichXayDung"].ToString();
                                                                                        assetStructure = reader_4["ketCauChiTiet"].ToString();
                                                                                        assetNumberFloor = reader_4["soTang"].ToString();
                                                                                    }
                                                                                }
                                                                            }

                                                                            string query_2 = "SELECT kyHieuLoaiCapNha FROM dbo.LoaiCapNha WHERE loaiCapNhaID = @Id";
                                                                            using (SqlCommand command_2 = new SqlCommand(query_2, conn))
                                                                            {
                                                                                command_2.Parameters.AddWithValue("@Id", loaiNhaId);
                                                                                using (SqlDataReader reader_5 = command_2.ExecuteReader())
                                                                                {
                                                                                    if (reader_5.Read())
                                                                                    {
                                                                                        assetLevel = reader_5["kyHieuLoaiCapNha"].ToString();
                                                                                    }
                                                                                }
                                                                            }

                                                                            string query_3 = "SELECT tenLoaiTaiSan FROM dbo.LoaiTaiSan WHERE loaiTaiSanId = @Id";
                                                                            using (SqlCommand command_3 = new SqlCommand(query_3, conn))
                                                                            {
                                                                                command_3.Parameters.AddWithValue("@Id", taisanID);
                                                                                using (SqlDataReader reader_6 = command_3.ExecuteReader())
                                                                                {
                                                                                    if (reader_6.Read())
                                                                                    {
                                                                                        assetName = reader_6["tenLoaiTaiSan"].ToString();
                                                                                    }
                                                                                }
                                                                            }


                                                                            if (item == 0)
                                                                            {
                                                                                assetUse = "Sử dụng chung";
                                                                            }
                                                                            else
                                                                            {
                                                                                assetUse = "Sử dụng riêng";
                                                                            }
                                                                           
                                                                            dataLand.Muc3.Add(assetCount, new Asset
                                                                            {
                                                                                AssetName = assetName,
                                                                                AssetArea = assetArea,
                                                                                AssetAreaUse = assetAreaUse,
                                                                                AssetNumberFloor = assetNumberFloor,
                                                                                AssetStructure = assetStructure,
                                                                                AssetLevel = assetLevel,
                                                                                AssetUse = assetUse,
                                                                                AssetUseTime = assetUseTime,
                                                                                AssetAddress = assetAddress
                                                                            });
                                                                            assetCount++;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dataLand.Muc3.Add(1, new Asset
                                                                    {
                                                                        AssetName = "Không xác định",
                                                                        AssetArea = "Không xác định",
                                                                        AssetAreaUse = "Không xác định",
                                                                        AssetNumberFloor = "Không xác định",
                                                                        AssetStructure = "Không xác định",
                                                                        AssetLevel = "Không xác định",
                                                                        AssetUse = "Không xác định",
                                                                        AssetUseTime = "Không xác định",
                                                                        AssetAddress = "Không xác định"
                                                                    });
                                                                }
                                                            }
                                                            //khong co thong tin
                                                            else
                                                            {
                                                                dataLand.Muc3.Add(1, new Asset
                                                                {
                                                                    AssetName = "Không xác định",
                                                                    AssetArea = "Không xác định",
                                                                    AssetAreaUse = "Không xác định",
                                                                    AssetNumberFloor = "Không xác định",
                                                                    AssetStructure = "Không xác định",
                                                                    AssetLevel = "Không xác định",
                                                                    AssetUse = "Không xác định",
                                                                    AssetUseTime = "Không xác định",
                                                                    AssetAddress = "Không xác định"
                                                                });
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                    }
                                }
                            }

                            //Lay thong tin anh
                            string query_img = "SELECT soDo FROM dbo.SoDoGiayChungNhan WHERE giayChungNhanId = @Id";
                            using (SqlCommand command_img = new SqlCommand(query_img, conn))
                            {
                                command_img.Parameters.AddWithValue("@Id", id);
                                using (SqlDataReader reader_4 = command_img.ExecuteReader())
                                {
                                    int img_id = 1;
                                    while (reader_4.Read())
                                    {
                                        byte[] imageData = (byte[])reader["soDo"];

                                        string base64Image = "data:image/png;base64," + Convert.ToBase64String(imageData);
                                        dataLand.Muc4.Add(img_id, base64Image);
                                        img_id++;
                                    }
                                }
                            }

                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dataLand);

                            web_1.CoreWebView2.PostWebMessageAsJson(json);
                        }
                        else
                        {
                            lb_check.Text = "No data found.";
                        }
                    }
                }
            }
        }

    }
}
