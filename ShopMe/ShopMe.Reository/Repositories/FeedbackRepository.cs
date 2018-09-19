using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Reository.Repositories
{
   public interface IFeedbackRepository:IRepository<Feedback>
    {
        void RatingUser(string user);
    }
    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void RatingUser(string ma_kh)
        {
            float diem_tb2 = 0;
            float diem_tb1 = 0;
            float diem1 = 0;
            float diem2 = 0;
            float diem_theo_sp = 0;
            float dotuongdong = 0;
            float s_tren = 0;
            float s_duoi = 0;
            float tong_tren = 0;
            float tong_duoi = 0;
            float diemdanhgia = 0;
            var khach_hang = DbContext.Users.Find(ma_kh);
            if (khach_hang != null)
            {
                //Chọn k người dùng làm hàng xóm.
                var ListKhachHangTuongDong = (from dtd in DbContext.PearsonScores
                                              where dtd.Sim_Pearson_Score <= 1
                                              orderby dtd.Sim_Pearson_Score descending
                                              select dtd).Take(1);
                List<UserRating> sanpham_u2_danhgia = new List<UserRating>();
                foreach (var x in ListKhachHangTuongDong)
                {
                    var sp_u2_danhgia = DbContext.UserRatings.Where(p => p.UserID
                    .Equals(x.UserID_2)).ToList();
                    if (sp_u2_danhgia != null)
                    {

                        foreach (var z in sp_u2_danhgia)
                        {
                            var trung = sanpham_u2_danhgia.Where(p => p.ProductID.Equals(z.ProductID)).ToList();
                            if (trung.Count < 1)
                            {
                                //list sản phẩm k người dùng đã đánh giá
                                sanpham_u2_danhgia.Add(z);
                            }
                        }

                    }
                }
                try
                {
                    //lấy tổng điểm đánh giá của khách hàng vừa đăng nhập
                    var tong1_result = DbContext.UserRatingAverages.Where(p => p.UserID.Equals(ma_kh)).Select(n => n.RatingAverage).SingleOrDefault();
                    diem_tb1 = (float)tong1_result;
                }
                catch
                {
                    diem1 = 0;
                }

                //Lấy danh sách sản phẩm  đã đánh giá của người dùng đang xét.
                var sp_u1_danhgia = DbContext.UserRatings.Where(p => p.UserID.Equals(ma_kh)).ToList();
                foreach (var item in sp_u1_danhgia)
                {
                    //Lấy danh sách sản phẩm mà k người dùng đã xếp hạng mà người dùng đang xét chưa xếp hạng.
                    var resutl = sanpham_u2_danhgia.Where(p => p.ProductID.Equals(item.ProductID)).ToList();
                    //Nếu tìm ra được danh sách có dữ liệu thì ta tiến hành xóa nó ra khỏi danh sách xếp hạng(vì ta đang tìm danh sách sản phẩm của người dùng đang xét chưa xếp hạng)
                    if (resutl != null)
                    {
                        foreach (var i in resutl)
                        {
                            sanpham_u2_danhgia.Remove(i);
                            //Kết quả nhận được là một list ListSanPham còn lại những sản phẩm mà u1 chưa xếp hạng nhưng u2 đã xếp hạng
                        }
                    }
                }
                //Duyệt qua từng sản phẩm để tìm ra danh sách user2 để dự đoán điểm xếp hạng cho sản phẩm duyệt qua đó.
                foreach (var sanpham in sanpham_u2_danhgia)
                {

                    //Duyệt qua từng user để áp dụng vô công thức tính dự đoán cho từng sản phẩm.Lấy từ (5 đến 10 user)
                    foreach (var user2 in ListKhachHangTuongDong)
                    {
                        try
                        {
                            //get độ tương đồng cho user2 duyệt qua
                            var do_td = DbContext.PearsonScores.Where(p => p.UserID_1.Equals(ma_kh) && p.UserID_2.Equals(user2.UserID_2)).Select(p => p.Sim_Pearson_Score).SingleOrDefault();
                            dotuongdong = (float)do_td;


                        }
                        catch
                        {
                            dotuongdong = 0;
                        }
                        //get tổng điểm của user2 duyệt qua.
                        var tong2 = DbContext.UserRatingAverages.Where(p => p.UserID.Equals(user2.UserID_2)).Select(p => p.RatingAverage).SingleOrDefault();
                        diem_tb2 = (float)tong2;


                        try
                        {
                            //Lấy điểm đánh giá của từng sản phẩm mà user đang duyệt qua 
                            diem_theo_sp = (float)(DbContext.UserRatings.Where(p => p.UserID.Equals(user2.UserID_2) && p.ProductID == sanpham.ProductID).Select(p => p.Rating)).Sum();
                            //Tính giá trị trên mẫu số
                            s_tren = (float)Math.Round((dotuongdong * (float)(diem_theo_sp - diem_tb2)), 4);
                            s_duoi = (float)Math.Round((float)Math.Abs(dotuongdong), 4);
                        }
                        catch
                        {
                            //Nếu điểm chưa có ta gán cho điểm xếp hạng này bằng 0
                            diem_theo_sp = 0;
                            //Tính giá trị trên mẫu số
                            s_tren = (float)Math.Round((dotuongdong * (float)(diem_theo_sp - diem_tb2)), 4);
                            s_duoi = (float)Math.Round((float)Math.Abs(dotuongdong), 4);
                        }
                        //Cộng dồn qua các lần lặp  (*)
                        tong_tren = (float)Math.Round(tong_tren, 4) + (float)Math.Round(s_tren, 4);
                        tong_duoi = (float)Math.Round(tong_duoi, 4) + (float)Math.Round(s_duoi, 4);
                    }
                    //Nếu mẫu số khác 0 ta thực hiện việc tính toán và lưu kết quả
                    if (tong_duoi != 0)
                    {
                        //Kiểm tra xem sản phẩm này có được khách hàng đang xét có đánh giá chưa
                        var khachhang_xephang = DbContext.DanhSachGoiSanPhams.Where(p => p.UserID.Equals(ma_kh) && p.ProductID.Equals(sanpham.ProductID)).SingleOrDefault();
                        //Nếu chưa ta thực hiện gán giá trị và lưu vào model
                        if (khachhang_xephang == null)
                        {
                            diemdanhgia = (float)Math.Round((diem_tb1 + (tong_tren / tong_duoi)), 4);
                            DanhSachGoiSanPham xephang = new DanhSachGoiSanPham();
                            int diem = 0;
                            diem = (int)Math.Round(diemdanhgia, 0);
                            if (diem > 5)
                            {
                                diem = 5;
                            }
                         
                            xephang.UserID = ma_kh;
                            xephang.ProductID = sanpham.ProductID;
                            xephang.Rating = diem;
                            DbContext.DanhSachGoiSanPhams.Add(xephang);
                           
                         
                           

                        }

                    }
             
                    //Ngược lại nếu tổng dưới bằng 0 thì ta bỏ qua lần duyệt này.tiếp tục quay về duyệt vòng duyệt khác.
                    else
                    {
                        continue;
                    }
                }

              
            }
        }
    }
}
