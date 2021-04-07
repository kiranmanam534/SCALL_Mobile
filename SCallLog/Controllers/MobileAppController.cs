using SCallLog.Models;
using SCallLog.Models.Packages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCallLog.Controllers
{
    public class MobileAppController : Controller
    {
        // GET: Mobile

        tbl_welcome ad = new tbl_welcome();
        GlobalClass gc = new GlobalClass();
        public ActionResult scallMobilelogin(string Username, string Password)
        {
            try
            {
                if (Username != "" && Password != "")
                {
                    var result = gc.db.SCL_Login.Where(s => s.username == Username && s.password == Password).FirstOrDefault();
                    if (result != null)
                    {
                        int? compId = 0;
                        string name = "";
                        int noOfUsers = 0;
                        int noOfComplints = 0;
                        if (result.type == "USER")
                        {
                            var user = gc.db.SCL_Users.FirstOrDefault(x => x.ID == result.CID);
                            if (user != null)
                            {
                                compId = user.CompanyID;
                                name = user.FirstName + " " + user.LastName;
                                noOfComplints = gc.db.SCL_Mobile_Complaints.Count(x => x.AllocateduserID == result.CID);
                            }

                        }
                        else if (result.type == "ADMIN")
                        {
                            compId = result.CID;
                            var company = gc.db.SCL_CompanyRegistration.FirstOrDefault(x => x.CID == result.CID);
                            name = company.company_name;
                            noOfUsers = gc.db.SCL_Users.Count(x => x.CompanyID == result.CID);
                            noOfComplints = gc.db.SCL_Mobile_Complaints.Count(x => x.CompanyID == result.CID);

                        }
                        var res = new MobileAppResponse<string>()
                        {
                            loggedType = result.type,
                            loggedId = result.CID,
                            companyId = compId,
                            name = name,
                            noOfUsers = noOfUsers,
                            noOfComplints = noOfComplints,
                            statusCode = 200
                        };

                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new MobileAppResponse<string>()
                    {

                        message = "Inavalid username and password!",
                        statusCode = 401,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new MobileAppResponse<string>()
                {

                    message = ex.Message,
                    statusCode = 500,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new MobileAppResponse<string>()
            {

                message = "Bad request",
                statusCode = 400,
            }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult scallMobileComplaintCounts(string loginType, int loginId)
        {
            try
            {
                if (loginType != "" && loginId != 0)
                {

                    if (loginType == "USER")
                    {
                        var userComplaints = gc.db.SCL_Mobile_Complaints.Where(x => x.AllocateduserID == loginId);

                        var d1 = gc.db.SCL_Comp_Status.GroupBy(f => f.STATUS).Select(d => new ComplaintCounts
                        {
                            name = d.Key,
                            count = userComplaints.Count(s => s.ComplaintStatus == d.Key)

                        }).AsEnumerable();

                        var res = new MobileAppResponse<ComplaintCounts>()
                        {
                            complaintCounts = d1,
                            statusCode = 200
                        };

                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                    else if (loginType == "ADMIN")
                    {
                        var compComplaints = gc.db.SCL_Mobile_Complaints.Where(x => x.CompanyID == loginId);

                        var d1 = gc.db.SCL_Comp_Status.GroupBy(f => f.STATUS).Select(d => new ComplaintCounts
                        {
                            name = d.Key,
                            count = compComplaints.Count(s => s.ComplaintStatus == d.Key)

                        }).AsEnumerable();

                        var res = new MobileAppResponse<ComplaintCounts>()
                        {
                            complaintCounts = d1,
                            statusCode = 200
                        };

                        return Json(res, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new MobileAppResponse<string>()
                    {

                        message = "bad request",
                        statusCode = 400,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new MobileAppResponse<string>()
                {

                    message = ex.Message,
                    statusCode = 500,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new MobileAppResponse<string>()
            {

                message = "No data found!",
                statusCode = 403,
            }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult scallMobileDepartments()
        {
            try
            {
                //
                var result = gc.db.SCL_Department.AsQueryable();
                if (result.Any())
                {

                    var res = new MobileAppResponse<MasterData>()
                    {

                        all = result.Select(s => new MasterData
                        {
                            id = s.DEPARTMENT_ID,
                            name = s.DEPARTMENT_DESC
                        }),
                        statusCode = 200
                    };

                    return Json(res, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new MobileAppResponse<string>()
                {

                    message = ex.Message,
                    statusCode = 500,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new MobileAppResponse<string>()
            {

                message = "Bad request",
                statusCode = 400,
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult scallMobileCategoriesByDptId(string departmentId)
        {
            try
            {
                var result = gc.db.SCL_Category.Where(c => c.DEPARTMENT_ID == departmentId).AsQueryable();
                if (result.Any())
                {

                    var res = new MobileAppResponse<MasterData>()
                    {
                        all = result.Select(s => new MasterData
                        {
                            id = s.CATEGORY_ID,
                            name = s.CATEGORY_DESC
                        }),
                        statusCode = 200
                    };

                    return Json(res, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new MobileAppResponse<string>()
                {

                    message = ex.Message,
                    statusCode = 500,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new MobileAppResponse<string>()
            {

                message = "Bad request",
                statusCode = 400,
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult scallMobileSubCategoriesByCatId(string catId)
        {
            try
            {
                var result = gc.db.SCL_Sub_Category.Where(c => c.CATEGORY_ID == catId).AsQueryable();
                if (result.Any())
                {

                    var res = new MobileAppResponse<MasterData>()
                    {
                        all = result.Select(s => new MasterData
                        {
                            id = s.SUB_CATEGORY_ID,
                            name = s.SUB_CATEGORY_DESC
                        }),
                        statusCode = 200
                    };

                    return Json(res, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new MobileAppResponse<string>()
                {

                    message = ex.Message,
                    statusCode = 500,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new MobileAppResponse<string>()
            {

                message = "Bad request",
                statusCode = 400,
            }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult scallMobileComplaints(string loginType, int loginId)
        {
            try
            {
                if (loginType != "" && loginId != 0)
                {

                    if (loginType == "USER")
                    {
                        var userComplaints = gc.db.SCL_Mobile_Complaints.Where(x => x.AllocateduserID == loginId);

                        var res = new MobileAppResponse<SCL_Mobile_Complaints>()
                        {
                            all = userComplaints.AsEnumerable().Select(s => new SCL_Mobile_Complaints
                            {
                                ID = s.ID,
                                Complaint_ReferenceNo = s.Complaint_ReferenceNo,
                                Complaint_Date = s.Complaint_Date,
                                Category_Id = s.Category_Id,
                                Category_Desc = s.Category_Desc,
                                SubCategory_Id = s.SubCategory_Id,
                                SubCategory_Desc = s.SubCategory_Desc,
                                Dept_Id = s.Dept_Id,
                                Dept_Desc = s.Dept_Desc,
                                FirstName = s.FirstName,
                                LastName = s.LastName,
                                MobileNo = s.MobileNo,
                                Email = s.Email,
                                Address = s.Address,
                                FileName = s.FileName,
                                CreatedBy = s.CreatedBy,
                                CreatedDate = s.CreatedDate,
                                ModifiedBy = s.ModifiedBy,
                                ModifiedDate = s.ModifiedDate,
                                ComplaintStatus = s.ComplaintStatus,
                                Lattitude = s.Lattitude,
                                Longitude = s.Longitude,
                                // img_data = s.img_data,//s.SCL_ComplaintImages.Where(i => i.Complaint_ID == s.ID).FirstOrDefault() != null ? s.SCL_ComplaintImages.Where(i => i.Complaint_ID == s.ID).FirstOrDefault().AttachmentName : "",
                                automatic_complaint = s.automatic_complaint,
                                userID = s.userID,
                                AllocateduserID = s.AllocateduserID,
                                Comments = s.Comments,
                                CompanyID = s.CompanyID,
                                JC_REF = s.JC_REF,

                            }).AsQueryable(),
                            statusCode = 200
                        };

                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                    else if (loginType == "ADMIN")
                    {
                        var compComplaints = gc.db.SCL_Mobile_Complaints.Where(x => x.CompanyID == loginId);


                        var res = new MobileAppResponse<SCL_Mobile_Complaints>()
                        {
                            all = compComplaints.AsEnumerable().Select(s => new SCL_Mobile_Complaints
                            {
                                ID = s.ID,
                                Complaint_ReferenceNo = s.Complaint_ReferenceNo,
                                Complaint_Date = s.Complaint_Date,
                                Category_Id = s.Category_Id,
                                Category_Desc = s.Category_Desc,
                                SubCategory_Id = s.SubCategory_Id,
                                SubCategory_Desc = s.SubCategory_Desc,
                                Dept_Id = s.Dept_Id,
                                Dept_Desc = s.Dept_Desc,
                                FirstName = s.FirstName,
                                LastName = s.LastName,
                                MobileNo = s.MobileNo,
                                Email = s.Email,
                                Address = s.Address,
                                FileName = s.FileName,
                                CreatedBy = s.CreatedBy,
                                CreatedDate = s.CreatedDate,
                                ModifiedBy = s.ModifiedBy,
                                ModifiedDate = s.ModifiedDate,
                                ComplaintStatus = s.ComplaintStatus,
                                Lattitude = s.Lattitude,
                                Longitude = s.Longitude,
                                // img_data = s.img_data,// s.SCL_ComplaintImages.Where(i => i.Complaint_ID == s.ID).FirstOrDefault() != null ? s.SCL_ComplaintImages.Where(i => i.Complaint_ID == s.ID).FirstOrDefault().AttachmentName : "",
                                automatic_complaint = s.automatic_complaint,
                                userID = s.userID,
                                AllocateduserID = s.AllocateduserID,
                                Comments = s.Comments,
                                CompanyID = s.CompanyID,
                                JC_REF = s.JC_REF,

                            }).AsQueryable(),
                            statusCode = 200
                        };

                        return Json(res, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new MobileAppResponse<string>()
                    {

                        message = "bad request",
                        statusCode = 400,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new MobileAppResponse<string>()
                {

                    message = ex.Message,
                    statusCode = 500,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new MobileAppResponse<string>()
            {

                message = "No data found!",
                statusCode = 403,
            }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult scallMobileComplaintsHistory(string refNumber)
        {
            try
            {
                if (refNumber != "")
                {

                    var complaintsHistory = gc.db.sp_getComplaintHistoryByRefNumber(refNumber).AsQueryable();


                    var res = new MobileAppResponse<sp_getComplaintHistoryByRefNumber_Result>()
                    {
                        all = complaintsHistory,
                        statusCode = 200
                    };

                    return Json(res, JsonRequestBehavior.AllowGet);


                }
                else
                {
                    return Json(new MobileAppResponse<string>()
                    {

                        message = "bad request",
                        statusCode = 400,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new MobileAppResponse<string>()
                {

                    message = ex.Message,
                    statusCode = 500,
                }, JsonRequestBehavior.AllowGet);
            }
        }





        public ActionResult scallMobileSaveComplaint(mobileComplaintInputs complaintResponse)
        {
            try
            {
                if (complaintResponse != null)
                {
                    string refNumber = complaintResponse.Category_Id;
                    var complants = gc.db.SCL_Mobile_Complaints.OrderByDescending(x => x.ID).FirstOrDefault();
                    if (complants != null)
                    {
                        refNumber = refNumber + (complants.ID + 1);
                    }
                    else
                    {
                        refNumber = refNumber + "000";
                    }

                    var dt = DateTime.Now;

                    SCL_Mobile_Complaints complaint = new SCL_Mobile_Complaints
                    {
                        Complaint_ReferenceNo = refNumber,
                        Complaint_Date = dt.ToString(),
                        CreatedDate = dt,
                        ComplaintStatus = "Received",
                        CreatedBy = complaintResponse.loggedId.ToString(),
                        Comments = complaintResponse.comments,
                        Dept_Id = complaintResponse.Dept_Id,
                        Dept_Desc = complaintResponse.Dept_Desc,
                        Category_Id = complaintResponse.Category_Id,
                        Category_Desc = complaintResponse.Category_Desc,
                        SubCategory_Id = complaintResponse.SubCategory_Id,
                        SubCategory_Desc = complaintResponse.SubCategory_Desc,
                        Lattitude = complaintResponse.Lattitude,
                        Longitude = complaintResponse.Longitude,
                        Address = complaintResponse.Address,
                        CompanyID = complaintResponse.companyId,
                        AllocateduserID = complaintResponse.loggedId,
                        FileName = refNumber + '_' + complaintResponse.companyId + '_' + complaintResponse.loggedId + ".jpg"
                    };


                    gc.db.SCL_Mobile_Complaints.Add(complaint);
                    if (gc.db.SaveChanges() > 0)
                    {


                        string filename = refNumber + '_' + complaintResponse.companyId + '_' + complaintResponse.loggedId + ".jpg";

                        bool imge = SaveImage(complaintResponse.img_data, filename);


                        SCL_ComplaintImages img = new SCL_ComplaintImages();

                        img.AttachmentName = filename;
                        img.Complaint_ID = complaint.ID;
                        img.Complaint_ReferenceNo = refNumber;
                        img.Device = "mobile";
                        gc.db.SCL_ComplaintImages.Add(img);
                        gc.db.SaveChanges();


                        var res = new MobileAppResponse<string>()
                        {
                            message = "Saved",
                            statusCode = 200
                        };

                        return Json(res, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new MobileAppResponse<string>()
                        {

                            message = "Not saved",
                            statusCode = 400,
                        }, JsonRequestBehavior.AllowGet);
                    }



                }
                else
                {
                    return Json(new MobileAppResponse<string>()
                    {

                        message = "bad request",
                        statusCode = 400,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new MobileAppResponse<string>()
                {

                    message = ex.Message,
                    statusCode = 500,
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            return image;
        }

        public bool SaveImage(string base64String, string imageName)
        {
            String path = HttpContext.Server.MapPath("~/Attachments"); //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }


            //set the image path
            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(base64String);

            System.IO.File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }


    }
}