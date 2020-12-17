using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Exten
{
    public static class SD
    {

        public static class Thong_ke
        {
            public static string DOANH_THU_THANG = "THONG_KE_DOANH_THU_THANG";
            public static string DOANH_THU_10_NGAY_GAN_NHAT = "DOANH_THU_10_NGAY_GAN_NHAT"; 
            public static string THONG_KE_10_KHACH_HANG_MUA_NHIEU_NHAT = "THONG_KE_10_KHACH_HANG_MUA_NHIEU_NHAT";
            public static string THONG_KE_10_NHAN_VIEN_DOANH_THU_CAO_NHAT = "THONG_KE_10_NHAN_VIEN_DOANH_THU_CAO_NHAT";
            public static string THONG_KE_10_SAN_PHAM_MUA_NHIEU_NHAT = "THONG_KE_10_SAN_PHAM_MUA_NHIEU_NHAT";
            public static string DOANH_THU_10_THANG_GAN_NHAT = "DOANH_THU_10_THANG_GAN_NHAT";

        }

        public static class Phan_Loai
        {
            public static string CREATE = "PHAN_LOAI_CREATE";
            public static string GET_ALL = "PHAN_LOAI_GETALL";
            public static string GET = "PHAN_LOAI_GET";
            public static string UPDATE = "PHAN_LOAI_UPDATE";
            public static string DELETE = "PHAN_LOAI_DELETE";
        }
        public static class Chuc_vu
        {
            public static string CREATE = "CHUC_VU_CREATE";
            public static string GET_ALL = "CHUC_VU_GETALL";
            public static string GET = "CHUC_VU_GET";
            public static string UPDATE = "CHUC_VU_UPDATE";
            public static string DELETE = "CHUC_VU_DELETE";


        }
        public static class Nhan_Vien
        {
            public static string CREATE = "NHAN_VIEN_CREATE";
            public static string GET_ALL = "NHAN_VIEN_GETALL";
            public static string GET = "NHAN_VIEN_GET";
            public static string UPDATE = "NHAN_VIEN_UPDATE";
            public static string DELETE = "NHAN_VIEN_DELETE";
        }
        public static class Mat_Hang
        {
            public static string CREATE = "MAT_HANG_CREATE";
            public static string GET_ALL = "MAT_HANG_GETALL";
            public static string GET = "MAT_HANG_GET";
            public static string UPDATE = "MAT_HANG_UPDATE";
            public static string DELETE = "MAT_HANG_DELETE";
        }
        public static class Nha_Cung_Cap
        {
            public static string CREATE = "NHA_CUNG_CAP_CREATE";
            public static string GET_ALL = "NHA_CUNG_CAP_GETALL";
            public static string GET = "NHA_CUNG_CAP_GET";
            public static string UPDATE = "NHA_CUNG_CAP_UPDATE";
            public static string DELETE = "NHA_CUNG_CAP_DELETE";
        }
        public static class Loai_Khach_Hang
        {
            public static string CREATE = "LOAI_KHACH_HANG_CREATE";
            public static string GET_ALL = "LOAI_KHACH_HANG_GETALL";
            public static string GET = "LOAI_KHACH_HANG_GET";
            public static string UPDATE = "LOAI_KHACH_HANG_UPDATE";
            public static string DELETE = "LOAI_KHACH_HANG_DELETE";
        }
        public static class Kho
        {
            public static string CREATE = "KHO_CREATE";
            public static string GET_ALL = "KHO_GETALL";
            public static string GET = "KHO_GET";
            public static string UPDATE = "KHO_UPDATE";
            public static string DELETE = "KHO_DELETE";
        }
        public static class Khach_Hang
        {
            public static string CREATE = "KHACH_HANG_CREATE";
            public static string GET_ALL = "KHACH_HANG_GETALL";
            public static string GET = "KHACH_HANG_GET";
            public static string UPDATE = "KHACH_HANG_UPDATE";
            public static string DELETE = "KHACH_HANG_DELETE";
        }
        public static class Hoa_Don
        {
            public static string CREATE = "HOA_DON_CREATE";
            public static string GET_ALL ="HOA_DON_GETALL";
            public static string GET =    "HOA_DON_GET";
            public static string UPDATE = "HOA_DON_UPDATE";
            public static string DELETE = "HOA_DON_DELETE";
            public static string GETMAXID = "HOA_DON_GET_MAX_ID";
            public static string GETDETAIL = "HOA_DON_GET_DETAILS";
            public static string CHECK_ENOUGH_QUANTITY_PRODUCT = "HOA_DON_CHECK_ENOUGH_QUANTITY_PRODUCT";
        }
        public static class Giam_Gia
        {
            public static string IS_EXIST = "GIAM_GIA_ISEXITS";
            public static string CREATE = "GIAM_GIA_CREATE";
            public static string GET_ALL = "GIAM_GIA_GETALL";
            public static string GET = "GIAM_GIA_GET";
            public static string UPDATE = "GIAM_GIA_UPDATE";
            public static string DELETE = "GIAM_GIA_DELETE";
            public static string CLEAR_REF = "GIAM_GIA_DELETE_MAGG_MAPL";
            
        }
        public static class Giam_Gia_Phan_Loai
        {
            public static string CREATE = "GGPL_CREATE";
            public static string GET_ALL = "GGPL_GETALL";
            public static string GET = "GGPL_GET";
            public static string UPDATE = "GGPL_UPDATE";
            public static string DELETE = "GGPL_DELETE";
        }
        public static class Giam_Gia_San_Phan
        {
            public static string CREATE = "GGMH_CREATE";
            public static string GET_ALL = "GGMH_GETALL";
            public static string GET = "GGMH_GET";
            public static string UPDATE = "GGMH_UPDATE";
            public static string DELETE = "GGMH_DELETE";
        }
        public static class Don_Nhap_Hang
        {
            public static string CREATE = "DON_NHAP_HANG_CREATE";
            public static string GET_ALL = "DON_NHAP_HANG_GETALL";
            public static string GET = "DON_NHAP_HANG_GET";
            public static string UPDATE = "DON_NHAP_HANG_UPDATE";
            public static string DELETE = "DON_NHAP_HANG_DELETE";
            public static string GETDETAIL = "DON_NHAP_HANG_GET_DETAILS";
        }
        public static class Chi_Tiet_Hoa_Don
        {
            public static string CREATE = "CHI_TIET_HOA_DON_CREATE";
            public static string GET_ALL = "CHI_TIET_HOA_DON_GETALL";
            public static string GET = "CHI_TIET_HOA_DON_GET";
            public static string UPDATE = "CHI_TIET_HOA_DON_UPDATE";
            public static string DELETE = "CHI_TIET_HOA_DON_DELETE";
        }
        public static class Chi_Tiet_Don_Nhap_Hang
        {
            public static string CREATE = "CHI_TIET_DON_NHAP_HANG_CREATE";
            public static string GET_ALL = "CHI_TIET_DON_NHAP_HANG_GETALL";
            public static string GET = "CHI_TIET_DON_NHAP_HANG_GET";
            public static string UPDATE = "CHI_TIET_DON_NHAP_HANG_UPDATE";
            public static string DELETE = "CHI_TIET_DON_NHAP_HANG_DELETE";
        }







    }
}
